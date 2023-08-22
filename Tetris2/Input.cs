using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris2
{
    public class Input
    {
        static KeyboardState currentKeyState;
        static KeyboardState previousKeyState;
        public static MouseState currentMouseState;
        public static MouseState previousMouseState;

        public static void GetStateCall()
        {
            GetState();
            GetMouseState();
        }

        public static KeyboardState GetState()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            return currentKeyState;
        }

        public static bool IsPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        public static bool HasBeenPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }

        public static MouseState GetMouseState()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            return currentMouseState;
        }

        public static bool MouseIsPressed(ButtonState buttonState)
        {
            if (buttonState == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        public static bool MouseHasBeenPressed(ButtonState buttonState, ButtonState previousButtonState)
        {
            if (buttonState == ButtonState.Pressed && previousButtonState == ButtonState.Released)
            {
                return true;
            }
            return false;
        }
    }
}
