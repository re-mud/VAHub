namespace VAHub.Search.Trie;

public class TrieStartWithResult<T>
{
    public readonly string RemainingText;

    public readonly T Value;

    public TrieStartWithResult(string RemainingText, T Value)
    {
        this.RemainingText = RemainingText;
        this.Value = Value;
    }
}
