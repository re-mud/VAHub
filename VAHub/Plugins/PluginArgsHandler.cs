using VAHub.Parsers.Args;
using VAHub.Commands;
using VAHub.Logging;

namespace VAHub.Plugins;

public class PluginArgsHandler(ArgsParser parser, ILogger logger, PluginManager pluginManager) : IArgsHandler
{
    private readonly ArgsParser _parser = parser;
    private readonly ILogger _logger = logger;
    private readonly PluginManager _pluginManager = pluginManager;

    public void Initialize()
    {
        if (_parser.GetFlag("commands"))
        {
            Dictionary<string, CommandModel> _commands = _pluginManager.GetCommands();
            _logger.Help("Загруженные команды:\n- " + string.Join("\n- ", _commands.Keys));
        }
    }
}
