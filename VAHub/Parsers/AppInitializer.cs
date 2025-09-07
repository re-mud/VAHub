using VAHub.Managers;
using VAHub.Logging;
using VAHub.Core;
using VAHub.Commands;
using VAHub.Commands.Handlers;
using VAHub.Plugins;

namespace VAHub.Parsers;

public class AppInitializer
{
    private readonly OptionsManager _optionsManager;
    private readonly ArgsParser _argsParser;

    public AppInitializer(OptionsManager optionsManager, ArgsParser argsParser)
    {
        _optionsManager = optionsManager;
        _argsParser = argsParser;
    }

    public App InitializeApp()
    {
        SetupLogger();

        AppOptions options = _optionsManager.Get<AppOptions>(nameof(App));
        VACore core = CreateCore();
        PluginManager pluginManager = CreatePluginManager();
        CommandManager commandManager = CreateCommandManager();
        Dictionary<string, CommandModel> commands = pluginManager.GetCommands();
        commandManager.SetCommands(commands);

        if (_argsParser.ShowCommands)
        {
            Logger.Info("Список команд:\n\t" + string.Join("\n\t", commands.Keys));
        }
        if (_argsParser.ShowHelp)
        {
            Logger.Info("Общие настройки:\n" +
                "-h|--help            справка\n" +
                "-v|--verbose         подробный вывод\n" +
                "-c|--commands        список команд");
        }

        return new(core, commandManager, options);
    }

    public VACore CreateCore()
    {
        CoreFactory coreFactory = new CoreFactory(_optionsManager);
        CoreOptions coreOptions = _optionsManager.Get<CoreOptions>(nameof(VACore));

        return coreFactory.CreateCore(coreOptions);
    }

    public PluginManager CreatePluginManager()
    {
        PluginManager pluginManager = new(_optionsManager.Get<PluginManagerOptions>(nameof(PluginManager)));
        pluginManager.LoadPlugins();

        return pluginManager;
    }

    public CommandManager CreateCommandManager()
    {
        CommandManager commandManager = new();

        try
        {
            PythonCommandHandler pythonCommandHandler = new(
                _optionsManager.Get<PythonCommandHandlerOptions>(nameof(PythonCommandHandler)));
            commandManager.AddHandler(CommandType.Python, pythonCommandHandler);
        }
        catch
        {
            Logger.Error("Не удалось инициализировать обработчик python");
        }

        commandManager.AddHandler(CommandType.Program, new ProgramCommandHandler());
        commandManager.AddHandler(CommandType.Json, new JsonCommandHandler());

        return commandManager;
    }

    public void SetupLogger()
    {
        FileLoggerOptions fileLoggerOptions = _optionsManager.Get<FileLoggerOptions>(nameof(FileLogger));
        FileLogger logger = new(fileLoggerOptions);

        Logger.SetLogger(logger);
        Logger.IsDebug = _argsParser.IsVerbose;
    }
}
