namespace OOPTask1
{
    /// <summary>
    /// Слово
    /// </summary>
    public class Word(string value)
    {
        public string Value { get; set; } = value;

        public bool IsValid() => Value.All(char.IsLetterOrDigit);
    }
}
