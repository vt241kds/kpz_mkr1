using System.Text;

enum DisplayType { Block, Inline }
enum CloseType   { SelfClosing, Normal }

class LightElementNode : LightNode
{
    public string      TagName { get; set; }
    public DisplayType Display { get; set; }
    public CloseType   Close   { get; set; }

    public List<string>    Classes  { get; set; } = new();
    public List<LightNode> Children { get; set; } = new();

    private Dictionary<string, List<Action>> eventListeners = new();

    public LightElementNode(string tagName,
                            DisplayType display = DisplayType.Block,
                            CloseType close     = CloseType.Normal)
    {
        TagName = tagName;
        Display = display;
        Close   = close;
    }

    public void AddChild(LightNode child) => Children.Add(child);

    public void AddEventListener(string eventName, Action handler)
    {
        if (!eventListeners.ContainsKey(eventName))
            eventListeners[eventName] = new List<Action>();

        eventListeners[eventName].Add(handler);
    }
    public void RemoveEventListener(string eventName, Action handler)
    {
        if (eventListeners.ContainsKey(eventName))
            eventListeners[eventName].Remove(handler);
    }

    public void TriggerEvent(string eventName)
    {
        if (eventListeners.ContainsKey(eventName))
            foreach (var handler in eventListeners[eventName])
                handler.Invoke();
    }

    public int ChildCount => Children.Count;

    public override string InnerHTML
    {
        get
        {
            var sb = new StringBuilder();
            foreach (var child in Children)
                sb.Append(child.OuterHTML);
            return sb.ToString();
        }
    }

    public override string OuterHTML
    {
        get
        {
            string classAttr = Classes.Count > 0
                ? $" class=\"{string.Join(" ", Classes)}\""
                : "";

            if (Close == CloseType.SelfClosing)
                return $"<{TagName}{classAttr}/>";

            return $"<{TagName}{classAttr}>{InnerHTML}</{TagName}>";
        }
    }
}
