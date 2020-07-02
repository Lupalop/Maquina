using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Entities;

namespace Maquina.UI
{
    public class Label : Control
    {
        private SpriteFont _font;
        private string _text;

        public Label(string name) : base (name)
        {
            Id = "UI_LABEL";
            DrawController = new DrawController();
            Font = (SpriteFont)ContentFactory.TryGetResource("default");
            Text = "";
        }

        public SpriteFont Font
        {
            get { return _font; }
            set
            {
                _font = value;
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
#if MGE_LOCALE
                _text = Application.Locale.TryGetString(value);
#else
                text = value;
#endif
            }
        }

        public override Point Size
        {
            get
            {
                if (Font != null && Text.Trim() != null)
                {
                    return Font.MeasureString(Text).ToPoint();
                }
                return Point.Zero;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                Font,
                Text,
                Location.ToVector2(),
                DrawController.Tint * DrawController.Opacity,
                DrawController.Rotation,
                DrawController.Origin,
                ActualScale,
                DrawController.SpriteEffects,
                DrawController.LayerDepth);
        }

        public override void Update()
        {
        }
    }
}
