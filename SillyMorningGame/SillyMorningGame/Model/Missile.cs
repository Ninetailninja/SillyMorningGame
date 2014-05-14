using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SillyMorningGame.View;

namespace SillyMorningGame.Model
{
    class Missile
    {
        public Animation MissileAnimation;

        // The position of the enemy ship relative to the top left corner of thescreen
        public Vector2 Position;

        // The state of the Enemy Ship
        public bool Active;

        Viewport screen;

        // The amount of damage the enemy inflicts on the player ship
        public int Damage;

        // Get the width of the enemy ship
        public int Width
        {
            get { return MissileAnimation.FrameWidth; }
        }
        // Determines how fast the projectile moves
        float projectileMoveSpeed;

        // Get the height of the enemy ship
        public int Height
        {
            get { return MissileAnimation.FrameHeight; }
        }


        public void Initialize(Animation animation, Vector2 position, Viewport viewport)
        {
            // Load the enemy ship texture
            MissileAnimation = animation;

            // Set the position of the enemy
            Position = position;

            // We initialize the enemy to be active so it will be update in the game
            Active = true;

            screen = viewport;

            // Set the amount of damage the enemy can do
            Damage = 50;

            // Set how fast the enemy moves
            projectileMoveSpeed = 15f;



        }


        public void Update(GameTime gameTime)
        {
            // Projectiles always move to the right
            Position.X += projectileMoveSpeed;

            // Update the position of the Animation
            MissileAnimation.Position = Position;

            // Update Animation
            MissileAnimation.Update(gameTime);

            if (Position.X > screen.Width)
            {
                // By setting the Active flag to false, the game will remove this objet fromthe 
                // active game list
                Active = false;
            }
        }


        public void Draw(SpriteBatch spriteBatch, Color color)
        {
           
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            MissileAnimation.Draw(spriteBatch);
        }
    }
}
