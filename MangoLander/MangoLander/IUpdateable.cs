using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangoLander
{
    /// <summary>
    /// Defines the interaction for performing per-frame updates
    /// </summary>
    interface IUpdateable
    {
        void Update(GameTime gameTime);
    }
}
