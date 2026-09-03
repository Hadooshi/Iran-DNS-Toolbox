using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DNSChangerApp.Models;

namespace DNSChangerApp.Services
{
    public class NetworkAdapterInfo
    {
        public string Name { get; set; } = string.Empty;
        public string InterfaceDescription { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsPhysical { get; set; } = true;

        public bool IsUp => Status.Equals("Up", StringComparison.OrdinalIgnoreCase);
        public string StatusText => IsUp ? "متصل (Up)" : "غیرفعال";
        public string StatusBg => IsUp ? "#1E3326" : "#2A2A2A";
        public string StatusFg => IsUp ? "#34D399" : "#888888";
    }

    public class VerificationResult
    {
        public string AdapterName { get; set; } = string.Empty;
        public string ActiveDnsList { get; set; } = string.Empty;
        public string IdentifiedService { get; set; } = string.Empty;
        public bool GoogleResolved { get; set; }
        public string GoogleIp { get; set; } = string.Empty;
        public bool SanctionedSiteResolved { get; set; }
        public string SanctionedSiteIp { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public static class NetworkService
    {
        private static readonly byte[] DnsQueryPayload = new byte[]
        {
            0x12, 0x34, 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x06, 0x67, 0x6f, 0x6f, 0x67, 0x6c, 0x65, 0x03, 0x63, 0x6f, 0x6d, 0x00,
            0x00, 0x01, 0x00, 0x01
        };

        public static List<NetworkAdapterInfo> GetPhysicalAdapters()
        {
            var list = new List<NetworkAdapterInfo>();
            try
            {
                // Run PowerShell to get physical adapters
                var psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"Get-NetAdapter | Where-Object { $_.HardwareInterface -eq $true } | Select-Object Name, InterfaceDescription, Status | ConvertTo-Json -Compress\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var proc = Process.Start(psi);
                if (proc != null)
                {
                    string output = proc.StandardOutput.ReadToEnd().Trim();
                    proc.WaitForExit(4000);

                    if (!string.IsNullOrWhiteSpace(output))
                    {
                        if (output.StartsWith("["))
                        {
                            var items = JsonSerializer.Deserialize<List<JsonElement>>(output);
                            if (items != null)
                            {
                                foreach (var item in items)
                                {
                                    list.Add(new NetworkAdapterInfo
                                    {
                                        Name = item.GetProperty("Name").GetString() ?? "",
                                        InterfaceDescription = item.GetProperty("InterfaceDescription").GetString() ?? "",
                                        Status = item.GetProperty("Status").GetString() ?? "Unknown"
                                    });
                                }
                            }
                        }
                        else if (output.StartsWith("{"))
                        {
                            var item = JsonSerializer.Deserialize<JsonElement>(output);
                            list.Add(new NetworkAdapterInfo
                            {
                                Name = item.GetProperty("Name").GetString() ?? "",
                                InterfaceDescription = item.GetProperty("InterfaceDescription").GetString() ?? "",
                                Status = item.GetProperty("Status").GetString() ?? "Unknown"
                            });
                        }
                    }
                }
            }
            catch { }

            // Fallback: Use .NET NetworkInterface if PowerShell didn't return
            if (list.Count == 0)
            {
                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                        nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel &&
                        !nic.Description.Contains("Virtual", StringComparison.OrdinalIgnoreCase) &&
                        !nic.Description.Contains("Nord", StringComparison.OrdinalIgnoreCase) &&
                        !nic.Description.Contains("TAP", StringComparison.OrdinalIgnoreCase))
                    {
                        list.Add(new NetworkAdapterInfo
                        {
                            Name = nic.Name,
                            InterfaceDescription = nic.Description,
                            Status = nic.OperationalStatus == OperationalStatus.Up ? "Up" : "Down"
                        });
                    }
                }
            }

            // Order Up adapters first
            return list.OrderByDescending(a => a.Status.Equals("Up", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static List<string> GetCurrentDns(string adapterName)
        {
            var addresses = new List<string>();
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"(Get-DnsClientServerAddress -InterfaceAlias '{adapterName}' -AddressFamily IPv4).ServerAddresses\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var proc = Process.Start(psi);
                if (proc != null)
                {
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit(3000);

                    var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        string trimmed = line.Trim();
                        if (IPAddress.TryParse(trimmed, out _))
                        {
                            addresses.Add(trimmed);
                        }
                    }
                }
            }
            catch { }

            return addresses;
        }

        public static bool ApplyDns(string adapterName, string primary, string secondary)
        {
            try
            {
                string script;
                if (string.IsNullOrWhiteSpace(secondary))
                {
                    script = $"Set-DnsClientServerAddress -InterfaceAlias '{adapterName}' -ServerAddresses ('{primary}')";
                }
                else
                {
                    script = $"Set-DnsClientServerAddress -InterfaceAlias '{adapterName}' -ServerAddresses ('{primary}', '{secondary}')";
                }

                var psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var proc = Process.Start(psi);
                proc?.WaitForExit(5000);

                FlushDns();
                return proc?.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool ResetDnsToDhcp(string adapterName)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"Set-DnsClientServerAddress -InterfaceAlias '{adapterName}' -ResetServerAddresses\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var proc = Process.Start(psi);
                proc?.WaitForExit(5000);

                FlushDns();
                return proc?.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        public static void FlushDns()
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "ipconfig",
                    Arguments = "/flushdns",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using var proc = Process.Start(psi);
                proc?.WaitForExit(3000);
            }
            catch { }
        }

        public static async Task<long> PingIpAsync(string ip, int timeoutMs = 800)
        {
            if (string.IsNullOrWhiteSpace(ip)) return 9999;
            try
            {
                using var ping = new Ping();
                long totalLatency = 0;
                int successes = 0;

                for (int i = 0; i < 2; i++)
                {
                    var reply = await ping.SendPingAsync(ip, timeoutMs);
                    if (reply.Status == IPStatus.Success)
                    {
                        totalLatency += reply.RoundtripTime;
                        successes++;
                    }
                }

                if (successes > 0)
                {
                    return totalLatency / successes;
                }
            }
            catch { }

            return 9999;
        }

        public static async Task<bool> TestUdp53Async(string ip, int timeoutMs = 1200)
        {
            if (string.IsNullOrWhiteSpace(ip)) return false;
            try
            {
                if (!IPAddress.TryParse(ip, out var ipAddr)) return false;

                using var udp = new UdpClient();
                udp.Client.SendTimeout = timeoutMs;
                udp.Client.ReceiveTimeout = timeoutMs;
                udp.Connect(ipAddr, 53);

                await udp.SendAsync(DnsQueryPayload, DnsQueryPayload.Length);

                var receiveTask = udp.ReceiveAsync();
                var timeoutTask = Task.Delay(timeoutMs);

                var completed = await Task.WhenAny(receiveTask, timeoutTask);
                if (completed == receiveTask)
                {
                    var result = await receiveTask;
                    return result.Buffer != null && result.Buffer.Length > 12;
                }
            }
            catch { }

            return false;
        }

        public static async Task TestDnsItemAsync(DnsItem item)
        {
            item.IsTesting = true;
            try
            {
                var p1Task = PingIpAsync(item.Primary);
                var p2Task = PingIpAsync(item.Secondary);
                var u1Task = TestUdp53Async(item.Primary);
                var u2Task = TestUdp53Async(item.Secondary);

                await Task.WhenAll(p1Task, p2Task, u1Task, u2Task);

                item.Ping1 = await p1Task;
                item.Ping2 = await p2Task;
                item.Udp1 = await u1Task;
                item.Udp2 = await u2Task;
            }
            finally
            {
                item.IsTesting = false;
            }
        }

        public static async Task<VerificationResult> VerifySystemAsync(string adapterName, List<DnsItem> knownServers)
        {
            var result = new VerificationResult
            {
                AdapterName = adapterName
            };

            var activeIps = GetCurrentDns(adapterName);
            if (activeIps.Count == 0)
            {
                result.ActiveDnsList = "خودکار (DHCP - پیش‌فرض مودم / ISP)";
                result.IdentifiedService = "پیش‌فرض سرویس‌دهنده اینترنت (ISP)";
            }
            else
            {
                result.ActiveDnsList = string.Join(", ", activeIps);
                var match = knownServers.FirstOrDefault(s => activeIps.Contains(s.Primary) || activeIps.Contains(s.Secondary));
                if (match != null)
                {
                    result.IdentifiedService = $"{match.Name} ({match.TypeBadgeText})";
                }
                else
                {
                    // Check standard Iranian ISP DNS
                    if (activeIps.Contains("5.200.200.200"))
                        result.IdentifiedService = "مخابرات ایران (TCI)";
                    else if (activeIps.Contains("217.218.127.127"))
                        result.IdentifiedService = "دیتاسنتر زیرساخت ایران (DCI)";
                    else
                        result.IdentifiedService = "تنظیم دستی کاربر (Custom DNS)";
                }
            }

            // Test 1: google.com
            try
            {
                var ips = await Dns.GetHostAddressesAsync("google.com");
                if (ips != null && ips.Length > 0)
                {
                    result.GoogleResolved = true;
                    result.GoogleIp = ips[0].ToString();
                }
            }
            catch { }

            // Test 2: developer.android.com (Sanctioned domain test)
            try
            {
                var ips = await Dns.GetHostAddressesAsync("developer.android.com");
                if (ips != null && ips.Length > 0)
                {
                    result.SanctionedSiteResolved = true;
                    result.SanctionedSiteIp = ips[0].ToString();
                }
            }
            catch { }

            return result;
        }

        private static readonly System.Net.Http.HttpClient _httpClient = new(new System.Net.Http.SocketsHttpHandler
        {
            AllowAutoRedirect = true,
            PooledConnectionLifetime = TimeSpan.FromMinutes(2),
            ConnectTimeout = TimeSpan.FromSeconds(3)
        })
        {
            Timeout = TimeSpan.FromSeconds(4),
            DefaultRequestHeaders = { { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) DNSChanger/2026" } }
        };

        public static List<ServiceCheckItem> GetDefaultServiceChecks()
        {
            return new List<ServiceCheckItem>
            {
                new() { Group = "AI", Name = "Antigravity (Google)", HostName = "antigravity.google", Url = "https://antigravity.google/" },
                new() { Group = "AI", Name = "ChatGPT (OpenAI)", HostName = "chatgpt.com", Url = "https://chatgpt.com/" },
                new() { Group = "AI", Name = "Claude (Anthropic)", HostName = "claude.ai", Url = "https://claude.ai/" },
                new() { Group = "AI", Name = "Gemini (Google)", HostName = "gemini.google.com", Url = "https://gemini.google.com/" },
                new() { Group = "AI", Name = "Perplexity", HostName = "www.perplexity.ai", Url = "https://www.perplexity.ai/" },

                new() { Group = "Dev", Name = "Google", HostName = "www.google.com", Url = "https://www.google.com/" },
                new() { Group = "Dev", Name = "Google Android Dev", HostName = "developer.android.com", Url = "https://developer.android.com/" },
                new() { Group = "Dev", Name = "GitHub", HostName = "github.com", Url = "https://github.com/" },

                new() { Group = "Creative", Name = "Adobe", HostName = "www.adobe.com", Url = "https://www.adobe.com/" },
                new() { Group = "Creative", Name = "Nvidia", HostName = "www.nvidia.com", Url = "https://www.nvidia.com/" },

                new() { Group = "Media", Name = "Spotify", HostName = "open.spotify.com", Url = "https://open.spotify.com/" },
                new() { Group = "Media", Name = "Spotify for Creators", HostName = "creators.spotify.com", Url = "https://creators.spotify.com/" },

                new() { Group = "Gaming", Name = "Epic Games Store", HostName = "store.epicgames.com", Url = "https://store.epicgames.com/" },
                new() { Group = "Gaming", Name = "Steam", HostName = "store.steampowered.com", Url = "https://store.steampowered.com/" },

                new() { Group = "Learning", Name = "Mimo", HostName = "mimo.org", Url = "https://mimo.org/" },
                new() { Group = "Learning", Name = "Duolingo", HostName = "www.duolingo.com", Url = "https://www.duolingo.com/" },
                new() { Group = "Learning", Name = "Coursera", HostName = "www.coursera.org", Url = "https://www.coursera.org/" },

                new() { Group = "Freelance", Name = "Upwork", HostName = "www.upwork.com", Url = "https://www.upwork.com/" },
                new() { Group = "Freelance", Name = "Fiverr", HostName = "www.fiverr.com", Url = "https://www.fiverr.com/" },
                new() { Group = "Freelance", Name = "Freelancer", HostName = "www.freelancer.com", Url = "https://www.freelancer.com/" }
            };
        }

        public static async Task TestServiceCheckAsync(ServiceCheckItem item)
        {
            item.IsChecking = true;
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var addrs = await Dns.GetHostAddressesAsync(item.HostName);
                sw.Stop();
                item.LatencyMs = sw.ElapsedMilliseconds;

                if (addrs != null && addrs.Length > 0)
                {
                    var ipv4 = addrs.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork) ?? addrs[0];
                    string ipStr = ipv4.ToString();
                    item.Ip = ipStr;

                    // Sinkhole detection (Iran ISP filtering or local sinkholes)
                    if (ipStr.StartsWith("10.10.34.") || ipStr == "0.0.0.0" || ipStr.StartsWith("127."))
                    {
                        item.Resolved = false;
                        item.Note = "Sinkhole / مسدود در شبکه";
                        return;
                    }

                    item.Resolved = true;

                    // Probe HTTP HEAD
                    try
                    {
                        using var req = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Head, item.Url);
                        using var resp = await _httpClient.SendAsync(req, System.Net.Http.HttpCompletionOption.ResponseHeadersRead);
                        item.StatusCode = (int)resp.StatusCode;
                        if ((int)resp.StatusCode < 400)
                        {
                            item.HttpOk = true;
                            item.Note = "در دسترس";
                        }
                        else if ((int)resp.StatusCode == 403 || (int)resp.StatusCode == 451)
                        {
                            item.HttpOk = false;
                            item.Note = "حل شد ولی IP تحریم است (403)";
                        }
                        else
                        {
                            item.HttpOk = false;
                            item.Note = $"کد HTTP {(int)resp.StatusCode}";
                        }
                    }
                    catch
                    {
                        item.HttpOk = false;
                        item.Note = "دامنه حل شد؛ اتصال وب ناموفق";
                    }
                }
                else
                {
                    item.Resolved = false;
                    item.Note = "عدم دریافت رکورد A از DNS";
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                item.LatencyMs = sw.ElapsedMilliseconds;
                item.Resolved = false;
                item.Note = ex.Message;
            }
            finally
            {
                item.IsChecking = false;
            }
        }
    }
}
