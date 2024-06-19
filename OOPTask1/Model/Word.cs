namespace OOPTask1.Model
{
    public class Word
    {
        public string Value { get; }

        public Word(string value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value);

            if (!value.All(char.IsLetterOrDigit))
            {
                throw new ArgumentException("Must contains only letters or digits!", nameof(value));
            }

            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Word word)
            {
                return word.Value.Equals(Value);
            }
            else if (obj is string str)
            {
                return str.Equals(Value);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
