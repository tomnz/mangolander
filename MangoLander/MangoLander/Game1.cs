using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using MangoLander.Entities;
using MangoLander.Physics;
using MangoLander.Graphics;

namespace MangoLander
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        PrimitiveBatch _primitiveBatch;

        // Textures
        Texture2D _landerTexture;

        // Entities
        Lander _lander;

        // Forces
        Gravity _gravity;
        Thruster _thruster;

        // Sensors
        Motion _motion;

        // Level
        Level _level;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            _graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";

            if (Motion.IsSupported)
            {
                _motion = new Motion();
            }

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Setup entities
            _lander = new Lander(new Vector2(_graphics.PreferredBackBufferWidth / 2, 100));

            // Setup physics
            _gravity = new Gravity();
            _thruster = new Thruster();

            // Setup sensors
            if (_motion != null)
            {
                _motion.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<MotionReading>>(_motion_CurrentValueChanged);
                _motion.Start();
            }

            // Setup level
            _level = Level.GenerateStandardLevel(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, 4, 20);

            base.Initialize();
        }

        void _motion_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
        {
            float rotation = e.SensorReading.Attitude.Pitch / 1.2f;

            // Account for the phone being "upside down" in alternate orientations
            if (this.Window.CurrentOrientation == DisplayOrientation.LandscapeLeft)
                rotation = -rotation;

            _lander.Rotation = rotation;
            _thruster.Rotation = rotation;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create a new PrimitiveBatch
            _primitiveBatch = new PrimitiveBatch(GraphicsDevice);

            // Load textures
            _landerTexture = this.Content.Load<Texture2D>(".\\Sprites\\Lander");

            // Initialize lander based on texture
            _lander.Width = _landerTexture.Width;
            _lander.Height = _landerTexture.Height;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            this.Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Accept touch
            TouchCollection touches = TouchPanel.GetState();
            foreach (TouchLocation touch in touches)
            {
                if (touch.State == TouchLocationState.Pressed)
                {
                    _thruster.Active = true;
                }
                if (touch.State == TouchLocationState.Released)
                {
                    _thruster.Active = false;
                }
            }

            // Gravity
            _lander.Accelerate(_gravity.GetAcceleration(), gameTime.ElapsedGameTime);
            _lander.Accelerate(_thruster.GetAcceleration(), gameTime.ElapsedGameTime);

            // Update lander position
            _lander.DoMovement(gameTime.ElapsedGameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Draw sprites
            _spriteBatch.Begin();
            _spriteBatch.Draw(_landerTexture, _lander.GetScreenRectangle(), null, _thruster.Active ? Color.OrangeRed : Color.White, _lander.Rotation, _lander.GetScreenOrigin(), SpriteEffects.None, 0);
            _spriteBatch.End();

            // Draw terrain
            _primitiveBatch.Begin(PrimitiveType.TriangleList);

            if (_level.Terrain.Count > 1)
            {
                for (int i = 0; i < (_level.Terrain.Count - 1); i++)
                {
                    _primitiveBatch.AddVertex(new Vector2(_level.Terrain[i].X, _level.Terrain[i].Y), Color.White);
                    _primitiveBatch.AddVertex(new Vector2(_level.Terrain[i + 1].X, _graphics.PreferredBackBufferHeight), Color.White);
                    _primitiveBatch.AddVertex(new Vector2(_level.Terrain[i].X, _graphics.PreferredBackBufferHeight), Color.White);

                    _primitiveBatch.AddVertex(new Vector2(_level.Terrain[i].X, _level.Terrain[i].Y), Color.White);
                    _primitiveBatch.AddVertex(new Vector2(_level.Terrain[i + 1].X, _level.Terrain[i + 1].Y), Color.White);
                    _primitiveBatch.AddVertex(new Vector2(_level.Terrain[i + 1].X, _graphics.PreferredBackBufferHeight), Color.White);
                }
            }

            _primitiveBatch.End();

            base.Draw(gameTime);
        }
    }
}
