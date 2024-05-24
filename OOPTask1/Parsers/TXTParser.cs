using System.Text;

namespace OOPTask1.Parsers
{
    /// <summary>
    /// Реализация анализа текста файла txt
    /// </summary>
    public class TXTParser : ParserBase
    {
        /// <inheritdoc />
        public override string FileExtension => ".txt";

        /// <inheritdoc />
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
