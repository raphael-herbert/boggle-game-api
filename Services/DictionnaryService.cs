public class DictionaryService : IDictionaryService
{
    private Trie dictionaryTrie = new Trie();

    public DictionaryService()    
    {
        LoadDictionaryAsync().Wait();
    }

    private async Task LoadDictionaryAsync()
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, "assets", "french.txt");
        string response = await File.ReadAllTextAsync(filePath);
        string[] words = response.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        dictionaryTrie = new Trie();
        foreach (string word in words)
        {
            dictionaryTrie.Insert(word.Trim().ToUpper());
        }
    }

    public bool HasWord(string word)
    {
        return dictionaryTrie.HasWord(word) && word.Length >= 3;
    }

    public bool HasPrefix(string prefix)
    {
        return dictionaryTrie.HasPrefix(prefix);
    }
}

public class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; set; } = new Dictionary<char, TrieNode>();
    public bool IsWord { get; set; } = false;
}

public class Trie
{
    public TrieNode Root { get; set; } = new TrieNode();

    public void Insert(string word)
    {
        TrieNode node = Root;

        foreach (char letter in word)
        {
            if (!node.Children.ContainsKey(letter))
            {
                node.Children[letter] = new TrieNode();
            }
            node = node.Children[letter];
        }

        node.IsWord = true;
    }

    public TrieNode? SearchPrefix(string prefix)
    {
        TrieNode node = Root;

        foreach (char letter in prefix)
        {
            if (!node.Children.ContainsKey(letter))
            {
                return null;
            }
            node = node.Children[letter];
        }

        return node;
    }

    public bool HasWord(string word)
    {
        TrieNode? node = SearchPrefix(word);
        return node != null && node.IsWord;
    }

    public bool HasPrefix(string prefix)
    {
        return SearchPrefix(prefix) != null;
    }
}

