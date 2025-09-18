using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VAHub.Configurations;
using VAHub.Parsers.Args;
using VAHub.Logging;
using VAHub;



IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true)
    .Build();
IServiceProvider serviceProvider = new ServiceCollection()
    .AddServices(configuration)
    .BuildServiceProvider();

ILogger logger = serviceProvider.GetRequiredService<ILogger>();
ArgsParser parser = serviceProvider.GetRequiredService<ArgsParser>();
parser.Parse(args);
logger.SetLevelEnabled(LogLevel.Debug, parser.GetFlag("verbose"));
foreach (var hdl in serviceProvider.GetServices<IArgsHandler>())
    hdl.Initialize();

App app = serviceProvider.GetRequiredService<App>();
app.Run();