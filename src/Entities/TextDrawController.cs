using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Maquina.Entities
{
    public class TextDrawController : DrawController
    {
        private SpriteFont _font;
        private string _text;

        public TextDrawController() : base()
        {
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
                if (Font != null && Text.Trim() != null)
                {
                    Size = Font.MeasureString(Text).ToPoint();
                }
            }
        }
    }
}
