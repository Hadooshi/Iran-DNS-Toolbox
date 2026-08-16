# PowerShell UTF-8 Output Setup
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::InputEncoding  = [System.Text.Encoding]::UTF8
try { [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12 -bor [Net.SecurityProtocolType]::Tls11 -bor [Net.SecurityProtocolType]::Tls } catch {}
[Net.ServicePointManager]::DefaultConnectionLimit = 100

$Host.UI.RawUI.WindowTitle = "1-Click DNS Changer & System DNS Verifier (2026)"

# =====================================================================================================
#  DNS SERVER DATABASE
#  Doh = RFC 8484 DNS-over-HTTPS endpoint (port 443). $null when the provider has no public DoH.
#  DohOnly = $true  -> the IPs DO NOT answer on plain UDP/53; usable only via DoH/DoT.
# =====================================================================================================
$script:dnsList = @(
    # --- Iranian Anti-Sanction & Gaming DNS ---
    @{ Id = 1;  Name = "Shecan"; Primary = "178.22.122.100"; Secondary = "185.51.200.2"; Type = "Anti-Sanction"; Cat = "Websites & Launchers"; Doh = "https://free.shecan.ir/dns-query"; DohOnly = $false },
    @{ Id = 2;  Name = "Radar Game"; Primary = "10.202.10.10"; Secondary = "10.202.10.11"; Type = "Anti-Sanction"; Cat = "Gaming / Ping (Xbox & PC)"; Doh = $null; DohOnly = $false },
    @{ Id = 3;  Name = "Electro"; Primary = "78.157.42.100"; Secondary = "78.157.42.101"; Type = "Anti-Sanction"; Cat = "Gaming / Matchmaking (Steam/PS)"; Doh = $null; DohOnly = $false },
    @{ Id = 4;  Name = "Begzar (Server 1)"; Primary = "185.55.226.26"; Secondary = "185.55.225.25"; Type = "Anti-Sanction"; Cat = "Game Downloads & PC Loading"; Doh = $null; DohOnly = $false },
    @{ Id = 5;  Name = "Begzar (Server 2)"; Primary = "185.55.224.24"; Secondary = "185.55.225.25"; Type = "Anti-Sanction"; Cat = "Game Downloads & PC Loading"; Doh = $null; DohOnly = $false },
    @{ Id = 6;  Name = "403 Online"; Primary = "10.202.10.202"; Secondary = "10.202.10.102"; Type = "Anti-Sanction"; Cat = "Developer Tools & Libraries"; Doh = "https://dns.403.online/dns-query"; DohOnly = $false },
    @{ Id = 7;  Name = "Vanilla"; Primary = "10.139.177.21"; Secondary = "10.139.177.22"; Type = "Anti-Sanction"; Cat = "Downloads & Blocked Services"; Doh = $null; DohOnly = $false },
    @{ Id = 8;  Name = "HostIran (Server 1)"; Primary = "172.29.0.100"; Secondary = "172.29.2.100"; Type = "Anti-Sanction"; Cat = "Websites & Mobile Apps"; Doh = $null; DohOnly = $false },
    @{ Id = 9;  Name = "HostIran (Server 2)"; Primary = "172.28.2.100"; Secondary = "179.29.0.100"; Type = "Anti-Sanction"; Cat = "Websites & Mobile Apps (Cloud)"; Doh = $null; DohOnly = $false },
    @{ Id = 10; Name = "Shelter"; Primary = "94.103.125.157"; Secondary = "94.103.125.158"; Type = "Anti-Sanction"; Cat = "Online Gaming Connection"; Doh = $null; DohOnly = $false },
    @{ Id = 11; Name = "Beshkan"; Primary = "181.41.194.177"; Secondary = "181.41.194.186"; Type = "Anti-Sanction"; Cat = "Adobe, Nvidia, Unity, Intel"; Doh = $null; DohOnly = $false },

    # --- Global Public DNS ---
    @{ Id = 12; Name = "Cloudflare"; Primary = "1.1.1.1"; Secondary = "1.0.0.1"; Type = "Global"; Cat = "Fastest Speed & Web Stability"; Doh = "https://cloudflare-dns.com/dns-query"; DohOnly = $false },
    @{ Id = 13; Name = "Google Main"; Primary = "8.8.8.8"; Secondary = "8.8.4.4"; Type = "Global"; Cat = "Maximum ISP Compatibility"; Doh = "https://dns.google/dns-query"; DohOnly = $false },
    @{ Id = 14; Name = "Google / Level3"; Primary = "4.2.2.4"; Secondary = "4.2.2.2"; Type = "Global"; Cat = "Routing Stability (4.2.2.4)"; Doh = $null; DohOnly = $false },
    @{ Id = 15; Name = "OpenDNS (Cisco)"; Primary = "208.67.222.222"; Secondary = "208.67.220.220"; Type = "Global"; Cat = "Network Security & Web"; Doh = "https://doh.opendns.com/dns-query"; DohOnly = $false },
    @{ Id = 16; Name = "Quad9"; Primary = "9.9.9.9"; Secondary = "149.112.112.112"; Type = "Global"; Cat = "Privacy & Anti-Phishing"; Doh = "https://dns.quad9.net/dns-query"; DohOnly = $false },
    @{ Id = 17; Name = "NTT Asia"; Primary = "129.250.35.250"; Secondary = "129.250.35.251"; Type = "Global"; Cat = "Optimized Asia Routing"; Doh = $null; DohOnly = $false },
    @{ Id = 18; Name = "Level3 Main"; Primary = "209.244.0.3"; Secondary = "209.244.0.4"; Type = "Global"; Cat = "International Backbone"; Doh = $null; DohOnly = $false },
    @{ Id = 19; Name = "AdGuard Public"; Primary = "94.140.14.14"; Secondary = "94.140.15.15"; Type = "Global"; Cat = "Ad & Tracker Blocking"; Doh = "https://dns.adguard-dns.com/dns-query"; DohOnly = $false },
    @{ Id = 20; Name = "Control D"; Primary = "76.76.2.0"; Secondary = "76.76.10.0"; Type = "Global"; Cat = "High Speed & No-Log"; Doh = "https://freedns.controld.com/p0"; DohOnly = $false },
    @{ Id = 21; Name = "Comodo Secure"; Primary = "8.26.56.26"; Secondary = "8.20.247.20"; Type = "Global"; Cat = "Malware Shield"; Doh = $null; DohOnly = $false },
    @{ Id = 22; Name = "DNS.WATCH"; Primary = "84.200.69.80"; Secondary = "84.200.70.40"; Type = "Global"; Cat = "Uncensored & Fast"; Doh = $null; DohOnly = $false },
    @{ Id = 23; Name = "Alternate DNS"; Primary = "76.76.19.19"; Secondary = "76.76.20.20"; Type = "Global"; Cat = "Ad Removal & Web"; Doh = $null; DohOnly = $false },
    @{ Id = 24; Name = "Yandex Basic"; Primary = "77.88.8.8"; Secondary = "77.88.8.1"; Type = "Global"; Cat = "European & Unfiltered"; Doh = $null; DohOnly = $false },
    @{ Id = 25; Name = "CleanBrowsing Security"; Primary = "185.228.168.9"; Secondary = "185.228.169.9"; Type = "Global"; Cat = "Malware & Phishing Filter"; Doh = "https://doh.cleanbrowsing.org/doh/security-filter/"; DohOnly = $false },
    @{ Id = 26; Name = "Pishgaman ISP"; Primary = "5.202.100.100"; Secondary = "5.202.100.101"; Type = "ISP"; Cat = "Pishgaman ADSL/Fiber"; Doh = $null; DohOnly = $false },
    @{ Id = 27; Name = "Shatel ADSL"; Primary = "85.15.1.14"; Secondary = "85.15.1.15"; Type = "ISP"; Cat = "Shatel ADSL Network"; Doh = $null; DohOnly = $false },

    # --- NEW (2026): Iranian additions ---
    @{ Id = 28; Name = "Pars Online"; Primary = "91.99.101.12"; Secondary = ""; Type = "Anti-Sanction"; Cat = "Free ISP Anti-Sanction Resolver"; Doh = $null; DohOnly = $false },
    @{ Id = 29; Name = "AbrBaran IDC"; Primary = "172.16.1.100"; Secondary = "172.16.2.100"; Type = "Anti-Sanction"; Cat = "Anti-Sanction (Datacenter Clients)"; Doh = $null; DohOnly = $false },
    @{ Id = 30; Name = "AsiaTech ISP"; Primary = "194.36.174.161"; Secondary = "178.22.122.100"; Type = "ISP"; Cat = "AsiaTech Subscribers / Low Ping"; Doh = $null; DohOnly = $false },

    # --- NEW (2026): Privacy & anti-censorship ---
    @{ Id = 31; Name = "Mullvad (DoH only)"; Primary = "194.242.2.2"; Secondary = ""; Type = "Privacy"; Cat = "No-Log Privacy - DoH/DoT ONLY"; Doh = "https://dns.mullvad.net/dns-query"; DohOnly = $true },
    @{ Id = 32; Name = "UncensoredDNS"; Primary = "91.239.100.100"; Secondary = "89.233.43.71"; Type = "Privacy"; Cat = "Uncensored Open Web (Denmark)"; Doh = "https://anycast.uncensoreddns.org/dns-query"; DohOnly = $false },
    @{ Id = 33; Name = "LibreDNS"; Primary = "116.202.176.26"; Secondary = ""; Type = "Privacy"; Cat = "Privacy, No-Log (Germany)"; Doh = "https://doh.libredns.gr/dns-query"; DohOnly = $false },
    @{ Id = 34; Name = "DNS.SB"; Primary = "185.222.222.222"; Secondary = "45.11.45.11"; Type = "Privacy"; Cat = "Anti-Censorship & DNSSEC"; Doh = "https://doh.dns.sb/dns-query"; DohOnly = $false },
    @{ Id = 35; Name = "AdGuard Non-filtering"; Primary = "94.140.14.140"; Secondary = "94.140.14.141"; Type = "Privacy"; Cat = "AdGuard Unfiltered (Pure Speed)"; Doh = "https://unfiltered.adguard-dns.com/dns-query"; DohOnly = $false },

    # --- NEW (2026): Security & family ---
    @{ Id = 36; Name = "Cloudflare Malware"; Primary = "1.1.1.2"; Secondary = "1.0.0.2"; Type = "Security"; Cat = "Anti-Malware Blocking"; Doh = "https://security.cloudflare-dns.com/dns-query"; DohOnly = $false },
    @{ Id = 37; Name = "Cloudflare Family"; Primary = "1.1.1.3"; Secondary = "1.0.0.3"; Type = "Family"; Cat = "Adult Content + Malware Block"; Doh = "https://family.cloudflare-dns.com/dns-query"; DohOnly = $false },
    @{ Id = 38; Name = "OpenDNS FamilyShield"; Primary = "208.67.222.123"; Secondary = "208.67.220.123"; Type = "Family"; Cat = "Cisco Family Protection"; Doh = "https://doh.familyshield.opendns.com/dns-query"; DohOnly = $false },
    @{ Id = 39; Name = "AdGuard Family"; Primary = "94.140.14.15"; Secondary = "94.140.15.16"; Type = "Family"; Cat = "Family Filter + Ad Blocking"; Doh = "https://family.adguard-dns.com/dns-query"; DohOnly = $false },
    @{ Id = 40; Name = "Quad9 No-Filter"; Primary = "9.9.9.10"; Secondary = "149.112.112.10"; Type = "Privacy"; Cat = "Quad9 Without Blocking (Speed)"; Doh = "https://dns10.quad9.net/dns-query"; DohOnly = $false },
    @{ Id = 41; Name = "Yandex Safe"; Primary = "77.88.8.88"; Secondary = "77.88.8.2"; Type = "Security"; Cat = "Anti-Phishing & Malware"; Doh = $null; DohOnly = $false },
    @{ Id = 42; Name = "Yandex Family"; Primary = "77.88.8.7"; Secondary = "77.88.8.3"; Type = "Family"; Cat = "Yandex Family Filter"; Doh = $null; DohOnly = $false },
    @{ Id = 43; Name = "CleanBrowsing Family"; Primary = "185.228.168.168"; Secondary = "185.228.169.168"; Type = "Family"; Cat = "Full Family Protection"; Doh = "https://doh.cleanbrowsing.org/doh/family-filter/"; DohOnly = $false },

    # --- NEW (2026): Gaming & regional routing ---
    @{ Id = 44; Name = "KT Korea"; Primary = "168.126.63.1"; Secondary = "168.126.63.2"; Type = "Gaming"; Cat = "Korea/Asia Game Server Ping"; Doh = $null; DohOnly = $false },
    @{ Id = 45; Name = "Hurricane Electric"; Primary = "74.82.42.42"; Secondary = ""; Type = "Global"; Cat = "Stable US Routing"; Doh = $null; DohOnly = $false },
    @{ Id = 46; Name = "Verisign / UltraDNS"; Primary = "64.6.64.6"; Secondary = "64.6.65.6"; Type = "Global"; Cat = "Max Stability, No-Log"; Doh = $null; DohOnly = $false },
    @{ Id = 47; Name = "Neustar UltraDNS"; Primary = "156.154.70.1"; Secondary = "156.154.71.1"; Type = "Global"; Cat = "Enterprise Infrastructure"; Doh = $null; DohOnly = $false }
)

# NOTE: Measure-Object -Property cannot read hashtable keys on Windows PowerShell 5.1,
# so the Id values are projected with ForEach-Object first.
$script:maxId = ($script:dnsList | ForEach-Object { [int]$_.Id } | Sort-Object | Select-Object -Last 1)
if (-not $script:maxId) { $script:maxId = $script:dnsList.Count }

# =====================================================================================================
#  SANCTIONED / RESTRICTED SERVICE CHECK LIST
#  Each entry is resolved through the *currently applied* system DNS, then probed over HTTPS.
# =====================================================================================================
$script:serviceChecks = @(
    @{ Group = "AI";        Name = "Antigravity (Google)"; HostName = "antigravity.google";          Url = "https://antigravity.google/" },
    @{ Group = "AI";        Name = "ChatGPT (OpenAI)";     HostName = "chatgpt.com";                 Url = "https://chatgpt.com/" },
    @{ Group = "AI";        Name = "Claude (Anthropic)";   HostName = "claude.ai";                   Url = "https://claude.ai/" },
    @{ Group = "AI";        Name = "Gemini (Google)";      HostName = "gemini.google.com";           Url = "https://gemini.google.com/" },
    @{ Group = "AI";        Name = "Perplexity";           HostName = "www.perplexity.ai";           Url = "https://www.perplexity.ai/" },
    @{ Group = "Dev";       Name = "Google";               HostName = "www.google.com";              Url = "https://www.google.com/" },
    @{ Group = "Dev";       Name = "Google Android Dev";   HostName = "developer.android.com";       Url = "https://developer.android.com/" },
    @{ Group = "Dev";       Name = "GitHub";               HostName = "github.com";                  Url = "https://github.com/" },
    @{ Group = "Creative";  Name = "Adobe";                HostName = "www.adobe.com";               Url = "https://www.adobe.com/" },
    @{ Group = "Creative";  Name = "Nvidia";               HostName = "www.nvidia.com";              Url = "https://www.nvidia.com/" },
    @{ Group = "Media";     Name = "Spotify";              HostName = "open.spotify.com";            Url = "https://open.spotify.com/" },
    @{ Group = "Media";     Name = "Spotify for Creators"; HostName = "creators.spotify.com";        Url = "https://creators.spotify.com/" },
    @{ Group = "Gaming";    Name = "Epic Games Store";     HostName = "store.epicgames.com";         Url = "https://store.epicgames.com/" },
    @{ Group = "Gaming";    Name = "Steam";                HostName = "store.steampowered.com";      Url = "https://store.steampowered.com/" },
    @{ Group = "Learning";  Name = "Mimo";                 HostName = "mimo.org";                    Url = "https://mimo.org/" },
    @{ Group = "Learning";  Name = "Duolingo";             HostName = "www.duolingo.com";            Url = "https://www.duolingo.com/" },
    @{ Group = "Learning";  Name = "Coursera";             HostName = "www.coursera.org";            Url = "https://www.coursera.org/" },
    @{ Group = "Freelance"; Name = "Upwork";               HostName = "www.upwork.com";              Url = "https://www.upwork.com/" },
    @{ Group = "Freelance"; Name = "Fiverr";               HostName = "www.fiverr.com";              Url = "https://www.fiverr.com/" },
    @{ Group = "Freelance"; Name = "Freelancer";           HostName = "www.freelancer.com";          Url = "https://www.freelancer.com/" }
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
            Write-Host ("Physical Adapter : {0} ({1})" -f $ad.Name, $ad.InterfaceDescription) -ForegroundColor Cyan
            Write-Host ("Applied DNS      : {0}" -f $dnsStr) -ForegroundColor Yellow
        }
    } else {
        Write-Host "No physical network adapter found." -ForegroundColor Red
    }
}

