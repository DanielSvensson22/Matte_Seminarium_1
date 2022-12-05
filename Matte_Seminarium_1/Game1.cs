using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Matte_Seminarium_1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        GameState state = GameState.Executing;

        private Texture2D ballTex;

        private Ball ballA;
        private Ball ballB;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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

            if(state == GameState.Executing)
            {
                ballTex = Content.Load<Texture2D>("ball");

                ballA = new(ballTex, new(100, 100), 20, 1, new(0.05f, 0.1f));
                ballB = new(ballTex, new(100, 300), 30, 2, new(-0.1f, -0.01f));
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
                ballA.Update(gameTime);
                ballB.Update(gameTime);

                ballA.CollisionCheck(ballB);

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