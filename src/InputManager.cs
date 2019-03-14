using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Maquina
{
    public enum MouseButton { Left, Middle, Right, XButton1, XButton2 };
    public class InputManager
    {
        public InputManager(Game game)
        {
            this.game = game;
            game.Activated += delegate
            {
                ShouldAcceptInput = true;
            };
            game.Deactivated += delegate
            {
                ShouldAcceptInput = false;
            };
            ShouldAcceptInput = true;
        }

        private Game game;
        // Previous state
        public KeyboardState PreviousKeyboardState { get; private set; }
        public GamePadState PreviousGamepadState { get; private set; }
        public MouseState PreviousMouseState { get; private set; }
        public TouchPanelState PreviousTouchState { get; private set; }
        // Current state
        public KeyboardState KeyboardState { get; private set; }
        public GamePadState GamepadState { get; private set; }
        public MouseState MouseState { get; private set; }
        public TouchPanelState TouchState { get; private set; }
        // Determine if input should be accepted
        public bool ShouldAcceptInput { get; set; }

        // Keyboard
        public bool KeyPressed(Keys key)
        {
            if (!ShouldAcceptInput)
                return false;
            return KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }
        public bool KeyDown(Keys key)
        {
            if (!ShouldAcceptInput)
                return false;
            return KeyboardState.IsKeyDown(key);
        }
        public bool KeyUp(Keys key)
        {
            if (!ShouldAcceptInput)
                return false;
            return KeyboardState.IsKeyUp(key);
        }

        // Mouse
        public bool MousePressed(MouseButton mb)
        {
            if (!ShouldAcceptInput)
                return false;
            switch (mb)
            {
                case MouseButton.Left:
                    return MouseState.LeftButton == ButtonState.Pressed &&
                        PreviousMouseState.LeftButton == ButtonState.Released;
                case MouseButton.Middle:
                    return MouseState.MiddleButton == ButtonState.Pressed &&
                        PreviousMouseState.MiddleButton == ButtonState.Released;
                case MouseButton.Right:
                    return MouseState.RightButton == ButtonState.Pressed &&
                        PreviousMouseState.RightButton == ButtonState.Released;
                case MouseButton.XButton1:
                    return MouseState.XButton1 == ButtonState.Pressed &&
                        PreviousMouseState.XButton1 == ButtonState.Released;
                case MouseButton.XButton2:
                    return MouseState.XButton2 == ButtonState.Pressed &&
                        PreviousMouseState.XButton2 == ButtonState.Released;
            }
            return false;
        }
        public bool MouseDown(MouseButton mb)
        {
            if (!ShouldAcceptInput)
                return false;
            switch (mb)
            {
                case MouseButton.Left:
                    return MouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return MouseState.MiddleButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return MouseState.RightButton == ButtonState.Pressed;
                case MouseButton.XButton1:
                    return MouseState.XButton1 == ButtonState.Pressed;
                case MouseButton.XButton2:
                    return MouseState.XButton2 == ButtonState.Pressed;
            }
            return false;
        }
        public bool MouseUp(MouseButton mb)
        {
            if (!ShouldAcceptInput)
                return false;
            switch (mb)
            {
                case MouseButton.Left:
                    return MouseState.LeftButton == ButtonState.Released;
                case MouseButton.Middle:
                    return MouseState.MiddleButton == ButtonState.Released;
                case MouseButton.Right:
                    return MouseState.RightButton == ButtonState.Released;
                case MouseButton.XButton1:
                    return MouseState.XButton1 == ButtonState.Released;
                case MouseButton.XButton2:
                    return MouseState.XButton2 == ButtonState.Released;
            }
            return false;
        }

        public Point MousePosition { get; set; }

        public void UpdateInput()
        {
            //
            PreviousGamepadState = GamepadState;
            PreviousKeyboardState = KeyboardState;
            PreviousMouseState = MouseState;
            PreviousTouchState = TouchState;
            //
            GamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            TouchState = TouchPanel.GetState(game.Window);
            //
            MousePosition = new Point(
                MathHelper.Clamp(MouseState.Position.X, 0, game.GraphicsDevice.Viewport.Bounds.Right),
                MathHelper.Clamp(MouseState.Position.Y, 0, game.GraphicsDevice.Viewport.Bounds.Bottom));
        }
    }
}
