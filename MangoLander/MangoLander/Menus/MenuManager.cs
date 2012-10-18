using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MangoLander;
using MangoLander.Graphics;

namespace MangoLander.Menus
{
    public class MenuManager : IRenderable, IInteractive, IUpdateable
    {
        // Menus
        public MainMenu MainMenu { get; set; }
        public Intro Intro { get; set; }
        public Settings Settings { get; set; }

        // State
        private MenuState _currentState;
        public MenuState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        // To be used sparingly: reference to the calling Game class
        private Game1 _game;

        public MenuManager(Game1 game)
        {
            this.MainMenu = new MainMenu();
            this.Intro = new Intro();
            _game = game;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
        {
            switch (this.CurrentState)
            {
                case MenuState.Intro:
                    {
                        this.Intro.Draw(graphics, spriteBatch, primitiveBatch);
                    }
                    break;
                case MenuState.Main:
                    {
                        this.MainMenu.Draw(graphics, spriteBatch, primitiveBatch);
                    }
                    break;
                case MenuState.Settings:
                    {
                        this.Settings.Draw(graphics, spriteBatch, primitiveBatch);
                    }
                    break;
                default:
                    break;
            }
        }

        public void Interact(GamePadState gamePadState, TouchCollection touches)
        {
            switch (this.CurrentState)
            {
                case MenuState.Intro:
                    {
                        this.Intro.Interact(gamePadState, touches, ref _currentState);
                    }
                    break;
                case MenuState.Main:
                    {
                        this.MainMenu.Interact(gamePadState, touches, ref _currentState);
                    }
                    break;
                case MenuState.Settings:
                    {
                        this.Settings.Interact(gamePadState, touches, ref _currentState);
                    }
                    break;
                default:
                    break;
            }

            if (this.CurrentState == MenuState.Playing)
            {
                // We probably shouldn't be doing anything here - change the main game state
                _game.CurrentState = GameState.Active;
            }
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
