using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MangoLander
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        // Textures
        Texture2D _landerTexture;

        // Entities
        Lander _lander;

        // Forces
        Gravity _gravity;
        Thruster _thruster;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

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
            _lander = new Lander(new Vector2(_graphics.PreferredBackBufferWidth / 2, 50));

            // Setup physics
            _gravity = new Gravity();
            _thruster = new Thruster();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            _landerTexture = this.Content.Load<Texture2D>(".\\Sprites\\Lander");
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
            _spriteBatch.Draw(_landerTexture, _lander.Position, _thruster.Active ? Color.OrangeRed : Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
