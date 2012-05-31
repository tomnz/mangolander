using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MangoLander;
using MangoLander.Graphics;
using MangoLander.Physics;

namespace MangoLander.Entities
{
    public class Level : IMotionInteractive, IInteractive, IUpdateable, IRenderable
    {
        private List<Vector2> _terrain;
        public List<Vector2> Terrain { get { return _terrain; } }

        // Entities
        public Lander Lander { get; set; }

        // Fonts
        public SpriteFont UIFont { get; set; }

        public Level()
        {
            _terrain = new List<Vector2>();
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
        {
            // Draw sprites
            this.Lander.Draw(graphics, spriteBatch, primitiveBatch);

            // Draw terrain
            primitiveBatch.Begin(PrimitiveType.TriangleList);

            if (this.Terrain.Count > 1)
            {
                for (int i = 0; i < (this.Terrain.Count - 1); i++)
                {
                    primitiveBatch.AddVertex(new Vector2(this.Terrain[i].X, this.Terrain[i].Y), Color.White);
                    primitiveBatch.AddVertex(new Vector2(this.Terrain[i + 1].X, graphics.PreferredBackBufferHeight), Color.White);
                    primitiveBatch.AddVertex(new Vector2(this.Terrain[i].X, graphics.PreferredBackBufferHeight), Color.White);

                    primitiveBatch.AddVertex(new Vector2(this.Terrain[i].X, this.Terrain[i].Y), Color.White);
                    primitiveBatch.AddVertex(new Vector2(this.Terrain[i + 1].X, this.Terrain[i + 1].Y), Color.White);
                    primitiveBatch.AddVertex(new Vector2(this.Terrain[i + 1].X, graphics.PreferredBackBufferHeight), Color.White);
                }
            }

            primitiveBatch.End();

            // Draw UI text
            spriteBatch.Begin();
            spriteBatch.DrawString(UIFont, string.Format("Fuel: {0:0.0}", this.Lander.Fuel), new Vector2(20, 20), Color.White);
            spriteBatch.End();
        }


        /// <summary>
        /// Generates a simple sawtooth wave
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Level GenerateDummyLevel(int width, int height)
        {
            Level level = new Level();

            bool up = false;

            for (int i = 1; i <= width; i += 20)
            {
                level.Terrain.Add(new Vector2(i, height / 2 + (up ? 10 : -10)));
                up = !up;
            }

            return level;
        }

        /// <summary>
        /// Generates terrain with random variations. Vary the inputs to adjust the look of the terrain.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="stepSize"></param>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public static Level GenerateStandardLevel(int width, int height, int stepSize, double difficulty)
        {
            Level level = new Level();

            Random rand = new Random();

            double y = ((double)height * 0.67);
            double baseY = y;

            for (int i = 1; i <= width; i += stepSize)
            {
                level.Terrain.Add(new Vector2(i, (int)y));

                y += (rand.NextDouble() * difficulty - (difficulty / 2)) + ((baseY - y) / (difficulty * 2));
            }

            return level;
        }

        public void Update(GameTime gameTime)
        {
            this.Lander.Update(gameTime);
        }

        public void Interact(GamePadState gamePadState, Microsoft.Xna.Framework.Input.Touch.TouchCollection touches)
        {
            this.Lander.Interact(gamePadState, touches);
        }

        public void InteractMotion(MotionReading motion, DisplayOrientation orientation)
        {
            this.Lander.InteractMotion(motion, orientation);
        }
    }
}
