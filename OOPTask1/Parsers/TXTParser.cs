using System.Text;

namespace OOPTask1.Parsers
{
    public class TXTParser : ParserBase
    {
        public override string FileExtension => ".txt";

        public override void Parse(string filename)
        {
            try
            {
                StartFilling(filename);

                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        var sb = new StringBuilder();

                        while (!sr.EndOfStream)
                        {
                            var letterIndex = sr.Read();

                            if (letterIndex == -1)
                                break;

                            var letter = (char)letterIndex;

                            if (char.IsControl(letter))
                                continue;

                            if (!char.IsLetterOrDigit(letter))
                            {
                                var word = sb.ToString();

                                if (!string.IsNullOrEmpty(word))
                                    RecordWord(word);

                                sb.Clear();
                            }
                            else
                                sb.Append(letter);
                        }
                    }
                }

                FillAllWords(true);
            }
            finally
            {
                StopFilling();
            }
        }
    }
}
