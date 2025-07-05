using VAHub;
using VAHub.Factories;
using VAHub.Interfaces;
using VAHub.Logging;
using VAHub.Managers;
using VAHub.Options;
using VAHub.Services;

OptionsManager optionsManager = new OptionsManager("appsettings.json", true);
CoreFactory coreFactory = new CoreFactory(optionsManager);
FileLoggerOptions fileLoggerOptions = optionsManager.Get<FileLoggerOptions>("FileLogger");
CoreOptions coreOptions = optionsManager.Get<CoreOptions>(nameof(Core));
ILogger logger = new FileLogger(fileLoggerOptions);
Core core = coreFactory.CreateCore(coreOptions);
App app = new(core);

Logger.SetLogger(logger);

app.Run();