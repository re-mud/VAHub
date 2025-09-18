namespace VAHub.Search.Trie;

public class TrieNode<T>
{
    public Dictionary<char, TrieNode<T>> Children = [];
    public T? Value = default;
    public bool IsEnd = false;
}
