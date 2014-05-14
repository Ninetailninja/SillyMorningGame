using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SillyMorningGame.View;

namespace SillyMorningGame.Model
{
    class Boss1
    {
        // Animation representing the enemy
        public Animation bossAnimation;

        // The position of the enemy ship relative to the top left corner of thescreen
        public Vector2 Position;

        // The state of the Enemy Ship
        public bool Active;

        // The hit points of the enemy, if this goes to zero the enemy dies
        public int Health;

        // The amount of damage the enemy inflicts on the player ship
        public int Damage;

        // The amount of score the enemy will give to the player
        public int Value;

        // Get the width of the enemy ship
        public int Width
        {
            get { return bossAnimation.FrameWidth; }
        }

        // Get the height of the enemy ship
        public int Height
        {
            get { return bossAnimation.FrameHeight; }
        }

        // The speed at which the enemy moves
        float enemyMoveSpeed;

        public void Initialize(Animation animation, Vector2 position)
        {
            // Load the enemy ship texture
            bossAnimation = animation;

            // Set the position of the enemy
            Position = position;

            // We initialize the enemy to be active so it will be update in the game
            Active = true;


            // Set the health of the enemy
            Health = 700;

            // Set the amount of damage the enemy can do
            Damage = 100;

            // Set how fast the enemy moves
            enemyMoveSpeed = 1.5f;


            // Set the score value of the enemy
            Value = 1000;

        }

        public void Update(GameTime gameTime)
        {
            // The enemy always moves to the left so decrement it's xposition
            Position.X -= enemyMoveSpeed;

            // Update the position of the Animation
            bossAnimation.Position = Position;

            // Update Animation
            bossAnimation.Update(gameTime);

            // If the enemy is past the screen or its health reaches 0 then deactivateit
            if (Position.X < -Width || Health <= 0)
            {
                // By setting the Active flag to false, the game will remove this objet fromthe 
                // active game list
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the animation
            bossAnimation.Draw(spriteBatch);
        }
    }
}
