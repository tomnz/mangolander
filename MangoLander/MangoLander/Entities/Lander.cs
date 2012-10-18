using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using MangoLander.Physics;
using MangoLander.Graphics;

namespace MangoLander.Entities
{
    public class Lander : IInteractive, IMotionInteractive, IUpdateable, IRenderable
    {
        // Constants
        private const double FUEL_USE_RATE = 5;

        // Properties
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        // Forces
        public Gravity Gravity { get; set; }
        public Thruster Thruster { get; set; }

        // Dimensions
        public int Width { get; set; }
        public int Height { get; set; }

        public float Rotation { get; set; }

        // Textures
        public Texture2D LanderTexture { get; set; }
        private AnimatedSprite _landerThrustingSprite;

        // State
        public double Fuel { get; set; }
        public bool Dead { get; set; }

        // Constructor
        public Lander()
        {
            Position = new Vector2();
            Velocity = new Vector2();
            Fuel = 500;
            Dead = false;
            Thruster = new Thruster();
            Gravity = new Gravity();
        }

        public Lander(Vector2 position) :
            this()
        {
            this.Position = position;
        }

        public void InitializeAnimatedSprites(Texture2D thrusterTexture)
        {
            _landerThrustingSprite = new AnimatedSprite(thrusterTexture, 80, 80, 0.05, true);
        }

        public void Accelerate(Vector2 accel, TimeSpan elapsedTime)
        {
            Velocity += Vector2.Multiply(accel, (float)elapsedTime.TotalSeconds);
        }

        public Rectangle GetScreenRectangle()
        {
            return new Rectangle(
                (int)(this.Position.X),
                (int)(this.Position.Y),
                this.Width,
                this.Height
                );
        }

        public Vector2 GetScreenOrigin()
        {
            return new Vector2(this.Width / 2, this.Height / 2);
        }

        /// <summary>
        /// Test for a collision between the Lander and a Level
        /// </summary>
        /// <param name="level">Level to test for collision</param>
        /// <returns>True if a collision is detected, false otherwise</returns>
        public bool DoCollision(Level level)
        {
            Rectangle rect = this.GetScreenRectangle();

            double baseAngle = Math.Atan(this.Height / this.Width);
            double len = Math.Sqrt(this.Height * this.Height / 4 + this.Width * this.Width / 4);

            Vector2 tr = new Vector2((float)(Math.Sin(baseAngle + this.Rotation) * len) + this.Position.X, (float)(Math.Cos(baseAngle + this.Rotation) * len) + this.Position.Y);
            Vector2 tl = new Vector2((float)(Math.Sin(Math.PI - baseAngle + this.Rotation) * len) + this.Position.X, (float)(Math.Cos(Math.PI - baseAngle + this.Rotation) * len) + this.Position.Y);
            Vector2 bl = new Vector2((float)(Math.Sin(Math.PI + baseAngle + this.Rotation) * len) + this.Position.X, (float)(Math.Cos(Math.PI + baseAngle + this.Rotation) * len) + this.Position.Y);
            Vector2 br = new Vector2((float)(Math.Sin(2 * Math.PI - baseAngle + this.Rotation) * len) + this.Position.X, (float)(Math.Cos(2 * Math.PI - baseAngle + this.Rotation) * len) + this.Position.Y);

            // Check intersection
            int minX = (int)(this.Position.X + this.Width / 2 - len);
            int maxX = (int)(this.Position.X + this.Width / 2 + len);

            int lastPoint = 0;
            bool collision = false;
            for (int i = 0; i < level.Terrain.Count; i++)
            {
                if (level.Terrain[i].X >= minX && level.Terrain[i].X <= maxX)
                {
                    // If this is the first point being checked, then we should
                    // also check backwards by one level point to make sure we
                    // cover all possible intersections
                    if (lastPoint < i - 1)
                    {
                        if (LineIntersectsLine(level.Terrain[i - 1], level.Terrain[i], tr, tl))
                        {
                            collision = true;
                            break;
                        }
                        if (LineIntersectsLine(level.Terrain[i - 1], level.Terrain[i], tl, bl))
                        {
                            collision = true;
                            break;
                        }
                        if (LineIntersectsLine(level.Terrain[i - 1], level.Terrain[i], bl, br))
                        {
                            collision = true;
                            break;
                        }
                        if (LineIntersectsLine(level.Terrain[i - 1], level.Terrain[i], br, tr))
                        {
                            collision = true;
                            break;
                        }
                    }
                    // Intersect the line segment with each of the sides of the lander
                    if (LineIntersectsLine(level.Terrain[i], level.Terrain[i + 1], tr, tl))
                    {
                        collision = true;
                        break;
                    }
                    if (LineIntersectsLine(level.Terrain[i], level.Terrain[i + 1], tl, bl))
                    {
                        collision = true;
                        break;
                    }
                    if (LineIntersectsLine(level.Terrain[i], level.Terrain[i + 1], bl, br))
                    {
                        collision = true;
                        break;
                    }
                    if (LineIntersectsLine(level.Terrain[i], level.Terrain[i + 1], br, tr))
                    {
                        collision = true;
                        break;
                    }
                    lastPoint = i;
                }
            }

            if (collision) return true;
            else return false;
        }


