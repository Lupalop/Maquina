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
            Background.Texture = (Texture2D)ContentFactory.TryGetResource("textbox-default");
            Label.Font = (SpriteFont)ContentFactory.TryGetResource("o-default_m");
            Tooltip.Font = (SpriteFont)ContentFactory.TryGetResource("o-default_m");
            Background.SpriteType = SpriteType.None;

            Application.Game.Window.TextInput += Window_TextInput;
            Label.Text = "";
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
                if (e.Key == Keys.Back && Label.Text.Length > 0)
                {
                    Label.Text = Label.Text.Remove(MathHelper.Clamp(Label.Text.Length - 1, 0, int.MaxValue), 1);
                    return;
                }
                if (InputManager.ReservedKeys.Contains(e.Key) || Label.Text.Length > MaxInput)
                {
                    return;
                }
                Label.Text += e.Character;
            }
        }
    }
}
