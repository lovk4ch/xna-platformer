using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace AnimatedApp_15.Servants
{
    internal static class InputManager
    {
        static KeyboardState keyOldState;
        static KeyboardState keyState;
        static MouseState mouseState;
        static MouseState mouseOldState;
        internal static bool isActive = true;
        internal static void Update()
        {
            keyOldState = keyState;
            keyState = Keyboard.GetState();

            mouseOldState = mouseState;
            mouseState = Mouse.GetState();
        }
        internal static bool IsKeyDown(Keys key)
        {
            if (isActive) return keyState.IsKeyDown(key);
            else return false;
        }
        internal static bool IsKeyPress(Keys key)
        {
            if (isActive) return IsKeyDown(key) && keyOldState.IsKeyUp(key);
            else return false;
        }
        internal static bool isAnyKeyDown()
        {
            if (isActive) return keyState.GetPressedKeys().Length > 0;
            else return false;
        }
        internal static bool isAnyKeyPress()
        {
            if (isActive) return keyState.GetPressedKeys().Length > 0
                && keyOldState.GetPressedKeys().Length == 0;
            else return false;
        }
        internal static bool IsMouseLeftDown()
        {
            return mouseState.LeftButton == ButtonState.Pressed;
        }
        internal static bool IsMouseLeftClick()
        {
            return IsMouseLeftDown() && mouseOldState.LeftButton == ButtonState.Released;
        }
        internal static bool IsMouseRightDown()
        {
            return mouseState.RightButton == ButtonState.Pressed;
        }
        internal static bool IsMouseRightClick()
        {
            return IsMouseRightDown() && mouseOldState.RightButton == ButtonState.Released;
        }
        internal static bool IsMouseMiddleDown()
        {
            return mouseState.MiddleButton == ButtonState.Pressed;
        }
        internal static bool IsMouseMiddleClick()
        {
            return IsMouseMiddleDown() && mouseOldState.MiddleButton == ButtonState.Released;
        }
        internal static bool IsMouseWheelUp()
        {
            return mouseState.ScrollWheelValue > mouseOldState.ScrollWheelValue;
        }
        internal static bool IsMouseWheelDown()
        {
            return mouseState.ScrollWheelValue < mouseOldState.ScrollWheelValue;
        }
        internal static Point GetMousePoint()
        {
            return new Point(mouseState.X, mouseState.Y);
        }
        internal static Vector2 GetMousePosition()
        {
            return new Vector2(mouseState.X, mouseState.Y);
        }
    }
}