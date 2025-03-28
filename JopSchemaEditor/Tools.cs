using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.WpfCore.MonoGameControls;
using System.Windows.Input;

namespace JopSchemaEditor
{
    class Tools : MonoGameViewModel
    {
        private readonly int Width = 204;
        private readonly int Height = 768;

        private SpriteBatch _spriteBatch = null!;
        private GameStore _store = null!;

        List<MGButton> _buttons = [];

        private int _newButtonX = 4;
        private int _newButtonY = 6;

        public override void LoadContent()
        {
            _store = new(Content, GraphicsDevice, 8, 12);
            _spriteBatch = new(GraphicsDevice);

            _buttons.Add(BigButton(0, "VYBRAT"));
            _buttons.Add(BigButton(0, "VYMAZAT"));
            _buttons.Add(BigButton(0, "TEXT"));
            _buttons.Add(SmallButton(0, " SV\u0089TL\u009d RE\u0092IM ", BackgroundChange));

            MGButton grid = SmallButton(0, " M\u009e\u008b\u0092KA ", GridChange);
            grid.Background = App.Grid ? ESAColor.Orange : ESAColor.DarkBlue;
            _buttons.Add(grid);

            _newButtonX = 4;
            _newButtonY += 32;

            for (int i = 1; i < 255; i++)
            {
                char c = (char)i;

                if (c.IsKamenickyLetterOrDigit())
                    continue;

                _buttons.Add(SmallButton(i, ((char)i).ToString()));
            }

            _newButtonX = 4;
            _newButtonY = Height - 64;
            int colorId = _buttons.Count;
            _buttons.AddRange(ESAColor.GetColors().Select(color => ColorButton(color.Name, color.Color)));

            App.SelectedButton = _buttons[0];
            App.SelectedColor = _buttons[colorId];
        }

        private MGButton BigButton(byte name, string text) => BigButton(name, text, ButtonClick);

        private MGButton BigButton(byte name, string text, MouseClickEventHandler mouseClick)
        {
            if (_newButtonX != 4)
            {
                _newButtonX = 4;
                _newButtonY += 32;
            }
            MGButton button = new(name, new(_newButtonX, _newButtonY, Width - 8, 24), text, ESAColor.White, ESAColor.DarkBlue, mouseClick);
            _newButtonY += 32;
            return button;
        }

        private MGButton SmallButton(int name, string text) => SmallButton(name, text, ButtonClick);

        private MGButton SmallButton(int name, string text, MouseClickEventHandler mouseClick)
        {
            int width = text.Length * 8 + 8;

            if (_newButtonX + width > Width - 4)
            {
                _newButtonX = 4;
                _newButtonY += 32;
            }

            MGButton button = new(name, new(_newButtonX, _newButtonY, width, 24), text, ESAColor.White, ESAColor.DarkBlue, mouseClick);
            _newButtonX += width + 4;
            return button;
        }

        private MGButton ColorButton(int name, Color color)
        {
            const int width = 16;

            if (_newButtonX + width > Width - 4)
            {
                _newButtonX = 4;
                _newButtonY += 32;
            }

            MGButton button = new(name, new(_newButtonX, _newButtonY, width, 24), string.Empty, ColorUtils.GetMoreContrastColor(color), color, ColorButtonClick);
            _newButtonX += width + 4;
            return button;
        }

        private void ButtonClick(MouseButton mouseButton, MGButton button)
        {
            if (mouseButton == MouseButton.Left)
                App.SelectedButton = button;
        }

        private void ColorButtonClick(MouseButton mouseButton, MGButton button)
        {
            if (mouseButton == MouseButton.Left)
                App.SelectedColor = button;
        }

        private void BackgroundChange(MouseButton mouseButton, MGButton button)
        {
            if (mouseButton != MouseButton.Left)
                return;

            App.LightMode = !App.LightMode;
            button.Background = App.LightMode ? ESAColor.Orange : ESAColor.DarkBlue;
        }

        private void GridChange(MouseButton mouseButton, MGButton button)
        {
            if (mouseButton != MouseButton.Left)
                return;

            App.Grid = !App.Grid;
            button.Background = App.Grid ? ESAColor.Orange : ESAColor.DarkBlue;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void OnMouseDown(MouseStateArgs mouseState)
        {
            foreach (var button in _buttons)
                button.MouseDown(mouseState);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ESAColor.Black);

            _spriteBatch.Begin();

            _spriteBatch.DrawString(_store.ETM, "V\u0098b\u0088r barvy:", new(4, Height - 80), ESAColor.Gray);

            foreach (var button in _buttons)
                button.Draw(_spriteBatch, _store.ETM, _store.FillTexture);

            _spriteBatch.End();
        }
    }



    public delegate void MouseClickEventHandler(MouseButton mouseButton, MGButton button);

    public class MGButton
    {
        public int Name { get; init; }

        public Rectangle Position { get; set; }

        public string Text { get; set; }

        public Color Foreground { get; set; }

        public Color Background { get; set; }

        public event MouseClickEventHandler? MouseClick;

        public bool IsSelected { get; set; }

        public MGButton(int name, Rectangle position, string text, Color foreground, Color background, MouseClickEventHandler? handler = null)
        {
            Name = name;
            Position = position;
            Text = text;
            Foreground = foreground;
            Background = background;
            MouseClick += handler;
        }

        public void MouseDown(MouseStateArgs mouseState)
        {
            if (Position.Contains(mouseState.Position))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    OnMouseClick(MouseButton.Left);
                else if (mouseState.RightButton == ButtonState.Pressed)
                    OnMouseClick(MouseButton.Right);
                else if (mouseState.MiddleButton == ButtonState.Pressed)
                    OnMouseClick(MouseButton.Middle);
            }
        }

        public void Draw(SpriteBatch spriteBatch, BitmapFont font, Texture2D fillTexture)
        {
            if (Text.Length == 0)
            {
                spriteBatch.Draw(fillTexture, Position, Background);

                if (Background == ESAColor.Black)
                {
                    spriteBatch.Draw(fillTexture, Position.BorderLeft(1), ESAColor.White);
                    spriteBatch.Draw(fillTexture, Position.BorderTop(1), ESAColor.White);
                    spriteBatch.Draw(fillTexture, Position.BorderRight(1), ESAColor.White);
                    spriteBatch.Draw(fillTexture, Position.BorderBottom(1), ESAColor.White);
                }

                if (!IsSelected)
                    return;

                int xx = Position.X + (Position.Width - 8) / 2;
                int yy = Position.Y + (Position.Height - 12) / 2;
                spriteBatch.DrawString(font, "\x0B", new Vector2(xx, yy), Foreground);

                return;
            }

            spriteBatch.Draw(fillTexture, Position, IsSelected ? ESAColor.DarkTurquoise : Background);
            int x = Position.X + (Position.Width - Text.Length * 8) / 2;
            int y = Position.Y + (Position.Height - 12) / 2;
            spriteBatch.DrawString(font, Text, new Vector2(x, y), Foreground);
        }

        protected void OnMouseClick(MouseButton button) => MouseClick?.Invoke(button, this);
    }
}
