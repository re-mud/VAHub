using VAHub;
using VAHub.Core;
using VAHub.Logging;
using VAHub.Managers;
using VAHub.Plugins;

PluginFactory CreatePluginFactory(OptionsManager optionsManager)
{
    PluginFactory factory = new();
    factory.Register("program", (path, manifest) => new ProgramPlugin(path, manifest));
    return factory;
}

VACore CreateCore(OptionsManager optionsManager)
{
    CoreFactory coreFactory = new CoreFactory(optionsManager);
    CoreOptions coreOptions = optionsManager.Get<CoreOptions>(nameof(VACore));
    return coreFactory.CreateCore(coreOptions);
}

PluginManager CreatePluginManager(OptionsManager optionsManager, PluginFactory pluginFactory)
{
    PluginManager pluginManager = new(optionsManager.Get<PluginManagerOptions>(nameof(PluginManager)), pluginFactory);
    return pluginManager;
}

void SetupLogger(OptionsManager optionsManager)
{
    FileLoggerOptions fileLoggerOptions = optionsManager.Get<FileLoggerOptions>("FileLogger");
    FileLogger logger = new(fileLoggerOptions);
    Logger.SetLogger(logger);
}

OptionsManager optionsManager = new OptionsManager("appsettings.json", true);
SetupLogger(optionsManager);

VACore core = CreateCore(optionsManager);
PluginFactory pluginFactory = CreatePluginFactory(optionsManager);
PluginManager pluginManager = CreatePluginManager(optionsManager, pluginFactory);
App app = new(core, pluginManager);

app.Run();