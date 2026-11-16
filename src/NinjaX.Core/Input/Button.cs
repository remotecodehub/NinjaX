using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NinjaX.Core.Input;

class Button
{
    private string text;
    private Vector2 position;
    private Vector2 size;
    public Rectangle rectangle;
    private SpriteFont font;
    private Color neutral;
    private Color pressed;

    public bool IsPressed;

    public Button(string text, Vector2 position, Color neutral, Color pressed, SpriteFont font)
    {
        this.text = text;
        this.position = position;
        this.font = font;
        this.neutral = neutral;
        this.pressed = pressed;

        this.size = font.MeasureString(text);
        this.rectangle = new Rectangle((int)Math.Round(position.X - size.X * 0.5f), (int)Math.Round(position.Y - size.Y * 0.5f), (int)Math.Round(size.X), (int)Math.Round(size.Y));
    }

    public bool Entered(float x, float y)
    {
        IsPressed = rectangle.Contains(x, y);
        return IsPressed;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var origin = size * 0.5f;
        var color = IsPressed ? pressed : neutral;
        spriteBatch.DrawString(font, text, position, color, 0, origin, 1, SpriteEffects.None, 0);
    }
}