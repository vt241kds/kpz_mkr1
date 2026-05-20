//  (State)

interface IElementState
{
    string Render(LightStatefulElement element);
    void   Click(LightStatefulElement element);
    string StateName { get; }
}

class NormalState : IElementState
{
    public string StateName => "Normal";

    public string Render(LightStatefulElement el)
        => $"<{el.TagName}>{el.Content}</{el.TagName}>";

    public void Click(LightStatefulElement el)
        => Console.WriteLine($"<{el.TagName}> клікнуто!");
}

class HiddenState : IElementState
{
    public string StateName => "Hidden";

    public string Render(LightStatefulElement el)
        => $"<{el.TagName} style=\"display:none\">{el.Content}</{el.TagName}>";

    public void Click(LightStatefulElement el)
        => Console.WriteLine($"<{el.TagName}> прихований — клік ігнорується.");
}
class DisabledState : IElementState
{
    public string StateName => "Disabled";

    public string Render(LightStatefulElement el)
        => $"<{el.TagName} disabled>{el.Content}</{el.TagName}>";

    public void Click(LightStatefulElement el)
        => Console.WriteLine($"<{el.TagName}> вимкнено — клік заблоковано.");
}

class LightStatefulElement : LightNode
{
    public string TagName { get; }
    public string Content { get; set; }

    private IElementState _state;

    public LightStatefulElement(string tagName, string content)
    {
        TagName  = tagName;
        Content  = content;
        _state   = new NormalState();
    }

    public void SetState(IElementState state)
    {
        Console.WriteLine($"Стан <{TagName}>: {_state.StateName} → {state.StateName}");
        _state = state;
    }

    public void Click()   => _state.Click(this);

    public override string OuterHTML => _state.Render(this);
    public override string InnerHTML => Content;
}
