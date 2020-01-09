using NLog;
using Plugin.NLogSample.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NLogSample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : ContentPage
    {
        private ILoggingService _logger;
        public ILoggingService Logger
        {
            get
            {
                if (_logger == null) _logger = CrossLoggingService.Current;
                return _logger;
            }
        }

        public  List<LogLevel> LogLevels = new List<LogLevel>()
        {
            LogLevel.Trace,
            LogLevel.Debug,
            LogLevel.Info,
            LogLevel.Warn,
            LogLevel.Error,
            LogLevel.Fatal,
            LogLevel.Off
        };

        public Main()
        {
            InitializeComponent();
            LogLevelPicker.ItemsSource = LogLevels;
            LogLevelPicker.SelectedIndex = 0;
        }

        void OnLoggingButton(object sender, EventArgs e)
        {
            Logger.Info("Logging with informatin level.");
            Logger.Warn("Logging with warning level.");
            Logger.Debug("Logging with debug level.");
            Logger.Trace("Logging with verbose level.");
            Logger.Error("Logging with error level.");
            Logger.Fatal("Logging with fatal level.");
        }

        void OnExceptionButton(object sender, EventArgs e)
        {
            // Logging exception 
            try
            {
                throw new InvalidOperationException("invalid operation");
            }
            catch(Exception ex)
            {
                Logger.Error(ex, "Exception occuerd.");
            }
        }

        private void LogLevelPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = LogLevelPicker.SelectedItem;
            Logger.ChangeLogLevel((LogLevel)selectedItem);
        }
    }
}
