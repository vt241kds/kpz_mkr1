// ВІДВІДУВАЧ (Visitor)

interface IHtmlVisitor
{
    void Visit(LightElementNode element);
    void Visit(LightTextNode text);
}

class TextExtractorVisitor : IHtmlVisitor
{
    private readonly List<string> _texts = new();

    public void Visit(LightElementNode element)
    {
    }

    public void Visit(LightTextNode text)
    {
        _texts.Add(text.Text);
    }

    public string GetText() => string.Join(" ", _texts);
}

class TagCounterVisitor : IHtmlVisitor
{
    private readonly Dictionary<string, int> _counts = new();

    public void Visit(LightElementNode element)
    {
        if (!_counts.ContainsKey(element.TagName))
            _counts[element.TagName] = 0;
        _counts[element.TagName]++;
    }

    public void Visit(LightTextNode text) { }

    public void PrintStats()
    {
        Console.WriteLine("Статистика тегів:");
        foreach (var kv in _counts)
            Console.WriteLine($"  <{kv.Key}>: {kv.Value} шт.");
    }
}
class PrettyPrintVisitor : IHtmlVisitor
{
    private int _depth = 0;
    private readonly System.Text.StringBuilder _sb = new();

    public void Visit(LightElementNode element)
    {
        string indent = new string(' ', _depth * 2);
        string classes = element.Classes.Count > 0
            ? $" class=\"{string.Join(" ", element.Classes)}\""
            : "";

        _sb.AppendLine($"{indent}<{element.TagName}{classes}>");
        _depth++;

        foreach (var child in element.Children)
        {
            if (child is LightElementNode el) Visit(el);
            else if (child is LightTextNode txt) Visit(txt);
        }

        _depth--;
        _sb.AppendLine($"{indent}</{element.TagName}>");
    }

    public void Visit(LightTextNode text)
    {
        string indent = new string(' ', _depth * 2);
        _sb.AppendLine($"{indent}{text.Text}");
    }

    public string GetResult() => _sb.ToString();
}

static class LightNodeVisitorExtensions
{
    public static void Accept(this LightNode node, IHtmlVisitor visitor)
    {
        if (node is LightElementNode element)
        {
            visitor.Visit(element);
            foreach (var child in element.Children)
                child.Accept(visitor);
        }
        else if (node is LightTextNode text)
        {
            visitor.Visit(text);
        }
    }
}
