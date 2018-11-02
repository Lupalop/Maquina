using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Objects;
using Maquina.UI.Scenes;

namespace Maquina.UI.Controls
{
    public class Label : GuiElement
    {
        public Label(string objectName)
            : base (objectName)
        {
            Text = "";
        }

        public Vector2 GraphicCenter { get; set; }
        public SpriteFont Font { get; set; }
        public string Text { get; set; }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.DrawString(Font, Text, GraphicCenter, Tint, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 1f);
            base.Draw(gameTime);
        }

        public override Vector2 Dimensions
        {
            get
            {
                return Font.MeasureString(Text);
            }
        }

        public override void Update(GameTime gameTime)
        {
            GraphicCenter = new Vector2(Location.X, Location.Y);
            base.Update(gameTime);
        }
    }
}
