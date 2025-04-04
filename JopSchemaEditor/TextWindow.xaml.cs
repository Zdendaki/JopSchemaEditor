using Microsoft.Xna.Framework;
using System.Windows;
using System.Windows.Input;

namespace JopSchemaEditor
{
    /// <summary>
    /// Interakční logika pro TextWindow.xaml
    /// </summary>
    public partial class TextWindow : Window
    {
        public string Text => textField.Text;

        public Color Color => ((ColorData)color.SelectedItem).Color;

        public TextWindow(Window owner)
        {
            InitializeComponent();
            Owner = owner;

            var colors = ESAColor.GetColors().ToList();
            color.ItemsSource = colors;
            color.SelectedIndex = colors.FindIndex(x => x.Color == App.SelectedColor.Background);

            Loaded += TextWindow_Loaded;
        }

        private void TextWindow_Loaded(object sender, RoutedEventArgs e)
        {
            textField.Focus();
            textField.SelectAll();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
            }
        }
    }
}
