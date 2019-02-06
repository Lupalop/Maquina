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

namespace Maquina.UI
{
    public class InputManager
    {
        public InputManager(Game game)
        {
            this._game = game;
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

        private Game _game { get; set; }
        // Previous state
        public KeyboardState PreviousKeyboardState { get; set; }
        public GamePadState PreviousGamepadState { get; set; }
        public MouseState PreviousMouseState { get; set; }
        public TouchPanelState PreviousTouchState { get; set; }
        // Current state
        public KeyboardState KeyboardState { get; set; }
        public GamePadState GamepadState { get; set; }
        public MouseState MouseState { get; set; }
        public TouchPanelState TouchState { get; set; }
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
                    return MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released;
                case MouseButton.Middle:
                    return MouseState.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton == ButtonState.Released;
                case MouseButton.Right:
                    return MouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released;
                case MouseButton.XButton1:
                    return MouseState.XButton1 == ButtonState.Pressed && PreviousMouseState.XButton1 == ButtonState.Released;
                case MouseButton.XButton2:
                    return MouseState.XButton2 == ButtonState.Pressed && PreviousMouseState.XButton2 == ButtonState.Released;
            }
            return false;
        }

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
            TouchState = TouchPanel.GetState(_game.Window);
        }
    }
}
