using Microsoft.Xna.Framework;
using System.Windows;

namespace JopSchemaEditor;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    internal static readonly JOPData[,] Fields = new JOPData[480, 180];

    internal static readonly object Lock = new();

    internal static string? AwaitingString = null;

    internal static Color AwaitingColor = ESAColor.Gray;

    public static bool LightMode { get; internal set; } = false;

    public static bool Grid { get; internal set; } = true;

    private static MGButton selectedButton = null!;
    public static MGButton SelectedButton
    {
        get => selectedButton;
        internal set
        {
            if (selectedButton is not null)
                selectedButton.IsSelected = false;

            selectedButton = value;

            if (selectedButton is not null)
                selectedButton.IsSelected = true;
        }
    }

    private static MGButton selectedColor = null!;
    public static MGButton SelectedColor
    {
        get => selectedColor;
        internal set
        {
            if (selectedColor is not null)
                selectedColor.IsSelected = false;

            selectedColor = value;

            if (selectedColor is not null)
                selectedColor.IsSelected = true;
        }
    }
}

internal struct JOPData
{
    public byte Data;
    public Color Color;

    public JOPData(byte data, Color color)
    {
        Data = data;
        Color = color;
    }
}

