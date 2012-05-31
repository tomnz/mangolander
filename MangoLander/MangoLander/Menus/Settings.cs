using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MangoLander.Graphics;

namespace MangoLander.Menus
{
    public class Settings : IRenderable
    {
        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
        {
        }

        public void Interact(GamePadState gamePadState, TouchCollection touches, ref MenuState currentState)
        {
        }
    }
}
