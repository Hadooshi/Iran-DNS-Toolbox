using System;
using System.Threading;
using System.Windows;

namespace DNSChangerApp
{
    public partial class App : Application
    {
        private static Mutex? _mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += (s, args) =>
            {
                MessageBox.Show($"خطای سیستمی رخ داد:\n{args.Exception.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Handled = true;
            };

            const string appName = "DNSChanger_Arezoo_SingleInstanceMutex";
            _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                // App is already running!
                MessageBox.Show("نرم‌افزار DNS Changer در حال حاضر در حال اجرا است.", "اطلاع", MessageBoxButton.OK, MessageBoxImage.Information);
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
            base.OnExit(e);
        }
    }
}
