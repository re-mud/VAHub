using System.Diagnostics;
using VAHub.Enums;
using VAHub.Models;

namespace VAHub.Plugins;

public class ProgramPlugin : BasePlugin
{
    public ProgramPlugin(string path, Manifest manifest) : base(path, manifest)
    {

    }

    public override Response Execute(string path)
    {
        try
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = Path.Combine(PluginDir, path),
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
}
