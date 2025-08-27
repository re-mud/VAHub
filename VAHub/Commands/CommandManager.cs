using VAHub.Commands.Handlers;
using VAHub.Models;
using VAHub.Enums;

namespace VAHub.Commands;

public class CommandManager
{
    private Dictionary<string, CommandModel> _commands = [];
    private Dictionary<CommandType, BaseCommandHandler> _handlers = [];

    public Report Handle(string text)
    {
        if (!_commands.TryGetValue(text, out CommandModel? command))
        {
            return Report.NotFound($"Команда '{text}' не найдена");
        }
        if (!_handlers.TryGetValue(command.Type, out BaseCommandHandler? handler))
        {
            return Report.NotFound($"Незарегистрированный тип '{command.Type}'");
        }

        try
        {
            return handler.Execute(command.ExecuteData, command.RelativePath, text);
        }
        catch (Exception e)
        {
            return Report.Error($"Необработанная ошибка: {e}");
        }
    }

    public void SetCommands(Dictionary<string, CommandModel> commands)
    {
        _commands = new Dictionary<string, CommandModel>(commands);
    }

    public void AddCommand(string text, CommandModel model)
    {
        _commands.Add(text, model);
    }

    public void ClearCommands()
    {
        _commands.Clear();
    }

    public void SetHandlers(Dictionary<CommandType, BaseCommandHandler> handlers)
    {
        _handlers = new Dictionary<CommandType, BaseCommandHandler>(handlers);
    }

    public void AddHandler(CommandType type, BaseCommandHandler handler)
    {
        _handlers.Add(type, handler);
    }

    public void ClearHandlers()
    {
        _handlers.Clear();
    }
}
