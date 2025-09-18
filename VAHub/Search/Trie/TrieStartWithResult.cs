namespace VAHub.Search.Trie;

public class TrieStartWithResult<T>(string RemainingText, T Value)
{
    public readonly string RemainingText = RemainingText;

    public readonly T Value = Value;
}