        /// <summary>
        /// Detect an intersection between two line segments
        /// </summary>
        /// <param name="a1">First point of line A</param>
        /// <param name="a2">Second point of line A</param>
        /// <param name="b1">First point of line B</param>
        /// <param name="b2">Second point of line B</param>
        /// <returns>True if the lines intersect, false otherwise</returns>
        private bool LineIntersectsLine(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            float ud, ua = 0, ub;
            ud = (b2.Y - b1.Y) * (a2.X - a1.X) - (b2.X - b1.X) * (a2.Y - a1.Y);
            if (ud != 0)
            {
                ua = ((b2.X - b1.X) * (a1.Y - b1.Y) - (b2.Y - b1.Y) * (a1.X - b1.X)) / ud;
                ub = ((a2.X - a1.X) * (a1.Y - b1.Y) - (a2.Y - a1.Y) * (a1.X - b1.X)) / ud;
                if (ua < 0 || ua > 1 || ub < 0 || ub > 1) ua = 0;
            }
            return ua == 0 ? false : true;
        }

        public void Interact(GamePadState gamePadState, TouchCollection touches)
        {
            foreach (TouchLocation touch in touches)
            {
                if (touch.State == TouchLocationState.Pressed)
                {
                    this.Thruster.Active = true;
                    _landerThrustingSprite.Play();
                }
                if (touch.State == TouchLocationState.Released)
                {
                    this.Thruster.Active = false;
                    _landerThrustingSprite.Stop();
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            // Do motion
            this.Accelerate(this.Gravity.GetAcceleration(), gameTime.ElapsedGameTime);
            this.Accelerate(this.Thruster.GetAcceleration(), gameTime.ElapsedGameTime);

            Position += Vector2.Multiply(Velocity, (float)gameTime.ElapsedGameTime.TotalSeconds);

            // Fuel usage
            if (this.Thruster.Active)
            {
                this.Fuel -= gameTime.ElapsedGameTime.TotalSeconds * FUEL_USE_RATE;
            }

            _landerThrustingSprite.Update(gameTime);
        }

        public void InteractMotion(MotionReading motion, DisplayOrientation orientation)
        {
            // Rotate the lander based on the phone's current physical angle
            float rotation = motion.Attitude.Pitch / 1.2f;

            // Account for the phone being "upside down" in alternate orientations
            if (orientation == DisplayOrientation.LandscapeLeft)
                rotation = -rotation;

            this.Rotation = rotation;
            this.Thruster.Rotation = rotation;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
        {
            spriteBatch.Begin();
            if (this.Thruster.Active)
            {
                _landerThrustingSprite.Draw(spriteBatch, this.GetScreenRectangle(), Color.White, this.Rotation, this.GetScreenOrigin(), SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(this.LanderTexture, this.GetScreenRectangle(), null, this.Dead ? Color.Red : Color.White, this.Rotation, this.GetScreenOrigin(), SpriteEffects.None, 0);
            }
            spriteBatch.End();
        }
    }
}
