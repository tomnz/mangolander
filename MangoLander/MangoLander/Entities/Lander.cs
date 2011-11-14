using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MangoLander.Entities
{
    public class Lander
    {
        // Properties
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public double Fuel { get; set; }

        // Constructor
        public Lander()
        {
            Position = new Vector2();
            Velocity = new Vector2();
            Fuel = 500;
        }
    }
}
