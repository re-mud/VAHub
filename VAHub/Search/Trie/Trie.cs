namespace VAHub.Search.Trie;

public class Trie<T>
{
    private readonly TrieNode<T> _root = new();

    /// <summary>
    /// false if already contains the value from the key
    /// </summary>
    public bool Add(string key, T value)
    {
        TrieNode<T> node = _root;

        foreach (char c in key)
        {
            if (!node.Children.ContainsKey(c))
            {
                node.Children[c] = new();
            }
            node = node.Children[c];
        }
        if (node.IsEnd)
            return false;

        node.IsEnd = true;
        node.Value = value;
        return true;
    }

    public TrieStartWithResult<T>? StartWith(string text)
    {
        TrieNode<T> node = _root;
        int i = 0;

        foreach (char c in text)
        {
            if (!node.Children.ContainsKey(c))
            {
                if (node.IsEnd)
                {
                    return new TrieStartWithResult<T>(text.Substring(i), node.Value);
                }
                else
                {
                    return null;
                }
            }
            node = node.Children[c];
            i++;
        }

        if (node.IsEnd)
            return new TrieStartWithResult<T>("", node.Value);

        return null;
    }
}
