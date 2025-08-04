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
        Response response = new Response();

        try
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = Path.Combine(PluginDir, path),
                CreateNoWindow = true,
                UseShellExecute = false
            });
            response.Status = Status.Success;
        }
        catch (Exception e) 
        {
            response.Message = e.Message;
            response.Status = Status.Error;
        }

        return response;
    }
}
