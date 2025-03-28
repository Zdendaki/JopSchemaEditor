using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.WpfCore.MonoGameControls;

namespace JopSchemaEditor
{
    public class JOP : MonoGameViewModel
    {
        protected int Width => GraphicsDevice.Viewport.Width;

        protected int Height => GraphicsDevice.Viewport.Height;

        private int _maxX;
        private int _mayY;

        private int _mouseX;
        private int _mouseY;

        private SpriteBatch _spriteBatch = null!;
        private GameStore _store = null!;

        public override void LoadContent()
        {
            _store = new(Content, GraphicsDevice, 8, 12);
            _spriteBatch = new(GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            _maxX = Math.Min(Width / _store.FontWidth * Width + 1, App.Fields.GetLength(0));
            _mayY = Math.Min(Height / _store.FontHeight * Height + 1, App.Fields.GetLength(1));
        }

        public override void OnMouseDown(MouseStateArgs mouseState)
        {
            _mouseX = (int)(mouseState.Position.X / _store.FontWidth);
            _mouseY = (int)(mouseState.Position.Y / _store.FontHeight);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (InsertTextBox())
                    return;
                if (InsertField())
                    return;
            }
            else if (mouseState.RightButton == ButtonState.Pressed)
            {
                RemoveField();
            }
        }

        public override void OnMouseMove(MouseStateArgs mouseState)
        {
            _mouseX = (int)(mouseState.Position.X / _store.FontWidth);
            _mouseY = (int)(mouseState.Position.Y / _store.FontHeight);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                InsertField();
            }
            else if (mouseState.RightButton == ButtonState.Pressed)
            {
                RemoveField();
            }
        }

        private bool InsertTextBox()
        {
            lock (App.Lock)
            {
                if (App.AwaitingString is null)
                    return false;

                int x = _mouseX;
                foreach (byte b in KamenickyEncoding.EncodeByte(App.AwaitingString))
                {
                    App.Fields[x, _mouseY] = new(b, App.AwaitingColor);
                    x++;

                    if (x >= _maxX)
                        break;
                }

                App.AwaitingString = null;
            }

            return true;
        }

        private bool InsertField()
        {
            int name = App.SelectedButton.Name;

            if (name > byte.MaxValue || name < byte.MinValue)
                return false;

            App.Fields[_mouseX, _mouseY] = new((byte)name, App.SelectedColor.Background);
            return true;
        }

        private void RemoveField()
        {
            App.Fields[_mouseX, _mouseY] = default;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(App.LightMode ? ESAColor.White : ESAColor.Black);

            _spriteBatch.Begin();
            DrawGrid();
            DrawFields();
            _spriteBatch.End();
        }

        private void DrawGrid()
        {
            if (!App.Grid)
                return;

            for (int x = _store.FontWidth; x < Width; x += _store.FontWidth)
            {
                for (int y = _store.FontHeight; y < Height; y += _store.FontHeight)
                {
                    _spriteBatch.Draw(_store.FillTexture, new Vector2(x, y), ESAColor.DarkGray);
                }
            }
        }

        private void DrawFields()
        {
            for (int x = 0; x < _maxX; x++)
            {
                for (int y = 0; y < _mayY; y++)
                {
                    if (App.Fields[x, y].Data == 0)
                        continue;

                    char c = (char)App.Fields[x, y].Data;

                    _spriteBatch.DrawString(_store.ETM, c.ToString(), new Vector2(x * _store.FontWidth, y * _store.FontHeight), App.Fields[x, y].Color);
                }
            }
        }
    }
}
