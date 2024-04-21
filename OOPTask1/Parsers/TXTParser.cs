namespace OOPTask1.Parsers
{
    public class TXTParser : ParserBase
    {
        public override string FileExtension => ".txt";

        protected override void Parse(string filename)
        {
            StartFilling(filename);

            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                //Слова разделяются любыми символами которые не 
                //fs.read
            }

            Fill("123", 0, 0);

            StopFilling();
        }
    }
}
