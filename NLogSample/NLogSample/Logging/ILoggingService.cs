using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLogSample.Logging
{
    /// <summary>
    /// This interface provides logging interface.
    /// </summary>
    public interface ILoggingService
    {
        void Info(string message);
        void Info(string format, params object[] args);

        void Error(string message);
        void Error(string format, params object[] args);
        void Error(Exception e, string message);
        void Error(Exception e, string format, params object[] args);

        void Fatal(string message);
        void Fatal(string format, params object[] args);
        void Fatal(Exception e, string message);
        void Fatal(Exception e, string format, params object[] args);

        void Debug(string message);
        void Debug(string format, params object[] args);

        void Trace(string message);
        void Trace(string format, params object[] args);

        void Warn(string message);
        void Warn(string format, params object[] args);
    }
}
