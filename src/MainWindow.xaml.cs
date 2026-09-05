using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using DNSChangerApp.Models;
using DNSChangerApp.Services;

namespace DNSChangerApp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _preferredColumns = 4;
        private int _gridColumns = 4;
        public int GridColumns
        {
            get => _gridColumns;
            set
            {
                if (_gridColumns != value)
                {
                    _gridColumns = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            IntPtr handle = new WindowInteropHelper(this).Handle;
            HwndSource? source = HwndSource.FromHwnd(handle);
            source?.AddHook(WindowProc);
        }

        private const int WM_GETMINMAXINFO = 0x0024;
        private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y) { X = x; Y = y; }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, [In, Out] MONITORINFO lpmi);

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_GETMINMAXINFO)
            {
                WmGetMinMaxInfo(hwnd, lParam);
                handled = true;
            }
            return IntPtr.Zero;
        }

        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO))!;
            IntPtr hMonitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (hMonitor != IntPtr.Zero)
            {
                MONITORINFO mi = new MONITORINFO();
                if (GetMonitorInfo(hMonitor, mi))
                {
                    mmi.ptMaxPosition.X = Math.Abs(mi.rcWork.Left - mi.rcMonitor.Left);
                    mmi.ptMaxPosition.Y = Math.Abs(mi.rcWork.Top - mi.rcMonitor.Top);
                    mmi.ptMaxSize.X = Math.Abs(mi.rcWork.Right - mi.rcWork.Left);
                    mmi.ptMaxSize.Y = Math.Abs(mi.rcWork.Bottom - mi.rcWork.Top);
                }
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        private readonly List<DnsItem> _masterDnsList = new();
        private readonly ObservableCollection<DnsItem> _displayedDnsList = new();

        private readonly List<ServiceCheckItem> _verifyServicesMaster = new();
        private readonly ObservableCollection<ServiceCheckItem> _verifyServicesDisplayed = new();

        private List<NetworkAdapterInfo> _adapters = new();
        private NetworkAdapterInfo? _selectedAdapter = null;
        private string _activeFilter = "All";
        private string _activeSort = "Default";
        private string _activeVerifyFilter = "All";
        private bool _isTestingAll = false;
        private bool _isVerifyingServices = false;

        public MainWindow()
        {
            InitializeComponent();
            DnsCardsList.ItemsSource = _displayedDnsList;
            DnsTableList.ItemsSource = _displayedDnsList;
            ListVerifyServices.ItemsSource = _verifyServicesDisplayed;
            InitializeDnsList();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetColumns(_preferredColumns);
            await RefreshAdaptersAsync();
            await RunTestAllPingsAsync();
        }

        private void InitializeDnsList()
        {
            _masterDnsList.Clear();

            // 1. Iranian Anti-Sanction & Gaming DNS (14 Servers)
            _masterDnsList.Add(new DnsItem { Id = 1, Name = "شکن", Primary = "178.22.122.100", Secondary = "185.51.200.2", Type = "Anti-Sanction", Category = "وبسایت‌ها، استیم و لانچرهای بازی" });
            _masterDnsList.Add(new DnsItem
            {
                Id = 50,
                Name = "شکن حرفه‌ای",
                Primary = "178.22.122.101",
                Secondary = "185.51.200.1",
                Type = "Anti-Sanction",
                Category = "سرویس ویژه شکن با پایداری بالاتر",
                HasNotice = true,
                NoticeText = "این سرور نیازمند اشتراک فعال شکن حرفه‌ای است. با هر بار تغییر اینترنت یا خاموش و روشن شدن مودم، باید نشانی آی‌پی جدید خود را در پنل کاربری سایت شکن ثبت و اعلام نمایید."
            });
            _masterDnsList.Add(new DnsItem { Id = 2, Name = "رادار گیم", Primary = "10.202.10.10", Secondary = "10.202.10.11", Type = "Anti-Sanction", Category = "گیمینگ و کاهش پینگ ویژه کنسول و بازی" });
            _masterDnsList.Add(new DnsItem { Id = 3, Name = "الکترو", Primary = "78.157.42.100", Secondary = "78.157.42.101", Type = "Anti-Sanction", Category = "مچمیکینگ استیم، پلی‌استیشن و آنلاین" });
            _masterDnsList.Add(new DnsItem { Id = 4, Name = "بگذر سرور ۱", Primary = "185.55.226.26", Secondary = "185.55.225.25", Type = "Anti-Sanction", Category = "دانلود بازی و لودینگ وب PC" });
            _masterDnsList.Add(new DnsItem { Id = 5, Name = "بگذر سرور ۲", Primary = "185.55.224.24", Secondary = "185.55.225.25", Type = "Anti-Sanction", Category = "دانلود بازی و لودینگ وب PC" });
            _masterDnsList.Add(new DnsItem { Id = 6, Name = "۴۰۳ آنلاین", Primary = "10.202.10.202", Secondary = "10.202.10.102", Type = "Anti-Sanction", Category = "ابزارهای برنامه‌نویسی و کتابخانه‌ها" });
            _masterDnsList.Add(new DnsItem { Id = 7, Name = "وانیلا", Primary = "10.139.177.21", Secondary = "10.139.177.22", Type = "Anti-Sanction", Category = "هوش مصنوعی، دانلود و بازی‌ها" });
            _masterDnsList.Add(new DnsItem { Id = 8, Name = "هاست ایران ۱", Primary = "172.29.0.100", Secondary = "172.29.2.100", Type = "Anti-Sanction", Category = "وبسایت‌ها و اپلیکیشن‌های خارجی" });
            _masterDnsList.Add(new DnsItem { Id = 9, Name = "هاست ایران ۲", Primary = "172.28.2.100", Secondary = "179.29.0.100", Type = "Anti-Sanction", Category = "وبسایت‌ها و اپلیکیشن‌های ابری" });
            _masterDnsList.Add(new DnsItem { Id = 10, Name = "شلتر", Primary = "94.103.125.157", Secondary = "94.103.125.158", Type = "Anti-Sanction", Category = "پل ارتباطی بازی‌های آنلاین و پینگ" });
            _masterDnsList.Add(new DnsItem { Id = 11, Name = "بشکن", Primary = "181.41.194.177", Secondary = "181.41.194.186", Type = "Anti-Sanction", Category = "رفع تحریم Adobe, Nvidia, Unity, Intel" });
            _masterDnsList.Add(new DnsItem { Id = 28, Name = "پارس آنلاین", Primary = "91.99.101.12", Secondary = "", Type = "Anti-Sanction", Category = "ریزالور ضد تحریم پارس آنلاین" });
            _masterDnsList.Add(new DnsItem { Id = 29, Name = "ابر باران", Primary = "172.16.1.100", Secondary = "172.16.2.100", Type = "Anti-Sanction", Category = "ضد تحریم کلاینت‌های دیتاسنتر" });

            // 2. Global Public Backbone DNS (18 Servers)
            _masterDnsList.Add(new DnsItem { Id = 12, Name = "Cloudflare", Primary = "1.1.1.1", Secondary = "1.0.0.1", Type = "Global", Category = "سریع‌ترین پاسخگویی و پایداری وب" });
            _masterDnsList.Add(new DnsItem { Id = 13, Name = "Google Public", Primary = "8.8.8.8", Secondary = "8.8.4.4", Type = "Global", Category = "حداکثر سازگاری با انواع ISP و وب" });
            _masterDnsList.Add(new DnsItem { Id = 14, Name = "Google / Level3", Primary = "4.2.2.4", Secondary = "4.2.2.2", Type = "Global", Category = "پایداری فوق‌العاده در مسیریابی شبکه" });
            _masterDnsList.Add(new DnsItem { Id = 15, Name = "OpenDNS Cisco", Primary = "208.67.222.222", Secondary = "208.67.220.220", Type = "Global", Category = "امنیت سایبری و پالایش شبکه سیسکو" });
            _masterDnsList.Add(new DnsItem { Id = 16, Name = "Quad9", Primary = "9.9.9.9", Secondary = "149.112.112.112", Type = "Global", Category = "حفظ حریم خصوصی و ضد فیشینگ" });
            _masterDnsList.Add(new DnsItem { Id = 17, Name = "NTT Asia", Primary = "129.250.35.250", Secondary = "129.250.35.251", Type = "Global", Category = "مسیریابی بهینه سرورهای قاره آسیا" });
            _masterDnsList.Add(new DnsItem { Id = 18, Name = "Level3 Main", Primary = "209.244.0.3", Secondary = "209.244.0.4", Type = "Global", Category = "ستون فقرات زیرساخت بین‌المللی" });
            _masterDnsList.Add(new DnsItem { Id = 19, Name = "AdGuard Public", Primary = "94.140.14.14", Secondary = "94.140.15.15", Type = "Global", Category = "مسدودسازی تبلیغات و ترکرها" });
            _masterDnsList.Add(new DnsItem { Id = 20, Name = "Control D", Primary = "76.76.2.0", Secondary = "76.76.10.0", Type = "Global", Category = "سرعت بالا بدون ثبت هیچ‌گونه لاگ" });
            _masterDnsList.Add(new DnsItem { Id = 21, Name = "Comodo Secure", Primary = "8.26.56.26", Secondary = "8.20.247.20", Type = "Global", Category = "سپر محافظ در برابر بدافزارها" });
            _masterDnsList.Add(new DnsItem { Id = 22, Name = "DNS.WATCH", Primary = "84.200.69.80", Secondary = "84.200.70.40", Type = "Global", Category = "بدون سانسور، سریع و بدون فیلتر" });
            _masterDnsList.Add(new DnsItem { Id = 23, Name = "Alternate DNS", Primary = "76.76.19.19", Secondary = "76.76.20.20", Type = "Global", Category = "حذف تبلیغات مزاحم و تسریع وب" });
            _masterDnsList.Add(new DnsItem { Id = 24, Name = "Yandex Basic", Primary = "77.88.8.8", Secondary = "77.88.8.1", Type = "Global", Category = "سرورهای پایه و پایدار یاندکس" });
            _masterDnsList.Add(new DnsItem { Id = 25, Name = "CleanBrowsing Security", Primary = "185.228.168.9", Secondary = "185.228.169.9", Type = "Global", Category = "فیلتر امنیتی ضد مخرب و فیشینگ" });
            _masterDnsList.Add(new DnsItem { Id = 44, Name = "KT Korea", Primary = "168.126.63.1", Secondary = "168.126.63.2", Type = "Global", Category = "پینگ بهینه سرورهای بازی شرق آسیا" });
            _masterDnsList.Add(new DnsItem { Id = 45, Name = "Hurricane Electric", Primary = "74.82.42.42", Secondary = "", Type = "Global", Category = "پایداری فوق‌العاده در مسیریابی آمریکا" });
            _masterDnsList.Add(new DnsItem { Id = 46, Name = "Verisign / UltraDNS", Primary = "64.6.64.6", Secondary = "64.6.65.6", Type = "Global", Category = "حداکثر پایداری و بدون ثبت لاگ" });
            _masterDnsList.Add(new DnsItem { Id = 47, Name = "Neustar UltraDNS", Primary = "156.154.70.1", Secondary = "156.154.71.1", Type = "Global", Category = "زیرساخت پایدار سازمانی آمریکا" });

            // 3. Privacy & Anti-Censorship DNS (6 Servers)
            _masterDnsList.Add(new DnsItem { Id = 31, Name = "مولواد", Primary = "194.242.2.2", Secondary = "", Type = "Privacy", Category = "بدون لاگ و حداکثر حریم خصوصی" });
            _masterDnsList.Add(new DnsItem { Id = 32, Name = "UncensoredDNS", Primary = "91.239.100.100", Secondary = "89.233.43.71", Type = "Privacy", Category = "اینترنت آزاد و بدون سانسور دانمارک" });
            _masterDnsList.Add(new DnsItem { Id = 33, Name = "LibreDNS", Primary = "116.202.176.26", Secondary = "", Type = "Privacy", Category = "حریم خصوصی و امنیت بدون لاگ آلمان" });
            _masterDnsList.Add(new DnsItem { Id = 34, Name = "DNS.SB", Primary = "185.222.222.222", Secondary = "45.11.45.11", Type = "Privacy", Category = "ضد سانسور با پشتیبانی از DNSSEC" });
            _masterDnsList.Add(new DnsItem { Id = 35, Name = "AdGuard Non-filtering", Primary = "94.140.14.140", Secondary = "94.140.14.141", Type = "Privacy", Category = "ادگارد بدون فیلتر با حداکثر سرعت" });
            _masterDnsList.Add(new DnsItem { Id = 40, Name = "Quad9 No-Filter", Primary = "9.9.9.10", Secondary = "149.112.112.10", Type = "Privacy", Category = "سرور پرسرعت Quad9 بدون فیلترینگ" });

            // 4. Family & Malware Protection DNS (8 Servers)
            _masterDnsList.Add(new DnsItem { Id = 36, Name = "Cloudflare Malware", Primary = "1.1.1.2", Secondary = "1.0.0.2", Type = "Family", Category = "مسدودسازی بدافزار و سایت‌های مخرب" });
            _masterDnsList.Add(new DnsItem { Id = 37, Name = "Cloudflare Family", Primary = "1.1.1.3", Secondary = "1.0.0.3", Type = "Family", Category = "فیلتر محتوای نامناسب و بدافزار" });
            _masterDnsList.Add(new DnsItem { Id = 38, Name = "OpenDNS FamilyShield", Primary = "208.67.222.123", Secondary = "208.67.220.123", Type = "Family", Category = "محافظت شبکه خانواده سیسکو" });
            _masterDnsList.Add(new DnsItem { Id = 39, Name = "AdGuard Family", Primary = "94.140.14.15", Secondary = "94.140.15.16", Type = "Family", Category = "فیلتر خانوادگی ادگارد به همراه حذف تبلیغات" });
            _masterDnsList.Add(new DnsItem { Id = 41, Name = "Yandex Safe", Primary = "77.88.8.88", Secondary = "77.88.8.2", Type = "Family", Category = "فیلتر ضد فیشینگ و بدافزار یاندکس" });
            _masterDnsList.Add(new DnsItem { Id = 42, Name = "Yandex Family", Primary = "77.88.8.7", Secondary = "77.88.8.3", Type = "Family", Category = "فیلتر محافظت خانواده یاندکس" });
            _masterDnsList.Add(new DnsItem { Id = 43, Name = "CleanBrowsing Family", Primary = "185.228.168.168", Secondary = "185.228.169.168", Type = "Family", Category = "بالاترین سطح محافظت خانواده" });

            // 5. Domestic ISP (3 Servers)
            _masterDnsList.Add(new DnsItem { Id = 26, Name = "پیشگامان", Primary = "5.202.100.100", Secondary = "5.202.100.101", Type = "ISP", Category = "شبکه پیشگامان ADSL و فیبرنوری" });
            _masterDnsList.Add(new DnsItem { Id = 27, Name = "شاتل", Primary = "85.15.1.14", Secondary = "85.15.1.15", Type = "ISP", Category = "شبکه ADSL و اینترنت شاتل" });
            _masterDnsList.Add(new DnsItem { Id = 30, Name = "آسیاتک", Primary = "194.36.174.161", Secondary = "178.22.122.100", Type = "ISP", Category = "مشترکین آسیاتک و پینگ پایین" });

            // 6. User Custom DNS (Persisted from custom_dns.json)
            var savedCustoms = NetworkService.LoadCustomDnsItems();
            foreach (var ci in savedCustoms)
            {
                _masterDnsList.Add(ci);
            }

            UpdateTabCounters();
            ApplyFilterAndSearch();
        }

        private void UpdateTabCounters()
        {
            if (TabAll == null) return;
            UpdateTabHeader(TabAll, "همه سرورها", _masterDnsList.Count);
            UpdateTabHeader(TabAntiSanction, "رفع تحریم", _masterDnsList.Count(d => d.Type == "Anti-Sanction"));
            UpdateTabHeader(TabGlobal, "عمومی جهانی", _masterDnsList.Count(d => d.Type == "Global"));
            UpdateTabHeader(TabPrivacy, "حریم خصوصی", _masterDnsList.Count(d => d.Type == "Privacy"));
            UpdateTabHeader(TabFamily, "خانواده و امنیت", _masterDnsList.Count(d => d.Type == "Family"));
            UpdateTabHeader(TabIsp, "داخلی ISP", _masterDnsList.Count(d => d.Type == "ISP"));
            UpdateTabHeader(TabCustom, "سفارشی کاربر", _masterDnsList.Count(d => d.IsCustom));
        }

        private static void UpdateTabHeader(RadioButton tab, string title, int count)
        {
            var sp = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center };
            sp.Children.Add(new TextBlock { Text = title, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 6, 0) });
            var badge = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#292929")),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(6, 1, 6, 1),
                VerticalAlignment = VerticalAlignment.Center
            };
            badge.Child = new TextBlock
            {
                Text = count.ToString(),
                FontSize = 10.5,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAAAAA")),
                FontFamily = new FontFamily("Segoe UI, Consolas")
            };
            sp.Children.Add(badge);
            tab.Content = sp;
        }

        private async Task RefreshAdaptersAsync()
        {
            ShowToast("در حال اسکن و شناسایی کارت‌های شبکه فیزیکی...");
            await Task.Run(() =>
            {
                _adapters = NetworkService.GetPhysicalAdapters();
            });

            CmbAdapters.ItemsSource = null;
            CmbAdapters.ItemsSource = _adapters;

            if (_adapters.Count > 0)
            {
                CmbAdapters.SelectedIndex = 0;
                _selectedAdapter = _adapters[0];
            }
            else
            {
                ShowToast("کارت شبکه فیزیکی فعال شناسایی نشد.", true);
            }
        }

        private async void CmbAdapters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbAdapters.SelectedItem is NetworkAdapterInfo ad)
            {
                _selectedAdapter = ad;
                await CheckActiveDnsOnSelectedAdapterAsync();
            }
        }

        private async Task CheckActiveDnsOnSelectedAdapterAsync()
        {
            if (_selectedAdapter == null) return;

            string adapterName = _selectedAdapter.Name;
            List<string> currentIps = new();

            await Task.Run(() =>
            {
                currentIps = NetworkService.GetCurrentDns(adapterName);
            });

            // Update UI
            if (currentIps.Count == 0)
            {
                TxtActiveDns.Text = "تنظیم خودکار (DHCP — مودم / روتر)";
                TxtActiveBadge.Text = "حالت DHCP";
                BadgeActiveDns.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2A2A2A"));
                BadgeActiveDns.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#404040"));
                TxtActiveBadge.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCCCCC"));
                DotActiveStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8A8A8A"));

                foreach (var item in _masterDnsList)
                {
                    item.IsActive = false;
                }
            }
            else
            {
                string joined = string.Join(", ", currentIps);
                TxtActiveDns.Text = joined;
                DotActiveStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#107C41"));

                // Match against known DNS
                var matched = _masterDnsList.FirstOrDefault(d => currentIps.Contains(d.Primary) || currentIps.Contains(d.Secondary));
                if (matched != null)
                {
                    TxtActiveBadge.Text = matched.Name;
                    BadgeActiveDns.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3326"));
                    BadgeActiveDns.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E543C"));
                    TxtActiveBadge.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#34D399"));

                    foreach (var item in _masterDnsList)
                    {
                        item.IsActive = (item.Id == matched.Id);
                    }
                }
                else
                {
                    if (currentIps.Contains("5.200.200.200"))
                    {
                        TxtActiveBadge.Text = "مخابرات ایران";
                    }
                    else
                    {
                        TxtActiveBadge.Text = "سفارشی کاربر";
                    }
                    BadgeActiveDns.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1C293A"));
                    BadgeActiveDns.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E4766"));
                    TxtActiveBadge.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#93C5FD"));

                    foreach (var item in _masterDnsList)
                    {
                        item.IsActive = false;
                    }
                }
            }
        }

        private async Task RunTestAllPingsAsync()
        {
            if (_isTestingAll) return;
            _isTestingAll = true;
            ShowToast("در حال سنجش موازی تاخیر پینگ و عملکرد پورت ۵۳ سرورها...");

            var tasks = _masterDnsList.Select(dns => NetworkService.TestDnsItemAsync(dns)).ToList();
            await Task.WhenAll(tasks);

            _isTestingAll = false;
            ShowToast($"سنجش سرعت و وضعیت تمام {_masterDnsList.Count} سرور با موفقیت پایان یافت.");
            ApplyFilterAndSearch();
        }

        private static string GetSearchAliases(int id) => id switch
        {
            1 => "shecan",
            50 => "shecan pro",
            2 => "radar game xbox pc",
            3 => "electro",
            4 => "begzar 1",
            5 => "begzar 2",
            6 => "403 online",
            7 => "vanilla",
            8 => "hostiran 1",
            9 => "hostiran 2",
            10 => "shelter",
            11 => "beshkan",
            28 => "pars online",
            29 => "abrbaran",
            26 => "pishgaman",
            27 => "shatel",
            30 => "asiatech",
            31 => "mullvad",
            _ => string.Empty
        };

        private void ApplyFilterAndSearch()
        {
            string query = (TxtSearch?.Text ?? "").Trim().ToLower();

            var filtered = _masterDnsList.Where(item =>
            {
                bool matchesTab = _activeFilter switch
                {
                    "AntiSanction" => item.Type == "Anti-Sanction",
                    "Global" => item.Type == "Global",
                    "Privacy" => item.Type == "Privacy",
                    "Family" => item.Type == "Family",
                    "ISP" => item.Type == "ISP",
                    "Custom" => item.IsCustom,
                    _ => true
                };

                if (!matchesTab) return false;

                if (string.IsNullOrEmpty(query)) return true;

                return item.Name.ToLower().Contains(query) ||
                       item.Category.ToLower().Contains(query) ||
                       item.Primary.Contains(query) ||
                       item.Secondary.Contains(query) ||
                       item.Type.ToLower().Contains(query) ||
                       GetSearchAliases(item.Id).Contains(query);
            });

            IEnumerable<DnsItem> sorted = _activeSort switch
            {
                "PingAsc" => filtered.OrderBy(d => (d.BestPing <= 0 || d.BestPing >= 9999) ? 999999 : d.BestPing).ThenBy(d => d.Id),
                "PingDesc" => filtered.OrderByDescending(d => d.BestPing >= 9999 ? -1 : d.BestPing).ThenBy(d => d.Id),
                "NameAsc" => filtered.OrderBy(d => d.Name, StringComparer.CurrentCultureIgnoreCase),
                "Port53" => filtered.OrderByDescending(d => d.IsPort53Working).ThenBy(d => (d.BestPing <= 0 || d.BestPing >= 9999) ? 999999 : d.BestPing).ThenBy(d => d.Id),
                _ => filtered.OrderBy(d => d.Id)
            };

            _displayedDnsList.Clear();
            foreach (var item in sorted)
            {
                _displayedDnsList.Add(item);
            }
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbSort?.SelectedItem is ComboBoxItem item && item.Tag is string sortKey)
            {
                _activeSort = sortKey;
                ApplyFilterAndSearch();
            }
        }

        private void FilterTab_Checked(object sender, RoutedEventArgs e)
        {
            if (TabAll == null) return;

            if (TabAll.IsChecked == true) _activeFilter = "All";
            else if (TabAntiSanction.IsChecked == true) _activeFilter = "AntiSanction";
            else if (TabGlobal.IsChecked == true) _activeFilter = "Global";
            else if (TabPrivacy.IsChecked == true) _activeFilter = "Privacy";
            else if (TabFamily.IsChecked == true) _activeFilter = "Family";
            else if (TabIsp.IsChecked == true) _activeFilter = "ISP";
            else if (TabCustom.IsChecked == true) _activeFilter = "Custom";

            ApplyFilterAndSearch();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilterAndSearch();
        }

        private async void BtnApplyDns_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is DnsItem item)
            {
                await ApplySelectedDnsAsync(item);
            }
        }

        private async Task ApplySelectedDnsAsync(DnsItem item)
        {
            if (_selectedAdapter == null)
            {
                ShowToast("لطفاً ابتدا کارت شبکه مورد نظر را انتخاب کنید.", true);
                return;
            }

            ShowToast($"در حال اعمال DNS روی {_selectedAdapter.Name}: {item.Name}...");

            bool success = false;
            await Task.Run(() =>
            {
                success = NetworkService.ApplyDns(_selectedAdapter.Name, item.Primary, item.Secondary);
            });

            if (success)
            {
                ShowToast($"دی‌ان‌اس سیستم با موفقیت به {item.Name} تغییر یافت و حافظه کش پاک شد.");
                await CheckActiveDnsOnSelectedAdapterAsync();
            }
            else
            {
                ShowToast("خطا در تغییر دی‌ان‌اس. لطفاً اطمینان حاصل کنید برنامه با دسترسی Administrator اجرا شده است.", true);
            }
        }

        private async void BtnAutoFastest_Click(object sender, RoutedEventArgs e)
        {
            var working = _masterDnsList.Where(d => d.BestPing < 9999 && d.IsPort53Working).OrderBy(d => d.BestPing).FirstOrDefault();
            if (working == null)
            {
                working = _masterDnsList.Where(d => d.BestPing < 9999).OrderBy(d => d.BestPing).FirstOrDefault();
            }

            if (working != null)
            {
                ShowToast($"انتخاب سریع‌ترین سرور: {working.Name} با تاخیر {working.BestPing}ms");
                await ApplySelectedDnsAsync(working);
            }
            else
            {
                ShowToast("هیچ سرور فعالی با پینگ موفق شناسایی نشد. لطفاً مجدداً تست را اجرا کنید.", true);
            }
        }

        private async void BtnResetDhcp_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAdapter == null)
            {
                ShowToast("لطفاً ابتدا کارت شبکه مورد نظر را انتخاب کنید.", true);
                return;
            }

            ShowToast($"در حال بازگردانی تنظیمات به حالت خودکار روی {_selectedAdapter.Name}...");

            bool success = false;
            await Task.Run(() =>
            {
                success = NetworkService.ResetDnsToDhcp(_selectedAdapter.Name);
            });

            if (success)
            {
                ShowToast("تنظیمات DNS کارت شبکه با موفقیت به حالت خودکار (DHCP) بازگردانی شد.");
                await CheckActiveDnsOnSelectedAdapterAsync();
            }
            else
            {
                ShowToast("خطا در بازگردانی تنظیمات شبکه.", true);
            }
        }

        private void BtnFlushDns_Click(object sender, RoutedEventArgs e)
        {
            NetworkService.FlushDns();
            ShowToast("حافظه موقت DNS ویندوز با موفقیت پاک‌سازی شد (ipconfig /flushdns).");
        }

        private async void BtnEmergencyReset_Click(object sender, RoutedEventArgs e)
        {
            var confirm = MessageBox.Show(
                "این عملیات پشته کامل شبکه ویندوز (TCP/IP & Winsock)، کش DNS و آدرس‌های اختصاصی IP را به طور عمیق ریست می‌کند.\n\n" +
                "فرمان‌های اجرایی به ترتیب:\n" +
                "1. ipconfig /flushdns\n" +
                "2. ipconfig /release\n" +
                "3. ipconfig /renew\n" +
                "4. netsh winsock reset\n" +
                "5. netsh int ip reset\n\n" +
                "ویندوز برای اعمال این تغییرات نیاز به ریستارت دارد.\n" +
                "آیا مایل به اجرای تعمیر اضطراری شبکه هستید؟",
                "تعمیر اضطراری و ریست کامل شبکه",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes) return;

            ShowToast("در حال اجرای زنجیره تعمیر عمیق شبکه (Winsock & IP Stack)...");
            var (success, log) = await NetworkService.ExecuteEmergencyNetworkResetAsync();

            await RefreshAdaptersAsync();

            var restartPrompt = MessageBox.Show(
                "عملیات تعمیر کامل پشته شبکه ویندوز با موفقیت انجام شد.\n\n" +
                "جهت بازسازی کاتالوگ‌های Winsock و فعال‌سازی مجدد پروتکل‌های شبکه، سیستم باید ریستارت شود.\n\n" +
                "آیا مایلید سیستم هم‌اکنون ریستارت شود؟",
                "درخواست ریستارت سیستم",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (restartPrompt == MessageBoxResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process.Start("shutdown.exe", "/r /t 5 /c \"DNS Changer: سیستم در حال ریستارت پس از تعمیر شبکه است...\"");
                }
                catch { }
            }
            else
            {
                ShowToast("تعمیر شبکه انجام شد. لطفاً در اولین فرصت سیستم را ریستارت کنید.");
            }
        }

        private async void BtnRetestPings_Click(object sender, RoutedEventArgs e)
        {
            await RunTestAllPingsAsync();
        }

        // ==============================================================
        //  20-SERVICE VERIFICATION LOGIC (Restricted & Sanctioned Sites)
        // ==============================================================

        private async void BtnVerifySanctions_Click(object sender, RoutedEventArgs e)
        {
            OverlayVerification.Visibility = Visibility.Visible;
            await RunAllServiceChecksAsync();
        }

        private async void BtnRerunVerification_Click(object sender, RoutedEventArgs e)
        {
            await RunAllServiceChecksAsync();
        }

        private async Task RunAllServiceChecksAsync()
        {
            if (_isVerifyingServices) return;
            _isVerifyingServices = true;

            // Clear OS DNS cache before verifying so tests query current live DNS
            NetworkService.FlushDns();

            TxtVerifySummary.Text = "در حال ارسال درخواست و سنجش ۲۰ سرویس...";

            _verifyServicesMaster.Clear();
            _verifyServicesMaster.AddRange(NetworkService.GetDefaultServiceChecks());
            ApplyVerifyFilter();

            // Run in parallel
            var tasks = _verifyServicesMaster.Select(s => NetworkService.TestServiceCheckAsync(s)).ToList();
            await Task.WhenAll(tasks);

            int resolvedCount = _verifyServicesMaster.Count(s => s.Resolved);
            int httpOkCount = _verifyServicesMaster.Count(s => s.HttpOk);
            int geoBlockCount = _verifyServicesMaster.Count(s => s.Resolved && (s.StatusCode == 403 || s.StatusCode == 451));

            TxtVerifySummary.Text = $"{resolvedCount} از ۲۰ دامنه حل شد ({httpOkCount} وب فعال، {geoBlockCount} تحریم IP)";
            _isVerifyingServices = false;
        }

        private void FilterVerifyGroup_Checked(object sender, RoutedEventArgs e)
        {
            if (TabVerifyAll == null) return;

            if (TabVerifyAll.IsChecked == true) _activeVerifyFilter = "All";
            else if (TabVerifyAi.IsChecked == true) _activeVerifyFilter = "AI";
            else if (TabVerifyDev.IsChecked == true) _activeVerifyFilter = "Dev";
            else if (TabVerifyCreative.IsChecked == true) _activeVerifyFilter = "Creative";
            else if (TabVerifyMedia.IsChecked == true) _activeVerifyFilter = "Media";
            else if (TabVerifyGaming.IsChecked == true) _activeVerifyFilter = "Gaming";
            else if (TabVerifyLearning.IsChecked == true) _activeVerifyFilter = "Learning";
            else if (TabVerifyFreelance.IsChecked == true) _activeVerifyFilter = "Freelance";

            ApplyVerifyFilter();
        }

        private void ApplyVerifyFilter()
        {
            var filtered = _verifyServicesMaster.Where(s =>
            {
                if (_activeVerifyFilter == "All") return true;
                return s.Group.Equals(_activeVerifyFilter, StringComparison.OrdinalIgnoreCase);
            });

            _verifyServicesDisplayed.Clear();
            foreach (var item in filtered)
            {
                _verifyServicesDisplayed.Add(item);
            }
        }

        private void BtnCloseVerification_Click(object sender, RoutedEventArgs e)
        {
            OverlayVerification.Visibility = Visibility.Collapsed;
        }

        private void BtnCustomDns_Click(object sender, RoutedEventArgs e)
        {
            OverlayCustomDns.Visibility = Visibility.Visible;
            TxtCustomPrimary.Focus();
        }

        private void BtnCancelCustomDns_Click(object sender, RoutedEventArgs e)
        {
            OverlayCustomDns.Visibility = Visibility.Collapsed;
        }

        private async void BtnSaveCustomDns_Click(object sender, RoutedEventArgs e)
        {
            string name = string.IsNullOrWhiteSpace(TxtCustomName.Text) ? "DNS اختصاصی" : TxtCustomName.Text.Trim();
            string primary = TxtCustomPrimary.Text.Trim();
            string secondary = TxtCustomSecondary.Text.Trim();

            if (!IPAddress.TryParse(primary, out _))
            {
                MessageBox.Show("لطفاً یک آدرس IP معتبر برای DNS اصلی وارد کنید.", "ورودی نامعتبر", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(secondary) && !IPAddress.TryParse(secondary, out _))
            {
                MessageBox.Show("آدرس IP وارد شده برای DNS ثانویه نامعتبر است.", "ورودی نامعتبر", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            OverlayCustomDns.Visibility = Visibility.Collapsed;

            int nextId = _masterDnsList.Count > 0 ? _masterDnsList.Max(d => d.Id) + 1 : 100;
            var customItem = new DnsItem
            {
                Id = nextId,
                Name = name,
                Primary = primary,
                Secondary = secondary,
                Type = "Custom",
                Category = "تنظیم دستی کاربر",
                IsCustom = true
            };

            _masterDnsList.Add(customItem);
            NetworkService.SaveCustomDnsItems(_masterDnsList.Where(d => d.IsCustom).ToList());
            UpdateTabCounters();
            ApplyFilterAndSearch();

            ShowToast($"کارت DNS سفارشی «{name}» ایجاد و ذخیره شد.");

            // Test ping in background for the new card
            _ = NetworkService.TestDnsItemAsync(customItem);

            // Apply it on the selected adapter
            await ApplySelectedDnsAsync(customItem);
        }

        private void BtnDeleteCustomDns_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is DnsItem item && item.IsCustom)
            {
                var res = MessageBox.Show($"آیا از حذف کارت DNS سفارشی «{item.Name}» اطمینان دارید؟", "حذف کارت DNS", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    _masterDnsList.Remove(item);
                    _displayedDnsList.Remove(item);
                    NetworkService.SaveCustomDnsItems(_masterDnsList.Where(d => d.IsCustom).ToList());
                    UpdateTabCounters();
                    ApplyFilterAndSearch();
                    ShowToast($"کارت DNS «{item.Name}» با موفقیت حذف شد.");
                }
            }
        }

        private void BtnCopyIp_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is DnsItem item)
            {
                string text = $"{item.Primary}\r\n{item.Secondary}";
                Clipboard.SetText(text);
                ShowToast($"آدرس‌های DNS {item.Name} در کلیپ‌بورد کپی شد.");
            }
        }

        private async void BtnRefreshAdapters_Click(object sender, RoutedEventArgs e)
        {
            await RefreshAdaptersAsync();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                BtnMaximize_Click(sender, e);
                return;
            }

            if (e.ButtonState == MouseButtonState.Pressed)
            {
                if (WindowState == WindowState.Maximized)
                {
                    Point screenPoint = PointToScreen(e.GetPosition(this));
                    double relativeX = e.GetPosition(this).X / ActualWidth;
                    WindowState = WindowState.Normal;
                    Left = screenPoint.X - (ActualWidth * relativeX);
                    Top = screenPoint.Y - 18;
                }
                DragMove();
            }
        }

        private void BtnGitHub_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/Hadooshi/Iran-DNS-Toolbox",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                ShowToast($"خطا در باز کردن مرورگر: {ex.Message}", true);
            }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        private void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                if (RootBorder != null)
                {
                    RootBorder.BorderThickness = new Thickness(0);
                    RootBorder.Margin = new Thickness(0);
                }
                if (PathMaximize != null)
                    PathMaximize.Data = (Geometry)FindResource("GeomRestore");
                if (BtnMaximize != null)
                    BtnMaximize.ToolTip = "بازیابی اندازه پنجره";
            }
            else
            {
                if (RootBorder != null)
                {
                    RootBorder.BorderThickness = new Thickness(1);
                    RootBorder.Margin = new Thickness(0);
                }
                if (PathMaximize != null)
                    PathMaximize.Data = (Geometry)FindResource("GeomMaximize");
                if (BtnMaximize != null)
                    BtnMaximize.ToolTip = "بزرگ‌کردن / بازیابی";
            }

            SetColumns(_preferredColumns);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                if (OverlayVerification.Visibility == Visibility.Visible)
                {
                    _ = RunAllServiceChecksAsync();
                }
                else
                {
                    _ = RunTestAllPingsAsync();
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                if (OverlayVerification.Visibility == Visibility.Visible)
                {
                    OverlayVerification.Visibility = Visibility.Collapsed;
                    e.Handled = true;
                }
                else if (OverlayCustomDns.Visibility == Visibility.Visible)
                {
                    OverlayCustomDns.Visibility = Visibility.Collapsed;
                    e.Handled = true;
                }
                else if (!string.IsNullOrEmpty(TxtSearch.Text))
                {
                    TxtSearch.Text = string.Empty;
                    e.Handled = true;
                }
            }
        }

        private void RadioViewMode_Checked(object sender, RoutedEventArgs e)
        {
            if (CardsContainer == null || TableContainer == null) return;

            if (RadioViewCards.IsChecked == true)
            {
                CardsContainer.Visibility = Visibility.Visible;
                TableContainer.Visibility = Visibility.Collapsed;
                if (PanelCardSizeControls != null) PanelCardSizeControls.Visibility = Visibility.Visible;
            }
            else
            {
                CardsContainer.Visibility = Visibility.Collapsed;
                TableContainer.Visibility = Visibility.Visible;
                if (PanelCardSizeControls != null) PanelCardSizeControls.Visibility = Visibility.Collapsed;
            }
        }

        private static T? FindVisualChild<T>(DependencyObject? parent) where T : DependencyObject
        {
            if (parent == null) return null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild) return typedChild;
                T? childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null) return childOfChild;
            }
            return null;
        }

        private void SetColumns(int cols)
        {
            _preferredColumns = cols;
            GridColumns = cols;

            var ug = FindVisualChild<UniformGrid>(DnsCardsList);
            if (ug != null)
            {
                ug.Columns = cols;
            }

            UpdatePresetButtonHighlight(cols);
            UpdateCardWidthDisplay();
        }

        private void UpdateCardWidthDisplay()
        {
            if (CardsContainer == null || TxtCardWidthDisplay == null) return;

            double containerWidth = CardsContainer.ViewportWidth > 0
                ? CardsContainer.ViewportWidth
                : (CardsContainer.ActualWidth > 0 ? CardsContainer.ActualWidth - 24 : 1000);

            double availableWidth = containerWidth - (CardsContainer.Padding.Left + CardsContainer.Padding.Right);

            int cols = GridColumns > 0 ? GridColumns : 4;
            double colWidth = Math.Max(0, (availableWidth / cols) - 8);

            TxtCardWidthDisplay.Text = $"{colWidth:0}px";

            if (SliderCardWidth != null && !SliderCardWidth.IsMouseCaptureWithin)
            {
                SliderCardWidth.ValueChanged -= SliderCardWidth_ValueChanged;
                SliderCardWidth.Value = Math.Clamp(colWidth, SliderCardWidth.Minimum, SliderCardWidth.Maximum);
                SliderCardWidth.ValueChanged += SliderCardWidth_ValueChanged;
            }
        }

        private void CardsContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                double availableWidth = e.NewSize.Width - (CardsContainer.Padding.Left + CardsContainer.Padding.Right);
                if (availableWidth > 200)
                {
                    int maxPossibleCols = Math.Max(1, (int)(availableWidth / 175));
                    int effectiveCols = Math.Min(_preferredColumns, maxPossibleCols);
                    effectiveCols = Math.Max(2, effectiveCols);
                    SetColumns(effectiveCols);
                }
                else
                {
                    UpdateCardWidthDisplay();
                }
            }
        }

        private void SliderCardWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded || CardsContainer == null) return;

            double containerWidth = CardsContainer.ViewportWidth > 0 ? CardsContainer.ViewportWidth : CardsContainer.ActualWidth;
            double availableWidth = containerWidth - (CardsContainer.Padding.Left + CardsContainer.Padding.Right);
            if (availableWidth <= 180) return;

            int cols = (int)Math.Round(availableWidth / (e.NewValue + 8.0));
            cols = Math.Clamp(cols, 2, 5);

            SetColumns(cols);
        }

        private void UpdatePresetButtonHighlight(int activeCols)
        {
            HighlightButton(BtnCol5, activeCols == 5);
            HighlightButton(BtnCol4, activeCols == 4);
            HighlightButton(BtnCol3, activeCols == 3);
            HighlightButton(BtnCol2, activeCols == 2);
        }

        private static void HighlightButton(Button? btn, bool isActive)
        {
            if (btn == null) return;
            if (isActive)
            {
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1D4ED8"));
                btn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B82F6"));
                btn.Foreground = Brushes.White;
            }
            else
            {
                btn.ClearValue(Button.BackgroundProperty);
                btn.ClearValue(Button.BorderBrushProperty);
                btn.ClearValue(Button.ForegroundProperty);
            }
        }

        private void BtnPreset5_Click(object sender, RoutedEventArgs e)
        {
            SetColumns(5);
        }

        private void BtnPreset4_Click(object sender, RoutedEventArgs e)
        {
            SetColumns(4);
        }

        private void BtnPreset3_Click(object sender, RoutedEventArgs e)
        {
            SetColumns(3);
        }

        private void BtnPreset2_Click(object sender, RoutedEventArgs e)
        {
            SetColumns(2);
        }

        private void ShowToast(string message, bool isError = false)
        {
            TxtStatusToast.Text = message;
            TxtStatusToast.Foreground = isError
                ? (SolidColorBrush)FindResource("ErrorRed")
                : (SolidColorBrush)FindResource("TextSecondary");
        }
    }
}
