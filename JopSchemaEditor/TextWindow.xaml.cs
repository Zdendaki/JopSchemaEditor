using Microsoft.Xna.Framework;
using System.Windows;

namespace JopSchemaEditor
{
    /// <summary>
    /// Interakční logika pro TextWindow.xaml
    /// </summary>
    public partial class TextWindow : Window
    {
        public string Text => textField.Text;

        public Color Color => ((ColorData)color.SelectedItem).Color;

        public TextWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
