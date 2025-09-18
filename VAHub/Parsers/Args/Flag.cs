namespace VAHub.Parsers.Args;

public class Flag(string shortFlag, string longFlag, string description)
{
    public readonly string ShortFlag = shortFlag;
    public readonly string LongFlag = longFlag;
    public readonly string Description = description;
}
