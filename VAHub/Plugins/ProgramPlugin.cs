using System.Diagnostics;
using System.Text;
using VAHub.Enums;
using VAHub.Models;

namespace VAHub.Plugins;

public class ProgramPlugin : BasePlugin
{
    public ProgramPlugin(string path, Manifest manifest) : base(path, manifest)
    {

    }

    public override Response Execute(string command, string text)
    {
        try
        {
            string[] parts = ParseArgs(command);
            ReplaceArg(parts, "{text}", $"\"{text}\"");

            string file = Path.Combine(PluginDir, parts[0]);
            string args = string.Join(" ", parts.Skip(1));

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
