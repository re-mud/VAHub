using VAHub.Commands.Handlers;
using VAHub.Commands.DTO;
using VAHub.Search.Trie;
using VAHub.Commands.Enums;

namespace VAHub.Commands;

public class CommandManager
{
    private Trie<CommandModel> _commands = new();
    private Dictionary<CommandType, BaseCommandHandler> _handlers = [];

    public CommandManagerResult Handle(string text, CommandContext? context = null)
    {
        CommandModel command;
        string remainingText;

        if (context == null)
        {
            TrieStartWithResult<CommandModel>? result = _commands.StartWith(text);

            if (result == null)
            {
                return new(Status.NotFound, message: $"Команда '{text}' не найдена");
            }
            command = result.Value;
            remainingText = result.RemainingText;
        }
        else
        {
            command = new CommandModel(context.CommandData, context.CommandType, context.CommandPath);
            remainingText = text;
        }

        if (!_handlers.TryGetValue(command.Type, out BaseCommandHandler? handler))
        {
            return new(Status.NotFound, message: $"Незарегистрированный тип '{command.Type}'");
        }

        try
        {
            return HandleCommand(handler, command, remainingText);
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
        _commands = new Trie<CommandModel>();

        foreach (var command in commands)
        {
            _commands.Add(command.Key, command.Value);
        }
    }

    public void AddCommand(string text, CommandModel model)
    {
        _commands.Add(text, model);
    }

    public void ClearCommands()
    {
        _commands = new();
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
