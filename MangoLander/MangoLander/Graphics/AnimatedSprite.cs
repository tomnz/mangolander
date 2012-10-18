using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangoLander.Graphics
{
    class AnimatedSprite : IUpdateable
    {
        public Texture2D Texture { get; private set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }

        public int CurrentFrame { get; private set; }
        public int TotalFrames { get; private set; }

        public double FrameDelay { get; set; }
        public bool LoopAnimation { get; set; }

        private bool _initialized = false;

        private double _elapsedSinceLastFrame = 0;
        private bool _playing = false;

        public AnimatedSprite()
        {
        }

        public AnimatedSprite(Texture2D texture, int frameWidth, int frameHeight, double frameDelay, bool loopAnimation)
        {
            Initialize(texture, frameWidth, frameHeight, frameDelay, loopAnimation);
        }

        public void Initialize(Texture2D texture, int frameWidth, int frameHeight, double frameDelay, bool loopAnimation)
        {
            this.Texture = texture;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.FrameDelay = frameDelay;
            this.LoopAnimation = loopAnimation;
            this.CurrentFrame = 0;

            if (this.FrameWidth > this.Texture.Width || this.FrameHeight > this.Texture.Height)
            {
                // Illegal texture and frame height/width specification
                _initialized = false;
                return;
            }

            this.TotalFrames = this.Texture.Width / this.FrameWidth;
            _initialized = true;
        }

        private Rectangle GetCurrentFrameRectangle()
        {
            return new Rectangle(this.FrameWidth * this.CurrentFrame, 0, this.FrameWidth, this.FrameHeight);
        }

        // Control methods
        public void Play()
        {
            this.CurrentFrame = 0;
            _playing = true;
            _elapsedSinceLastFrame = 0;
        }

        public void Stop()
        {
            _playing = false;
            _elapsedSinceLastFrame = 0;
        }

        public void Reset()
        {
            this.CurrentFrame = 0;
            _elapsedSinceLastFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            if (!_initialized)
                return;

            spriteBatch.Draw(this.Texture, destinationRectangle, this.GetCurrentFrameRectangle(), color, rotation, origin, effects, layerDepth);
        }

        public void Update(GameTime gameTime)
        {
            if (!_initialized || !_playing || this.FrameDelay <= 0 || this.TotalFrames == 0)
                return;

            double currentFrameTime = gameTime.ElapsedGameTime.TotalSeconds;
            _elapsedSinceLastFrame += currentFrameTime;

            while (_elapsedSinceLastFrame > this.FrameDelay)
            {
                _elapsedSinceLastFrame -= this.FrameDelay;
                this.CurrentFrame++;

                // Are we at the end?
                if (this.CurrentFrame == this.TotalFrames)
                {
                    if (this.LoopAnimation)
                    {
                        // Loop back to the start
                        this.CurrentFrame = 0;
                    }
                    else
                    {
                        // Stop playing
                        _playing = false;
                        _elapsedSinceLastFrame = 0;
                        break;
                    }
                }
            }
        }
    }
}
