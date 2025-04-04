using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.WpfCore.MonoGameControls;
using System.IO;

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

            if (name == -2) // VYMAZAT
            {
                RemoveField();
                return true;
            }
            
            if (name == -4) // OBARVIT
            {
                ref JOPData data = ref App.Fields[_mouseX, _mouseY];
                if (data.Data > 0)
                {
                    data.Color = App.SelectedColor.Background;
                    App.Changed = true;
                }
                return true;
            }

            if (name > byte.MaxValue || name <= 0)
                return false;

            App.Fields[_mouseX, _mouseY] = new((byte)name, App.SelectedColor.Background);
            App.Changed = true;
            return true;
        }

        private void RemoveField()
        {
            App.Fields[_mouseX, _mouseY] = default;
            App.Changed = true;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(App.LightMode ? ESAColor.White : ESAColor.Black);

            _spriteBatch.Begin();
            DrawGrid();
            DrawFields();
            DrawSelection();
            _spriteBatch.End();
        }

        private void DrawGrid()
        {
            if (!App.Grid)
                return;

            for (int x = _store.FontWidth / 2; x < Width; x += _store.FontWidth)
            {
                for (int y = _store.FontHeight / 2; y < Height; y += _store.FontHeight)
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

        private void DrawSelection()
        {
            int stringLength;
            lock (App.Lock)
            {
                stringLength = App.AwaitingString?.Length ?? 0;
            }

            if (stringLength == 0)
                return;

            _spriteBatch.DrawRectangle(_mouseX * _store.FontWidth, _mouseY * _store.FontHeight, stringLength * _store.FontWidth, _store.FontHeight, ESAColor.Green, 1f);
        }

        public void SaveScreenshot(string fileName)
        {
            int width = App.Fields.GetLength(0) * _store.FontWidth;
            int height = App.Fields.GetLength(1) * _store.FontHeight;

            try
            {
                using FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                using RenderTarget2D texture = new(GraphicsDevice, width, height);

                GraphicsDevice.SetRenderTarget(texture);
                Draw(new GameTime());
                GraphicsDevice.SetRenderTarget(null);
                texture.SaveAsPng(stream, width, height);
                texture.Dispose();
            }
            catch { }
        }

        public void SaveScreenshotCropped(string fileName)
        {
            int width = App.Fields.GetLength(0) * _store.FontWidth;
            int height = App.Fields.GetLength(1) * _store.FontHeight;

            int firstX = int.MaxValue;
            int firstY = int.MaxValue;
            int lastX = 0;
            int lastY = 0;

            for (int x = 0; x < App.Fields.GetLength(0); x++)
            {
                for (int y = 0; y < App.Fields.GetLength(1); y++)
                {
                    if (App.Fields[x, y].Data > 0)
                    {
                        if (x < firstX)
                            firstX = x;
                        if (x > lastX)
                            lastX = x;

                        if (y < firstY)
                            firstY = y;
                        if (y > lastY)
                            lastY = y;
                    }
                }
            }

            if (firstX > lastX || firstY > lastY)
                return;

            int croppedWidth = (lastX - firstX + 1) * _store.FontWidth;
            int croppedHeight = (lastY - firstY + 1) * _store.FontHeight;

            Rectangle crop = new(firstX * _store.FontWidth, firstY * _store.FontHeight, croppedWidth, croppedHeight);

            try
            {
                using FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                using RenderTarget2D texture = new(GraphicsDevice, width, height);
                using RenderTarget2D croppedTexture = new(GraphicsDevice, croppedWidth, croppedHeight);

                GraphicsDevice.SetRenderTarget(texture);
                Draw(new GameTime());
                GraphicsDevice.SetRenderTarget(null);

                Color[] data = new Color[croppedWidth * croppedHeight];
                texture.GetData(0, crop, data, 0, data.Length);

                croppedTexture.SetData(data);
                croppedTexture.SaveAsPng(stream, croppedWidth, croppedHeight);
            }
            catch { }
        }
    }
}
