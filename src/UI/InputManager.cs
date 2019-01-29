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
        }

        private Game _game { get; set; }
        // Previous
        public KeyboardState PreviousKeyboardState { get; set; }
        public GamePadState PreviousGamepadState { get; set; }
        public MouseState PreviousMouseState { get; set; }
        public TouchPanelState PreviousTouchState { get; set; }
        // Current
        public KeyboardState KeyboardState { get; set; }
        public GamePadState GamepadState { get; set; }
        public MouseState MouseState { get; set; }
        public TouchPanelState TouchState { get; set; }

        public bool KeyPressed(Keys key)
        {
            return KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }
        public bool KeyDown(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }
        public bool KeyUp(Keys key)
        {
            return KeyboardState.IsKeyUp(key);
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
