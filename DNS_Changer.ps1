# PowerShell UTF-8 Output Setup
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::InputEncoding  = [System.Text.Encoding]::UTF8

$Host.UI.RawUI.WindowTitle = "1-Click DNS Changer & System DNS Verifier (2026)"

$script:dnsList = @(
    # --- Iranian Anti-Sanction & Gaming DNS ---
    @{ Id = 1;  Name = "Shecan"; Primary = "178.22.122.100"; Secondary = "185.51.200.2"; Type = "Anti-Sanction"; Cat = "Websites & Launchers" },
    @{ Id = 2;  Name = "Radar Game"; Primary = "10.202.10.10"; Secondary = "10.202.10.11"; Type = "Anti-Sanction"; Cat = "Gaming / Ping (Xbox & PC)" },
    @{ Id = 3;  Name = "Electro"; Primary = "78.157.42.100"; Secondary = "78.157.42.101"; Type = "Anti-Sanction"; Cat = "Gaming / Matchmaking (Steam/PS)" },
    @{ Id = 4;  Name = "Begzar (Server 1)"; Primary = "185.55.226.26"; Secondary = "185.55.225.25"; Type = "Anti-Sanction"; Cat = "Game Downloads & PC Loading" },
    @{ Id = 5;  Name = "Begzar (Server 2)"; Primary = "185.55.224.24"; Secondary = "185.55.225.25"; Type = "Anti-Sanction"; Cat = "Game Downloads & PC Loading" },
    @{ Id = 6;  Name = "403 Online"; Primary = "10.202.10.202"; Secondary = "10.202.10.102"; Type = "Anti-Sanction"; Cat = "Developer Tools & Libraries" },
    @{ Id = 7;  Name = "Vanilla"; Primary = "10.139.177.21"; Secondary = "10.139.177.22"; Type = "Anti-Sanction"; Cat = "Downloads & Blocked Services" },
    @{ Id = 8;  Name = "HostIran (Server 1)"; Primary = "172.28.2.100"; Secondary = "179.29.0.100"; Type = "Anti-Sanction"; Cat = "Websites & Mobile Apps" },
    @{ Id = 9;  Name = "HostIran (Server 2)"; Primary = "172.29.2.100"; Secondary = "172.29.0.100"; Type = "Anti-Sanction"; Cat = "Websites & Mobile Apps" },
    @{ Id = 10; Name = "Shelter"; Primary = "94.103.125.157"; Secondary = "94.103.125.158"; Type = "Anti-Sanction"; Cat = "Online Gaming Connection" },
    @{ Id = 11; Name = "Beshkan"; Primary = "181.41.194.177"; Secondary = "181.41.194.186"; Type = "Anti-Sanction"; Cat = "Adobe, Nvidia, Unity, Intel" },

    # --- Global Public DNS ---
    @{ Id = 12; Name = "Cloudflare"; Primary = "1.1.1.1"; Secondary = "1.0.0.1"; Type = "Global"; Cat = "Fastest Speed & Web Stability" },
    @{ Id = 13; Name = "Google Main"; Primary = "8.8.8.8"; Secondary = "8.8.4.4"; Type = "Global"; Cat = "Maximum ISP Compatibility" },
    @{ Id = 14; Name = "Google / Level3"; Primary = "4.2.2.4"; Secondary = "4.2.2.2"; Type = "Global"; Cat = "Routing Stability (4.2.2.4)" },
    @{ Id = 15; Name = "OpenDNS (Cisco)"; Primary = "208.67.222.222"; Secondary = "208.67.220.220"; Type = "Global"; Cat = "Network Security & Web" },
    @{ Id = 16; Name = "Quad9"; Primary = "9.9.9.9"; Secondary = "149.112.112.112"; Type = "Global"; Cat = "Privacy & Anti-Phishing" },
    @{ Id = 17; Name = "NTT Asia"; Primary = "129.250.35.250"; Secondary = "129.250.35.251"; Type = "Global"; Cat = "Optimized Asia Routing" },
    @{ Id = 18; Name = "Level3 Main"; Primary = "209.244.0.3"; Secondary = "209.244.0.4"; Type = "Global"; Cat = "International Backbone" },
    @{ Id = 19; Name = "AdGuard Public"; Primary = "94.140.14.14"; Secondary = "94.140.15.15"; Type = "Global"; Cat = "Ad & Tracker Blocking" },
    @{ Id = 20; Name = "Control D"; Primary = "76.76.2.0"; Secondary = "76.76.10.0"; Type = "Global"; Cat = "High Speed & No-Log" },
    @{ Id = 21; Name = "Comodo Secure"; Primary = "8.26.56.26"; Secondary = "8.20.247.20"; Type = "Global"; Cat = "Malware Shield" },
    @{ Id = 22; Name = "DNS.WATCH"; Primary = "84.200.69.80"; Secondary = "84.200.70.40"; Type = "Global"; Cat = "Uncensored & Fast" },
    @{ Id = 23; Name = "Alternate DNS"; Primary = "76.76.19.19"; Secondary = "76.76.20.20"; Type = "Global"; Cat = "Ad Removal & Web" },
    @{ Id = 24; Name = "Yandex Safe"; Primary = "77.88.8.8"; Secondary = "77.88.8.1"; Type = "Global"; Cat = "European & Safe" },
    @{ Id = 25; Name = "CleanBrowsing"; Primary = "185.228.168.9"; Secondary = "185.228.169.9"; Type = "Global"; Cat = "Family Security Filter" },
    @{ Id = 26; Name = "Pishgaman ISP"; Primary = "5.202.100.100"; Secondary = "5.202.100.101"; Type = "ISP"; Cat = "Pishgaman ADSL/Fiber" },
    @{ Id = 27; Name = "Shatel ADSL"; Primary = "85.15.1.14"; Secondary = "85.15.1.15"; Type = "ISP"; Cat = "Shatel ADSL Network" }
)

