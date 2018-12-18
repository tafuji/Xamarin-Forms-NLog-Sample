using System;

namespace Plugin.NLogSample.Logging
{
    /// <summary>
    /// Cross CrossLoggingService
    /// </summary>
    public static class CrossLoggingService
    {
        static Lazy<ILoggingService> implementation = new Lazy<ILoggingService>(() => CreateNLogSampleLogging(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Gets if the plugin is supported on the current platform.
    /// </summary>
    public static bool IsSupported => implementation.Value == null ? false : true;

    /// <summary>
    /// Current plugin implementation to use
    /// </summary>
    public static ILoggingService Current
    {
        get
        {
            ILoggingService ret = implementation.Value;
            if (ret == null)
            {
                throw NotImplementedInReferenceAssembly();
            }
            return ret;
        }
    }

    static ILoggingService CreateNLogSampleLogging()
    {
#if NETSTANDARD2_0
            return null;
#else
#pragma warning disable IDE0022 // Use expression body for methods
        return new LoggingService();
#pragma warning restore IDE0022 // Use expression body for methods
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly() =>
        new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");

}
}
