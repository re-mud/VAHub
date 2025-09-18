using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VAHub.Configurations;
using VAHub;



IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true)
    .Build();
IServiceProvider serviceProvider = new ServiceCollection()
    .AddServices(configuration)
    .AddArgs(args)
    .BuildServiceProvider();

App app = serviceProvider.GetRequiredService<App>();
app.Run();