function Get-ActivePhysicalAdapters {
    $physical = Get-NetAdapter | Where-Object { $_.HardwareInterface -eq $true -and $_.Status -eq 'Up' }
    if (-not $physical) {
        $physical = Get-NetAdapter | Where-Object { $_.HardwareInterface -eq $true }
    }
    return $physical
}

function Show-CurrentDns {
    $adapters = Get-ActivePhysicalAdapters
    if ($adapters) {
        foreach ($ad in $adapters) {
            $dnsAddresses = (Get-DnsClientServerAddress -InterfaceAlias $ad.Name -AddressFamily IPv4).ServerAddresses
            $dnsStr = if ($dnsAddresses) { $dnsAddresses -join ', ' } else { 'Automatic (DHCP)' }
            Write-Host "📡 Physical Adapter : "$ad.Name" ("$ad.InterfaceDescription")" -ForegroundColor Cyan
            Write-Host "📌 Applied DNS       : "$dnsStr -ForegroundColor Yellow
        }
    } else {
        Write-Host "⚠️ No physical network adapter found." -ForegroundColor Red
    }
}

function Verify-SystemDns {
    Write-Host "`n===========================================================================================================" -ForegroundColor Cyan
    Write-Host "🔍 LIVE SYSTEM DNS VERIFICATION REPORT (Windows OS Direct Inspection)" -ForegroundColor Yellow
    Write-Host "===========================================================================================================" -ForegroundColor Cyan

    $adapters = Get-ActivePhysicalAdapters
    if (-not $adapters) {
        Write-Host "❌ No physical network adapter connected to verify." -ForegroundColor Red
        return
    }

    foreach ($ad in $adapters) {
        Write-Host "📡 Adapter Name        : "$ad.Name -ForegroundColor White
        Write-Host "💻 Hardware Device     : "$ad.InterfaceDescription -ForegroundColor White

        $dnsAddrs = (Get-DnsClientServerAddress -InterfaceAlias $ad.Name -AddressFamily IPv4).ServerAddresses
        if (-not $dnsAddrs) {
            Write-Host "📌 Active System DNS   : Automatic (DHCP) from Router/ISP" -ForegroundColor Yellow
            Write-Host "🏷️ Identified Service  : Default ISP DNS" -ForegroundColor Gray
        } else {
            $joined = $dnsAddrs -join ', '
            Write-Host "📌 Active System DNS   : $joined" -ForegroundColor Green

            # Match against known DNS list
            $match = $script:dnsList | Where-Object { $_.Primary -in $dnsAddrs -or $_.Secondary -in $dnsAddrs } | Select-Object -First 1
            if ($match) {
                $typeTag = if ($match.Type -eq "Anti-Sanction") { "[Anti-Sanction / Iran Geo-Bypass]" } else { "[$($match.Type)]" }
                Write-Host "🏷️ Identified Service  : $($match.Name) $typeTag" -ForegroundColor Magenta
                Write-Host "🎯 Category / Use Case : $($match.Cat)" -ForegroundColor Cyan
            } else {
                Write-Host "🏷️ Identified Service  : Custom User Configured DNS" -ForegroundColor Gray
            }
        }
    }

    Write-Host "`n🌐 Live Domain Resolution Test (via Windows Native Resolver):" -ForegroundColor Yellow

    # Test 1: Standard domain resolution
    try {
        $g = [System.Net.Dns]::GetHostAddresses('google.com')
        if ($g) {
            $ipStr = $g[0].IPAddressToString
            Write-Host "   • google.com              : 🟢 SUCCESS (Resolved to $ipStr)" -ForegroundColor Green
        } else {
            Write-Host "   • google.com              : 🔴 FAILED (No IP returned)" -ForegroundColor Red
        }
    } catch {
        Write-Host "   • google.com              : 🔴 FAILED ($($_.Exception.Message))" -ForegroundColor Red
    }

    # Test 2: Sanctioned domain resolution test
    try {
        $a = [System.Net.Dns]::GetHostAddresses('developer.android.com')
        if ($a) {
            $ipStr = $a[0].IPAddressToString
            Write-Host "   • developer.android.com   : 🟢 SUCCESS (Sanctioned site resolved: $ipStr)" -ForegroundColor Green
        } else {
            Write-Host "   • developer.android.com   : ⚠️ UNRESOLVED" -ForegroundColor Yellow
        }
    } catch {
        Write-Host "   • developer.android.com   : ⚠️ UNRESOLVED (May require Anti-Sanction DNS)" -ForegroundColor Yellow
    }

    Write-Host "===========================================================================================================" -ForegroundColor Cyan
}

