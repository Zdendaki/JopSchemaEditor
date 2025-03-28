using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace JopSchemaEditor
{
    /// <summary>
    /// Interakční logika pro ToolWindow.xaml
    /// </summary>
    public partial class ToolWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x20;

        [LibraryImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
        private static partial int GetWindowLong(nint hWnd, int nIndex);

        [LibraryImport("user32.dll", EntryPoint = "SetWindowLongA")]
        private static partial int SetWindowLong(nint hWnd, int nIndex, int dwNewLong);

        private bool _close = false;

        public ToolWindow()
        {
            InitializeComponent();

            Loaded += ToolWindow_Loaded;
        }

        private void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            nint hwnd = new WindowInteropHelper(this).Handle;
            STYLE(hwnd);
            EXSTYLE(hwnd);
        }

        private void STYLE(nint hwnd)
        {
            int currentStyle = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, currentStyle & ~WS_SYSMENU);
        }

        private void EXSTYLE(nint hwnd)
        {
            int currentStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, currentStyle | WS_EX_TRANSPARENT);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_close)
                e.Cancel = true;
        }

        public new void Close()
        {
            _close = true;
            base.Close();
        }
    }
}
