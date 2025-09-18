namespace VAHub.Plugins;

public class PluginModel(Manifest manifest, string path)
{
    public Manifest Manifest { get; set; } = manifest;
    public string Path { get; set; } = path;
}
