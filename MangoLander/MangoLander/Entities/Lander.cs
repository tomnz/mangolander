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

        // Dimensions
        public int Width { get; set; }
        public int Height { get; set; }

        public float Rotation { get; set; }

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
    }
}
