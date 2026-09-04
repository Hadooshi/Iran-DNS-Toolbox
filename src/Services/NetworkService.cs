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

    public class CustomDnsRecord
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Primary { get; set; } = string.Empty;
        public string Secondary { get; set; } = string.Empty;
        public string Category { get; set; } = "تنظیم دستی کاربر";
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
            bool success = false;
            try
            {
                string script = string.IsNullOrWhiteSpace(secondary)
                    ? $"Set-DnsClientServerAddress -InterfaceAlias '{adapterName}' -ServerAddresses ('{primary}')"
                    : $"Set-DnsClientServerAddress -InterfaceAlias '{adapterName}' -ServerAddresses ('{primary}', '{secondary}')";

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
                success = (proc?.ExitCode == 0);
            }
            catch { }

            // Robust fallback to netsh if PowerShell method didn't succeed
            if (!success)
            {
                try
                {
                    var psi1 = new ProcessStartInfo
                    {
                        FileName = "netsh",
                        Arguments = $"interface ip set dns name=\"{adapterName}\" static {primary} validate=no",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using var p1 = Process.Start(psi1);
                    p1?.WaitForExit(4000);
                    success = (p1?.ExitCode == 0);

                    if (!string.IsNullOrWhiteSpace(secondary))
                    {
                        var psi2 = new ProcessStartInfo
                        {
                            FileName = "netsh",
                            Arguments = $"interface ip add dns name=\"{adapterName}\" {secondary} index=2 validate=no",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        using var p2 = Process.Start(psi2);
                        p2?.WaitForExit(4000);
                    }
                }
                catch { }
            }

            FlushDns();
            return success;
        }

        public static bool ResetDnsToDhcp(string adapterName)
        {
            bool success = false;
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
                success = (proc?.ExitCode == 0);
            }
            catch { }

            if (!success)
            {
                try
                {
                    var psiNetsh = new ProcessStartInfo
                    {
                        FileName = "netsh",
                        Arguments = $"interface ip set dns name=\"{adapterName}\" dhcp",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using var procNetsh = Process.Start(psiNetsh);
                    procNetsh?.WaitForExit(4000);
                    success = (procNetsh?.ExitCode == 0);
                }
                catch { }
            }

            FlushDns();
            return success;
        }

        public static async Task<(bool Success, string Log)> ExecuteEmergencyNetworkResetAsync()
        {
            var sb = new System.Text.StringBuilder();
            bool overallSuccess = true;

            string[] commands = new[]
            {
                "ipconfig /flushdns",
                "ipconfig /release",
                "ipconfig /renew",
                "netsh winsock reset",
                "netsh int ip reset"
            };

            await Task.Run(() =>
            {
                foreach (var cmd in commands)
                {
                    try
                    {
                        var parts = cmd.Split(' ', 2);
                        string exe = parts[0];
                        string args = parts.Length > 1 ? parts[1] : "";

                        var psi = new ProcessStartInfo
                        {
                            FileName = exe,
                            Arguments = args,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        using var proc = Process.Start(psi);
                        if (proc != null)
                        {
                            string output = proc.StandardOutput.ReadToEnd();
                            string err = proc.StandardError.ReadToEnd();
                            proc.WaitForExit(10000);

                            sb.AppendLine($"[CMD] {cmd}");
                            if (!string.IsNullOrWhiteSpace(output))
                                sb.AppendLine(output.Trim());
                            if (!string.IsNullOrWhiteSpace(err))
                                sb.AppendLine("[ERR] " + err.Trim());

                            if (proc.ExitCode != 0 && !cmd.Contains("/release") && !cmd.Contains("/renew"))
                            {
                                overallSuccess = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendLine($"[EXCEPTION] {cmd}: {ex.Message}");
                        overallSuccess = false;
                    }
                }
            });

            return (overallSuccess, sb.ToString());
        }

        public static string GetCustomDnsFilePath()
        {
            try
            {
                string localDir = AppDomain.CurrentDomain.BaseDirectory;
                string localPath = Path.Combine(localDir, "custom_dns.json");
                // Test write permissions
                string testFile = Path.Combine(localDir, $".write_test_{Guid.NewGuid():N}.tmp");
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);
                return localPath;
            }
            catch
            {
                string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IranDnsToolbox");
                if (!Directory.Exists(appData))
                {
                    Directory.CreateDirectory(appData);
                }
                return Path.Combine(appData, "custom_dns.json");
            }
        }

        public static List<DnsItem> LoadCustomDnsItems()
        {
            var list = new List<DnsItem>();
            try
            {
                string path = GetCustomDnsFilePath();
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    var items = JsonSerializer.Deserialize<List<CustomDnsRecord>>(json);
                    if (items != null)
                    {
                        foreach (var record in items)
                        {
                            list.Add(new DnsItem
                            {
                                Id = record.Id,
                                Name = record.Name,
                                Primary = record.Primary,
                                Secondary = record.Secondary,
                                Type = "Custom",
                                Category = string.IsNullOrWhiteSpace(record.Category) ? "تنظیم دستی کاربر" : record.Category,
                                IsCustom = true
                            });
                        }
                    }
                }
            }
            catch { }
            return list;
        }

        public static void SaveCustomDnsItems(List<DnsItem> items)
        {
            try
            {
                string path = GetCustomDnsFilePath();
                var records = items.Where(i => i.IsCustom).Select(i => new CustomDnsRecord
                {
                    Id = i.Id,
                    Name = i.Name,
                    Primary = i.Primary,
                    Secondary = i.Secondary,
                    Category = i.Category
                }).ToList();

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(records, options);
                File.WriteAllText(path, json);
            }
            catch { }
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
            PooledConnectionLifetime = TimeSpan.FromSeconds(5), // Short lifetime so DNS changes take effect immediately without caching sockets
            PooledConnectionIdleTimeout = TimeSpan.FromSeconds(2),
            ConnectTimeout = TimeSpan.FromSeconds(4)
        })
        {
            Timeout = TimeSpan.FromSeconds(5),
            DefaultRequestHeaders =
            {
                { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36" }
            }
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
                if (addrs == null || addrs.Length == 0)
                {
                    sw.Stop();
                    item.LatencyMs = sw.ElapsedMilliseconds;
                    item.Resolved = false;
                    item.Note = "عدم دریافت رکورد A از DNS";
                    return;
                }

                var ipv4 = addrs.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork) ?? addrs[0];
                string ipStr = ipv4.ToString();
                item.Ip = ipStr;

                // Sinkhole detection (Iran ISP filtering or local sinkholes)
                if (ipStr.StartsWith("10.10.34.") || ipStr == "0.0.0.0" || ipStr.StartsWith("127."))
                {
                    sw.Stop();
                    item.LatencyMs = sw.ElapsedMilliseconds;
                    item.Resolved = false;
                    item.Note = "Sinkhole / مسدود در شبکه";
                    return;
                }

                item.Resolved = true;

                // Check for direct Google IP on Gemini:
                // Anti-sanction DNS servers (Shecan, 403, Electro, etc.) always return their own reverse proxy IP
                // (e.g. 94.130.*, 10.202.*, 78.157.*, 185.55.*).
                // If it resolves to direct Google IP (142.251.*, 172.217.*, 216.58.*, 216.239.*),
                // Gemini in Iran gives 403 Forbidden in browser!
                bool isGeminiDirectGoogle = item.HostName.Equals("gemini.google.com", StringComparison.OrdinalIgnoreCase) &&
                    (ipStr.StartsWith("142.251.") || ipStr.StartsWith("172.217.") || ipStr.StartsWith("216.58.") || ipStr.StartsWith("216.239."));

                // Probe HTTP GET
                try
                {
                    using var req = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, item.Url);
                    req.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    req.Headers.Add("Sec-Fetch-Dest", "document");
                    req.Headers.Add("Sec-Fetch-Mode", "navigate");

                    using var resp = await _httpClient.SendAsync(req, System.Net.Http.HttpCompletionOption.ResponseHeadersRead);
                    sw.Stop();
                    item.LatencyMs = sw.ElapsedMilliseconds;
                    item.StatusCode = (int)resp.StatusCode;

                    if (isGeminiDirectGoogle)
                    {
                        item.StatusCode = 403;
                        item.HttpOk = false;
                        item.Note = "سرویس تحریم است (IP مستقیم گوگل 403)";
                        return;
                    }

                    if ((int)resp.StatusCode == 403 || (int)resp.StatusCode == 451)
                    {
                        item.HttpOk = false;
                        item.Note = "حل شد ولی IP تحریم است (403)";
                    }
                    else if ((int)resp.StatusCode < 400)
                    {
                        // Check if Google/Cloudflare returned a 200 header but an error 403 page
                        bool is403Page = false;
                        try
                        {
                            using var stream = await resp.Content.ReadAsStreamAsync();
                            byte[] buffer = new byte[1024];
                            int read = await stream.ReadAsync(buffer, 0, buffer.Length);
                            if (read > 0)
                            {
                                string text = System.Text.Encoding.UTF8.GetString(buffer, 0, read);
                                if (text.Contains("403. That's an error") || text.Contains("does not have permission"))
                                {
                                    is403Page = true;
                                }
                            }
                        }
                        catch { }

                        if (is403Page)
                        {
                            item.StatusCode = 403;
                            item.HttpOk = false;
                            item.Note = "سرویس تحریم است (خطای 403)";
                        }
                        else
                        {
                            item.HttpOk = true;
                            item.Note = "در دسترس";
                        }
                    }
                    else
                    {
                        item.HttpOk = false;
                        item.Note = $"کد HTTP {(int)resp.StatusCode}";
                    }
                }
                catch
                {
                    sw.Stop();
                    item.LatencyMs = sw.ElapsedMilliseconds;
                    item.HttpOk = false;
                    item.Note = "دامنه حل شد؛ اتصال وب ناموفق";
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