function Show-Header {
    Clear-Host
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
    Write-Host "                     ⚡ 1-CLICK DNS CHANGER & SYSTEM DNS VERIFIER (2026)                                  " -ForegroundColor Yellow
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
    Show-CurrentDns
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
    Write-Host ""
}

function Test-AllDns {
    Show-Header
    Write-Host "⏳ Parallel testing ICMP Ping & UDP Port 53 DNS query for all 27 servers..." -ForegroundColor Yellow
    Write-Host "   Please wait 2 seconds...`n" -ForegroundColor DarkGray

    $runspacePool = [runspacefactory]::CreateRunspacePool(1, 35)
    $runspacePool.Open()
    $tasks = @()

    foreach ($item in $script:dnsList) {
        $sb = {
            param($dns)
            function Ping-IP ($ip) {
                if ([string]::IsNullOrWhiteSpace($ip)) { return 9999 }
                $ping = New-Object System.Net.NetworkInformation.Ping
                $latencies = @()
                $success = 0
                for ($i = 0; $i -lt 3; $i++) {
                    try {
                        $reply = $ping.Send($ip, 700)
                        if ($reply.Status -eq 'Success') {
                            $latencies += $reply.RoundtripTime
                            $success++
                        }
                    } catch {}
                }
                if ($success -gt 0) {
                    return [math]::Round(($latencies | Measure-Object -Average).Average)
                }
                return 9999
            }

            function Test-Udp53 ($ip) {
                if ([string]::IsNullOrWhiteSpace($ip)) { return $false }
                try {
                    $client = New-Object System.Net.Sockets.UdpClient
                    $client.Client.SendTimeout = 1000
                    $client.Client.ReceiveTimeout = 1000
                    $client.Connect($ip, 53)
                    $query = [byte[]](
                        0x12, 0x34, 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x06, 0x67, 0x6f, 0x6f, 0x67, 0x6c, 0x65, 0x03, 0x63, 0x6f, 0x6d, 0x00,
                        0x00, 0x01, 0x00, 0x01
                    )
                    [void]$client.Send($query, $query.Length)
                    $ep = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
                    $resp = $client.Receive([ref]$ep)
                    $client.Close()
                    return ($resp.Length -gt 12)
                } catch {
                    return $false
                }
            }

            $p1 = Ping-IP $dns.Primary
            $p2 = Ping-IP $dns.Secondary
            $u1 = Test-Udp53 $dns.Primary
            $u2 = Test-Udp53 $dns.Secondary

            $bestPing = [math]::Min($p1, $p2)
            $isWorking = ($u1 -or $u2)

            return [PSCustomObject]@{
                Id = $dns.Id
                Name = $dns.Name
                Primary = $dns.Primary
                Ping1 = $p1
                Udp1 = $u1
                Secondary = $dns.Secondary
                Ping2 = $p2
                Udp2 = $u2
                BestPing = $bestPing
                IsWorking = $isWorking
                Type = $dns.Type
                Cat = $dns.Cat
            }
        }
        $ps = [powershell]::Create().AddScript($sb).AddArgument($item)
        $ps.RunspacePool = $runspacePool
        $tasks += [PSCustomObject]@{ Pipe = $ps; AsyncResult = $ps.BeginInvoke() }
    }

    $results = foreach ($t in $tasks) {
        $t.Pipe.EndInvoke($t.AsyncResult)
        $t.Pipe.Dispose()
    }
    $runspacePool.Close()
    $runspacePool.Dispose()

    $script:lastResults = $results | Sort-Object BestPing

    Write-Host ("{0,-4} | {1,-24} | {2,-16} | {3,-15} | {4,-10} | {5,-15} | {6,-10} | {7}" -f "#", "SERVICE NAME", "TYPE / SANCTION", "PRIMARY DNS", "PING 1", "SECONDARY DNS", "PING 2", "BEST USE / CATEGORY") -ForegroundColor Cyan
    Write-Host ("-" * 125) -ForegroundColor Gray

    foreach ($r in $script:lastResults) {
        $p1Str = if ($r.Ping1 -eq 9999) { "TimeOut" } else { "$($r.Ping1) ms" }
        $p2Str = if ($r.Ping2 -eq 9999) { "TimeOut" } else { "$($r.Ping2) ms" }

        if ($r.Udp1) { $p1Str += " [OK]" } elseif ($r.Ping1 -ne 9999) { $p1Str += " [!]" }
        if ($r.Udp2) { $p2Str += " [OK]" } elseif ($r.Ping2 -ne 9999) { $p2Str += " [!]" }

        $typeLabel = if ($r.Type -eq "Anti-Sanction") { "[Anti-Sanction]" } else { "[$($r.Type)]" }

        $color = "Red"
        if ($r.BestPing -lt 80 -and $r.IsWorking) { $color = "Green" }
        elseif ($r.BestPing -lt 160 -and $r.IsWorking) { $color = "Yellow" }
        elseif ($r.BestPing -lt 9999) { $color = "DarkYellow" }

        Write-Host ("{0,-4} | {1,-24} | {2,-16} | {3,-15} | {4,-10} | {5,-15} | {6,-10} | {7}" -f $r.Id, $r.Name, $typeLabel, $r.Primary, $p1Str, $r.Secondary, $p2Str, $r.Cat) -ForegroundColor $color
    }

    Write-Host "`n===========================================================================================================" -ForegroundColor Cyan
    Write-Host " STATUS LEGEND:" -ForegroundColor White
    Write-Host "  [Anti-Sanction] : Configured to bypass Iran sanctions & geo-restrictions (Shecan, Radar, 403, Begzar, etc.)" -ForegroundColor Magenta
    Write-Host "  Green [OK]      : Fast Latency (<80ms) & UDP Port 53 active on your ISP" -ForegroundColor Green
    Write-Host "  Yellow [OK]     : Moderate Latency (80-160ms) & Port 53 DNS active" -ForegroundColor Yellow
    Write-Host "  Warning [!]     : Responds to ICMP ping, but UDP Port 53 DNS query is blocked/hijacked by ISP" -ForegroundColor DarkYellow
    Write-Host "  Red TimeOut     : Server offline or blocked" -ForegroundColor Red
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
}

