using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Maquina.Entities;

namespace Maquina.UI
{
    public class TextBox : Button
    {
        public TextBox(string name) : base(name)
        {
            // Default TB graphic
            BackgroundSprite = (Texture2D)ContentFactory.TryGetResource("textbox-default");
            TextSprite = (SpriteFont)ContentFactory.TryGetResource("o-default_m");
            //Tooltip.Font = (SpriteFont)ContentFactory.TryGetResource("o-default_m");
            //Background.SpriteType = SpriteType.None;

            Application.Game.Window.TextInput += Window_TextInput;
            //Label.Text = "";
            MaxInput = 30;
        }

        // Properties
        public int MaxInput { get; set; }

        // Events
        public event EventHandler OnInput;

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            if (OnInput != null)
            {
                OnInput(this, EventArgs.Empty);
            }
            if (!Enabled || !Focused)
            {
                return;
            }
            if (Application.Input.Enabled)
            {
                if (e.Key == Keys.Back && Text.Length > 0)
                {
                    Text = Text.Remove(MathHelper.Clamp(Text.Length - 1, 0, int.MaxValue), 1);
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
}
