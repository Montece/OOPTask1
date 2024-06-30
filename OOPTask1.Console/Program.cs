using NLog;
using OOPTask1.Parsers;

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

            var fileinfo = new FileInfo(args[0]);

            var parsingManager = new ParsingManager();
            parsingManager.Register(new TXTParser());
            parsingManager.Execute(fileinfo);
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