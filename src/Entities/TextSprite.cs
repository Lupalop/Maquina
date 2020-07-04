using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public class TextSprite
    {
        private SpriteFont _font;
        private string _text;
        private Point _size;

        public TextSprite(SpriteFont font, string text)
        {
            Font = font;
            Text = text;
        }

        public TextSprite(SpriteFont font)
            : this(font, "")
        {
        }

        public TextSprite(string text)
            : this((SpriteFont)ContentFactory.TryGetResource("default"), text)
        {
        }

        public TextSprite()
            : this((SpriteFont)ContentFactory.TryGetResource("default"))
        {
        }

        public static implicit operator TextSprite(SpriteFont font)
        {
            return new TextSprite(font);
        }

        public static implicit operator TextSprite(string text)
        {
            return new TextSprite(text);
        }

        private Point ComputeSize()
        {
            if (Font == null || string.IsNullOrEmpty(Text))
            {
                return Point.Zero;
            }
            return Font.MeasureString(Text).ToPoint();
        }

        public virtual SpriteFont Font
        {
            get { return _font; }
            protected set
            {
                _font = value;
                _size = ComputeSize();
            }
        }

        public virtual string Text
        {
            get { return _text; }
            set
            {
#if MGE_LOCALE
                _text = Application.Locale.TryGetString(value);
#else
                text = value;
#endif
                _size = ComputeSize();
            }
        }

        public virtual Point Size
        {
            get { return _size; }
        }

        public virtual void Draw(SpriteBatch spriteBatch, DrawController controller, Point location, float scale)
        {
            spriteBatch.DrawString(
                Font,
                Text,
                location.ToVector2(),
                controller.Tint * controller.Opacity,
                controller.Rotation,
                controller.Origin,
                scale,
                controller.SpriteEffects,
                controller.LayerDepth);
        }
    }
}
