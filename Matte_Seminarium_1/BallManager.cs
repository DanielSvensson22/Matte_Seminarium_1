using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Matte_Seminarium_1.Game1;

namespace Matte_Seminarium_1
{
    public class BallManager
    {
        Game1 game;
        private List<Ball> ballList = new List<Ball>();
        private Ball currentBall;

        public BallManager(Game1 game)
        {
            this.game = game;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Ball ball in ballList)
            {
                ball.Update(gameTime);

                ball.WallCollision(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            }

            if (BallList[0].CollisionCheck(BallList[1]))
            {
                game.HitTime = game.Timer.time;
                game.HitTimes.Add(game.HitTime);

                game.BallAHits.Add(BallList[0].Origin);
                game.BallBHits.Add(BallList[1].Origin);
            }

        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            if (game.State == GameState.Executing || game.State == GameState.Modifying)
            {
                foreach (Ball ball in ballList)
                {
                    ball.Draw(_spriteBatch);
                }
            }
        }

        public virtual List<Ball> BallList
        {
            get { return ballList; }
            set { ballList = value; }
        }
        public virtual Ball CurrentBall
        {
            get { return currentBall; }
            set { currentBall = value; }
        }

        public virtual bool PickBall(Point mousePoint)
        {
            for (int i = 0; i < BallList.Count; i++)
            {
                if (BallList[i].HitBox.Contains(mousePoint))
                {
                    CurrentBall = BallList[i];
                    return true;
                }
            }

            return false;
        }

        public void SetProperties(Vector2 mousePosition)
        {
            Vector2 newVelocity = mousePosition - CurrentBall.Origin;
            CurrentBall.Velocity = newVelocity * 0.001f;
            CurrentBall = null;
        }


    }
}
