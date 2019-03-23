﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;

namespace Maquina.UI
{
    public class Label : GuiElement
    {
        public Label(string objectName)
            : base (objectName)
        {
            Text = "";
        }

        public SpriteFont Font { get; set; }
        public string Text { get; set; }
        public override string ID
        {
            get { return "GUI_LABEL"; }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.DrawString(Font, Text, Location, Tint, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 1f);
            base.Draw(gameTime);
        }

        public override Vector2 Dimensions
        {
            get
            {
                Vector2 CurrentDimensions = Font.MeasureString(Text);
                return new Vector2(CurrentDimensions.X * Scale, CurrentDimensions.Y * Scale);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
