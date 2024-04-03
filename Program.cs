using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using SimpleInterpreter.Interpreter;
using SimpleInterpreter.Lexer;
using SimpleInterpreter.Parser;

// ref -> https://stackoverflow.com/a/8946847/15407937
void ClearCurrentConsoleLine()
{
    int currentLineCursor = Console.CursorTop;
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write("\r");
    Console.SetCursorPosition(0, currentLineCursor);
}

void repl()
{
    // ref -> https://stackoverflow.com/a/929717/15407937
    Console.CancelKeyPress += (sender, e) =>
    {
        e.Cancel = true;

        // Console.SetCursorPosition(0, Console.CursorTop - 1);
        ClearCurrentConsoleLine();

        Environment.Exit(0);
    };

    while (true)
    {
        try
        {
            Console.Write("> ");
            var src = Console.ReadLine()!;
            run(src);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

void run(string src)
{
    var lexer = new Lexer(src);
    var tokens = lexer.scan();

    var parser = new Parser(tokens);
    var statments = parser.prase();

    var interpreter = new Interpreter(statments);
    foreach (var result in interpreter.cal())
    {
        Console.WriteLine(result);
    }
}

async Task run_file(string path)
{
    var src = await File.ReadAllTextAsync(path);
    run(src);
}

async Task main()
{
    var rootCommand = new RootCommand();

    var fileOption = new Option<string>
    (aliases: new string[] { "--file", },
    description: "",
    getDefaultValue: () => "");
    rootCommand.AddOption(fileOption);

    rootCommand.Handler = CommandHandler.Create<string>(async file =>
    {
        if (string.IsNullOrEmpty(file))
        {
            repl();
        }
        else
        {
            await run_file(file);
        }
    });
    await rootCommand.InvokeAsync(args);
}

await main();