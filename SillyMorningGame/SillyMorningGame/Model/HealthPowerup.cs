using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SillyMorningGame.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SillyMorningGame.Model
{
    class HealthPowerup
    {
        // Animation representing the enemy
        public Texture2D PowerupTexture;

        public Animation PowerupAnimation;

        Viewport viewport;

        // The position of the enemy ship relative to the top left corner of thescreen
        public Vector2 Position;

        // The state of the Enemy Ship
        public bool Active;

        // Get the width of the enemy ship
        public int Width
        {
            get { return PowerupAnimation.FrameWidth; }
        }

        // Get the height of the enemy ship
        public int Height
        {
            get { return PowerupAnimation.FrameHeight; }
        }

        // The speed at which the enemy moves
        float powerupMoveSpeed;

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
        {
            PowerupTexture = texture;
            Position = position;
            this.viewport = viewport;

            Active = true;

            powerupMoveSpeed = 6f;
        }

        public void Initialize(Animation animation, Vector2 position)
        {
            PowerupAnimation = animation;
            Position = position;
            Active = true;
            powerupMoveSpeed = 6f;
        }

        public void Update(GameTime gameTime)
        {
            // The enemy always moves to the left so decrement it's xposition
            Position.X -= powerupMoveSpeed;

            // Update the position of the Animation
            PowerupAnimation.Position = Position;

            // Update Animation
            PowerupAnimation.Update(gameTime);

            // If the enemy is past the screen or its health reaches 0 then deactivateit
            if (Position.X < -Width)
            {
                // By setting the Active flag to false, the game will remove this objet fromthe 
                // active game list
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(PowerupAnimation, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            PowerupAnimation.Draw(spriteBatch);
        } 
    }
}
