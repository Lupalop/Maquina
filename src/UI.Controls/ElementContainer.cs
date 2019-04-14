using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Elements
{
    // TODO: Implement columns and rows
    public class ElementContainer : GuiElement
    {
        public ElementContainer(string name) : base(name)
        {
            Children = new EventDictionary<string, GenericElement>();
            ContainerAlignment = ContainerAlignment.Vertical;
        }

        // Properties
        public EventDictionary<string, GenericElement> Children { get; set; }
        public ContainerAlignment ContainerAlignment { get; set; }
        public int ElementSpacing { get; set; }
        private bool IsFirstUpdateDone = false;

        // Element ID
        public override string ID
        {
            get { return "GUI_CONTAINER"; }
        }

        // Override methods
        public override void Draw(GameTime gameTime)
        {
            if (!IsFirstUpdateDone)
            {
                return;
            }

            foreach (GenericElement element in Children.Values)
            {
                element.Draw(gameTime);
            }

            if (OnDraw != null)
            {
                OnDraw(this);
            }
        }

        public override void Update(GameTime gameTime)
        {
            float DistanceFromLeft = Location.X;
            float DistanceFromTop = Location.Y;

            foreach (GenericElement element in Children.Values)
            {
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

                // TODO: Add considerations for other control alignments
                if (element is GuiElement)
                {
                    GuiElement newElement = (GuiElement)element;
                    if (ContainerAlignment == ContainerAlignment.Vertical)
                    {
                        switch (newElement.ControlAlignment)
                        {
                            case ControlAlignment.Left:
                                break;
                            case ControlAlignment.Center:
                                if (newElement.Graphic != null || newElement.Dimensions != null)
                                {
                                    newElement.Location = new Vector2(this.Bounds.Center.X - (newElement.Bounds.Width / 2), newElement.Location.Y);
                                }
                                else
                                {
                                    newElement.Location = new Vector2(this.Bounds.Center.X, newElement.Location.Y);
                                }
                                break;
                            case ControlAlignment.Right:
                                break;
                            case ControlAlignment.Fixed:
                            default:
                                break;
                        }
                    }
                    if (ContainerAlignment == ContainerAlignment.Horizontal)
                    {
                        switch (newElement.ControlAlignment)
                        {
                            case ControlAlignment.Left:
                                break;
                            case ControlAlignment.Center:
                                if (newElement.Graphic != null || newElement.Dimensions != null)
                                {
                                    newElement.Location = new Vector2(newElement.Location.X, this.Bounds.Center.Y - (newElement.Bounds.Height / 2));
                                }
                                else
                                {
                                    newElement.Location = new Vector2(newElement.Location.X, this.Bounds.Center.Y);
                                }
                                break;
                            case ControlAlignment.Right:
                                break;
                            case ControlAlignment.Fixed:
                            default:
                                break;
                        }
                    }
                }

                element.Update(gameTime);
            }

            UpdatePoints();

            if (OnUpdate != null)
            {
                OnUpdate(this);
            }
            if (!IsFirstUpdateDone)
            {
                IsFirstUpdateDone = true;
            }
        }

        public override void UpdatePoints()
        {
            float ComputedWidth = 0;
            float ComputedHeight = 0;

            foreach (GenericElement element in Children.Values)
            {
                if (ContainerAlignment == ContainerAlignment.Horizontal)
                {
                    ComputedWidth += element.Bounds.Width;
                    if (element.Bounds.Height > ComputedHeight)
                    {
                        ComputedHeight = element.Bounds.Height;
                    }
                }

                if (ContainerAlignment == ContainerAlignment.Vertical)
                {
                    ComputedHeight += element.Bounds.Height;
                    if (element.Bounds.Width > ComputedWidth)
                    {
                        ComputedWidth = element.Bounds.Width;
                    }
                }
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
