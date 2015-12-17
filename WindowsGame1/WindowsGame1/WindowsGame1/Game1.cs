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

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle playerRectangle;
        Texture2D player;
        Vector2 playerPosition;

        Rectangle blocoRectangle;
        Texture2D bloco;
        Vector2 blocoPosition;

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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player = Content.Load<Texture2D>("square-rounded-64.2");
            playerPosition = new Vector2(10, 10);
            playerRectangle = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, player.Width, player.Height);

            bloco = Content.Load<Texture2D>("square-rounded-64.2");
            blocoPosition = new Vector2(300, 300);
            blocoRectangle = new Rectangle((int)blocoPosition.X, (int)blocoPosition.Y, bloco.Width, bloco.Height);

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            int movimento = 3;

            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.Right))
            {
                if ((playerPosition.X + movimento + player.Width) < Window.ClientBounds.Width)
                {
                    playerPosition.X += movimento;
                }
                else
                {
                    playerPosition.X = Window.ClientBounds.Width - player.Width;
                }
            }
            
            if (kbState.IsKeyDown(Keys.Left))
            {
                if ((playerPosition.X - movimento) > 0)
                {
                    playerPosition.X -= movimento;
                }
                else
                {
                    playerPosition.X = 0;
                }
            }

            if (kbState.IsKeyDown(Keys.Up))
            {
                if ((playerPosition.Y - movimento) > 0)
                {
                    playerPosition.Y -= movimento;
                }
                else
                {
                    playerPosition.Y = 0;
                }
            }
            
            if (kbState.IsKeyDown(Keys.Down))
            {
                if ((playerPosition.Y + movimento + player.Height) < Window.ClientBounds.Height)
                {
                    playerPosition.Y += movimento;
                }
                else
                {
                    playerPosition.Y = (Window.ClientBounds.Height - player.Height);
                }
            }

            playerRectangle.X = (int)playerPosition.X;
            playerRectangle.Y = (int)playerPosition.Y;

            if (playerRectangle.Intersects(blocoRectangle))
            {
                playerPosition.X = 10;
                playerPosition.Y = 10;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(player, playerPosition, Color.White);
            spriteBatch.Draw(bloco, blocoPosition, Color.Black);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
