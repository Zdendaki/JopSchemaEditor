using Microsoft.Xna.Framework;

namespace JopSchemaEditor
{
    static class RectangleUtils
    {
        public static Rectangle BorderTop(this Rectangle rectangle, int thickness)
        {
            return new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness);
        }

        public static Rectangle BorderBottom(this Rectangle rectangle, int thickness)
        {
            return new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness, rectangle.Width, thickness);
        }

        public static Rectangle BorderLeft(this Rectangle rectangle, int thickness)
        {
            return new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height);
        }

        public static Rectangle BorderRight(this Rectangle rectangle, int thickness)
        {
            return new Rectangle(rectangle.X + rectangle.Width - thickness, rectangle.Y, thickness, rectangle.Height);
        }
    }
}
