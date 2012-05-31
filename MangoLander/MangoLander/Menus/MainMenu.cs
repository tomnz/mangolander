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
    public class MainMenu : IRenderable
    {
        // Textures
        public Texture2D BackgroundTexture { get; set; }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(this.BackgroundTexture, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();
        }

        public void Interact(GamePadState gamePadState, TouchCollection touches, ref MenuState currentState)
        {
            if (touches.Count > 0)
            {
                if (touches[0].State == TouchLocationState.Pressed)
                {
                    currentState = MenuState.Playing;
                }
            }
        }
    }
}
