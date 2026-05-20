class LightTextNode : LightNode
{
    public string Text { get; set; }

    public LightTextNode(string text)
    {
        Text = text;
    }

    public override string OuterHTML => Text;
    public override string InnerHTML => Text;
}
