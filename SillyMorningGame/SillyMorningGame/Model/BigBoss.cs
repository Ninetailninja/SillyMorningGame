using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SillyMorningGame.View;

namespace SillyMorningGame.Model
{
    class BigBoss
    {
        // Animation representing the enemy
        public Animation bossAnimation;

        // The position of the enemy ship relative to the top left corner of thescreen
        public Vector2 Position;
        public Vector2 WeaponPosition;

        // The state of the Enemy Ship
        public bool Active;
        public bool WeaponActive;

        // The hit points of the enemy, if this goes to zero the enemy dies
        public int Health;
        public int WeaponHealth;

        // The amount of damage the enemy inflicts on the player ship
        public int Damage;

        // The amount of score the enemy will give to the player
        public int Value;

        // Get the width of the enemy ship
        public int Width
        {
            get { return bossAnimation.FrameWidth; }
        }
        public int WeaponWidth
        {
            get { return bossAnimation.FrameWidth - 178; }
        }

        // Get the height of the enemy ship
        public int Height
        {
            get { return bossAnimation.FrameHeight; }
        }
        public int WeaponHeight
        {
            get { return bossAnimation.FrameHeight - 140; }
        }

        // The speed at which the enemy moves
        float enemyMoveSpeed;

        public void Initialize(Animation animation, Vector2 position)
        {
            // Load the enemy ship texture
            bossAnimation = animation;

            // Set the position of the enemy
            Position = position;
            WeaponPosition.X = position.X + 160;
            WeaponPosition.Y = position.Y + 200;

            // We initialize the enemy to be active so it will be update in the game
            Active = true;
            WeaponActive = true;


            // Set the health of the enemy
            Health = 20000;
            WeaponHealth = 10000;

            // Set the amount of damage the enemy can do
            Damage = 100;

            // Set how fast the enemy moves
            enemyMoveSpeed = 1.5f;


            // Set the score value of the enemy
            Value = 10000;

        }

        public void Update(GameTime gameTime)
        {
            // The enemy always moves to the left so decrement it's xposition
            Position.X -= enemyMoveSpeed;
            WeaponPosition.X -= enemyMoveSpeed;

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
            if (WeaponHealth <= 0)
                WeaponActive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the animation
            bossAnimation.Draw(spriteBatch);
        }
    }
}
