using VAHub.Logging;
using VAHub.Services;

namespace VAHub;

public class App
{
    private Core _core;
    private ManualResetEventSlim _mre;

    public App(Core core)
    {
        _core = core;
        _mre = new ManualResetEventSlim(false);

        _core.RecognitionCompleted += Core_RecognitionCompleted;
    }

    public void Run()
    {
        _core.Start();
        Logger.Info("Приложение запущено");
        _mre.Wait();
    }

    public void Exit()
    {
        _mre.Set();
    }

    private void Core_RecognitionCompleted(string text)
    {
        Console.WriteLine(text);
        _core.Speak(text);
    }
}