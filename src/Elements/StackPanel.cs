using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Elements
{
    public class StackPanel : GuiElement, IContainer
    {
        public StackPanel(string name) : base(name)
        {
            Children = new EventDictionary<string, GenericElement>();
            Orientation = Orientation.Vertical;
            ElementMargin = new Region();
        }

        // Properties
        public EventDictionary<string, GenericElement> Children { get; set; }
        public Orientation Orientation { get; set; }
        public Region ElementMargin { get; set; }
        private bool IsFirstUpdateDone = false;

        // Element ID
        public override string ID
        {
            get { return "GENERIC_STACKPANEL"; }
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
                if (Orientation == Orientation.Horizontal)
                {
                    if (element.Graphic != null || element.Dimensions != null)
                    {
                        element.Location = new Vector2(DistanceFromLeft, Location.Y);
                        DistanceFromLeft += ElementMargin.Left;
                        DistanceFromLeft += element.Bounds.Width;
                        DistanceFromLeft += ElementMargin.Right;
                    }
                    else
                    {
                        element.Location = new Vector2(DistanceFromLeft, Location.Y);
                    }
                }
                if (Orientation == Orientation.Vertical)
                {
                    if (element.Graphic != null || element.Dimensions != null)
                    {
                        element.Location = new Vector2(Location.X, DistanceFromTop);
                        DistanceFromTop += ElementMargin.Top;
                        DistanceFromTop += element.Bounds.Height;
                        DistanceFromTop += ElementMargin.Bottom;
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
                    if (Orientation == Orientation.Vertical)
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
                    if (Orientation == Orientation.Horizontal)
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
                if (Orientation == Orientation.Horizontal)
                {
                    ComputedWidth += ElementMargin.Left;
                    ComputedWidth += element.Bounds.Width;
                    ComputedWidth += ElementMargin.Right;
                    if (element.Bounds.Height > ComputedHeight)
                    {
                        ComputedHeight = element.Bounds.Height;
                    }
                }

                if (Orientation == Orientation.Vertical)
                {
                    ComputedHeight += ElementMargin.Top;
                    ComputedHeight += element.Bounds.Height;
                    ComputedHeight += ElementMargin.Bottom;
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
}
