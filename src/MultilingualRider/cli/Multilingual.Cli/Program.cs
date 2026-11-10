namespace Multilingual.Cli;

public static class Program
{
    public static Task<int> Main(string[] args) => Cli.Run(args, Console.Out, Console.Error);
}
