using System.Diagnostics;
using System.Text;
using VAHub.Commands.DTO;
using VAHub.Commands.Enums;

namespace VAHub.Commands.Handlers;

public class ProgramCommandHandler : BaseCommandHandler
{
    public override CommandResult Execute(string executeData, string path, string commandText)
    {
        if (string.IsNullOrEmpty(executeData))
            return new(Status.Error, "Пустая команда");

        string[] parts = ParseArgs(executeData);
        ReplaceArg(parts, "{text}", $"\"{commandText}\"");

        string file = Path.Combine(path, parts[0]);

        if (!File.Exists(file))
            return new(Status.Error, $"Не найден исполняемый файл '{file}'");

        string args = string.Join(" ", parts.Skip(1));

        try
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = file,
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = false
            });

            return new(Status.Success);
        }
        catch (Exception e)
        {
            return new(Status.Error, e.Message);
        }
    }

    private void ReplaceArg(string[] args, string oldValue, string newValue)
    {
        int index = Array.IndexOf(args, oldValue);

        if (index != -1)
            args[index] = newValue;
    }

    private string[] ParseArgs(string input)
    {
        var args = new List<string>();
        bool inQuotes = false;
        var current = new StringBuilder();

        foreach (char c in input)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ' ' && !inQuotes)
            {
                if (current.Length > 0)
                {
                    args.Add(current.ToString());
                    current.Clear();
                }
            }
            else
            {
                current.Append(c);
            }
        }

        if (current.Length > 0)
            args.Add(current.ToString());

        return args.ToArray();
    }
}
