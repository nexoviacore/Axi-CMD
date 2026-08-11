using System;

namespace AxiApi.Lib.Utils;

public class Trie
{
    private sealed class Node
    {
        public Dictionary<char, Node> Children = new();
        public bool IsWord;
    }

    private readonly Node _root = new();

    public void Insert(string word)
    {
        var node = _root;

        foreach (var c in word.ToLowerInvariant())
        {
            if (!node.Children.TryGetValue(c, out var next))
                node.Children[c] = next = new Node();

            node = next;
        }

        node.IsWord = true;
    }

    public List<string> Search(string prefix, int limit = 10)
    {
        var results = new List<string>();
        var node = _root;

        foreach (var c in prefix.ToLowerInvariant())
        {
            if (!node.Children.TryGetValue(c, out node!))
                return results;
        }

        DFS(node, prefix.ToLowerInvariant(), results, limit);
        return results;
    }

    private void DFS(Node node, string path, List<string> res, int limit)
    {
        if (res.Count >= limit)
            return;

        if (node.IsWord)
            res.Add(path);

        foreach (var (c, next) in node.Children)
            DFS(next, path + c, res, limit);
    }
}
