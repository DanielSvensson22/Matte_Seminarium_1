using Microsoft.Xna.Framework;

namespace Matte_Seminarium_1
{
    public class Timer
    {
        public double time;

        public void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
