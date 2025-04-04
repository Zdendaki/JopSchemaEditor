using Microsoft.Xna.Framework;

namespace JopSchemaEditor
{
    internal static class ESAColor
    {
        public static Color Red => new Color(255, 0, 0);

        public static Color DarkRed => new Color(128, 0, 0);

        public static Color Violet => new Color(255, 0, 255);

        public static Color DarkViolet => new Color(128, 0, 128);

        public static Color Turquoise => new Color(0, 255, 255);

        public static Color DarkTurquoise => new Color(0, 128, 128);

        public static Color White => new Color(255, 255, 255);

        public static Color Green => new Color(0, 255, 0);

        public static Color DarkGreen => new Color(0, 128, 0);

        public static Color Brown => new Color(128, 64, 0);

        public static Color Blue => new Color(0, 0, 255);

        public static Color DarkBlue => new Color(0, 0, 128);

        public static Color Yellow => new Color(255, 255, 0);

        public static Color Gray => new Color(192, 192, 192);

        public static Color DarkGray => new Color(128, 128, 128);

        public static Color Black => new Color(0, 0, 0);

        public static Color Orange => new Color(255, 153, 0);

        public static Color Transparent => Color.Transparent;

        /// <summary>
        /// Gets the color based on the specified color name.
        /// </summary>
        /// <param name="color">The color name.</param>
        /// <returns>The corresponding color.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the color name is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the color name is invalid.</exception>
        public static Color GetColor(string color)
        {
            const string EMPTY = "";

            if (color is null)
                throw new ArgumentNullException(nameof(color), "Color name cannot be null.");

            switch (color.ToLowerInvariant())
            {
                case "red":
                    return Red;
                case "darkred":
                    return DarkRed;
                case "violet":
                    return Violet;
                case "darkviolet":
                    return DarkViolet;
                case "turquoise":
                    return Turquoise;
                case "darkturquoise":
                    return DarkTurquoise;
                case "white":
                    return White;
                case "green":
                    return Green;
                case "darkgreen":
                    return DarkGreen;
                case "brown":
                    return Brown;
                case "blue":
                    return Blue;
                case "darkblue":
                    return DarkBlue;
                case "yellow":
                    return Yellow;
                case "gray":
                    return Gray;
                case "darkgray":
                    return DarkGray;
                case "black":
                    return Black;
                case "orange":
                    return Orange;
                case "transparent":
                case "none":
                case EMPTY:
                    return Transparent;
                default:
                    throw new ArgumentException("Invalid color name.", nameof(color));
            }
        }

        public static Color GetColor(int value)
        {
            switch (value)
            {
                case 4096:
                    return Red;
                case 4097:
                    return DarkRed;
                case 4098:
                    return Violet;
                case 4099:
                    return DarkViolet;
                case 4100:
                    return Turquoise;
                case 4101:
                    return DarkTurquoise;
                case 4102:
                    return White;
                case 4103:
                    return Green;
                case 4104:
                    return DarkGreen;
                case 4105:
                    return Brown;
                case 4106:
                    return Blue;
                case 4107:
                    return DarkBlue;
                case 4108:
                    return Yellow;
                case 4109:
                    return Gray;
                case 4110:
                    return DarkGray;
                case 4111:
                    return Black;
                case 4112:
                    return Orange;
                case 4113:
                    return Transparent;
                default:
                    throw new ArgumentException("Invalid color value.", nameof(value));
            }
        }

        public static IEnumerable<ColorData> GetColors()
        {
            yield return new ColorData(Red, 4096, nameof(Red));
            yield return new ColorData(DarkRed, 4097, nameof(DarkRed));
            yield return new ColorData(Violet, 4098, nameof(Violet));
            yield return new ColorData(DarkViolet, 4099, nameof(DarkViolet));
            yield return new ColorData(Turquoise, 4100, nameof(Turquoise));
            yield return new ColorData(DarkTurquoise, 4101, nameof(DarkTurquoise));
            yield return new ColorData(White, 4102, nameof(White));
            yield return new ColorData(Green, 4103, nameof(Green));
            yield return new ColorData(DarkGreen, 4104, nameof(DarkGreen));
            yield return new ColorData(Brown, 4105, nameof(Brown));
            yield return new ColorData(Blue, 4106, nameof(Blue));
            yield return new ColorData(DarkBlue, 4107, nameof(DarkBlue));
            yield return new ColorData(Yellow, 4108, nameof(Yellow));
            yield return new ColorData(Gray, 4109, nameof(Gray));
            yield return new ColorData(DarkGray, 4110, nameof(DarkGray));
            yield return new ColorData(Black, 4111, nameof(Black));
            yield return new ColorData(Orange, 4112, nameof(Orange));
            yield return new ColorData(Transparent, 4113, nameof(Transparent));
        }
    }

    public record ColorData
    {
        public Color Color { get; init; }

        public int Value { get; init; }

        public string Name { get; init; }

        public ColorData(Color color, int value, string name)
        {
            Color = color;
            Value = value;
            Name = name;
        }

        public override string ToString() => Name;
    }
}