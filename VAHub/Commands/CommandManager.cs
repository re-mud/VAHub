using VAHub.Commands.Handlers;
using VAHub.Commands.DTO;
using VAHub.Enums;

namespace VAHub.Commands;

public class CommandManager
{
    private Dictionary<string, CommandModel> _commands = [];
    private Dictionary<CommandType, BaseCommandHandler> _handlers = [];

    public CommandManagerResult Handle(string text, CommandContext? context = null)
    {
        CommandModel? command;
        if (context == null)
        {
            if (!_commands.TryGetValue(text, out command))
            {
                return new(Status.NotFound, message: $"Команда '{text}' не найдена");
            }
        }
        else
        {
            command = new CommandModel(context.CommandData, context.CommandType, context.CommandPath);
        }

        BaseCommandHandler? handler;
        if (!_handlers.TryGetValue(command.Type, out handler))
        {
            return new(Status.NotFound, message: $"Незарегистрированный тип '{command.Type}'");
        }

        try
        {
            return HandleCommand(handler, command, text);
        }
        catch (Exception e)
        {
            return new(Status.Error, message: $"Необработанная ошибка: {e}");
        }
    }

    private CommandManagerResult HandleCommand(BaseCommandHandler handler, CommandModel command, string text)
    {
        CommandResult commandResult = handler.Execute(command.ExecuteData, command.RelativePath, text);
        return new(commandResult.Status, command.Type, command.RelativePath, commandResult.Message, commandResult.CommandResponse);
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