# --- Resolve a hostname through the OS resolver and report timing -----------------------------------
function Resolve-HostQuick {
    param([string]$Name, [int]$TimeoutMs = 4000)

    $result = [PSCustomObject]@{ Ok = $false; Ip = ""; Error = ""; Ms = 0 }
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    try {
        $task = [System.Net.Dns]::GetHostAddressesAsync($Name)
        if ($task.Wait($TimeoutMs)) {
            $addrs = $task.Result | Where-Object { $_.AddressFamily -eq 'InterNetwork' }
            if (-not $addrs) { $addrs = $task.Result }
            if ($addrs -and $addrs.Count -gt 0) {
                $result.Ok = $true
                $result.Ip = $addrs[0].IPAddressToString
            } else {
                $result.Error = "No A record"
            }
        } else {
            $result.Error = "DNS timeout"
        }
    } catch {
        $inner = $_.Exception
        while ($inner.InnerException) { $inner = $inner.InnerException }
        $result.Error = $inner.Message
    }
    $sw.Stop()
    $result.Ms = [int]$sw.ElapsedMilliseconds
    return $result
}

# --- Full service reachability check (DNS resolve + HTTPS handshake) --------------------------------
function Test-ServiceAccess {
    param($Check, [int]$TimeoutMs = 6000)

    $out = [PSCustomObject]@{
        Group = $Check.Group; Name = $Check.Name; HostName = $Check.HostName
        Resolved = $false; Ip = ""; Http = $false; Code = ""; Note = ""; Ms = 0
    }

    $dns = Resolve-HostQuick -Name $Check.HostName -TimeoutMs $TimeoutMs
    $out.Ms = $dns.Ms
    if (-not $dns.Ok) {
        $out.Note = $dns.Error
        return $out
    }
    $out.Resolved = $true
    $out.Ip = $dns.Ip

    # Sinkhole detection: many ISPs / filtered resolvers answer with a local or blackhole IP.
    if ($dns.Ip -match '^(0\.0\.0\.0|127\.|10\.10\.34\.)') {
        $out.Note = "Sinkhole/blocked IP"
        return $out
    }

    try {
        $req = [System.Net.HttpWebRequest]::Create($Check.Url)
        $req.Method = "HEAD"
        $req.Timeout = $TimeoutMs
        $req.ReadWriteTimeout = $TimeoutMs
        $req.AllowAutoRedirect = $true
        $req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) DNSChanger/2026"
        $resp = $req.GetResponse()
        $out.Code = [int]$resp.StatusCode
        $out.Http = $true
        $resp.Close()
    } catch [System.Net.WebException] {
        $we = $_.Exception
        if ($we.Response) {
            $code = [int]$we.Response.StatusCode
            $out.Code = $code
            # 403 / 451 = reachable but geo-blocked by the service itself (sanction at IP level).
            if ($code -eq 403 -or $code -eq 451) {
                $out.Note = "Geo-blocked (HTTP $code) - needs VPN, not DNS"
            } elseif ($code -eq 405 -or $code -eq 400) {
                # Some CDNs reject HEAD but the host is clearly reachable.
                $out.Http = $true
                $out.Note = "Reachable (HEAD not allowed)"
            } else {
                $out.Note = "HTTP $code"
            }
        } else {
            $out.Note = $we.Status.ToString()
        }
    } catch {
        $out.Note = $_.Exception.Message
    }
    return $out
}

