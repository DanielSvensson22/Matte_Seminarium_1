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

        private MouseState currentMouse { get; set; }
        private MouseState previousMouse { get; set; }
        private KeyboardState currentKeyboard { get; set; }


        public void Update(GameTime gameTime)
        {
            UpdateStates();
            UpdateValues();
        }

        private void UpdateStates()
        {
            CurrentMouse = Mouse.GetState();
            CurrentKeyboard = Keyboard.GetState();
        }

        private void UpdateValues()
        {
            MousePosition = new Vector2(CurrentMouse.X, CurrentMouse.Y);
            if (CurrentMouseL && !PreviousMouseL) { MouseOrigin = new Vector2(CurrentMouse.X, CurrentMouse.Y); }
            if (!CurrentMouseL && PreviousMouseL) { Direction = MouseOrigin - MousePosition; Distance = Vector2.Distance(MouseOrigin, MousePosition); }
        }

        #region States
        public virtual MouseState CurrentMouse
        {
            get { return currentMouse; }
            set { PreviousMouse = currentMouse; currentMouse = value; }
        }
        public virtual MouseState PreviousMouse
        {
            get { return previousMouse; }
            set { previousMouse = value; }
        }
        
        public virtual KeyboardState CurrentKeyboard
        {
            get { return currentKeyboard; }
            set { currentKeyboard = value; }
        }
        #endregion

        public virtual bool CurrentMouseL
        {
            get { if (CurrentMouse.LeftButton == ButtonState.Pressed) { return true; } else { return false; } }
        }

        public virtual bool PreviousMouseL
        {
            get { if (PreviousMouse.LeftButton == ButtonState.Pressed) { return true; } else { return false; } }
        }

        public virtual bool SpaceDown
        {
            get { if (CurrentKeyboard.IsKeyDown(Keys.Space)) { return true; } else { return false; } }
        }

        public virtual Vector2 MouseOrigin
        {
            get { return mouseOrigin; }
            set { mouseOrigin = value; }
        }
        
        public virtual Vector2 MousePosition
        {
            get { return mousePosition; }
            set { mousePosition = value; }
        }

        public virtual Vector2 Direction
        {
            get { return direction; }
            set { direction = value; direction.Normalize(); }
        }
        
        public virtual float Distance
        {
            get { return distance; }
            set { distance = value; }
        }
    }
}
