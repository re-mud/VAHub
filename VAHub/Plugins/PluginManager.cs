using System.Text.Json;
using VAHub.Enums;
using VAHub.Logging;
using VAHub.Models;

namespace VAHub.Plugins;

public class PluginManager
{
    private List<BasePlugin> _plugins = [];
    private PluginFactory _pluginFactory;
    private PluginManagerOptions _options;

    public PluginManager(PluginManagerOptions options, PluginFactory pluginFactory)
    {
        _options = options;
        _pluginFactory = pluginFactory;

        LoadPlugins(_options.PluginsPath);
    }

    public Response Handle(string text)
    {
        foreach (var plugin in _plugins)
        {
            string? result = plugin.Handle(text);
            if (result != null)
            {
                Logger.Debug($"Исполнение '{result}' из '{plugin.Manifest.Name}'");

                try
                {
                    return plugin.Execute(result);
                }
                catch (Exception e)
                {
                    return new Response()
                    {
                        Status = Status.Error,
                        Message = e.ToString()
                    };
                }
            }
        }

        return new Response()
        {
            Status = Status.NotFound,
            Message = $"Команда '{text}' не найдена"
        };
    }

    /// <exception cref="ArgumentNullException"></exception>
    public void AddPlugin(BasePlugin plugin)
    {
        ArgumentNullException.ThrowIfNull(plugin, nameof(plugin));

        _plugins.Add(plugin);
    }

    public void LoadPlugins(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Logger.Warn($"Не удалось найти папку '{dirPath}'");
            return;
        }

        string[] directories = Directory.GetDirectories(dirPath);

        foreach (string directory in directories)
        {
            string manifestPath = Path.Combine(directory, "manifest.json");
            if (!File.Exists(manifestPath)) continue;

            Manifest manifest;
            try
            {
                string json = File.ReadAllText(manifestPath);
                manifest = JsonSerializer.Deserialize<Manifest>(json) ?? throw new ArgumentNullException();
            }
            catch
            {
                Logger.Warn($"Ошибка форматирования '{manifestPath}'");
                continue;
            }

            BasePlugin plugin;
            try
            {
                plugin = _pluginFactory.Create(manifest.Type, directory, manifest);
            }
            catch
            {
                Logger.Warn($"Не удалось загрузить плагин '{directory}' с типом '{manifest.Type}'");
                continue;
            }

            AddPlugin(plugin);
        }
    }
}