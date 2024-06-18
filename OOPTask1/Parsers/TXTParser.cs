﻿using OOPTask1.Model;
using System.Text;

namespace OOPTask1.Parsers
{
    /// <summary>
    /// Реализация анализа текста файла txt
    /// </summary>
    public sealed class TXTParser : TextFileParserBase
    {
        /// <inheritdoc />
        public override string FileExtension => "txt";

        /// <inheritdoc />
        protected override void Parse(FileInfo textFile)
        {
            using (var fs = new FileStream(textFile.FullName, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    var sb = new StringBuilder();

                    while (!sr.EndOfStream)
                    {
                        var letterIndex = sr.Read();

                        if (letterIndex == -1)
                        {
                            break;
                        }

                        var letter = (char)letterIndex;

                        if (char.IsControl(letter))
                        {
                            continue;
                        }                 

                        if (!char.IsLetterOrDigit(letter))
                        {
                            var wordStr = sb.ToString();

                            if (!string.IsNullOrEmpty(wordStr))
                            {
                                var word = new Word(wordStr);
                                RecordWord(word);
                            }   

                            sb.Clear();
                        }
                        else
                        {
                            sb.Append(letter);
                        }
                    }
                }
            }

            FillAllWords(true);
        }
    }
}
