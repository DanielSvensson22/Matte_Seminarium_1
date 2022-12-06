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

        GameState state = GameState.Preparing;

        private Texture2D ballTex;
        private SpriteFont font;

        Timer timer = new();
        private double hitTime;
        private List<double> hitTimes = new();
        private List<Vector2> ballAHits = new();
        private List<Vector2> ballBHits = new();

        private Ball ballA;
        private Ball ballB;

        private float radiusA;
        private float radiusB;

        private InputHandler inputHandler = new();

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
            font = Content.Load<SpriteFont>("font");

            if (state == GameState.Preparing)
            {
                timer = new();
                hitTime = 0;

                radiusA = 20;
                radiusB = 20;

                hitTimes.Clear();
                ballAHits.Clear();
                ballBHits.Clear();
            }
            else if(state == GameState.Executing)
            {
                ballTex = Content.Load<Texture2D>("ball");

                ballA = new(ballTex, new(100, 100), radiusA, 1, new(0.1f, 0.1f));
                ballB = new(ballTex, new(300, 300), radiusB, 1, new(-0.1f, -0.1f));
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

            inputHandler.Update(gameTime);

            if(state == GameState.Preparing)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && radiusA > 0 && radiusB > 0)
                {
                    state = GameState.Executing;
                    LoadContent();
                }

                radiusA += inputHandler.RadiusChange(Keys.S, Keys.A);
                radiusB += inputHandler.RadiusChange(Keys.X, Keys.Z);
            }
            else if (state == GameState.Executing)
            {
                timer.Update(gameTime);

                ballA.Update(gameTime);
                ballB.Update(gameTime);

                //Checks for ball collisions.
                if (ballA.CollisionCheck(ballB))
                {
                    hitTime = timer.time;
                    hitTimes.Add(hitTime);

                    ballAHits.Add(ballA.Origin);
                    ballBHits.Add(ballB.Origin);
                }

                //Keeps balls within screen.
                ballA.WallCollision(Window.ClientBounds.Width, Window.ClientBounds.Height);
                ballB.WallCollision(Window.ClientBounds.Width, Window.ClientBounds.Height);

                if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    state = GameState.ListInspection;
                    LoadContent();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if(state == GameState.Preparing)
            {
                _spriteBatch.DrawString(font, $"Radius of ball A: {radiusA}", new(400, 200), Color.Red);

                _spriteBatch.DrawString(font, $"Radius of ball B: {radiusB}", new(400, 400), Color.Orange);
            }
            else if (state == GameState.Executing)
            {
                ballA.Draw(_spriteBatch);
                ballB.Draw(_spriteBatch);

                //Draws text describing when the balls collided.
                if(hitTimes.Count > 0)
                {
                    _spriteBatch.DrawString(font, $"Collision at {hitTimes[^1]} seconds.", new(10, Window.ClientBounds.Height - 20), Color.Gold);
                }
            }
            else if(state == GameState.ListInspection)
            {
                for(int i = 0; i < hitTimes.Count; i++)
                {
                    _spriteBatch.DrawString(font, $"Hit at {hitTimes[i]} seconds.", new(10, 10 + 20 * i), Color.Gold);
                }

                for (int i = 0; i < ballAHits.Count; i++)
                {
                    _spriteBatch.DrawString(font, $"Ball A collided at coordinates: {(int)ballAHits[i].X}; {(int)ballAHits[i].Y}", new(300, 10 + 20 * i), Color.Red);
                }

                for (int i = 0; i < ballBHits.Count; i++)
                {
                    _spriteBatch.DrawString(font, $"Ball B collided at coordinates: {(int)ballBHits[i].X}; {(int)ballBHits[i].Y}", new(600, 10 + 20 * i), Color.Orange);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        enum GameState
        {
            Preparing,
            Executing,
            ListInspection,
        }
    }
}