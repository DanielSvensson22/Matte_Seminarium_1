using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Matte_Seminarium_1
{
    public class Ball
    {
        private readonly Texture2D tex;
        public float Radius { get; private set; }
        public float Mass { get; private set; }
        public Vector2 Pos { get; set; }
        public  Vector2 Velocity { get; set; }
        public Vector2 Origin { get; private set; }

        private Rectangle hitBox;

        public Ball(Texture2D tex, Vector2 pos, float radius, float mass, Vector2 speed)
        {
            this.tex = tex;
            Radius = radius;
            this.Pos = pos;
            Mass = mass;
            Velocity = speed;

            hitBox = new((int)pos.X, (int)pos.Y, (int)radius * 2, (int)radius * 2);
        }

        public void Update(GameTime gameTime)
        {
            Pos += Velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            Origin = new(Pos.X + Radius, Pos.Y + Radius);
            hitBox.Location = Pos.ToPoint();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, hitBox, Color.White);
        }

        public bool CollisionCheck(Ball ball)
        {
            if(Vector2.Distance(Origin, ball.Origin) <= Radius + ball.Radius)
            {
                float newVelocityXA = (Velocity.X * (Mass - ball.Mass) + (2 * ball.Mass * ball.Velocity.X)) / (Mass + ball.Mass);
                float newVelocityYA = (Velocity.Y * (Mass - ball.Mass) + (2 * ball.Mass * ball.Velocity.Y)) / (Mass + ball.Mass);

                float newVelocityXB = (ball.Velocity.X * (ball.Mass - Mass) + (2 * Mass * Velocity.X)) / (Mass + ball.Mass);
                float newVelocityYB = (ball.Velocity.Y * (ball.Mass - Mass) + (2 * Mass * Velocity.Y)) / (Mass + ball.Mass);

                Pos = new(Pos.X + newVelocityXA, Pos.Y + newVelocityYA);
                ball.Pos = new(ball.Pos.X + newVelocityXB, ball.Pos.Y + newVelocityYB);

                Velocity = new(newVelocityXA, newVelocityYA);
                ball.Velocity = new(newVelocityXB, newVelocityYB);

                return true;
            }

            return false;
        }

       /* public void Collision(Ball ball)
        {
            if(Vector2.Distance(Origin, ball.Origin) <= Radius + ball.Radius)
            {
                Collide(ball, this);
            }
        }*/
        /* Old collide
        private void Collide(Ball ballA, Ball ballB)
        {
            if (ballB != ballA)
            {
                float distance = Vector2.Distance(ballA.Origin, ballB.Origin);
                float penetration = ballA.Radius + ballB.Radius - distance;
                if (penetration >= 0)
                {
                    PenetrationReset(ballA, ballB, penetration + 2);

                    // The distance between the two balls.
                    distance = Vector2.Distance(ballA.Origin, ballB.Origin);

                    // A normal.
                    float normalX = (ballB.Origin.X - ballA.Origin.X) / distance;
                    float normalY = (ballB.Origin.Y - ballA.Origin.Y) / distance;

                    // A tangent.
                    float tangentX = -normalY;
                    float tangentY = normalX;

                    // Amount of velocity going to tangent direction.
                    float dotProductTangentA = ballA.Velocity.X * tangentX + ballA.Velocity.Y * tangentY;
                    float dotProductTangentB = ballB.Velocity.X * tangentX + ballB.Velocity.Y * tangentY;

                    // Amount of velocity going to normal direction.
                    float dotProductNormalA = ballA.Velocity.X * normalX + ballA.Velocity.Y * normalY;
                    float dotProductNormalB = ballB.Velocity.X * normalX + ballB.Velocity.Y * normalY;


                    // Elastic collision
                    float momentumA = (dotProductNormalA * (ballA.Mass - ballB.Mass) + 2.0f * ballB.Mass * dotProductNormalB) / (ballA.Mass + ballB.Mass);
                    float momentumB = (dotProductNormalB * (ballB.Mass - ballA.Mass) + 2.0f * ballB.Mass * dotProductNormalA) / (ballA.Mass + ballB.Mass);

                    ballA.Velocity = new(tangentX * dotProductTangentA + normalX * momentumA, tangentY * dotProductTangentA + normalY * momentumA);
                    ballB.Velocity = new(tangentX * dotProductTangentB + normalX * momentumB, tangentY * dotProductTangentB + normalY * momentumB);

                }
            }

        }*/

        /* Old penetration resets.
        //Makes the balls stop colliding.
        //They are placed next to each other instead based on the angle of the collision.
        private void PenetrationReset(Ball ballA, Ball ballB, float penetration)
        {
            float angle = GetRotation(ballA.Origin, ballB.Origin) / 180 * MathF.PI;
            Vector2 angleVec = new(MathF.Cos(angle), MathF.Sin(angle));
            ballA.Pos += angleVec * penetration / 2;
            ballB.Pos -= angleVec * penetration / 2;
        }

        //Finds rotation of balls in relation to one another 
        // in order to know where to push them.
        public static float GetRotation(Vector2 posA, Vector2 posB)
        {
            float x = posA.X - posB.X;
            float y = posA.Y - posB.Y;
            return (-MathF.Atan2(x, y) * 180 / MathF.PI) + 90;
        }*/

        public void WallCollision(int maxPosX, int maxPosY)
        {
            if(Pos.X <= 0 || Pos.X >= maxPosX - hitBox.Width)
            {
                Velocity = new(Velocity.X * -1, Velocity.Y);
            }

            if (Pos.Y <= 0 || Pos.Y >= maxPosY - hitBox.Height)
            {
                Velocity = new(Velocity.X, Velocity.Y * -1);
            }
        }
    }
}
