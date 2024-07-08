using NLog;
using OOPTask1.Parsers;
using System.Reflection.PortableExecutable;

namespace OOPTask1.Console;

internal class Program
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    private static void Main(string[] args)
    {
        System.Console.Title = "OOPTask1.Console";

        try
        {
            _logger.Info("Программа запущена.");

            if (args.Length == 0)
            {
                _logger.Info("Не заданы аргументы!");
                return;
            }

            var fileInfo = new FileInfo(args[0]);
            var parsingManager = new FileParsingManager();
            parsingManager.Register(new TxtParser(new StreamParser()));
            parsingManager.Execute(fileInfo);
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
        }
        finally
        {
            _logger.Info("Программа завершена.");
            System.Console.ReadLine();
        }
    }
}