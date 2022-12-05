using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Matte_Seminarium_1
{
    internal class InputHandler
    {
        private Vector2 direction { get; set; }
        private Vector2 mousePosition { get; set; }
        private Vector2 mouseOrigin { get; set; }
        private float distance { get; set;}

        private bool mouseL;
        private bool space;

        private MouseState currentMouse { get; set; }
        private KeyboardState currentKeyboard { get; set; }


        public void Update(GameTime gameTime)
        {
            CurrentMouse = Mouse.GetState();
            CurrentKeyboard = Keyboard.GetState();
        }

        public virtual MouseState CurrentMouse
        {
            get { return currentMouse; }
            set { currentMouse = value; }
        }
        
        public virtual KeyboardState CurrentKeyboard
        {
            get { return currentKeyboard; }
            set { currentKeyboard = value; }
        }

        public virtual bool MouseLDown
        {
            //get { if (NewKeyboard.IsKeyDown(Keys.Space)) { space = true; } else { space = false; } return space; }
            get { if (CurrentMouse.LeftButton == ButtonState.Pressed) { mouseL = true; } else { mouseL = false; } return mouseL; }
        }

        public virtual bool SpaceDown
        {
            //get { if (NewKeyboard.IsKeyDown(Keys.Space)) { space = true; } else { space = false; } return space; }
            get { if (CurrentKeyboard.IsKeyDown(Keys.Space)) { space = true; } else { space = false; } return space; }
        }
    }
}
