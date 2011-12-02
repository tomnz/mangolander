using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MangoLander.Physics
{
    class Gravity
    {
        // Constants
        private const float _DEFAULT_STRENGTH = 15;

        // Properties
        public float Strength { get; set; }

        // Constructors
        public Gravity()
        {
            this.Strength = _DEFAULT_STRENGTH;
        }

        public Gravity(float strength)
        {
            this.Strength = strength;
        }

        // Methods
        public Vector2 GetAcceleration()
        {
            Vector2 result = new Vector2(0, this.Strength);
            return result;
        }
    }
}
