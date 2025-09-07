using VAHub.Managers;
using VAHub.Parsers;
using VAHub;



OptionsManager optionsManager = new("appsettings.json", true);
ArgsParser argsParser = new(args);
AppInitializer appInitializer = new(optionsManager, argsParser);
App app = appInitializer.InitializeApp();

app.Run();