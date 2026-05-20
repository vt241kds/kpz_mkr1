// ІТЕРАТОР (Iterator)

interface IHtmlIterator
{
    bool HasNext();
    LightNode Next();
    void Reset();
}

class DepthFirstIterator : IHtmlIterator
{
    private readonly List<LightNode> _nodes = new();
    private int _index = 0;

    public DepthFirstIterator(LightElementNode root)
    {
        Traverse(root);
    }

    private void Traverse(LightNode node)
    {
        _nodes.Add(node);
        if (node is LightElementNode element)
            foreach (var child in element.Children)
                Traverse(child);
    }

    public bool HasNext() => _index < _nodes.Count;
    public LightNode Next() => _nodes[_index++];
    public void Reset() => _index = 0;
}

class BreadthFirstIterator : IHtmlIterator
{
    private readonly List<LightNode> _nodes = new();
    private int _index = 0;

    public BreadthFirstIterator(LightElementNode root)
    {
        Traverse(root);
    }

    private void Traverse(LightElementNode root)
    {
        var queue = new Queue<LightNode>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            _nodes.Add(node);

            if (node is LightElementNode element)
                foreach (var child in element.Children)
                    queue.Enqueue(child);
        }
    }

    public bool HasNext() => _index < _nodes.Count;
    public LightNode Next() => _nodes[_index++];
    public void Reset() => _index = 0;
}

class HtmlDocument
{
    private LightElementNode _root;

    public HtmlDocument(LightElementNode root)
    {
        _root = root;
    }

    public IHtmlIterator GetDepthFirstIterator()   => new DepthFirstIterator(_root);
    public IHtmlIterator GetBreadthFirstIterator() => new BreadthFirstIterator(_root);
}
