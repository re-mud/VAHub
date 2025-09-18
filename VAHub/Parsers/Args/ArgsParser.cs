namespace VAHub.Parsers.Args;

public class ArgsParser
{
    private readonly Dictionary<string, Flag> _flags = [];
    private readonly HashSet<string> _founded = [];

    public void AddFlag(string key, Flag flag)
    {
        _flags[key] = flag;
    }

    public void Parse(string[] args)
    {
        _founded.Clear();

        foreach (var kvp in _flags)
        {
            if (args.Contains(kvp.Value.ShortFlag) ||
                args.Contains(kvp.Value.LongFlag))
                _founded.Add(kvp.Key);
        }
    }

    public bool GetFlag(string key)
    {
        return _founded.Contains(key);
    }

    public string GetHelp()
    {
        int max = _flags.Max(kvp => kvp.Value.LongFlag.Length + kvp.Value.ShortFlag.Length);
        return string.Join('\n', _flags.Select(kvp =>
        {
            string flag = $"{kvp.Value.ShortFlag}|{kvp.Value.LongFlag}";
            int indent = max - flag.Length + 9;
            return $"{flag}{new string(' ', indent)}{kvp.Value.Description}";
        }));
    }
}