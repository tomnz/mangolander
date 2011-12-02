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

        public Lander(Vector2 position) :
            this()
        {
            this.Position = position;
        }

        public void Accelerate(Vector2 accel, TimeSpan elapsedTime)
        {
            Velocity += Vector2.Multiply(accel, (float)elapsedTime.TotalSeconds);
        }

        public void DoMovement(TimeSpan elapsedTime)
        {
            Position += Vector2.Multiply(Velocity, (float)elapsedTime.TotalSeconds);
        }
    }
}
