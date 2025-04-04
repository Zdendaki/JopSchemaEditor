using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace JopSchemaEditor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const ushort VERSION = 2;
    private const string HEADER = "JOP DE";

    private readonly ToolWindow _toolWindow;

    private string? fileName;
    private string? FileName
    {
        get => fileName;
        set
        {
            fileName = value;

            RefreshFileName();
        }
    }

    private void RefreshFileName()
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            Title = $"JOP editor [{App.Fields.GetLength(0) * 8}x{App.Fields.GetLength(1) * 12}]";
            saveButton.IsEnabled = false;
        }
        else
        {
            Title = $"JOP editor - {fileName} [{App.Fields.GetLength(0) * 8}x{App.Fields.GetLength(1) * 12}]";
            saveButton.IsEnabled = true;
        }
    }

    public MainWindow()
    {
        App.Window = this;

        InitializeComponent();

        RefreshFileName();
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

    public void SetText()
    {
        Dispatcher.BeginInvoke(() =>
        {
            TextWindow tw = new(this);
            if (tw.ShowDialog() != true)
                return;

            if (string.IsNullOrWhiteSpace(tw.Text))
                return;

            lock (App.Lock)
            {
                App.AwaitingString = tw.Text;
                App.AwaitingColor = tw.Color;
            }
        });
    }

    protected override void OnClosed(EventArgs e)
    {
        _toolWindow.Close();
    }

    private bool PreserveChanges()
    {
        if (!App.Changed)
            return true;

        MessageBoxResult result = MessageBox.Show(this, "Soubor byl změněn. Chcete uložit změny?", "Uložit změny?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
        if (result == MessageBoxResult.Cancel)
            return false;
        if (result == MessageBoxResult.Yes)
        {
            if (FileName is null)
                SaveAs_Click(this, null!);
            else
                Save(FileName);
        }
        return true;
    }

    private void New_Click(object sender, RoutedEventArgs e)
    {
        if (!PreserveChanges())
            return;

        Clear();
        FileName = null;
        App.Changed = false;
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        if (!PreserveChanges())
            return;

        OpenFileDialog ofd = new()
        {
            Filter = "Jop Schema File (*.jop)|*.jop",
            Title = "Načíst JOP"
        };

        if (ofd.ShowDialog() != true || string.IsNullOrWhiteSpace(ofd.FileName))
            return;

        try
        {
            FileStream fs = new(ofd.FileName, FileMode.Open, FileAccess.Read);
            using BinaryReader br = new(fs);

            string header = br.ReadString();
            ushort version = br.ReadUInt16();

            if (header != HEADER || version != VERSION)
            {
                MessageBox.Show(this, "Neplatný formát souboru!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int width = br.ReadInt32();
            int height = br.ReadInt32();
            bool light = br.ReadBoolean();
            bool grid = br.ReadBoolean();

            JopControl.Width = width * 8;
            JopControl.Height = height * 12;
            App.LightMode = light;
            App.Grid = grid;

            App.Fields = new JOPData[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    App.Fields[x, y] = br.ReadStruct<JOPData>();
                }
            }

            FileName = ofd.FileName;
            App.Changed = false;
        }
        catch
        {
            MessageBox.Show(this, "Chyba při načítání souboru!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (FileName is not null)
            Save(FileName);
    }

    private void SaveAs_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog sfd = new()
        {
            Filter = "Jop Schema File (*.jop)|*.jop",
            Title = "Uložit JOP"
        };

        if (sfd.ShowDialog() != true || string.IsNullOrWhiteSpace(sfd.FileName))
            return;

        Save(sfd.FileName);
    }

    private void Clear()
    {
        Utils.Clear2DArray(App.Fields);

        lock (App.Lock)
            App.AwaitingString = null;
    }

    private void Save(string fileName)
    {
        int width = App.Fields.GetLength(0);
        int height = App.Fields.GetLength(1);

        try
        {
            using FileStream fs = new(fileName, FileMode.Create, FileAccess.Write);
            using BinaryWriter bw = new(fs);

            bw.Write(HEADER);
            bw.Write(VERSION);
            bw.Write(width);
            bw.Write(height);
            bw.Write(App.LightMode);
            bw.Write(App.Grid);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bw.Write(App.Fields[x, y]);
                }
            }

            FileName = fileName;
            App.Changed = false;
        }
        catch
        {
            MessageBox.Show(this, "Chyba při ukládání souboru!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }

    private void Export_Click(object sender, RoutedEventArgs e)
    {
        string? file = Export();
        if (file is null)
            return;

        Jop.SaveScreenshot(file);
    }

    private void Resize_Click(object sender, RoutedEventArgs e)
    {
        ResizeWindow rw = new(this);
        rw.ShowDialog();

        if (rw.Result is not Resolution res)
            return;

        int width = res.Width / 8;
        int height = res.Height / 12;

        JOPData[,] old = App.Fields;
        App.Fields = new JOPData[width, height];
        JopControl.Width = res.Width;
        JopControl.Height = res.Height;

        int cw = Math.Min(width, old.GetLength(0));
        int ch = Math.Min(height, old.GetLength(1));

        for (int x = 0; x < cw; x++)
        {
            for (int y = 0; y < ch; y++)
            {
                App.Fields[x, y] = old[x, y];
            }
        }

        RefreshFileName();
        App.Changed = true;
    }

    private void ExportCropped_Click(object sender, RoutedEventArgs e)
    {
        string? file = Export();
        if (file is null)
            return;

        Jop.SaveScreenshotCropped(file);
    }

    private string? Export()
    {
        SaveFileDialog sfd = new()
        {
            Filter = "Obrázky PNG (*.png)|*.png",
            Title = "Exportovat JOP"
        };

        if (sfd.ShowDialog() != true || string.IsNullOrWhiteSpace(sfd.FileName))
            return null;

        return sfd.FileName;
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        if (!App.Changed)
            return;

        MessageBoxResult result = MessageBox.Show(this, "Soubor byl změněn. Chcete uložit změny?", "Uložit změny?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
        
        if (result == MessageBoxResult.Cancel)
            e.Cancel = true;
        else if (result == MessageBoxResult.Yes)
        {
            if (FileName is null)
                SaveAs_Click(this, null!);
            else
                Save(FileName);
        }
    }
}