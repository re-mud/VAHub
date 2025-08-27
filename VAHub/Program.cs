using VAHub.Commands.Handlers;
using VAHub.Commands;
using VAHub.Managers;
using VAHub.Plugins;
using VAHub.Logging;
using VAHub.Core;
using VAHub;

VACore CreateCore(OptionsManager optionsManager)
{
    CoreFactory coreFactory = new CoreFactory(optionsManager);
    CoreOptions coreOptions = optionsManager.Get<CoreOptions>(nameof(VACore));
    return coreFactory.CreateCore(coreOptions);
}

PluginManager CreatePluginManager(OptionsManager optionsManager)
{
    PluginManager pluginManager = new(optionsManager.Get<PluginManagerOptions>(nameof(PluginManager)));
    pluginManager.LoadPlugins();
    return pluginManager;
}

CommandManager CreateCommandManager(OptionsManager optionsManager, Dictionary<string, CommandModel> commands)
{
    CommandManager commandManager = new();
    commandManager.SetCommands(commands);

    try
    {
        PythonCommandHandler pythonCommandHandler = new(
            optionsManager.Get<PythonCommandHandlerOptions>(nameof(PythonCommandHandler)));
        commandManager.AddHandler(CommandType.Python, pythonCommandHandler);
    }
    catch
    {
        Logger.Error("Не удалось инициализировать обработчик python");
    }

    commandManager.AddHandler(CommandType.Program, new ProgramCommandHandler());

    return commandManager;
}

void SetupLogger(OptionsManager optionsManager)
{
    if (args.Length != 0 && args.Contains("--debug"))
    {
        Logger.IsDebug = true;
    }

    FileLoggerOptions fileLoggerOptions = optionsManager.Get<FileLoggerOptions>(nameof(FileLogger));
    FileLogger logger = new(fileLoggerOptions);
    Logger.SetLogger(logger);
}

OptionsManager optionsManager = new OptionsManager("appsettings.json", true);
SetupLogger(optionsManager);

VACore core = CreateCore(optionsManager);
PluginManager pluginManager = CreatePluginManager(optionsManager);
CommandManager commandManager = CreateCommandManager(optionsManager, pluginManager.GetCommands());
App app = new(core, commandManager);

app.Run();