using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangoLander
{
    /// <summary>
    /// Defines the interaction for accepting accelerometer/motion input
    /// </summary>
    interface IMotionInteractive
    {
        void InteractMotion(MotionReading motion, DisplayOrientation orientation);
    }
}
