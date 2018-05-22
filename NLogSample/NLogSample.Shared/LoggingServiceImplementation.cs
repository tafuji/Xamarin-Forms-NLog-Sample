using System;
using NLogSample.Logging;
using Xamarin.Forms;
using NLog;
using NLog.Config;

[assembly: Dependency(typeof(LoggingServiceImplementation))]
namespace NLogSample.Logging
{

    /// <summary>
    /// implements <see cref="ILoggingService"/>.
    /// </summary>
    public class LoggingServiceImplementation : ILoggingService
    {
        private ILogger _logger;

        private ILogger Logger
        {
            get
            {
                if(_logger == null)
                {
                    var configName = string.Empty;
                    #if __ANDROID__
                    configName = "assets/NLog.config";
                    #endif
                    #if __IOS__
                    configName = "NLog.config";
                    #endif
                    LogManager.Configuration = new XmlLoggingConfiguration(configName);
                    _logger = LogManager.GetCurrentClassLogger();
                }
                return _logger;
            }
        }

#region Error

        public void Error(string message)
        {
            Logger.Error(message);
        }

        public void Error(Exception e, string message)
        {
            Logger.Error(e, message);
        }

        public void Error(string format, params object[] args)
        {
            Logger.Error(format, args);
        }

        public void Error(Exception e, string format, params object[] args)
        {
            Logger.Error(e, format, args);
        }

#endregion

#region Fatal

        public void Fatal(string message)
        {
            Logger.Fatal(message);
        }

        public void Fatal(string format, params object[] args)
        {
            Logger.Fatal(format, args);
        }

        public void Fatal(Exception e, string message)
        {
            Logger.Fatal(e, message);
        }

        public void Fatal(Exception e, string format, params object[] args)
        {
            Logger.Fatal(e, format, args);
        }

#endregion

#region Debug

        public void Debug(string message)
        {
            Logger.Debug(message);
        }

        public void Debug(string format, params object[] args)
        {
            Logger.Debug(format, args);
        }

#endregion

#region Info

        public void Info(string message)
        {
            Logger.Info(message);
        }

        public void Info(string message, params object[] args)
        {
            Logger.Info(message, args);
        }

#endregion

#region Trace

        public void Trace(string message)
        {
            Logger.Trace(message);
        }

        public void Trace(string format, params object[] args)
        {
            Logger.Trace(format, args);
        }

#endregion

#region Warn

        public void Warn(string message)
        {
            Logger.Warn(message);
        }

        public void Warn(string format, params object[] args)
        {
            Logger.Warn(format, args);
        }

#endregion
    }
}
