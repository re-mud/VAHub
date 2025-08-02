using VAHub.Input;
using VAHub.Managers;
using VAHub.Recognize;
using VAHub.Services;
using VAHub.Synthesize;

namespace VAHub.Factories;

public class CoreFactory
{
    private Dictionary<string, Func<IMicrophone>> _microphonesFactories = new();
    private Dictionary<string, Func<ISpeechRecognition>> _speechRecognitionsFactories = new();
    private Dictionary<string, Func<ISpeechSynthesizer>> _speechSynthesizersFactories = new();

    private OptionsManager _optionsManager;

    public CoreFactory(OptionsManager optionsManager)
    {
        _optionsManager = optionsManager;

        _microphonesFactories.Add(nameof(NAudioMicrophone),
            () => CreateWithOptions<NAudioMicrophone, NAudioMicrophoneOptions>());
        _speechRecognitionsFactories.Add(nameof(VoskSpeechRecognition),
            () => CreateWithOptions<VoskSpeechRecognition, VoskSpeechRecognitionOptions>());
        _speechSynthesizersFactories.Add(nameof(WindowsSpeechSynthesizer),
            () => CreateWithOptions<WindowsSpeechSynthesizer, WindowsSpeechSynthesizerOptions>());
    }

    public Core CreateCore(CoreOptions options)
    {
        IMicrophone microphone = CreateMicrophone(options.MicrophoneKey) ?? throw new KeyNotFoundException(options.MicrophoneKey);
        ISpeechRecognition recognition = CreateSpeechRecognition(options.RecognitionKey) ?? throw new KeyNotFoundException(options.RecognitionKey);
        ISpeechSynthesizer synthesizer = CreateSpeechSynthesizer(options.SynthesizerKey) ?? throw new KeyNotFoundException(options.SynthesizerKey);

        return new Core(microphone, recognition, synthesizer);
    }

    public IMicrophone? CreateMicrophone(string key)
    {
        if (_microphonesFactories.TryGetValue(key, out Func<IMicrophone>? createMicrophone))
            return createMicrophone();
        return null;
    }

    public ISpeechRecognition? CreateSpeechRecognition(string key)
    {
        if (_speechRecognitionsFactories.TryGetValue(key, out Func<ISpeechRecognition>? createRecognition))
            return createRecognition();
        return null;
    }

    public ISpeechSynthesizer? CreateSpeechSynthesizer(string key)
    {
        if (_speechSynthesizersFactories.TryGetValue(key, out Func<ISpeechSynthesizer>? createSynthesizer))
            return createSynthesizer();
        return null;
    }

    private TImplementation CreateWithOptions<TImplementation, TOptions>()
        where TImplementation : class
        where TOptions : new()
    {
        TOptions options = _optionsManager.Get<TOptions>(typeof(TImplementation).Name);
        return (TImplementation)Activator.CreateInstance(typeof(TImplementation), options);
    }
}