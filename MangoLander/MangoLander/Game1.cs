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
using MangoLander.Graphics;
using MangoLander.Menus;
using MangoLander.Physics;

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

        // Sensors
        Motion _motion;

        // Level
        Level _level;

        // State
        public GameState CurrentState { get; set; }

        // Menu
        MenuManager _menus;

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

            // Initial state
            CurrentState = GameState.Menu;

            // Menus
            _menus = new MenuManager(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Setup level
            _level = Level.GenerateStandardLevel(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, 4, 20);

            // Setup entities
            _level.Lander = new Lander(new Vector2(_graphics.PreferredBackBufferWidth / 2, 100));

            // Setup sensors
            if (_motion != null)
            {
                _motion.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<MotionReading>>(_motion_CurrentValueChanged);
                _motion.Start();
            }

            base.Initialize();
        }

        void _motion_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
        {
            if (CurrentState == GameState.Active)
            {
                _level.InteractMotion(e.SensorReading, this.Window.CurrentOrientation);
            }
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
            _level.Lander.LanderTexture = this.Content.Load<Texture2D>(".\\Sprites\\Lander");
            _menus.Intro.IntroTexture = this.Content.Load<Texture2D>(".\\Sprites\\Title");
            _menus.MainMenu.BackgroundTexture = this.Content.Load<Texture2D>(".\\Sprites\\Menu-Background");

            // Load fonts
            _level.UIFont = this.Content.Load<SpriteFont>(".\\Fonts\\UI");

            // Initialize lander based on texture
            _level.Lander.Width = _level.Lander.LanderTexture.Width;
            _level.Lander.Height = _level.Lander.LanderTexture.Height;
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
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            TouchCollection touches = TouchPanel.GetState();

            if (CurrentState == GameState.Active || CurrentState == GameState.GameOver || CurrentState == GameState.Paused)
            {
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();
            }

            switch (CurrentState)
            {
                case GameState.Menu:
                    {
                        _menus.Interact(gamePadState, touches);
                        _menus.Update(gameTime);
                    }
                    break;
                case GameState.Active:
                    {
                        _level.Interact(gamePadState, touches);
                        _level.Update(gameTime);

                        // Check for collisions
                        if (_level.Lander.DoCollision(_level))
                        {
                            _level.Lander.Dead = true;
                            CurrentState = GameState.GameOver;
                        }
                    }
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (CurrentState)
            {
                case GameState.Active:
                case GameState.GameOver:
                case GameState.Paused:
                    {
                        _level.Draw(_graphics, _spriteBatch, _primitiveBatch);
                    }
                    break;
                case GameState.Menu:
                    {
                        _menus.Draw(_graphics, _spriteBatch, _primitiveBatch);
                    }
                    break;
                default:
                    break;
            }


            base.Draw(gameTime);
        }
    }
}
