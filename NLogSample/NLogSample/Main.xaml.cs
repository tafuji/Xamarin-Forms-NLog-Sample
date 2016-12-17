using NLogSample.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace NLogSample
{
    public partial class Main : ContentPage
    {
        private ILoggingService _logger;
        public ILoggingService Logger
        {
            get
            {
                if (_logger == null) _logger = DependencyService.Get<ILoggingService>();
                return _logger;
            }
        }

        public Main()
        {
            InitializeComponent();
        }

        void OnLoggingButton(object sender, EventArgs e)
        {
            Logger.Info("Information レベルのログ出力");
            Logger.Warn("警告レベルのログ出力");
            Logger.Debug("デバッグレベルのログ出力");
            Logger.Trace("詳細レベルのログ出力");
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

    }
}