function Apply-Dns ($primary, $secondary, $name) {
    $adapters = Get-ActivePhysicalAdapters
    if (-not $adapters) {
        Write-Host "`n❌ Physical network adapter (Wi-Fi / Ethernet) not found." -ForegroundColor Red
        return
    }
    foreach ($adapter in $adapters) {
        Write-Host "`nApplying DNS: $name ($primary, $secondary) to Physical Adapter [$($adapter.Name)] ($($adapter.InterfaceDescription))..." -ForegroundColor Yellow
        try {
            Set-DnsClientServerAddress -InterfaceAlias $adapter.Name -ServerAddresses ($primary, $secondary) -ErrorAction Stop
            Write-Host "✅ SUCCESS! DNS changed to $name on $($adapter.Name)" -ForegroundColor Green
        } catch {
            Write-Host "⚠️ Error setting DNS on $($adapter.Name): $_" -ForegroundColor Red
            Write-Host "Tip: Make sure to run the script as Administrator!" -ForegroundColor Yellow
        }
    }
    ipconfig /flushdns | Out-Null
    Write-Host "✅ System DNS Cache flushed." -ForegroundColor Green
    Verify-SystemDns
}

function Reset-DnsToDhcp {
    $adapters = Get-ActivePhysicalAdapters
    if (-not $adapters) {
        Write-Host "`n❌ Physical network adapter not found." -ForegroundColor Red
        return
    }
    foreach ($adapter in $adapters) {
        Write-Host "`nResetting DNS to Automatic (DHCP) on Physical Adapter [$($adapter.Name)] ($($adapter.InterfaceDescription))..." -ForegroundColor Yellow
        try {
            Set-DnsClientServerAddress -InterfaceAlias $adapter.Name -ResetServerAddresses -ErrorAction Stop
            Write-Host "✅ SUCCESS! DNS reset to Automatic (DHCP) on $($adapter.Name)" -ForegroundColor Green
        } catch {
            Write-Host "⚠️ Error resetting DNS on $($adapter.Name): $_" -ForegroundColor Red
        }
    }
    ipconfig /flushdns | Out-Null
    Write-Host "✅ System DNS Cache flushed." -ForegroundColor Green
    Verify-SystemDns
}

