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
        private bool collision;

        //Vector2 tempPos1;
        //Vector2 tempPos2;
        //Vector2 tempVel1;
        //Vector2 tempVel2;

        Vector2 collisionPoint;
        //float collisionDistance;

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

                collision = true;

                /*stuff
                //tempPos1 = BallList[0].Origin;
                //tempPos2 = BallList[1].Origin;
                //tempVel1 = BallList[0].PreviousVelocity;
                //tempVel2 = BallList[1].PreviousVelocity;
                */
            }

            if (collision) { GiveCollisionPoint(); }

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

        public virtual Vector2 CollisionPoint
        {
            get { return collisionPoint; }
            set { collisionPoint = value; }
        }

        //public virtual float CollisionDistance
        //{
        //    get { return collisionDistance; }
        //    set { collisionDistance = value; }
        //}

        //public virtual float CombinedRadius { get { return BallList[0].Radius + BallList[1].Radius; } }
        

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

        //public void CalculatePoint(GameTime gameTime)
        //{
        //    CollisionDistance = Vector2.Distance(tempPos1, tempPos2);
        //    if (CollisionDistance < BallList[0].Radius + BallList[1].Radius)
        //    {
        //        tempPos1 -= tempVel1 * 0.001f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        //        tempPos2 -= tempVel2 * 0.001f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        //    }
        //    else
        //    {
        //        Vector2 dir = (tempPos2 - tempPos1);
        //        dir.Normalize();
        //        CollisionPoint = tempPos1 + dir * BallList[0].Radius;
        //        collision = false;
        //    }
        //}

        private void GiveCollisionPoint()
        {
            collisionPoint.X = ((BallList[0].Origin.X * BallList[1].Radius) + (BallList[1].Origin.X * BallList[0].Radius)) / (BallList[0].Radius + BallList[1].Radius);
            collisionPoint.Y = ((BallList[0].Origin.Y * BallList[1].Radius) + (BallList[1].Origin.Y * BallList[0].Radius)) / (BallList[0].Radius + BallList[1].Radius);

            collision = false;
        }
    }
}
