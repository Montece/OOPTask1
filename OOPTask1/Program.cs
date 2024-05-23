using OOPTask1.Loggers;
using OOPTask1.Parsers;

namespace OOPTask1
{
    public class Program
    {
        private static void Main(string[] args)
        {
            args = new string[] { "StringBuilder.txt" };

            Logger.Register(new ConsoleLogger());
            Logger.Initialize();

            Logger.Log("Программа запущена.");

            if (args.Length != 0)
            {
                var filePath = args[0];

                var parsingManager = new ParsingManager();
                parsingManager.Register<TXTParser>();
                parsingManager.Execute(filePath);
            }
            else
            {
                Logger.Log("Не заданы аргументы!", LogLevel.Warning);
            }    

            Logger.Log("Программа завершена.");
            Console.ReadLine();
        }
    }
}
