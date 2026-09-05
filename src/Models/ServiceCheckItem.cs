using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DNSChangerApp.Models
{
    public class ServiceCheckItem : INotifyPropertyChanged
    {
        private bool _resolved = false;
        private string _ip = string.Empty;
        private long _latencyMs = 0;
        private bool _httpOk = false;
        private int _statusCode = 0;
        private string _note = string.Empty;
        private bool _isChecking = false;

        public string Group { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        public bool Resolved
        {
            get => _resolved;
            set { if (_resolved != value) { _resolved = value; OnPropertyChanged(); UpdateStatus(); } }
        }

        public string Ip
        {
            get => _ip;
            set { if (_ip != value) { _ip = value; OnPropertyChanged(); } }
        }

        public long LatencyMs
        {
            get => _latencyMs;
            set { if (_latencyMs != value) { _latencyMs = value; OnPropertyChanged(); } }
        }

        public bool HttpOk
        {
            get => _httpOk;
            set { if (_httpOk != value) { _httpOk = value; OnPropertyChanged(); UpdateStatus(); } }
        }

        public int StatusCode
        {
            get => _statusCode;
            set { if (_statusCode != value) { _statusCode = value; OnPropertyChanged(); UpdateStatus(); } }
        }

        public string Note
        {
            get => _note;
            set { if (_note != value) { _note = value; OnPropertyChanged(); UpdateStatus(); } }
        }

        public bool IsChecking
        {
            get => _isChecking;
            set { if (_isChecking != value) { _isChecking = value; OnPropertyChanged(); UpdateStatus(); } }
        }

        public string StatusTitle { get; private set; } = "در انتظار بررسی";
        public string StatusColor { get; private set; } = "#8A8A8A";
        public string StatusBg { get; private set; } = "#242424";
        public string StatusBorder { get; private set; } = "#383838";

        public string GroupFa
        {
            get
            {
                return Group switch
                {
                    "AI" => "هوش مصنوعی",
                    "Dev" => "توسعه و برنامه‌نویسی",
                    "Creative" => "طراحی و گرافیک",
                    "Media" => "رسانه و صوت",
                    "Gaming" => "بازی و استیم",
                    "Learning" => "آموزش آنلاین",
                    "Freelance" => "فریلنسری",
                    _ => Group
                };
            }
        }

        private void UpdateStatus()
        {
            if (_isChecking)
            {
                StatusTitle = "در حال بررسی...";
                StatusColor = "#FBBF24";
                StatusBg = "#30261A";
                StatusBorder = "#503E20";
            }
            else if (_resolved && _httpOk)
            {
                StatusTitle = $"موفق - {_latencyMs}ms";
                StatusColor = "#34D399";
                StatusBg = "#1A2D22";
                StatusBorder = "#2E543C";
            }
            else if (_resolved && (_statusCode == 403 || _statusCode == 451))
            {
                StatusTitle = "محدودیت سرویس - کد ۴۰۳";
                StatusColor = "#FBBF24";
                StatusBg = "#30261A";
                StatusBorder = "#503E20";
            }
            else if (_resolved)
            {
                StatusTitle = $"حل شد - {_latencyMs}ms";
                StatusColor = "#60CDFF";
                StatusBg = "#1C293A";
                StatusBorder = "#2E4766";
            }
            else
            {
                StatusTitle = string.IsNullOrEmpty(_note) ? "عدم دسترسی / مسدود" : _note;
                StatusColor = "#F87171";
                StatusBg = "#2E1D1F";
                StatusBorder = "#542E32";
            }

            OnPropertyChanged(nameof(StatusTitle));
            OnPropertyChanged(nameof(StatusColor));
            OnPropertyChanged(nameof(StatusBg));
            OnPropertyChanged(nameof(StatusBorder));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
