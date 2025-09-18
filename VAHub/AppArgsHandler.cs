using VAHub.Parsers.Args;
using VAHub.Logging;

namespace VAHub;

public class AppArgsHandler(ArgsParser parser, ILogger logger) : IArgsHandler
{
    private readonly ArgsParser _parser = parser;
    private readonly ILogger _logger = logger;

    public void Initialize()
    {
        if (_parser.GetFlag("help"))
            _logger.Help("Общие настройки:\n" + _parser.GetHelp());
    }
}
