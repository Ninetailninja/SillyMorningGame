using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SillyMorningGame.Model;
using SillyMorningGame.View;

namespace SillyMorningGame.Controller
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Enemies
        
        Texture2D miniTexture;
        Texture2D enemyTexture;
        List<Enemy> enemies;
        List<Boss1> bosses;
        Texture2D bossTexture;

        int healthMultiplier;

        // The rate at which the enemies appear
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;

        // A random number generator
        Random random;

        // Image used to display the static background
        Texture2D mainBackground;

        // Parallaxing Layers
        ParallaxingBackground bgLayer1;
        ParallaxingBackground bgLayer2;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Represents the player 
        Player player;
        List<MiniShip> testShips;

        // Keyboard states used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        // Gamepad states used to determine button presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        Texture2D projectileTexture;
        List<Projectile> projectiles;
        int projectileBoost;
        Color projectileColor;

        // The rate of fire of the player laser
        TimeSpan fireTime;
        TimeSpan previousFireTime;

        List<TimeSpan> miniFireTimes;
        List<TimeSpan> miniPreviousFireTimes;

        Texture2D explosionTexture;
        List<Animation> explosions;

        Texture2D allyPowerTexture;
        Texture2D powerupTexture;
        List<Powerup> powerups;
        List<AllyPowerup> allyPowerups;
        TimeSpan powerupSpawnTime;
        TimeSpan previousPowerupSpawnTime;
        TimeSpan allyPowerupSpawnTime;
        TimeSpan allyPreviousPowerupSpawnTime;
        

        // The sound that is played when a laser is fired
        SoundEffect laserSound;

        // The sound used when the player or an enemy dies
        SoundEffect explosionSound;

        // The music played during gameplay
        Song gameplayMusic;

        //Number that holds the player score
        int score;
        // The font used to display UI elements
        SpriteFont font;

        // A movement speed for the player
        float playerMoveSpeed;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            projectileBoost = 0;
            healthMultiplier = 1;
            projectileColor = Color.White;
            // Initialize the player class
            player = new Player();
            testShips = new List<MiniShip>();
            // Set a constant player move speed
            playerMoveSpeed = 6f;
            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();

            // Initialize the enemies list
            enemies = new List<Enemy>();
            bosses = new List<Boss1>();
            powerups = new List<Powerup>();
            allyPowerups = new List<AllyPowerup>();
            explosions = new List<Animation>();

            //Set player's score to zero
            score = 0;

            // Set the time keepers to zero
            previousSpawnTime = TimeSpan.Zero;
            previousPowerupSpawnTime = TimeSpan.Zero;

            allyPreviousPowerupSpawnTime = TimeSpan.Zero;
            allyPowerupSpawnTime = TimeSpan.FromSeconds(45f);
            // Used to determine how fast enemy respawns
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            powerupSpawnTime = TimeSpan.FromSeconds(20.0f);
            // Initialize our random number generator
            random = new Random();

            projectiles = new List<Projectile>();

            // Set the laser to fire every quarter second
            fireTime = TimeSpan.FromSeconds(.15f);
            miniFireTimes = new List<TimeSpan>();
            miniPreviousFireTimes = new List<TimeSpan>();
            

            base.Initialize();
        }

        

        private void addPowerup()
        {
            Animation powerAnimation = new Animation();
            powerAnimation.Initialize(powerupTexture, Vector2.Zero, 36, 36, 4, 30, Color.White, 1f, true);
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));
            Powerup powerup = new Powerup();
            powerup.Initialize(powerAnimation, position);
            powerups.Add(powerup);
        }
       

        private void addAllyPowerup()
        {
            Animation allyPowerAnimation = new Animation();
            allyPowerAnimation.Initialize(allyPowerTexture, Vector2.Zero, 74, 75, 4, 20, Color.White, 1f, true);
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));
            AllyPowerup powerup = new AllyPowerup();
            powerup.Initialize(allyPowerAnimation, position);
            allyPowerups.Add(powerup);
        }

        private void AddBoss()
        {
            // Create the animation object
            Animation bossAnimation = new Animation();

            // Initialize the animation with the correct animation information
            bossAnimation.Initialize(bossTexture, Vector2.Zero, 308, 400, 8, 20, Color.White, 1f, true);

            // Randomly generate the position of the enemy
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));

            // Create an enemy
            Boss1 boss = new Boss1();

            // Initialize the enemy
            boss.Initialize(bossAnimation, position);

            bosses.Add(boss);
        }

        private void AddEnemy()
        {
            // Create the animation object
            Animation enemyAnimation = new Animation();

            // Initialize the animation with the correct animation information
            enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);

            // Randomly generate the position of the enemy
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));

            // Create an enemy
            Enemy enemy = new Enemy();

            // Initialize the enemy
            enemy.Initialize(enemyAnimation, position, healthMultiplier);

            // Add the enemy to the active enemies list
            enemies.Add(enemy);
        }

        private void addMini()
        {
            Animation miniAnimation = new Animation();
            miniTexture = Content.Load<Texture2D>("Sprites/MiniShip");
            miniAnimation.Initialize(miniTexture, Vector2.Zero, 58, 35, 8, 30, Color.White, 1f, true);

            Vector2 miniPosition = new Vector2(player.Position.X + random.Next(0, 75), GraphicsDevice.Viewport.TitleSafeArea.Y
            + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            MiniShip mini = new MiniShip();
            mini.Initialize(miniAnimation, miniPosition);

            TimeSpan miniPrevious = TimeSpan.Zero;
            TimeSpan miniFireTime = TimeSpan.FromSeconds(.15f);

            testShips.Add(mini);
            miniPreviousFireTimes.Add(miniPrevious);
            miniFireTimes.Add(miniFireTime);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            enemyTexture = Content.Load<Texture2D>("Sprites/mineAnimation");
            bossTexture = Content.Load<Texture2D>("Sprites/BigMine");

            // Load the player resources
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("Sprites/shipAnimation");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y
            + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(playerAnimation, playerPosition);

            // Load the parallaxing background
            bgLayer1.Initialize(Content, "Images/bgLayer1", GraphicsDevice.Viewport.Width, -1);
            bgLayer2.Initialize(Content, "Images/bgLayer2", GraphicsDevice.Viewport.Width, -2);

            mainBackground = Content.Load<Texture2D>("Images/mainbackground");

            projectileTexture = Content.Load<Texture2D>("Sprites/laser");

            explosionTexture = Content.Load<Texture2D>("Sprites/explosion");

            powerupTexture = Content.Load<Texture2D>("Sprites/Powerup-Laser");
            allyPowerTexture = Content.Load<Texture2D>("Sprites/Powerup-Ally");

            // Load the score font
            font = Content.Load<SpriteFont>("gameFont");

            // Load the music
            gameplayMusic = Content.Load<Song>("sound/gameMusic");

            // Load the laser and explosion sound effect
            laserSound = Content.Load<SoundEffect>("sound/laserFire");
            explosionSound = Content.Load<SoundEffect>("sound/explosion");

            // Start the music right away
            PlayMusic(gameplayMusic);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.T))
                this.Exit();
            //if (Keyboard.GetState().IsKeyDown(Keys.P))
                
            // TODO: Add your update logic here
            // Save the previous state of the keyboard and game pad so we can determinesingle key/button presses
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            // Read the current state of the keyboard and gamepad and store it
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            UpdatePlayer(gameTime);
            

            // Update the parallaxing background
            bgLayer1.Update();
            bgLayer2.Update();

            // Update the enemies
            UpdateEnemies(gameTime);
            // Update the collision
            UpdateCollision();
            // Update the projectiles
            UpdateProjectiles();
            // Update the explosions
            UpdateExplosions(gameTime);
            // Update the Powerups
            updatePowerups(gameTime);
            updateAllyPowerups(gameTime);
            UpdateBoss(gameTime);
            updateMini(gameTime);

            base.Update(gameTime);
        }

        private void PlayMusic(Song song)
        {
            // Due to the way the MediaPlayer plays music,
            // we have to catch the exception. Music will play when the game is not tethered
            try
            {
                // Play the music
                MediaPlayer.Play(song);

                // Loop the currently playing song
                MediaPlayer.IsRepeating = true;
            }
            catch { }
        }

        private void AddExplosion(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);
            explosions.Add(explosion);
        }

        private void AddProjectile(Vector2 position)
        {
            Projectile projectile = new Projectile();
            projectile.Initialize(GraphicsDevice.Viewport, projectileTexture, position, projectileBoost);
            projectiles.Add(projectile);
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].Active == false)
                {
                    explosions.RemoveAt(i);
                }
            }
        }

        private void UpdateBoss(GameTime gameTime)
        {
            
<<<<<<< HEAD
            if (score == 10000 && bosses.Count == 0)
=======
            if (score % 10000 == 0 && score != 0 && bosses.Count == 0)
>>>>>>> d563b6b01dcc10f68354d370c746c7ea6d6e8357
            {
                AddBoss();
                enemySpawnTime = TimeSpan.FromSeconds(.5);
                healthMultiplier += 2;
            }

           
            for (int i = bosses.Count - 1; i >= 0; i--)
            {
                bosses[i].Update(gameTime);

                if (bosses[i].Active == false)
                {
                    
                    if (bosses[i].Health <= 0)
                    {
                        // Add an explosion
                        AddExplosion(bosses[i].Position);
                        // Play the explosion sound
                        explosionSound.Play();
                        //Add to the player's score
                        score += bosses[i].Value;
                    }
                    bosses.RemoveAt(i);
                }

            }
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            // Spawn a new enemy enemy every 1.5 seconds
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                // Add an Enemy
                AddEnemy();
            }

            // Update the Enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime);

                if (enemies[i].Active == false)
                {
                    // If not active and health <= 0
                    if (enemies[i].Health <= 0)
                    {
                        // Add an explosion
                        AddExplosion(enemies[i].Position);
                        // Play the explosion sound
                        explosionSound.Play();
                        //Add to the player's score
                        score += enemies[i].Value;
                    }
                    enemies.RemoveAt(i);
                }
                
            }
        }

        private void updatePowerups(GameTime gameTime)
        {
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + powerupTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));
            // Spawn a new Powerup every 10.5 seconds
            if (gameTime.TotalGameTime - previousPowerupSpawnTime > powerupSpawnTime)
            {
                previousPowerupSpawnTime = gameTime.TotalGameTime;

                // Add a Powerup
                addPowerup();
            }

            // Update the Enemies
            for (int i = powerups.Count - 1; i >= 0; i--)
            {
                powerups[i].Update(gameTime);

                if (powerups[i].Active == false)
                {
                    powerups.RemoveAt(i);
                }

            }
        }

        private void updateAllyPowerups(GameTime gameTime)
        {
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + powerupTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));
            // Spawn a new Powerup every 10.5 seconds
            if (gameTime.TotalGameTime - allyPreviousPowerupSpawnTime > allyPowerupSpawnTime)
            {
                allyPreviousPowerupSpawnTime = gameTime.TotalGameTime;

                // Add a Powerup
                addAllyPowerup();
            }

            // Update the Enemies
            for (int i = allyPowerups.Count - 1; i >= 0; i--)
            {
                allyPowerups[i].Update(gameTime);

                if (allyPowerups[i].Active == false)
                {
                    allyPowerups.RemoveAt(i);
                }

            }
        }

        private void UpdateCollision()
        {
            // Use the Rectangle's built-in intersect function to 
            // determine if two objects are overlapping
            Rectangle rectangle1;
            Rectangle rectangle2;
            Rectangle rectangle3;
            Rectangle rectangle4;

            // Only create the rectangle once for the player
            rectangle1 = new Rectangle((int)player.Position.X,
            (int)player.Position.Y,
            player.Width,
            player.Height);

            // Do the collision between the player and the enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                rectangle2 = new Rectangle((int)enemies[i].Position.X,
                (int)enemies[i].Position.Y,
                enemies[i].Width,
                enemies[i].Height);

                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle2))
                {
                    // Subtract the health from the player based on
                    // the enemy damage
                    player.Health -= enemies[i].Damage;

                    // Since the enemy collided with the player
                    // destroy it
                    enemies[i].Health = 0;

                    // If the player health is less than zero we died
                    if (player.Health <= 0)
                        player.Active = false;
                }

            }

            for (int i = 0; i < bosses.Count; i++)
            {
                rectangle4 = new Rectangle((int)bosses[i].Position.X,
                (int)bosses[i].Position.Y,
                bosses[i].Width,
                bosses[i].Height);

                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle4))
                {
                    // Subtract the health from the player based on
                    // the enemy damage
                    player.Health -= bosses[i].Damage;

                    // Since the enemy collided with the player
                    // destroy it
                    bosses[i].Health = 0;

                    // If the player health is less than zero we died
                    if (player.Health <= 0)
                        player.Active = false;
                }

            }

            //player vs powerup collision
             for (int i = 0; i < powerups.Count; i++)
            {
                rectangle3 = new Rectangle((int)powerups[i].Position.X, (int)powerups[i].Position.Y, powerups[i].Width, powerups[i].Height);

                if (rectangle1.Intersects(rectangle3))
                 {
                     powerups[i].Active = false;

                     projectileBoost += 2;
                     if (projectileBoost == 2)
                     {
                         projectileColor = Color.Blue;
                     }
                     if (projectileBoost == 4)
                     {
                         projectileColor = Color.Green;
                     }
                     if (projectileBoost == 6)
                     {
                         projectileColor = Color.Yellow;
                     }
                     if (projectileBoost == 8)
                     {
                         projectileColor = Color.Orange;
                     }
                     if (projectileBoost == 10)
                     {
                         projectileColor = Color.Red;
                     }
                     if (projectileBoost > 10)
                     {
                         projectileColor = Color.Purple;
                     }
                 }
            }
             for (int i = 0; i < allyPowerups.Count; i++)
             {
                 rectangle3 = new Rectangle((int)allyPowerups[i].Position.X, (int)allyPowerups[i].Position.Y, allyPowerups[i].Width, allyPowerups[i].Height);

                 if (rectangle1.Intersects(rectangle3))
                 {
                     allyPowerups[i].Active = false;

                    
                     addMini();
                 }
             }

            // Projectile vs Enemy Collision
            for (int i = 0; i < projectiles.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    // Create the rectangles we need to determine if we collided with each other
                    rectangle1 = new Rectangle((int)projectiles[i].Position.X -
                    projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                    projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);

                    rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
                    (int)enemies[j].Position.Y - enemies[j].Height / 2,
                    enemies[j].Width, enemies[j].Height);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        enemies[j].Health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }
                }
            }

