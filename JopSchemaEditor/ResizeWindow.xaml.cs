using System.Windows;

namespace JopSchemaEditor
{
    /// <summary>
    /// Interakční logika pro ResizeWindow.xaml
    /// </summary>
    public partial class ResizeWindow : Window
    {
        public Resolution? Result { get; private set; }

        public ResizeWindow(Window owner)
        {
            InitializeComponent();
            Owner = owner;

            int width = App.Fields.GetLength(0) * 8;
            int height = App.Fields.GetLength(1) * 12;

            foreach (Resolution res in resolution.Items.OfType<Resolution>())
            {
                if (res.Width == width && res.Height == height)
                {
                    resolution.SelectedItem = res;
                    break;
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (resolution.SelectedItem is not Resolution res)
            {
                MessageBox.Show(this, "Vyberte rozlišení!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Result = res;
            Close();
        }
    }

    public record Resolution
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public override string ToString() => $"{Width}x{Height}";
    }
}
