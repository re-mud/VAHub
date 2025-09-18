namespace VAHub.Parsers;

public class ArgsParser
{
    public readonly bool IsVerbose;
    public readonly bool ShowCommands;
    public readonly bool ShowHelp;

    public ArgsParser(string[]? args = null)
    {
        args ??= [];

        if (args.Length == 0) return;

        IsVerbose = args.Contains("--verbose") || args.Contains("-v");

        ShowCommands = args.Contains("--commands") || args.Contains("-c");

        ShowHelp = args.Contains("--help") || args.Contains("-h");
    }
}