<<<<<<< HEAD
            // Projectile vs Boss Collision
=======
>>>>>>> d563b6b01dcc10f68354d370c746c7ea6d6e8357
            for (int i = 0; i < projectiles.Count; i++)
            {
                for (int j = 0; j < bosses.Count; j++)
                {
                    // Create the rectangles we need to determine if we collided with each other
                    rectangle1 = new Rectangle((int)projectiles[i].Position.X -
                    projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                    projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);

<<<<<<< HEAD
                    rectangle4 = new Rectangle((int)bosses[j].Position.X - enemies[j].Width / 2,
                    (int)bosses[j].Position.Y - enemies[j].Height / 2,
=======
                    rectangle4 = new Rectangle((int)bosses[j].Position.X - bosses[j].Width / 2,
                    (int)bosses[j].Position.Y - bosses[j].Height / 2,
>>>>>>> d563b6b01dcc10f68354d370c746c7ea6d6e8357
                    bosses[j].Width, bosses[j].Height);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle4))
                    {
                        bosses[j].Health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }
                }
            }
<<<<<<< HEAD
=======

            for (int i = 0; i < bosses.Count; i++)
            {
                rectangle4 = new Rectangle((int)bosses[i].Position.X,
                (int)bosses[i].Position.Y,
                bosses[i].Width,
                bosses[i].Height);

                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle4))
                {
                    // Subtract the health from the player based on
                    // the enemy damage
                    player.Health -= bosses[i].Damage;

                    // Since the enemy collided with the player
                    // destroy it
                    bosses[i].Health = 0;

                    // If the player health is less than zero we died
                    if (player.Health <= 0)
                        player.Active = false;
                }

            }
