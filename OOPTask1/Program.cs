using OOPTask1.Parsers;

namespace OOPTask1
{
    public class Program
    {
        public static readonly ParserBase[] Parsers = new[]
        {
            new TXTParser()
        };

        private static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            var filePath = args[0];
            var fileExtension = Path.GetExtension(filePath);

            if (!File.Exists(filePath))
                return;

            var parser = Parsers.FirstOrDefault(p => p.FileExtension.Equals(fileExtension));

            if (parser is null)
                return;

            char.IsLetterOrDigit();

            Console.ReadLine();
        }
    }
}