function Set-FastestDns {
    if (-not $script:lastResults) {
        Write-Host "❌ Please run ping test first." -ForegroundColor Red
        return
    }
    $top = $script:lastResults | Where-Object { $_.BestPing -lt 9999 -and $_.IsWorking } | Select-Object -First 1
    if (-not $top) { $top = $script:lastResults | Where-Object { $_.BestPing -lt 9999 } | Select-Object -First 1 }
    if ($top) {
        Apply-Dns $top.Primary $top.Secondary "$($top.Name) (Fastest - $($top.BestPing)ms)"
    } else {
        Write-Host "❌ No working DNS found in test results." -ForegroundColor Red
    }
}

# --- Main Loop ---
Test-AllDns

while ($true) {
    Write-Host "`n===========================================================================================================" -ForegroundColor Cyan
    Write-Host " QUICK 1-CLICK DNS COMMANDS:" -ForegroundColor Yellow
    Write-Host "  [1-27] Type any Number to IMMEDIATELY apply that DNS to your physical Wi-Fi/Ethernet card" -ForegroundColor White
    Write-Host "  [V]    🔍 VERIFY ACTIVE SYSTEM DNS (Direct Windows OS Check & Live Domain Test)" -ForegroundColor Green
    Write-Host "  [0]    ⚡ Auto-Set Fastest DNS from Test Results" -ForegroundColor White
    Write-Host "  [R]    🔄 Reset DNS to Automatic (DHCP)" -ForegroundColor White
    Write-Host "  [F]    🧹 Flush DNS Cache (ipconfig /flushdns)" -ForegroundColor White
    Write-Host "  [T]    📊 Re-Test Pings & DNS Reachability" -ForegroundColor White
    Write-Host "  [Q]    ❌ Exit Program" -ForegroundColor White
    Write-Host "===========================================================================================================" -ForegroundColor Cyan

    $inputStr = (Read-Host "Enter Choice (1-27, V, 0, R, F, T, Q)").Trim().ToUpper()

    if ($inputStr -eq "Q") { Write-Host "`nExiting... Have a great day!" -ForegroundColor Green; break }
    elseif ($inputStr -eq "V" -or $inputStr -eq "C") { Verify-SystemDns }
    elseif ($inputStr -eq "T") { Test-AllDns }
    elseif ($inputStr -eq "F") { Write-Host "`nFlushing DNS..." -ForegroundColor Yellow; ipconfig /flushdns; Write-Host "✅ Flushed!" -ForegroundColor Green }
    elseif ($inputStr -eq "R") { Reset-DnsToDhcp }
    elseif ($inputStr -eq "0") { Set-FastestDns }
    else {
        $num = [int]0
        if ([int]::TryParse($inputStr, [ref]$num) -and $num -ge 1 -and $num -le 27) {
            $target = $script:dnsList | Where-Object { $_.Id -eq $num }
            if ($target) {
                Apply-Dns $target.Primary $target.Secondary $target.Name
            }
        } else {
            Write-Host "Invalid option. Please enter a number (1-27) or command (V, 0, R, F, T, Q)." -ForegroundColor Red
        }
    }
}