>>>>>>> d563b6b01dcc10f68354d370c746c7ea6d6e8357
        }

        /// <summary>
        /// Updates the player based on the input from the user via gamepad or keyboard
        /// </summary>
        /// <param name="gameTime">The current time reference of the game</param>
        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);
            // Get Thumbstick Controls
            player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

            // Use the Keyboard / Dpad
            if (currentKeyboardState.IsKeyDown(Keys.Left) ||
            currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                player.Position.X -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) ||
            currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                player.Position.X += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) ||
            currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                player.Position.Y -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) ||
            currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                player.Position.Y += playerMoveSpeed;
            }

            // Make sure that the player does not go out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);

            // Fire only every interval we set as the fireTime
            if (gameTime.TotalGameTime - previousFireTime > fireTime)
            {
                // Reset our current time
                previousFireTime = gameTime.TotalGameTime;

                // Add the projectile, but add it to the front and center of the player
                AddProjectile(player.Position + new Vector2(player.Width / 2, 0));

                // Play the laser sound
                laserSound.Play();

                // reset score if player health goes to zero
                if (player.Health <= 0)
                {
                    player.Health = 100;
                    score = 0;
                    projectileBoost = 0;
                    projectileColor = Color.White;
                    enemySpawnTime = TimeSpan.FromSeconds(1.0);
                }
            }
        }

        private void updateMini(GameTime gameTime)
        {
            for (int i = testShips.Count - 1; i >= 0; i--)
            {
                testShips[i].Update(gameTime);
                // Get Thumbstick Controls
                testShips[i].Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
                testShips[i].Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;


                // Use the Keyboard / Dpad
                if (currentKeyboardState.IsKeyDown(Keys.Left) ||
                currentGamePadState.DPad.Left == ButtonState.Pressed)
                {
                    testShips[i].Position.X -= playerMoveSpeed;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Right) ||
                currentGamePadState.DPad.Right == ButtonState.Pressed)
                {
                    testShips[i].Position.X += playerMoveSpeed;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Up) ||
                currentGamePadState.DPad.Up == ButtonState.Pressed)
                {
                    testShips[i].Position.Y -= playerMoveSpeed;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Down) ||
                currentGamePadState.DPad.Down == ButtonState.Pressed)
                {
                    testShips[i].Position.Y += playerMoveSpeed;
                }

                // Make sure that the player does not go out of bounds
                //player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
                //player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);

                
                
                // Fire only every interval we set as the fireTime
                if (gameTime.TotalGameTime - miniPreviousFireTimes[i] > miniFireTimes[i])
                {
                    // Reset our current time
                    miniPreviousFireTimes[i] = gameTime.TotalGameTime;

                    // Add the projectile, but add it to the front and center of the player
                    AddProjectile(testShips[i].Position + new Vector2(testShips[i].Width / 2, 0));

                    // Play the laser sound
                    laserSound.Play();

                    // reset score if player health goes to zero
                    if (testShips[i].Health <= 0)
                    {
                        testShips[i].Active = false;
                    }
                }
                 
            }
        }

        private void EndGame()
        {

        }

        private void UpdateProjectiles()
        {
            // Update the Projectiles
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update();

                if (projectiles[i].Active == false)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SaddleBrown);

            // TODO: Add your drawing code here

            // Start drawing
            spriteBatch.Begin();

            spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);

            // Draw the moving background
            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);

            // Draw the Enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }
            for (int i = 0; i < bosses.Count; i++)
            {
                bosses[i].Draw(spriteBatch);
            }
            // Draw the Powerups
            for (int i = 0; i < powerups.Count; i++)
            {
                powerups[i].Draw(spriteBatch);
            }
            // Draw the ally Powerups
            for (int i = 0; i < allyPowerups.Count; i++)
            {
                allyPowerups[i].Draw(spriteBatch);
            }
            // Draw the Projectiles
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(spriteBatch, projectileColor);
            }
            // Draw the explosions
            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Draw(spriteBatch);

            }
            for (int i = 0; i < testShips.Count; i++)
            {
                testShips[i].Draw(spriteBatch);
            }

            // Draw the score
            spriteBatch.DrawString(font, "score: " + score, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.White);
            // Draw the player health
            spriteBatch.DrawString(font, "health: " + player.Health, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + 30), Color.White);
            // Draw the Player
            
            player.Draw(spriteBatch);


            // Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
