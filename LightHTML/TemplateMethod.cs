// ШАБЛОННИЙ МЕТОД (Template Method)
abstract class LightElementWithHooks : LightNode
{
    public string TagName { get; }

    protected LightElementWithHooks(string tagName)
    {
        TagName = tagName;
        OnCreated();
    }

    public string Render()
    {
        OnInserted();
        string html = BuildHTML();
        OnRendered();
        return html;
    }

    public void Remove()
    {
        OnRemoved();
    }

    protected virtual void OnCreated()   => Console.WriteLine($"[{TagName}] OnCreated");
    protected virtual void OnInserted()  => Console.WriteLine($"[{TagName}] OnInserted");
    protected virtual void OnRendered()  => Console.WriteLine($"[{TagName}] OnRendered");
    protected virtual void OnRemoved()   => Console.WriteLine($"[{TagName}] OnRemoved");

    protected abstract string BuildHTML();

    public override string OuterHTML => BuildHTML();
    public override string InnerHTML => BuildHTML();
}

class TrackedDiv : LightElementWithHooks
{
    private string content;

    public TrackedDiv(string content) : base("div")
    {
        this.content = content;
    }

    protected override void OnCreated()  => Console.WriteLine($"[div] Елемент створено з вмістом: '{content}'");
    protected override void OnInserted() => Console.WriteLine($"[div] Елемент додано до DOM");
    protected override void OnRendered() => Console.WriteLine($"[div] Елемент відрендерено");
    protected override void OnRemoved()  => Console.WriteLine($"[div] Елемент видалено з DOM");

    protected override string BuildHTML() => $"<div>{content}</div>";
}
