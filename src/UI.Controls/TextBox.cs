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
        public TextBox(string objectName) : base (objectName)
        {
            // Default TB graphic
            Graphic = Global.Textures["textbox-default"];
            Font = Global.Fonts["o-default_m"];
            TooltipFont = Global.Fonts["o-default_m"];
            SpriteType = SpriteType.None;

            Global.Game.Window.TextInput += Window_TextInput;
            Text = "";
            MaxInput = 30;
            OnInput = delegate { };
        }

        // Properties
        public int MaxInput { get; set; }

        public Action OnInput { get; set; }

        public override string ID
        {
            get { return "GUI_TEXTBOX"; }
        }

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            OnInput();
            if (Disabled && !InputManager.ShouldAcceptInput)
            {
                return;
            }
            // TODO: Concept of focus
            if (e.Key == Keys.Back && Text.Length > 0)
            {
                Text = Text.Remove(MathHelper.Clamp(Text.Length - 1, 0, Int32.MaxValue), 1);
                return;
            }
            if (InputManager.ReservedKeys.Contains(e.Key) || Text.Length > MaxInput)
            {
                return;
            }
            Text += e.Character;
        }
    }
}
