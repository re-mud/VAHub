using VAHub;
using VAHub.Core;
using VAHub.Logging;
using VAHub.Managers;

OptionsManager optionsManager = new OptionsManager("appsettings.json", true);
CoreFactory coreFactory = new CoreFactory(optionsManager);
FileLoggerOptions fileLoggerOptions = optionsManager.Get<FileLoggerOptions>("FileLogger");
CoreOptions coreOptions = optionsManager.Get<CoreOptions>(nameof(VACore));
ILogger logger = new FileLogger(fileLoggerOptions);
VACore core = coreFactory.CreateCore(coreOptions);
App app = new(core);

Logger.SetLogger(logger);

app.Run();