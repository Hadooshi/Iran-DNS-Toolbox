using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DNSChangerApp.Models
{
    public class DnsItem : INotifyPropertyChanged
    {
        private long _ping1 = 9999;
        private long _ping2 = 9999;
        private bool _udp1 = false;
        private bool _udp2 = false;
        private long _bestPing = 9999;
        private bool _isTesting = false;
        private bool _isActive = false;

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Primary { get; set; } = string.Empty;
        public string Secondary { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public bool IsCustom { get; set; } = false;
        public bool HasNotice { get; set; } = false;
        public string NoticeText { get; set; } = string.Empty;
        public bool CanDelete => IsCustom;

        public long Ping1
        {
            get => _ping1;
            set
            {
                if (_ping1 != value)
                {
                    _ping1 = value;
                    OnPropertyChanged();
                    UpdateBestPing();
                }
            }
        }

        public long Ping2
        {
            get => _ping2;
            set
            {
                if (_ping2 != value)
                {
                    _ping2 = value;
                    OnPropertyChanged();
                    UpdateBestPing();
                }
            }
        }

        public bool Udp1
        {
            get => _udp1;
            set
            {
                if (_udp1 != value)
                {
                    _udp1 = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsPort53Working));
                    OnPropertyChanged(nameof(UdpStatusText));
                }
            }
        }

        public bool Udp2
        {
            get => _udp2;
            set
            {
                if (_udp2 != value)
                {
                    _udp2 = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsPort53Working));
                    OnPropertyChanged(nameof(UdpStatusText));
                }
            }
        }

        public long BestPing
        {
            get => _bestPing;
            set
            {
                if (_bestPing != value)
                {
                    _bestPing = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PingDisplay));
                    OnPropertyChanged(nameof(PingStatusColor));
                }
            }
        }

        public bool IsTesting
        {
            get => _isTesting;
            set
            {
                if (_isTesting != value)
                {
                    _isTesting = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PingDisplay));
                }
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ActiveStatusText));
                }
            }
        }

        public bool IsPort53Working => _udp1 || _udp2;

        public string UdpStatusText
        {
            get
            {
                if (_bestPing >= 9999) return "عدم پاسخ";
                return IsPort53Working ? "پورت ۵۳ فعال" : "پورت ۵۳ مسدود";
            }
        }

        public string PingDisplay
        {
            get
            {
                if (_isTesting) return "در حال بررسی...";
                if (_bestPing >= 9999) return "تایم‌اوت";
                return $"{_bestPing} ms";
            }
        }

        public string PingStatusColor
        {
            get
            {
                if (_isTesting) return "#8A8A8A";
                if (_bestPing < 80 && IsPort53Working) return "#34D399";
                if (_bestPing < 160 && IsPort53Working) return "#FBBF24";
                if (_bestPing < 9999) return "#FB923C";
                return "#F87171";
            }
        }

        public string TypeBadgeText
        {
            get
            {
                if (IsCustom) return "سفارشی";
                return Type switch
                {
                    "Anti-Sanction" => "رفع تحریم",
                    "Global" => "بین‌المللی",
                    "Privacy" => "حریم خصوصی",
                    "Family" => "خانواده و امنیت",
                    "ISP" => "داخلی ISP",
                    _ => "سفارشی"
                };
            }
        }

        public string TypeBadgeBg
        {
            get
            {
                if (IsCustom) return "#331B2D";
                return Type switch
                {
                    "Anti-Sanction" => "#2E243D",
                    "Global" => "#1C293A",
                    "Privacy" => "#163328",
                    "Family" => "#352516",
                    "ISP" => "#232936",
                    _ => "#331B2D"
                };
            }
        }

        public string TypeBadgeBorder
        {
            get
            {
                if (IsCustom) return "#5C284E";
                return Type switch
                {
                    "Anti-Sanction" => "#553C73",
                    "Global" => "#2E4766",
                    "Privacy" => "#225C43",
                    "Family" => "#66431E",
                    "ISP" => "#3B4861",
                    _ => "#5C284E"
                };
            }
        }

        public string TypeBadgeForeground
        {
            get
            {
                if (IsCustom) return "#F472B6";
                return Type switch
                {
                    "Anti-Sanction" => "#D8B4FE",
                    "Global" => "#93C5FD",
                    "Privacy" => "#6EE7B7",
                    "Family" => "#FDBA74",
                    "ISP" => "#94A3B8",
                    _ => "#F472B6"
                };
            }
        }

        public string ActiveStatusText => _isActive ? "متصل" : "اتصال";

        private void UpdateBestPing()
        {
            long p1 = _ping1;
            long p2 = _ping2;
            BestPing = Math.Min(p1, p2);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
