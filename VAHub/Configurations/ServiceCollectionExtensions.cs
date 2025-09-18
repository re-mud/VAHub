using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using VAHub.Commands.Handlers;
using VAHub.Parsers.Args;
using VAHub.Recognition;
using VAHub.Synthesis;
using VAHub.Commands;
using VAHub.Plugins;
using VAHub.Logging;
using VAHub.Input;
using VAHub.Core;

namespace VAHub.Configurations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // configurations
        services.Configure<WindowsSpeechSynthesizerOptions>(configuration.GetSection(nameof(WindowsSpeechSynthesizer)));
        services.Configure<VoskSpeechRecognitionOptions>(configuration.GetSection(nameof(VoskSpeechRecognition)));
        services.Configure<PythonCommandHandlerOptions>(configuration.GetSection(nameof(PythonCommandHandler)));
        services.Configure<NAudioMicrophoneOptions>(configuration.GetSection(nameof(NAudioMicrophone)));
        services.Configure<PluginManagerOptions>(configuration.GetSection(nameof(PluginManager)));
        services.Configure<FileLoggerOptions>(configuration.GetSection(nameof(FileLogger)));
        services.Configure<VACoreOptions>(configuration.GetSection(nameof(VACore)));
        services.Configure<AppOptions>(configuration.GetSection(nameof(App)));

        // options
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<WindowsSpeechSynthesizerOptions>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<VoskSpeechRecognitionOptions>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<PythonCommandHandlerOptions>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<NAudioMicrophoneOptions>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<PluginManagerOptions>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<FileLoggerOptions>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<VACoreOptions>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppOptions>>().Value);

        // command handlers
        services.AddKeyedTransient<BaseCommandHandler, ProgramCommandHandler>(CommandType.Program);
        services.AddKeyedTransient<BaseCommandHandler, PythonCommandHandler>(CommandType.Python);
        services.AddKeyedTransient<BaseCommandHandler, JsonCommandHandler>(CommandType.Json);

        // args handlers
        services.AddTransient<IArgsHandler, PluginArgsHandler>();
        services.AddTransient<IArgsHandler, AppArgsHandler>();

        // services
        services.AddKeyedTransient<ISpeechSynthesizer, WindowsSpeechSynthesizer>(nameof(WindowsSpeechSynthesizer));
        services.AddKeyedTransient<ISpeechSynthesizer, ConsoleSpeechSynthesizer>(nameof(ConsoleSpeechSynthesizer));
        services.AddKeyedTransient<ISpeechRecognition, VoskSpeechRecognition>(nameof(VoskSpeechRecognition));
        services.AddTransient(CreateCommandManager);
        services.AddTransient(CreateVACore);
        services.AddTransient<IMicrophone, NAudioMicrophone>();
        services.AddTransient<App>();

        services.AddSingleton(CreatePluginManager);
        services.AddSingleton(CreateArgsParser);
        services.AddSingleton<ILogger, FileLogger>();

        return services;
    }

    private static ArgsParser CreateArgsParser(IServiceProvider provider)
    {
        ArgsParser parser = new ArgsParser();
        parser.AddFlag("help", new("-h", "--help", "справка"));
        parser.AddFlag("verbose", new("-v", "--verbose", "подробный вывод"));
        parser.AddFlag("commands", new("-c", "--commands", "список команд"));
        return parser;
    }

    private static VACore CreateVACore(IServiceProvider provider)
    {
        VACoreOptions options = provider.GetRequiredService<VACoreOptions>();
        IMicrophone microphone = provider.GetRequiredService<IMicrophone>();
        ILogger logger = provider.GetRequiredService<ILogger>();
        ISpeechSynthesizer synthesizer = provider.GetRequiredKeyedService<ISpeechSynthesizer>(options.SynthesizerKey);
        ISpeechRecognition recognition = provider.GetRequiredKeyedService<ISpeechRecognition>(options.RecognitionKey);

        return new(logger, microphone, recognition, synthesizer);
    }

    private static CommandManager CreateCommandManager(IServiceProvider provider)
    {
        PluginManager pluginManager = provider.GetRequiredService<PluginManager>();
        CommandManager commandManager = new();
        commandManager.AddHandler(CommandType.Python, provider.GetRequiredKeyedService<BaseCommandHandler>(CommandType.Python));
        commandManager.AddHandler(CommandType.Program, provider.GetRequiredKeyedService<BaseCommandHandler>(CommandType.Program));
        commandManager.AddHandler(CommandType.Json, provider.GetRequiredKeyedService<BaseCommandHandler>(CommandType.Json));
        commandManager.SetCommands(pluginManager.GetCommands());
        return commandManager;
    }

    private static PluginManager CreatePluginManager(IServiceProvider provider)
    {
        PluginManagerOptions options = provider.GetRequiredService<PluginManagerOptions>();
        ILogger logger = provider.GetRequiredService<ILogger>();
        PluginManager pluginManager = new(logger, options);
        pluginManager.LoadPlugins();
        return pluginManager;
    }
}
