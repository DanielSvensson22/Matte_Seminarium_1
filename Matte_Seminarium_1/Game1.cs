using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Matte_Seminarium_1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        GameState state = GameState.Executing;

        private Texture2D ballTex;
        private SpriteFont font;

        Timer timer = new();
        private double hitTime;
        private List<double> hitTimes = new();

        private Ball ballA;
        private Ball ballB;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            if(state == GameState.Preparing)
            {
                hitTimes.Clear();
            }
            else if(state == GameState.Executing)
            {
                ballTex = Content.Load<Texture2D>("ball");
                font = Content.Load<SpriteFont>("font");

                ballA = new(ballTex, new(100, 100), 20, 1, new(0.05f, 0.1f));
                ballB = new(ballTex, new(100, 300), 30, 1, new(-0.1f, -0.01f));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                state = GameState.Preparing;
                LoadContent();
            }

            if (state == GameState.Executing)
            {
                timer.Update(gameTime);

                ballA.Update(gameTime);
                ballB.Update(gameTime);

                //Checks for ball collisions.
                if (ballA.CollisionCheck(ballB))
                {
                    hitTime = timer.time;
                    hitTimes.Add(hitTime);
                }

                //Keeps balls within screen.
                ballA.WallCollision(Window.ClientBounds.Width, Window.ClientBounds.Height);
                ballB.WallCollision(Window.ClientBounds.Width, Window.ClientBounds.Height);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (state == GameState.Executing)
            {
                ballA.Draw(_spriteBatch);
                ballB.Draw(_spriteBatch);

                //Draws text describing when the balls collided.
                if(hitTimes.Count > 0)
                {
                    _spriteBatch.DrawString(font, $"Collision at {hitTimes[^1]} seconds.", new(10, Window.ClientBounds.Height - 20), Color.Gold);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        enum GameState
        {
            Preparing,
            Executing,
        }
    }
}