using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace JopSchemaEditor
{
    internal class GameStore : IDisposable
    {
        public int FontWidth { get; private set; }

        public int FontHeight { get; private set; }

        public int CursorWidth { get; private set; }

        public int CursorHeight { get; private set; }

        public BitmapFont ETM { get; private set; }

        public BitmapFont ETMG { get; private set; }

        public Texture2D FillTexture { get; private set; }

        public GameStore(ContentManager contentManager, GraphicsDevice graphicsDevice, int fontWidth, int fontHeight)
        {
            FontHeight = fontHeight;
            FontWidth = fontWidth;
            CursorWidth = fontWidth + 2;
            CursorHeight = fontHeight + 2;

            ETM = contentManager.Load<BitmapFont>($"ETM{fontWidth}x{fontHeight}");
            ETMG = contentManager.Load<BitmapFont>($"ETM{fontWidth}x{fontHeight}G");

            FillTexture = new(graphicsDevice, 1, 1);
            FillTexture.SetData([Color.White]);
        }

        public void Dispose()
        {
            FontHeight = default;
            FontWidth = default;
            CursorWidth = default;
            CursorHeight = default;

            ETM = null!;
            ETMG = null!;
            FillTexture.Dispose();
            FillTexture = null!;

            GC.Collect();
        }
    }
}
