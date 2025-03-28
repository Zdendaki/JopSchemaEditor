using System.Windows;

namespace JopSchemaEditor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ToolWindow _toolWindow;

    public MainWindow()
    {
        InitializeComponent();

        _toolWindow = new();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _toolWindow.Owner = this;
        _toolWindow.Left = Left + ActualWidth + 5;
        _toolWindow.Top = Top;
        _toolWindow.Show();
    }

    private void InsertText_Click(object sender, RoutedEventArgs e)
    {
        TextWindow tw = new();
        tw.Owner = this;
        if (tw.ShowDialog() != true)
            return;

        if (string.IsNullOrWhiteSpace(tw.Text))
            return;

        lock (App.Lock)
        {
            App.AwaitingString = tw.Text;
            App.AwaitingColor = tw.Color;
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        _toolWindow.Close();
    }
}