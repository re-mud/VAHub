using VAHub.Models;

namespace VAHub.Plugins;

public class PluginModel
{
    public PluginModel(Manifest manifest, string path)
    {
        Manifest = manifest;
        Path = path;
    }

    public Manifest Manifest { get; set; }
    public string Path { get; set; }
}
