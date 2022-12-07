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

        private Vector2 velocityA;
        private Vector2 velocityB;

        private Vector2 positionA;
        private Vector2 positionB;

        private InputHandler inputHandler = new();
        private BallManager ballManager;

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

                velocityA = new(0.1f, 0.1f);
                velocityB = new(-0.1f, -0.1f);

                positionA = new(100, 100);
                positionB = new(300, 300);

                hitTimes.Clear();
                ballAHits.Clear();
                ballBHits.Clear();
            }
            else if(state == GameState.Executing)
            {
                ballTex = Content.Load<Texture2D>("ball");

                ballA = new(ballTex, positionA, radiusA, 1, velocityA);
                ballB = new(ballTex, positionB, radiusB, 1, velocityB);

                ballManager = new BallManager(this);
                ballManager.BallList.Add(ballA);
                ballManager.BallList.Add(ballB);
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

                radiusA += inputHandler.ValueChange(Keys.S, Keys.A, 1);
                radiusB += inputHandler.ValueChange(Keys.X, Keys.Z, 1);

                velocityA.X += inputHandler.ValueChange(Keys.W, Keys.Q, 0.1f);
                velocityA.Y += inputHandler.ValueChange(Keys.R, Keys.E, 0.1f);

                velocityB.X += inputHandler.ValueChange(Keys.Y, Keys.T, 0.1f);
                velocityB.Y += inputHandler.ValueChange(Keys.I, Keys.U, 0.1f);

                positionA.X += inputHandler.ValueChange(Keys.D0, Keys.O, 1);
                positionA.Y += inputHandler.ValueChange(Keys.G, Keys.F, 1);

                positionB.X += inputHandler.ValueChange(Keys.J, Keys.H, 1);
                positionB.Y += inputHandler.ValueChange(Keys.L, Keys.K, 1);
            }
            else if (state == GameState.Executing)
            {
                timer.Update(gameTime);

                ballManager.Update(gameTime);

                if (inputHandler.CurrentMouseL && ballManager.PickBall(inputHandler.MousePoint)) 
                { 
                    state = GameState.Modifying; 
                }

                if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    state = GameState.ListInspection;
                    LoadContent();
                }
            }
            else if (state == GameState.Modifying)
            {
                if (!inputHandler.CurrentMouseL) { ballManager.SetProperties(inputHandler.MousePosition); state = GameState.Executing; }
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
                _spriteBatch.DrawString(font, $"Radius of ball A: {radiusA}", new(10, 10), Color.Red);

                _spriteBatch.DrawString(font, $"Radius of ball B: {radiusB}", new(10, 50), Color.Orange);

                _spriteBatch.DrawString(font, $"Velocity of ball A: {velocityA}", new(300, 10), Color.Red);

                _spriteBatch.DrawString(font, $"Velocity of ball B: {velocityB}", new(300, 50), Color.Orange);

                _spriteBatch.DrawString(font, $"Position of ball A: {positionA}", new(700, 10), Color.Red);

                _spriteBatch.DrawString(font, $"Position of ball B: {positionB}", new(700, 50), Color.Orange);
            }
            else if (state == GameState.Executing || state == GameState.Modifying)
            {
                ballManager.Draw(_spriteBatch);

                //Draws text describing when the balls collided.
                if (hitTimes.Count > 0)
                {
                    _spriteBatch.DrawString(font, $"Collision at {hitTimes[^1]} seconds.", new(10, Window.ClientBounds.Height - 20), Color.Gold);

                    //_spriteBatch.DrawString(font, $"Collision Distance: {ballManager.CollisionDistance} / {ballManager.CombinedRadius}", new(10, Window.ClientBounds.Height - 60), Color.Orange);

                    _spriteBatch.DrawString(font, $"Collision Point: {ballManager.CollisionPoint}", new(10, Window.ClientBounds.Height - 40), Color.Orange);
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

        public double HitTime
        {
            get { return hitTime; }
            set { hitTime = value; }
        }

        public Timer Timer
        {
            get { return timer; }
        }

        public List<double> HitTimes
        {
            get { return hitTimes; }
        }

        public List<Vector2> BallAHits
        {
            get { return ballAHits; }
        }

        public List<Vector2> BallBHits
        {
            get { return ballBHits; }
        }

        public GameState State
        {
            get { return state; }
            set { state = value; }
        }

        public enum GameState
        {
            Preparing,
            Executing,
            Modifying,
            ListInspection,
        }
    }
}