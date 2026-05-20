// КОМАНДА (Command)

interface IHtmlCommand
{
    void Execute();
    void Undo();
}

class AddChildCommand : IHtmlCommand
{
    private readonly LightElementNode _parent;
    private readonly LightNode _child;

    public AddChildCommand(LightElementNode parent, LightNode child)
    {
        _parent = parent;
        _child  = child;
    }

    public void Execute()
    {
        _parent.Children.Add(_child);
        Console.WriteLine($"Додано дочірній елемент до <{_parent.TagName}>");
    }

    public void Undo()
    {
        _parent.Children.Remove(_child);
        Console.WriteLine($"Скасовано додавання елемента з <{_parent.TagName}>");
    }
}

class AddClassCommand : IHtmlCommand
{
    private readonly LightElementNode _element;
    private readonly string _className;

    public AddClassCommand(LightElementNode element, string className)
    {
        _element   = element;
        _className = className;
    }

    public void Execute()
    {
        _element.Classes.Add(_className);
        Console.WriteLine($"Додано клас '{_className}' до <{_element.TagName}>");
    }

    public void Undo()
    {
        _element.Classes.Remove(_className);
        Console.WriteLine($"Видалено клас '{_className}' з <{_element.TagName}>");
    }
}

class DomCommandInvoker
{
    private readonly Stack<IHtmlCommand> _history = new();

    public void Execute(IHtmlCommand command)
    {
        command.Execute();
        _history.Push(command);
    }

    public void Undo()
    {
        if (_history.Count > 0)
            _history.Pop().Undo();
        else
            Console.WriteLine("Нічого скасовувати.");
    }
}
