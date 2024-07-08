using Microsoft.Win32;
using NLog;
using OOPTask1.Parsers;
using System.IO;
using System.Windows;

namespace OOPTask1.WPF;

public sealed partial class MainWindow : Window
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private FileInfo? _fileInfo;
    private FileParsingManager? _parsingManager;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _logger.Info("Программа запущена.");
    }

    private void Window_Unloaded(object sender, RoutedEventArgs e)
    {
        _logger.Info("Программа завершена.");
    }

    private void ChooseFile_Button_Click(object sender, RoutedEventArgs e)
    {
        var ofd = new OpenFileDialog()
        {
            Multiselect = false
        };
            
        if (ofd.ShowDialog().Value)
        {
            _fileInfo = new(ofd.FileName);
            _parsingManager = null;
        }
    }

    private void Execute_Button_Click(object sender, RoutedEventArgs e)
    {
        if (_fileInfo is null || _parsingManager is not null)
        {
            MessageBox.Show("Не выбран файл!");
            return;
        }

        try
        {
            _parsingManager = new FileParsingManager();
            _parsingManager.Register(new TxtParser(new StreamParser()));
            var result = _parsingManager.Execute(_fileInfo);
            MessageBox.Show($"Завершено {(result ? "успешно" : "с ошибками")}.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
        }
    }
}