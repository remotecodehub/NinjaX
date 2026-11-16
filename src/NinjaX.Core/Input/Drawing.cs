using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace NinjaX.Core.Input;

public static class Drawing
{
    public static SpriteFont font; // initialize me!
    public static void DrawStringCentered(string text, Vector2 position, Color color, SpriteBatch spriteBatch)
    {
        var size = font.MeasureString(text);
        var origin = size * 0.7f;

        spriteBatch.DrawString(font, text, position, Color.Black, 0, origin, 1, SpriteEffects.None, 0);
    }
  
}