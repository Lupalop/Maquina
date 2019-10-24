using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Maquina.Elements;

namespace Maquina.UI
{
    public class TextBox : MenuButton
    {
        public TextBox(string name) : base(name)
        {
            Id = "GUI_TEXTBOX";

            // Default TB graphic
            MenuBackground = Global.Textures["textbox-default"];
            MenuFont = Global.Fonts["o-default_m"];
            TooltipFont = Global.Fonts["o-default_m"];
            MenuBackgroundSpriteType = SpriteType.None;

            Global.Game.Window.TextInput += Window_TextInput;
            MenuLabel = "";
            MaxInput = 30;
        }

        // Properties
        public int MaxInput { get; set; }

        // Element events
        public event EventHandler OnInput;

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            if (OnInput != null)
            {
                OnInput(this, EventArgs.Empty);
            }
            if (Disabled || !Focused)
            {
                return;
            }
            if (InputManager.ShouldAcceptInput)
            {
                if (e.Key == Keys.Back && MenuLabel.Length > 0)
                {
                    MenuLabel = MenuLabel.Remove(MathHelper.Clamp(MenuLabel.Length - 1, 0, int.MaxValue), 1);
                    return;
                }
                if (InputManager.ReservedKeys.Contains(e.Key) || MenuLabel.Length > MaxInput)
                {
                    return;
                }
                MenuLabel += e.Character;
            }
        }
    }
}
