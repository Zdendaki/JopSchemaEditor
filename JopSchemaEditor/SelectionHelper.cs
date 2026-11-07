using Microsoft.Xna.Framework;

namespace JopSchemaEditor;

internal static class SelectionHelper
{
    public static Rectangle MoveUp(Rectangle selection)
    {
        int width = App.Fields.GetLength(0);
        int height = App.Fields.GetLength(1);

        if (!CheckBounds(selection, width, height, 0, -1))
            return selection;

        for (int x = selection.Left; x < selection.Right; x++)
        {
            for (int y = selection.Top - 1; y < selection.Bottom - 1; y++)
            {
                App.Fields[x, y] = App.Fields[x, y + 1];
            }
            App.Fields[x, selection.Bottom - 1] = default;
        }

        return new Rectangle(selection.X, selection.Y - 1, selection.Width, selection.Height);
    }

    public static Rectangle MoveDown(Rectangle selection)
    {
        int width = App.Fields.GetLength(0);
        int height = App.Fields.GetLength(1);

        if (!CheckBounds(selection, width, height, 0, 1))
            return selection;

        for (int x = selection.Left; x < selection.Right; x++)
        {
            for (int y = selection.Bottom; y > selection.Top; y--)
            {
                App.Fields[x, y] = App.Fields[x, y - 1];
            }
            App.Fields[x, selection.Top] = default;
        }

        return new Rectangle(selection.X, selection.Y + 1, selection.Width, selection.Height);
    }

    public static Rectangle MoveLeft(Rectangle selection)
    {
        int width = App.Fields.GetLength(0);
        int height = App.Fields.GetLength(1);

        if (!CheckBounds(selection, width, height, -1, 0))
            return selection;

        for (int y = selection.Top; y < selection.Bottom; y++)
        {
            for (int x = selection.Left - 1; x < selection.Right - 1; x++)
            {
                App.Fields[x, y] = App.Fields[x + 1, y];
            }
            App.Fields[selection.Right - 1, y] = default;
        }

        return new Rectangle(selection.X - 1, selection.Y, selection.Width, selection.Height);
    }

    public static Rectangle MoveRight(Rectangle selection)
    {
        int width = App.Fields.GetLength(0);
        int height = App.Fields.GetLength(1);

        if (!CheckBounds(selection, width, height, 1, 0))
            return selection;

        for (int y = selection.Top; y < selection.Bottom; y++)
        {
            for (int x = selection.Right; x > selection.Left; x--)
            {
                App.Fields[x, y] = App.Fields[x - 1, y];
            }
            App.Fields[selection.Left, y] = default;
        }

        return new Rectangle(selection.X + 1, selection.Y, selection.Width, selection.Height);
    }

    public static void Delete(Rectangle selection)
    {
        int width = App.Fields.GetLength(0);
        int height = App.Fields.GetLength(1);

        if (!CheckBounds(selection, width, height))
            return;

        for (int x = selection.Left; x < selection.Right; x++)
        {
            for (int y = selection.Top; y < selection.Bottom; y++)
            {
                App.Fields[x, y] = default;
            }
        }
    }

    private static bool CheckBounds(Rectangle rectangle, int width, int height, int dx = 0, int dy = 0)
    {
        return rectangle.X + dx >= 0 && rectangle.Y + dy >= 0 && rectangle.Right + dx <= width && rectangle.Bottom + dy <= height;
    }
}
