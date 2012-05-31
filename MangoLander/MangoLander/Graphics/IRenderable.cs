using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangoLander.Graphics
{
    /// <summary>
    /// Defines the interaction for drawing an object to the screen
    /// </summary>
    interface IRenderable
    {
        void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch);
    }
}
