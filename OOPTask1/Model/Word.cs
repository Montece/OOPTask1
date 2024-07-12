namespace OOPTask1.Model;

public sealed class Word
{
    public string Value { get; }

    public Word(string? value)
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

    public bool CustomEquals(object? targetObject)
    {
        if (targetObject == null)
        {
            return false;
        }

        if (targetObject is Word word)
        {
            return word.Value.Equals(Value);
        }
        else if (targetObject is string targetString)
        {
            return targetString.Equals(Value);
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