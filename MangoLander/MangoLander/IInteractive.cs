using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangoLander
{
    /// <summary>
    /// Defines the interaction for accepting touch and button input
    /// </summary>
    interface IInteractive
    {
        void Interact(GamePadState gamePadState, TouchCollection touches);
    }
}
