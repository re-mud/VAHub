using System.Diagnostics;
using VAHub.Models;

namespace VAHub.Plugins;

public abstract class BasePlugin
{
    public readonly string PluginDir;
    public readonly Manifest Manifest;

    private Dictionary<string, string> _commands = new();

    public BasePlugin(string dir, Manifest manifest)
    {
        PluginDir = dir;
        Manifest = manifest;

        foreach (var command in manifest.Commands)
        {
            foreach (string item in command.Key.Split('|'))
            {
                _commands[item] = command.Value;
            }
        }
    }

    public string? Handle(string text)
    {
        if (_commands.TryGetValue(text, out var value))
        {
            return value;
        }

        return null;
    }

    public abstract Response Execute(string path);
}
