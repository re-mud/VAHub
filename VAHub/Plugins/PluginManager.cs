using System.Text.Json;
using VAHub.Commands;
using VAHub.Logging;
using VAHub.Models;

namespace VAHub.Plugins;

public class PluginManager
{
    private PluginManagerOptions _options;
    private List<PluginModel> _plugins = [];

    public PluginManager(PluginManagerOptions options)
    {
        _options = options;
    }

    public void LoadPlugins()
    {
        if (!Directory.Exists(_options.PluginsPath))
        {
            Logger.Warn($"Не удалось найти папку '{_options.PluginsPath}'");
            return;
        }

        if (_plugins.Count > 0)
        {
            _plugins.Clear();
        }

        string[] directories = Directory.GetDirectories(_options.PluginsPath);

        foreach (string directory in directories)
        {
            LoadPlugin(directory);
        }
    }

    public void LoadPlugin(string dirPath)
    {
        string manifestPath = Path.Combine(dirPath, "manifest.json");

        if (!File.Exists(manifestPath)) return;

        Manifest manifest;
        try
        {
            string json = File.ReadAllText(manifestPath);
            manifest = JsonSerializer.Deserialize<Manifest>(json) ?? throw new ArgumentNullException();
        }
        catch
        {
            Logger.Warn($"Не удалось загрузить манифест '{dirPath}'");
            return;
        }

        _plugins.Add(new(manifest, dirPath));
        Logger.Debug($"Загружен плагин {manifest.Name} (v{manifest.Version})");
    }

    public PluginModel[] GetPlugins()
    {
        return _plugins.ToArray();
    }

    public Dictionary<string, CommandModel> GetCommands()
    {
        Dictionary<string, CommandModel> commands = [];

        foreach (var plugin in _plugins)
        {
            foreach (var command in plugin.Manifest.Commands)
            {
                foreach (var text in command.Key.Split('|'))
                {
                    bool state = commands.TryAdd(
                        text,
                        new(command.Value, plugin.Manifest.Type, plugin.Path)
                    );

                    if (!state)
                    {
                        Logger.Warn($"Команда '{text}' из '{plugin.Path}' уже была добавлена");
                    }
                }
            }
        }

        return commands;
    }
}