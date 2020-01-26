#module nuget:?package=Cake.LightModeConsole.Module&version=%CAKE_LIGHTMODECONSOLE_MODULE_VERSION%

Information("Hello");

Task("Hello-World")
 .DoesForEach(
     Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>(),
     logLevel=> {
     Context.Log.Write(Verbosity.Quiet, logLevel, "{0:f}", logLevel);
 });

 RunTarget("Hello-World");