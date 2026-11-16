using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NinjaX.Core.Input;

public interface IInput
{
    ActionRequest Read();
    IInput Update(GameTime gameTime);
    IInput Draw(SpriteBatch spriteBatch);
}