function Test-SanctionedServices {
    Write-Host ""
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
    Write-Host " SANCTIONED / RESTRICTED SERVICE ACCESS TEST (through your currently applied DNS)" -ForegroundColor Yellow
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
    Write-Host (" Checking {0} services in parallel, please wait..." -f $script:serviceChecks.Count) -ForegroundColor DarkGray
    Write-Host ""

    # Fresh cache so the test reflects the DNS that is applied right now.
    ipconfig /flushdns | Out-Null

    $pool = [runspacefactory]::CreateRunspacePool(1, 20)
    $pool.Open()
    $jobs = @()

    $worker = {
        param($check, $fnResolve, $fnTest)
        # Each runspace starts with default TLS settings; without this, PowerShell 5.1
        # negotiates TLS 1.0 and every modern HTTPS host would be reported as failed.
        try {
            [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12 -bor [Net.SecurityProtocolType]::Tls11 -bor [Net.SecurityProtocolType]::Tls
        } catch {}
        . ([scriptblock]::Create($fnResolve))
        . ([scriptblock]::Create($fnTest))
        return (Test-ServiceAccess -Check $check)
    }

    $srcResolve = "function Resolve-HostQuick { " + (Get-Command Resolve-HostQuick).Definition + " }"
    $srcTest    = "function Test-ServiceAccess { " + (Get-Command Test-ServiceAccess).Definition + " }"

    foreach ($c in $script:serviceChecks) {
        $ps = [powershell]::Create().AddScript($worker).AddArgument($c).AddArgument($srcResolve).AddArgument($srcTest)
        $ps.RunspacePool = $pool
        $jobs += [PSCustomObject]@{ Pipe = $ps; Async = $ps.BeginInvoke() }
    }

    $results = @()
    foreach ($j in $jobs) {
        try { $results += $j.Pipe.EndInvoke($j.Async) } catch {}
        $j.Pipe.Dispose()
    }
    $pool.Close(); $pool.Dispose()

    Write-Host ("{0,-11} | {1,-24} | {2,-30} | {3,-9} | {4}" -f "CATEGORY", "SERVICE", "HOSTNAME", "VERDICT", "DETAILS") -ForegroundColor Cyan
    Write-Host ("-" * 118) -ForegroundColor Gray

    $pass = 0; $fail = 0; $warn = 0
    foreach ($g in @("AI","Dev","Creative","Media","Gaming","Learning","Freelance")) {
        foreach ($r in ($results | Where-Object { $_.Group -eq $g })) {
            if ($r.Resolved -and $r.Http) {
                $verdict = "PASS"; $color = "Green"; $pass++
                $detail = "IP $($r.Ip)  $($r.Ms)ms"
                if ($r.Note) { $detail += "  ($($r.Note))" }
            } elseif ($r.Resolved) {
                $verdict = "PARTIAL"; $color = "Yellow"; $warn++
                $detail = "DNS ok ($($r.Ip)) but HTTPS failed: $($r.Note)"
            } else {
                $verdict = "FAIL"; $color = "Red"; $fail++
                $detail = "DNS could not resolve: $($r.Note)"
            }
            Write-Host ("{0,-11} | {1,-24} | {2,-30} | {3,-9} | {4}" -f $r.Group, $r.Name, $r.HostName, $verdict, $detail) -ForegroundColor $color
        }
    }

    Write-Host ("-" * 118) -ForegroundColor Gray
    Write-Host (" SUMMARY: {0} PASS  |  {1} PARTIAL  |  {2} FAIL   (out of {3})" -f $pass, $warn, $fail, $results.Count) -ForegroundColor White
    Write-Host " PASS    = DNS resolved AND the HTTPS endpoint answered -> the sanction is bypassed." -ForegroundColor Green
    Write-Host " PARTIAL = DNS resolved but the service refused the connection (usually IP-level geo-block: use a VPN)." -ForegroundColor Yellow
    Write-Host " FAIL    = The current DNS cannot resolve the domain at all -> pick another Anti-Sanction DNS." -ForegroundColor Red
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
}

function Verify-SystemDns {
    param([switch]$SkipServices)

    Write-Host ""
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
    Write-Host " LIVE SYSTEM DNS VERIFICATION REPORT (Windows OS Direct Inspection)" -ForegroundColor Yellow
    Write-Host "===========================================================================================================" -ForegroundColor Cyan

    $adapters = Get-ActivePhysicalAdapters
    if (-not $adapters) {
        Write-Host "No physical network adapter connected to verify." -ForegroundColor Red
        return
    }

    foreach ($ad in $adapters) {
        Write-Host ("Adapter Name        : {0}" -f $ad.Name) -ForegroundColor White
        Write-Host ("Hardware Device     : {0}" -f $ad.InterfaceDescription) -ForegroundColor White

        $dnsAddrs = (Get-DnsClientServerAddress -InterfaceAlias $ad.Name -AddressFamily IPv4).ServerAddresses
        if (-not $dnsAddrs) {
            Write-Host "Active System DNS   : Automatic (DHCP) from Router/ISP" -ForegroundColor Yellow
            Write-Host "Identified Service  : Default ISP DNS" -ForegroundColor Gray
        } else {
            $joined = $dnsAddrs -join ', '
            Write-Host ("Active System DNS   : {0}" -f $joined) -ForegroundColor Green

            $match = $script:dnsList | Where-Object { $_.Primary -in $dnsAddrs -or ($_.Secondary -and $_.Secondary -in $dnsAddrs) } | Select-Object -First 1
            if ($match) {
                $typeTag = if ($match.Type -eq "Anti-Sanction") { "[Anti-Sanction / Iran Geo-Bypass]" } else { "[$($match.Type)]" }
                Write-Host ("Identified Service  : {0} {1}" -f $match.Name, $typeTag) -ForegroundColor Magenta
                Write-Host ("Category / Use Case : {0}" -f $match.Cat) -ForegroundColor Cyan
                if ($match.Doh) {
                    Write-Host ("DoH (port 443)      : {0}" -f $match.Doh) -ForegroundColor Cyan
                } else {
                    Write-Host  "DoH (port 443)      : Not published by this provider" -ForegroundColor DarkGray
                }
            } else {
                Write-Host "Identified Service  : Custom User Configured DNS" -ForegroundColor Gray
            }
        }
    }

    if (-not $SkipServices) { Test-SanctionedServices }
}

function Show-Header {
    Clear-Host
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
    Write-Host "                     1-CLICK DNS CHANGER & SYSTEM DNS VERIFIER (2026)                                      " -ForegroundColor Yellow
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
    Show-CurrentDns
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
    Write-Host ""
}

function Test-AllDns {
    Show-Header
    Write-Host ("Parallel testing ICMP Ping, UDP Port 53 and DoH (port 443) for all {0} servers..." -f $script:dnsList.Count) -ForegroundColor Yellow
    Write-Host "   Please wait a few seconds..." -ForegroundColor DarkGray
    Write-Host ""

    $runspacePool = [runspacefactory]::CreateRunspacePool(1, 40)
    $runspacePool.Open()
    $tasks = @()

    foreach ($item in $script:dnsList) {
        $sb = {
            param($dns)
            # Runspaces do not inherit TLS settings; required for the DoH (HTTPS) probe below.
            try {
                [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12 -bor [Net.SecurityProtocolType]::Tls11 -bor [Net.SecurityProtocolType]::Tls
            } catch {}

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
                    $client.Client.SendTimeout = 1200
                    $client.Client.ReceiveTimeout = 1200
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

            # Live RFC 8484 DoH query for example.com over HTTPS/443.
            function Test-DohEndpoint ($url) {
                if ([string]::IsNullOrWhiteSpace($url)) { return "N/A" }
                try {
                    $wire = [byte[]](
                        0x00, 0x00, 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x07, 0x65, 0x78, 0x61, 0x6d, 0x70, 0x6c, 0x65, 0x03, 0x63, 0x6f, 0x6d, 0x00,
                        0x00, 0x01, 0x00, 0x01
                    )
                    $b64 = [Convert]::ToBase64String($wire).TrimEnd('=').Replace('+','-').Replace('/','_')
                    $sep = if ($url.Contains('?')) { '&' } else { '?' }
                    $req = [System.Net.HttpWebRequest]::Create("$url$sep" + "dns=$b64")
                    $req.Method = "GET"
                    $req.Accept = "application/dns-message"
                    $req.Timeout = 4000
                    $req.ReadWriteTimeout = 4000
                    $req.UserAgent = "DNSChanger/2026"
                    $resp = $req.GetResponse()
                    $ok = ([int]$resp.StatusCode -eq 200)
                    $resp.Close()
                    if ($ok) { return "OK" } else { return "BAD" }
                } catch {
                    return "BLOCKED"
                }
            }

            $p1 = Ping-IP $dns.Primary
            $p2 = Ping-IP $dns.Secondary
            $u1 = Test-Udp53 $dns.Primary
            $u2 = Test-Udp53 $dns.Secondary
            $doh = Test-DohEndpoint $dns.Doh

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
                Doh = $dns.Doh
                DohOnly = $dns.DohOnly
                DohStatus = $doh
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

    $fmt = "{0,-4} | {1,-24} | {2,-16} | {3,-16} | {4,-12} | {5,-16} | {6,-12} | {7,-8} | {8}"
    Write-Host ($fmt -f "#", "SERVICE NAME", "TYPE", "PRIMARY DNS", "PING 1", "SECONDARY DNS", "PING 2", "DoH:443", "BEST USE / CATEGORY") -ForegroundColor Cyan
    Write-Host ("-" * 155) -ForegroundColor Gray

    foreach ($r in $script:lastResults) {
        $p1Str = if ($r.Ping1 -eq 9999) { "TimeOut" } else { "$($r.Ping1) ms" }
        $p2Str = if ([string]::IsNullOrWhiteSpace($r.Secondary)) { "-" } elseif ($r.Ping2 -eq 9999) { "TimeOut" } else { "$($r.Ping2) ms" }

        if ($r.Udp1) { $p1Str += " [OK]" } elseif ($r.Ping1 -ne 9999) { $p1Str += " [!]" }
        if ($r.Udp2) { $p2Str += " [OK]" } elseif ($r.Ping2 -ne 9999 -and $r.Secondary) { $p2Str += " [!]" }

        $secStr = if ([string]::IsNullOrWhiteSpace($r.Secondary)) { "-" } else { $r.Secondary }

        # DoH column
        switch ($r.DohStatus) {
            "OK"      { $dohStr = "YES [OK]" }
            "BLOCKED" { $dohStr = "YES [X]" }
            "BAD"     { $dohStr = "YES [?]" }
            default   { $dohStr = "-" }
        }

        $typeLabel = if ($r.Type -eq "Anti-Sanction") { "[Anti-Sanction]" } else { "[$($r.Type)]" }

        $color = "Red"
        if ($r.DohOnly) {
            $color = if ($r.DohStatus -eq "OK") { "Cyan" } else { "DarkYellow" }
        }
        elseif ($r.BestPing -lt 80 -and $r.IsWorking) { $color = "Green" }
        elseif ($r.BestPing -lt 160 -and $r.IsWorking) { $color = "Yellow" }
        elseif ($r.BestPing -lt 9999) { $color = "DarkYellow" }

        $catStr = $r.Cat
        if ($r.DohOnly) { $catStr = "$catStr  <-- NOT usable as plain Windows DNS" }

        Write-Host ($fmt -f $r.Id, $r.Name, $typeLabel, $r.Primary, $p1Str, $secStr, $p2Str, $dohStr, $catStr) -ForegroundColor $color
    }

    $dohCount = ($script:lastResults | Where-Object { $_.DohStatus -eq "OK" }).Count
    Write-Host ""
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
    Write-Host " STATUS LEGEND:" -ForegroundColor White
    Write-Host "  [Anti-Sanction] : Bypasses Iran sanctions & geo-restrictions (Shecan, Radar, 403, Begzar, Pars Online...)" -ForegroundColor Magenta
    Write-Host "  Green [OK]      : Fast latency (<80ms) & UDP port 53 active on your ISP" -ForegroundColor Green
    Write-Host "  Yellow [OK]     : Moderate latency (80-160ms) & port 53 DNS active" -ForegroundColor Yellow
    Write-Host "  Warning [!]     : Answers ICMP ping, but UDP port 53 is blocked/hijacked by your ISP" -ForegroundColor DarkYellow
    Write-Host "  Red TimeOut     : Server offline or blocked" -ForegroundColor Red
    Write-Host "  Cyan            : DoH-only provider (Mullvad) - works over HTTPS/443, NOT as a plain Windows DNS" -ForegroundColor Cyan
    Write-Host ("  DoH:443 column  : YES [OK] = encrypted DNS-over-HTTPS verified live ({0} of {1} servers)" -f $dohCount, $script:lastResults.Count) -ForegroundColor Cyan
    Write-Host "                    YES [X] = provider has DoH but port 443 endpoint is unreachable from your network" -ForegroundColor DarkGray
    Write-Host "                    -       = provider publishes no public DoH endpoint" -ForegroundColor DarkGray
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
}

function Show-DohList {
    Write-Host ""
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
    Write-Host " DNS-over-HTTPS (DoH) ENDPOINTS - port 443, works even when your ISP hijacks UDP 53" -ForegroundColor Yellow
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
    Write-Host ("{0,-4} | {1,-24} | {2}" -f "#", "SERVICE NAME", "DoH ENDPOINT (RFC 8484)") -ForegroundColor Cyan
    Write-Host ("-" * 100) -ForegroundColor Gray
    foreach ($d in ($script:dnsList | Where-Object { $_.Doh })) {
        Write-Host ("{0,-4} | {1,-24} | {2}" -f $d.Id, $d.Name, $d.Doh) -ForegroundColor Green
    }
    Write-Host ("-" * 100) -ForegroundColor Gray
    Write-Host " Windows 11: Settings > Network & Internet > Wi-Fi/Ethernet > DNS server assignment > Manual >" -ForegroundColor White
    Write-Host "             set the IPv4 DNS, then set 'DNS over HTTPS' to 'On (manual template)' and paste the URL." -ForegroundColor White
    Write-Host " Firefox   : Settings > Privacy & Security > DNS over HTTPS > Max Protection > Custom." -ForegroundColor White
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
}

function Apply-Dns ($primary, $secondary, $name) {
    $adapters = Get-ActivePhysicalAdapters
    if (-not $adapters) {
        Write-Host "`nPhysical network adapter (Wi-Fi / Ethernet) not found." -ForegroundColor Red
        return
    }
    $servers = @($primary)
    if (-not [string]::IsNullOrWhiteSpace($secondary)) { $servers += $secondary }

    foreach ($adapter in $adapters) {
        Write-Host ("`nApplying DNS: {0} ({1}) to Physical Adapter [{2}] ({3})..." -f $name, ($servers -join ', '), $adapter.Name, $adapter.InterfaceDescription) -ForegroundColor Yellow
        try {
            Set-DnsClientServerAddress -InterfaceAlias $adapter.Name -ServerAddresses $servers -ErrorAction Stop
            Write-Host ("SUCCESS! DNS changed to {0} on {1}" -f $name, $adapter.Name) -ForegroundColor Green
        } catch {
            Write-Host ("Error setting DNS on {0}: {1}" -f $adapter.Name, $_) -ForegroundColor Red
            Write-Host "Tip: Make sure to run the script as Administrator!" -ForegroundColor Yellow
        }
    }
    ipconfig /flushdns | Out-Null
    Write-Host "System DNS cache flushed." -ForegroundColor Green
    Verify-SystemDns
}

function Reset-DnsToDhcp {
    $adapters = Get-ActivePhysicalAdapters
    if (-not $adapters) {
        Write-Host "`nPhysical network adapter not found." -ForegroundColor Red
        return
    }
    foreach ($adapter in $adapters) {
        Write-Host ("`nResetting DNS to Automatic (DHCP) on [{0}] ({1})..." -f $adapter.Name, $adapter.InterfaceDescription) -ForegroundColor Yellow
        try {
            Set-DnsClientServerAddress -InterfaceAlias $adapter.Name -ResetServerAddresses -ErrorAction Stop
            Write-Host ("SUCCESS! DNS reset to Automatic (DHCP) on {0}" -f $adapter.Name) -ForegroundColor Green
        } catch {
            Write-Host ("Error resetting DNS on {0}: {1}" -f $adapter.Name, $_) -ForegroundColor Red
        }
    }
    ipconfig /flushdns | Out-Null
    Write-Host "System DNS cache flushed." -ForegroundColor Green
    Verify-SystemDns
}

function Set-FastestDns {
    if (-not $script:lastResults) {
        Write-Host "Please run the ping test first." -ForegroundColor Red
        return
    }
    $candidates = $script:lastResults | Where-Object { -not $_.DohOnly }
    $top = $candidates | Where-Object { $_.BestPing -lt 9999 -and $_.IsWorking } | Select-Object -First 1
    if (-not $top) { $top = $candidates | Where-Object { $_.BestPing -lt 9999 } | Select-Object -First 1 }
    if ($top) {
        Apply-Dns $top.Primary $top.Secondary ("{0} (Fastest - {1}ms)" -f $top.Name, $top.BestPing)
    } else {
        Write-Host "No working DNS found in test results." -ForegroundColor Red
    }
}

# --- Main Loop ---
Test-AllDns

while ($true) {
    Write-Host ""
    Write-Host "===========================================================================================================" -ForegroundColor Cyan
    Write-Host " QUICK 1-CLICK DNS COMMANDS:" -ForegroundColor Yellow
    Write-Host ("  [1-{0}] Type any number from 1 to {0} ({1} DNS servers available) to IMMEDIATELY apply it" -f $script:maxId, $script:dnsList.Count) -ForegroundColor White
    Write-Host "         to your physical Wi-Fi/Ethernet card" -ForegroundColor White
    Write-Host "  [V]    VERIFY ACTIVE SYSTEM DNS + test 20 sanctioned services (ChatGPT, Claude, Steam, GitHub...)" -ForegroundColor Green
    Write-Host "  [S]    Run ONLY the sanctioned-service access test (no adapter re-inspection)" -ForegroundColor Green
    Write-Host "  [D]    Show all DNS-over-HTTPS (DoH / port 443) endpoints" -ForegroundColor White
    Write-Host "  [0]    Auto-set fastest DNS from test results" -ForegroundColor White
    Write-Host "  [R]    Reset DNS to Automatic (DHCP)" -ForegroundColor White
    Write-Host "  [F]    Flush DNS cache (ipconfig /flushdns)" -ForegroundColor White
    Write-Host "  [T]    Re-test pings, port 53 and DoH reachability" -ForegroundColor White
    Write-Host "  [Q]    Exit program" -ForegroundColor White
    Write-Host "===========================================================================================================" -ForegroundColor Cyan

    $inputStr = (Read-Host ("Enter Choice (1-{0}, V, S, D, 0, R, F, T, Q)" -f $script:maxId)).Trim().ToUpper()

    if ($inputStr -eq "Q") { Write-Host "`nExiting... Have a great day!" -ForegroundColor Green; break }
    elseif ($inputStr -eq "V" -or $inputStr -eq "C") { Verify-SystemDns }
    elseif ($inputStr -eq "S") { Test-SanctionedServices }
    elseif ($inputStr -eq "D") { Show-DohList }
    elseif ($inputStr -eq "T") { Test-AllDns }
    elseif ($inputStr -eq "F") { Write-Host "`nFlushing DNS..." -ForegroundColor Yellow; ipconfig /flushdns; Write-Host "Flushed!" -ForegroundColor Green }
    elseif ($inputStr -eq "R") { Reset-DnsToDhcp }
    elseif ($inputStr -eq "0") { Set-FastestDns }
    else {
        $num = [int]0
        if ([int]::TryParse($inputStr, [ref]$num) -and $num -ge 1 -and $num -le $script:maxId) {
            $target = $script:dnsList | Where-Object { $_.Id -eq $num }
            if ($target) {
                if ($target.DohOnly) {
                    Write-Host ""
                    Write-Host ("WARNING: {0} does NOT answer plain DNS on UDP/53." -f $target.Name) -ForegroundColor Yellow
                    Write-Host ("         Use its DoH template instead: {0}" -f $target.Doh) -ForegroundColor Yellow
                    $go = (Read-Host "Apply the IP anyway? (y/N)").Trim().ToUpper()
                    if ($go -ne "Y") { continue }
                }
                Apply-Dns $target.Primary $target.Secondary $target.Name
            }
        } else {
            Write-Host ("Invalid option. Enter a number (1-{0}) or a command (V, S, D, 0, R, F, T, Q)." -f $script:maxId) -ForegroundColor Red
        }
    }
}
