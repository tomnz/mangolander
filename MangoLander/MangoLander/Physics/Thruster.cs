using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace MangoLander.Physics
{
    public class Thruster
    {
        // Constants
        private const float _DEFAULT_STRENGTH = 60;

        // Properties
        public float Strength { get; set; }
        public bool Active { get; set; }

        public float Rotation { get; set; }

        // Constructors
        public Thruster()
        {
            this.Strength = _DEFAULT_STRENGTH;
            this.Active = false;
        }

        public Thruster(float strength) :
            this()
        {
            this.Strength = strength;
        }

        // Methods
        public Vector2 GetAcceleration()
        {
            if (!this.Active)
                return new Vector2();

            Vector2 result = new Vector2(0, - this.Strength);
            result = Vector2.Transform(result, Matrix.CreateRotationZ(this.Rotation));
            return result;
        }
    }
}
