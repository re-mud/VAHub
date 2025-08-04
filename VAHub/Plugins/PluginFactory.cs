using VAHub.Models;

namespace VAHub.Plugins;

public class PluginFactory
{
    private readonly Dictionary<string, Func<string, Manifest, BasePlugin>> _creators = new();

    public void Register(string type, Func<string, Manifest, BasePlugin> creator)
    {
        _creators[type] = creator;
    }

    /// <exception cref="ArgumentException"></exception>
    public BasePlugin Create(string type, string path, Manifest manifest)
    {
        if (_creators.TryGetValue(type, out var creator))
            return creator(path, manifest);

        throw new ArgumentException($"Invalid type: {type}");
    }
}
