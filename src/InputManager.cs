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
    public class InputManager
    {
        public InputManager()
        {
            Global.Game.Activated += delegate
            {
                ShouldAcceptInput = true;
            };
            Global.Game.Deactivated += delegate
            {
                ShouldAcceptInput = false;
            };
            ShouldAcceptInput = true;
        }

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
            TouchState = TouchPanel.GetState(Global.Game.Window);
            //
            MousePosition = new Point(
                MathHelper.Clamp(MouseState.Position.X, 0, Global.Game.GraphicsDevice.Viewport.Bounds.Right),
                MathHelper.Clamp(MouseState.Position.Y, 0, Global.Game.GraphicsDevice.Viewport.Bounds.Bottom));
        }

        public static List<Keys> ReservedKeys = new List<Keys>()
        {
            Keys.Back,
            Keys.Tab,
            Keys.Enter,
            Keys.Pause,
            Keys.CapsLock,
            Keys.Kana,
            Keys.Kanji,
            Keys.Escape,
            Keys.ImeConvert,
            Keys.ImeNoConvert,
            //Keys.Space,
            Keys.PageUp,
            Keys.PageDown,
            Keys.End,
            Keys.Home,
            Keys.Left,
            Keys.Up,
            Keys.Right,
            Keys.Down,
            Keys.Select,
            Keys.Print,
            Keys.Execute,
            Keys.PrintScreen,
            Keys.Insert,
            Keys.Delete,
            Keys.Help,
            Keys.LeftWindows,
            Keys.RightWindows,
            Keys.Apps,
            Keys.Sleep,
            Keys.NumPad0,
            Keys.NumPad1,
            Keys.NumPad2,
            Keys.NumPad3,
            Keys.NumPad4,
            Keys.NumPad5,
            Keys.NumPad6,
            Keys.NumPad7,
            Keys.NumPad8,
            Keys.NumPad9,
            Keys.Multiply,
            Keys.Add,
            Keys.Separator,
            Keys.Subtract,
            Keys.Decimal,
            Keys.Divide,
            Keys.F1,
            Keys.F2,
            Keys.F3,
            Keys.F4,
            Keys.F5,
            Keys.F6,
            Keys.F7,
            Keys.F8,
            Keys.F9,
            Keys.F10,
            Keys.F11,
            Keys.F12,
            Keys.F13,
            Keys.F14,
            Keys.F15,
            Keys.F16,
            Keys.F17,
            Keys.F18,
            Keys.F19,
            Keys.F20,
            Keys.F21,
            Keys.F22,
            Keys.F23,
            Keys.F24,
            Keys.NumLock,
            Keys.Scroll,
            Keys.LeftShift,
            Keys.RightShift,
            Keys.LeftControl,
            Keys.RightControl,
            Keys.LeftAlt,
            Keys.RightAlt,
            Keys.BrowserBack,
            Keys.BrowserForward,
            Keys.BrowserRefresh,
            Keys.BrowserStop,
            Keys.BrowserSearch,
            Keys.BrowserFavorites,
            Keys.BrowserHome,
            Keys.VolumeMute,
            Keys.VolumeDown,
            Keys.VolumeUp,
            Keys.MediaNextTrack,
            Keys.MediaPreviousTrack,
            Keys.MediaStop,
            Keys.MediaPlayPause,
            Keys.LaunchMail,
            Keys.SelectMedia,
            Keys.LaunchApplication1,
            Keys.LaunchApplication2,
            //Keys.OemSemicolon,
            //Keys.OemPlus,
            //Keys.OemComma,
            //Keys.OemMinus,
            //Keys.OemPeriod,
            //Keys.OemQuestion,
            //Keys.OemTilde,
            Keys.ChatPadGreen,
            Keys.ChatPadOrange,
            //Keys.OemOpenBrackets,
            //Keys.OemPipe,
            //Keys.OemCloseBrackets,
            //Keys.OemQuotes,
            Keys.Oem8,
            //Keys.OemBackslash,
            Keys.ProcessKey,
            Keys.OemCopy,
            Keys.OemAuto,
            Keys.OemEnlW,
            Keys.Attn,
            Keys.Crsel,
            Keys.Exsel,
            Keys.EraseEof,
            Keys.Play,
            Keys.Zoom,
            Keys.Pa1,
            Keys.OemClear
        };
    }
}
