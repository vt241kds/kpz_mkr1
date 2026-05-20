using System.Text;

Console.InputEncoding  = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

DemoTemplateMethod();
Pause();

DemoIterator();
Pause();

DemoCommand();
Pause();

DemoState();
Pause();

DemoVisitor();

Console.WriteLine("\nГотово. Натисніть будь-яку клавішу...");
Console.ReadKey();

static void Pause()
{
    Console.WriteLine("\nНатисніть Enter для продовження...");
    Console.ReadLine();
}

// ── 1. Шаблонний метод ────────────────────────────────────────────
static void DemoTemplateMethod()
{
    Console.WriteLine("===== TEMPLATE METHOD (lifecycle hooks) =====");

    var div = new TrackedDiv("Привіт, світ!");

    Console.WriteLine("\nРендеримо елемент:");
    string html = div.Render();
    Console.WriteLine(html);

    Console.WriteLine("\nВидаляємо елемент:");
    div.Remove();

    Console.WriteLine();
}

// ── 2. Ітератор ───────────────────────────────────────────────────
static void DemoIterator()
{
    Console.WriteLine("===== ITERATOR (DFS та BFS) =====");

    var html = new LightElementNode("html");
    var head = new LightElementNode("head");
    var title = new LightElementNode("title");
    title.AddChild(new LightTextNode("Моя сторінка"));
    head.AddChild(title);

    var body = new LightElementNode("body");
    var h1 = new LightElementNode("h1");
    h1.AddChild(new LightTextNode("Заголовок"));
    var p = new LightElementNode("p");
    p.AddChild(new LightTextNode("Параграф"));
    body.AddChild(h1);
    body.AddChild(p);

    html.AddChild(head);
    html.AddChild(body);

    var doc = new HtmlDocument(html);

    Console.WriteLine("Обхід в глибину (DFS):");
    var dfs = doc.GetDepthFirstIterator();
    while (dfs.HasNext())
    {
        var node = dfs.Next();
        if (node is LightElementNode el)
            Console.WriteLine($"  <{el.TagName}>");
        else if (node is LightTextNode txt)
            Console.WriteLine($"  \"{txt.Text}\"");
    }

    Console.WriteLine("\nОбхід в ширину (BFS):");
    var bfs = doc.GetBreadthFirstIterator();
    while (bfs.HasNext())
    {
        var node = bfs.Next();
        if (node is LightElementNode el)
            Console.WriteLine($"  <{el.TagName}>");
        else if (node is LightTextNode txt)
            Console.WriteLine($"  \"{txt.Text}\"");
    }

    Console.WriteLine();
}

// ── 3. Команда ────────────────────────────────────────────────────
static void DemoCommand()
{
    Console.WriteLine("===== COMMAND (Undo/Redo DOM операцій) =====");

    var div  = new LightElementNode("div");
    var span = new LightElementNode("span", DisplayType.Inline);
    span.AddChild(new LightTextNode("текст"));

    var invoker = new DomCommandInvoker();

    Console.WriteLine("Додаємо дочірній елемент:");
    invoker.Execute(new AddChildCommand(div, span));
    Console.WriteLine($"HTML: {div.OuterHTML}");

    Console.WriteLine("\nДодаємо клас:");
    invoker.Execute(new AddClassCommand(div, "container"));
    Console.WriteLine($"HTML: {div.OuterHTML}");

    Console.WriteLine("\nUndo класу:");
    invoker.Undo();
    Console.WriteLine($"HTML: {div.OuterHTML}");

    Console.WriteLine("\nUndo дочірнього елементу:");
    invoker.Undo();
    Console.WriteLine($"HTML: {div.OuterHTML}");

    Console.WriteLine();
}

// ── 4. Стейт ──────────────────────────────────────────────────────
static void DemoState()
{
    Console.WriteLine("===== STATE (стани елемента) =====");

    var button = new LightStatefulElement("button", "Натисни мене");

    Console.WriteLine("Normal стан:");
    Console.WriteLine(button.OuterHTML);
    button.Click();

    Console.WriteLine("\nПереходимо в Disabled:");
    button.SetState(new DisabledState());
    Console.WriteLine(button.OuterHTML);
    button.Click();

    Console.WriteLine("\nПереходимо в Hidden:");
    button.SetState(new HiddenState());
    Console.WriteLine(button.OuterHTML);
    button.Click();

    Console.WriteLine("\nПовертаємо в Normal:");
    button.SetState(new NormalState());
    Console.WriteLine(button.OuterHTML);
    button.Click();

    Console.WriteLine();
}

// ── 5. Відвідувач ─────────────────────────────────────────────────
static void DemoVisitor()
{
    Console.WriteLine("===== VISITOR =====");

    var article = new LightElementNode("article");
    article.Classes.Add("post");

    var h2 = new LightElementNode("h2");
    h2.AddChild(new LightTextNode("Заголовок статті"));

    var p1 = new LightElementNode("p");
    p1.AddChild(new LightTextNode("Перший параграф."));

    var p2 = new LightElementNode("p");
    p2.AddChild(new LightTextNode("Другий параграф."));

    article.AddChild(h2);
    article.AddChild(p1);
    article.AddChild(p2);

    // Відвідувач 1: витяг тексту
    var textExtractor = new TextExtractorVisitor();
    article.Accept(textExtractor);
    Console.WriteLine("Весь текст документу:");
    Console.WriteLine("  " + textExtractor.GetText());

    // Відвідувач 2: статистика тегів
    Console.WriteLine();
    var counter = new TagCounterVisitor();
    article.Accept(counter);
    counter.PrintStats();

    // Відвідувач 3: pretty print
    Console.WriteLine();
    Console.WriteLine("Pretty HTML:");
    var printer = new PrettyPrintVisitor();
    article.Accept(printer);
    Console.Write(printer.GetResult());

    Console.WriteLine();
}
