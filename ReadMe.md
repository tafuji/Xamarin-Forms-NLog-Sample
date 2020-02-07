# Xamarin.Forms NLog Sample

This is a simple Xamarin.Forms sample code for logging with [NLog](https://nlog-project.org/).

## Sample application

This repository provides a very simple application, which has a picker and two buttons. If you tap the buttons, sample applicaiton writes information to CSV format log file according to the selected value of the picker.

![application](https://gist.githubusercontent.com/tafuji/8f74b0645375bf82a4452656930e5a6c/raw/21352c4fc3a34e4a7c3600c392fe674dfede43d0/01-Application.png)

- If you tap the "LOGGING TEST" button, the application writes information to the csv log file.
- If you tap the "THROWS ERROR" button, the application writes error information to the csv log file.
- You can change the log level by selecting the item of the picker control.

## Visual Studio solution structure

The sample app has four projects. Each role of the projects are shown below.

|#|Project name|Notes|
|----|----|----|
|1|NLogSample|Xamarin.Forms project that calls the codes to write information to the log file|
|2|NLogSample.Android|Xamarin.Android project, which includes the configuration file for logging with NLog|
|3|NLogSample.iOS|Xamarin.iOS project, which includes the configuration file for logging with NLog|
|4|NLogSample.Logging|Multi-target project that uses NLog to write information to the log file|

## Configuration file

You have to copy the NLog.config file to correct location. The location and setting about NLog.config file are different in each platfrom (Android, iOS).

### Android

In Android platform, NLog.config file shoule be copied in the Assets folder.

![AndoridLocation](https://gist.githubusercontent.com/tafuji/8f74b0645375bf82a4452656930e5a6c/raw/21352c4fc3a34e4a7c3600c392fe674dfede43d0/02-Android-NLogConfig.png)

Then, you have to set the build action value of the property to ```AndroidAsset```.

![AndroidProperty](https://gist.githubusercontent.com/tafuji/8f74b0645375bf82a4452656930e5a6c/raw/21352c4fc3a34e4a7c3600c392fe674dfede43d0/03-Android-NLogConfig-Property.png)

In Android platform, the ```fileName``` attribute should have the value, which shoule be "```/data/data/[your package name]/```" format.

```xml
<target name="logfile"
    xsi:type="File"
    fileName="/data/data/NLogSample.Android/logs/nlog-sample.csv"
    archiveFileName="/data/data/NLogSample.Android/logs/archives/nlog-sample-{#}.csv"
    archiveEvery="Minute"
    archiveNumbering="Date"
    maxArchiveFiles="5"
    archiveDateFormat="yyyy-MM-dd-HH-mm"
    encoding="UTF8">
    <layout xsi:type="CSVLayout">
    <!-- ... -->
    </layout>
</target>
```

### iOS

In iOS platform, you have to copy the NLog.config file in the root of the project.

![iOSLocation](https://gist.githubusercontent.com/tafuji/8f74b0645375bf82a4452656930e5a6c/raw/21352c4fc3a34e4a7c3600c392fe674dfede43d0/04-iOS-NLogConfig.png)

The build action value of the property should be set to ```BundleResource```.

![iOSProperty](https://gist.githubusercontent.com/tafuji/8f74b0645375bf82a4452656930e5a6c/raw/21352c4fc3a34e4a7c3600c392fe674dfede43d0/05-iOS-NLogConfig-Property.png)

In iOS platform, the path of data used in your application shoule be located in the ```Library`` folder of your applicaiton. By using the configuration syntax of system special folder, you can set the value of log file path. Here is the example used in the sample codes.

```xml
<target name="logfile"
    xsi:type="File"
    fileName="${specialfolder:folder=MyDocuments}/../Library/nlog-sample.csv"
    archiveFileName="${specialfolder:folder=MyDocuments}/../Library/archives/nlog-sample-{#}.csv"
    archiveEvery="Minute"
    archiveNumbering="Date"
    maxArchiveFiles="5"
    archiveDateFormat="yyyy-MM-dd-HH-mm"
    encoding="UTF-8">
    <layout xsi:type="CSVLayout">
    <!-- ... -->
    </layout>
</target>
```

## Logging with NLog

The logging codes are implemented in the ```LoggingService``` class shown below.

```csharp
namespace Plugin.NLogSample.Logging
{
    internal class LoggingService : ILoggingService
    {
        private LogFactory logFactory;
        private LogFactory LogFactory
        {
            get
            {
                if(logFactory == null)
                {
                    var configName = PlatformLoggingService.ConfigFilePath;
                    logFactory = new LogFactory(new XmlLoggingConfiguration(configName));

                }
                return logFactory;
            }
        }

        private ILogger _logger;
        private ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = logFactory.GetLogger("NLogSample");
                }
                return _logger;
            }
        }

        public void Error(string message) => Logger.Error(message);
        public void Error(Exception e, string message) => Logger.Error(e, message);
        public void Error(string format, params object[] args) => Logger.Error(format, args);
        public void Error(Exception e, string format, params object[] args) => Logger.Error(e, format, args);

        // ....
    }
}
```

Each method just passes the argument to the ```ILogger```'s method. The important point is how to load the NLog.config in your applicatin code. In the next section, how to load the configuration file is explained.

### How to load the configuration file

It is said that NLog supports automatically loading NLog.config file in assets folder. However the feature does not seem to work fine. Therefore, it might be better to wirte the codes that load the configuration file in your applications. In sample codes, the codes are written in ```LoggingService``` class as shown below.

```csharp
internal class LoggingService : ILoggingService
{
        private LogFactory logFactory;
        private LogFactory LogFactory
        {
            get
            {
                if(logFactory == null)
                {
                     // load the configuration file explicitly
                    var configName = PlatformLoggingService.ConfigFilePath;
                    logFactory = new LogFactory(new XmlLoggingConfiguration(configName));

                }
                return logFactory;
            }
        }

        private ILogger _logger;
        private ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = logFactory.GetLogger("NLogSample");
                }
                return _logger;
            }
        }
    // ...
}
```

By passing the path of NLog.config file to the constructor of ```XmlLoggingConfiguration``` class, you can surely load NLog.config file in your application. The paths of configuration files are not same in each platform. Therefore, you have to write the codes that get the path of NLog.config file in each platform.

|Platrofm|path|
|---|---|
|Android|assets/NLog.config|
|iOS|NLog.config|

In the sample codes, the static field of ```PlatformLoggingService``` class provides the path of NLog.config file. Because the project that contains the codes loading NLog.config is multi-target project created by [Cross-Platform .NET Standard Plugin Templates](https://marketplace.visualstudio.com/items?itemName=vs-publisher-473885.PluginForXamarinTemplates), you can call the codes in the same way in one project. For example, the ```PlatformLogginService.ConfigFilePath``` in Android platform should be implemented as shown below.

```csharp
internal class PlatformLoggingService
{
    public static string ConfigFilePath = "assets/NLog.config";
}
```

After loading the NLog.config file, you can get ```ILogger``` instance by calling ```LogManager.GetLogger``` method.

## Calling the logging methods

The logging codes in the sample are following below:

```csharp
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

    public Main()
    {
        InitializeComponent();
    }

    void OnLoggingButton(object sender, EventArgs e)
    {
        Logger.Info("Logging with informatin level.");
        Logger.Warn("Logging with warning level.");
        Logger.Debug("Logging with debug level.");
        Logger.Trace("Logging with verbose level.");
    }
    // .....
}
```

In the NLogSample.Logging project, ```CrossLoggingService.Current``` property creates the instance of ```LoggingService``` class, where logging codes are implemented. Here is a screenshot that shows the contents of the CSV log file.

![logfile](https://gist.githubusercontent.com/tafuji/8f74b0645375bf82a4452656930e5a6c/raw/21352c4fc3a34e4a7c3600c392fe674dfede43d0/06-LoggingReuslts.png)

## Changing log level

You can change log level by calling ```DisableLoggingForLevels``` or ```EnableLoggingForLevels``` methods of ```LoggingRule``` class. ```LoggingService.ChangeLogLevel``` method change disable logs, where the log levels are under the value of the argument and enable logs, where the log levels are over the argument.

```csharp
public void ChangeLogLevel(LogLevel level)
{
    
    if (level == LogLevel.Off)
    {
        LogFactory.SuspendLogging();
    }
    else
    {
        if (!LogFactory.IsLoggingEnabled())
        {
            LogFactory.ResumeLogging();
        }

        int maxLogLevelOrdinal = LogLevel.AllLoggingLevels.Max(p => p.Ordinal);
        int minLogLevelOrdinal = LogLevel.AllLoggingLevels.Min(p => p.Ordinal);

        foreach (var rule in LogFactory.Configuration.LoggingRules)
        {
            rule.DisableLoggingForLevels(LogLevel.FromOrdinal(minLogLevelOrdinal), level);
            rule.EnableLoggingForLevels(level, LogLevel.FromOrdinal(maxLogLevelOrdinal));
        }
    }
    LogFactory.ReconfigExistingLoggers();
}
```
