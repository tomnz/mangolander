using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangoLander.Entities
{
    public class Level
    {
        private List<Point> _terrain;
        public List<Point> Terrain { get { return _terrain; } }

        public Level()
        {
            _terrain = new List<Point>();
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
                level.Terrain.Add(new Point(i, height / 2 + (up ? 10 : -10)));
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
                level.Terrain.Add(new Point(i, (int)y));

                y += (rand.NextDouble() * difficulty - (difficulty / 2)) + ((baseY - y) / (difficulty * 2));
            }

            return level;
        }
    }
}
