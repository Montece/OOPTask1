using NLog;
using NLog.Filters;
using OOPTask1.Abstract;
using OOPTask1.Model;
using System.Text;

namespace OOPTask1;

public sealed class RecordsFiller
{
    public bool IsFilling => _isFilling;
    public IReadOnlyCollection<Record> Records => _records;

    private const string TARGET_FILENAME = "result.csv";
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private bool _isFilling = false;
    private StreamWriter? _stream;
    private ulong _allWordsCount = 0;
    private List<Record> _records = new(128);

    public bool StartFilling(StreamWriter stream)
    {
        if (_isFilling)
        {
            return false;
        }

        ArgumentNullException.ThrowIfNull(stream);

        _stream = stream;
        _isFilling = true;

        return true;
    }

    public void AddWord(Word word)
    {
        ArgumentNullException.ThrowIfNull(word);

        var record = TryGetRecord(word);

        if (record is null)
        {
            record = new Record(word, 1);
            _records.Add(record);
        }
        else
        {
            record.Count++;
        }

        _allWordsCount++;
    }

    public Record? TryGetRecord(Word word)
    {
        ArgumentNullException.ThrowIfNull(word);

        var record = _records.FirstOrDefault(r => r.Word != null && r.Word.CustomEquals(word));

        return record;
    }

    public void FillAll(bool needSorting)
    {
        if (needSorting)
        {
            SortWords();
        }

        foreach (var record in _records)
        {
            Fill(record);
        }
    }

    private bool Fill(Record record)
    {
        ArgumentNullException.ThrowIfNull(record);

        if (!IsFilling || _allWordsCount == 0 || _stream is null)
        {
            return false;
        }

        var frequency = record.GetFrequency(_allWordsCount);
        var frequencyInPercents = frequency / 100;

        var frequencyText = frequency.ToString("0.0000").Replace(',', '.');
        var frequencyInPercentsText = $"{frequencyInPercents.ToString("0.000").Replace(',', '.')}%";
        var splitChar = ';';
        var line = $"{record.Word}{splitChar}{frequencyText}{splitChar}{frequencyInPercentsText}{Environment.NewLine}";
        
        _stream.WriteLine(line);
        _stream.Flush();

        _logger.Info($"Сделана запись '{line}'");

        return true;
    }

    private void SortWords()
    {
        _records = _records.OrderByDescending(x => x.Count).ToList();
    }

    public bool StopFilling()
    {
        if (!_isFilling)
        {
            return false;
        }

        try
        {
            /*stream?.Close();
            _stream?.Dispose();*/
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
            return false;
        }

        _isFilling = false;

        return true;
    }

    ~RecordsFiller()
    {
        if (!_isFilling)
        {
            return;
        }

        try
        {
            _stream?.Close();
            _stream?.Dispose();
        }
        catch (Exception ex)
        {
            // Ignore
        }
    }
}