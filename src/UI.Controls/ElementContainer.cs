﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Elements
{
    public class ElementContainer : GuiElement
    {
        public ElementContainer(string name) : base(name)
        {
            Children = new Dictionary<string, GenericElement>();
            ContainerAlignment = ContainerAlignment.Vertical;
        }

        // Properties
        public Dictionary<string, GenericElement> Children { get; set; }
        public ContainerAlignment ContainerAlignment { get; set; }
        public int ElementSpacing { get; set; }

        // Element ID
        public override string ID
        {
            get { return "GUI_CONTAINER"; }
        }

        // Override methods
        public override void Draw(GameTime gameTime)
        {
            foreach (GenericElement element in Children.Values)
            {
                element.Draw(gameTime);
            }

            if (OnDraw != null)
                OnDraw();
        }

        public override void Update(GameTime gameTime)
        {
            int DistanceFromLeft = (int)Location.X;
            int DistanceFromTop = (int)Location.Y;

            foreach (GenericElement element in Children.Values)
            {
                // TODO: Add considerations for other control alignments
                if (ContainerAlignment == ContainerAlignment.Horizontal)
                {
                    if (element.Graphic != null || element.Dimensions != null)
                    {
                        element.Location = new Vector2(DistanceFromLeft, Location.Y);
                        DistanceFromLeft += element.Bounds.Width;
                        DistanceFromLeft += ElementSpacing;
                    }
                    else
                    {
                        element.Location = new Vector2(DistanceFromLeft, Location.Y);
                    }
                }
                else
                {
                    if (element.Graphic != null || element.Dimensions != null)
                    {
                        element.Location = new Vector2(Location.X, DistanceFromTop);
                        DistanceFromTop += element.Bounds.Height;
                        DistanceFromTop += ElementSpacing;
                    }
                    else
                    {
                        element.Location = new Vector2(Location.X, DistanceFromTop);
                    }
                }

                element.Update(gameTime);
            }

            UpdatePoints();

            if (OnUpdate != null)
                OnUpdate();
        }

        public override void UpdatePoints()
        {
            float ComputedWidth = 0;
            float ComputedHeight = 0;

            foreach (GenericElement element in Children.Values)
            {
                ComputedWidth += element.Bounds.Width;
                ComputedHeight += element.Bounds.Height;
            }

            Dimensions = new Vector2(ComputedWidth, ComputedHeight);
            Bounds = new Rectangle(Location.ToPoint(), Dimensions.ToPoint());
        }
    }

    public enum ContainerAlignment
    {
        Horizontal,
        Vertical
    }
}
