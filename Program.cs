namespace TimeLog;

using System.CommandLine;
using TimeLog.Parser;
using TimeLog.Lexer;
using Avalonia;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        // parameters
        // -period|p day|week|month  d|w|m  (default: d)
        // -code|c "foo" (default: "")
        // -task|t "foo" (default: "")
        var periodOption = new Option<string?>("period", "--period", "-p");
        var codeOption = new Option<string?>("code", "--code", "-c");
        var taskOption = new Option<string?>("task", "--task", "-t");

        // commands
        // --report|r  (default: -p d)
        // --list|l (default: -p d)
        var reportCommand = new System.CommandLine.Command("report", "Report time");
        reportCommand.Options.Add(periodOption);
        reportCommand.Options.Add(codeOption);
        reportCommand.Options.Add(taskOption);
        reportCommand.SetAction(Report);

        var listCommand = new System.CommandLine.Command("list", "List entries");
        listCommand.Options.Add(periodOption);
        listCommand.Options.Add(codeOption);
        listCommand.Options.Add(taskOption);
        listCommand.SetAction(List);

        var rootCommand = new RootCommand("TimeLog cli")
        {
            reportCommand,
            listCommand
        };
        rootCommand.SetAction((p) => Start(p, args));

        var cmd = rootCommand.Parse(args);

        cmd.Invoke();
    }

    private static void Start(ParseResult result, params string[] args)
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .UseHarfBuzz()
            .LogToTrace();

    static void Report(ParseResult parseResult)
    {
        var period = parseResult.GetValue<string>("period") ?? "day";
        var code = parseResult.GetValue<string>("code") ?? "";
        var task = parseResult.GetValue<string>("task") ?? "";

        var lines = Array.Empty<string>();

        // TODO: config
        //if (File.Exists("Timesheet.txt")) lines = File.ReadAllLines("Timesheet.txt");
        if (File.Exists("timesheet.txt")) lines = File.ReadAllLines("timesheet.txt");

        if (lines.Count() == 0)
        {
            Console.WriteLine("Can't find timsheet.txt");
            return;
        }

        var parser = new LogParser(new LineLexer());
        var log = parser.Parse(lines);

        Console.WriteLine(log?.GetStatistics(DateTime.Today) ?? "");
    }

    static void List(ParseResult parseResult)
    {
        var period = parseResult.GetValue<string>("period") ?? "day";
        var code = parseResult.GetValue<string>("code") ?? "";
        var task = parseResult.GetValue<string>("task") ?? "";

        Console.WriteLine($"TODO: LIST");
        Console.WriteLine($"--period = {period}");
        Console.WriteLine($"--code = {code}");
        Console.WriteLine($"--task = {task}");
    }
}