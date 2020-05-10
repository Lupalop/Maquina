using Maquina.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    public class StackPanel : Control, IContainerElement
    {
        public StackPanel(string name) : base(name)
        {
            Id = "UI_STACKPANEL";
            Background = new Sprite();
            Children = new ElementDictionary();
            Orientation = Orientation.Vertical;
            ElementMargin = new Region();
            OverrideContainerSize = false;
            Children.ElementChanged += Children_ElementChanged;
            ElementChanged += StackPanel_ElementChanged;
            Application.Display.ScaleChanged += Global_ScaleChanged;
            DisabledStateChanged += StackPanel_DisabledStateChanged;
            IsScaleSupported = false;
        }

        // General
        public ElementDictionary Children { get; protected set; }
        private Orientation orientation;
        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                orientation = value;
                OnElementChanged(new ElementChangedEventArgs(ElementChangedProperty.Custom));
            }
        }
        private Region elementMargin;
        public Region ElementMargin
        {
            get { return elementMargin; }
            set
            {
                elementMargin = value;
                OnElementChanged(new ElementChangedEventArgs(ElementChangedProperty.Custom));
            }
        }

        // Child elements
        public Sprite Background { get; set; }

        // Alias
        public Texture2D ContainerBackground
        {
            get { return Background.Texture; }
            set { Background.Texture = value; }
        }

        // This flag prevents automatic updating of the container size
        // based on the elements inside it.
        // Useful when you want to use a custom width/height for the control.
        public bool OverrideContainerSize { get; set; }

        // Draw and update methods
        public override void Draw()
        {
            Children.Draw();

            base.Draw();
        }

        public override void Update()
        {
            Children.Update();

            base.Update();
        }

        // Listeners
        private void Children_ElementChanged(object sender, ElementChangedEventArgs e)
        {
            if (e.Property != ElementChangedProperty.Location)
            {
                UpdateSize();
                UpdateLayout();
            }
        }

        private void StackPanel_ElementChanged(object sender, ElementChangedEventArgs e)
        {
            if (e.Property != ElementChangedProperty.Location)
            {
                UpdateSize();
            }
            UpdateLayout();
        }

        private void Global_ScaleChanged(object sender, EventArgs e)
        {
            UpdateSize();
            UpdateLayout();
        }

        private void StackPanel_DisabledStateChanged(object sender, EventArgs e)
        {
            GuiUtils.SetElementDisabledState(Children, Disabled);
        }

        public void UpdateLayout()
        {
            int DistanceFromLeft = Location.X;
            int DistanceFromTop = Location.Y;

            foreach (BaseElement element in Children.Values)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    if (element.Size != null)
                    {
                        element.Location = new Point(DistanceFromLeft, Location.Y);
                        DistanceFromLeft += ElementMargin.Left;
                        DistanceFromLeft += element.ActualBounds.Width;
                        DistanceFromLeft += ElementMargin.Right;
                    }
                    else
                    {
                        element.Location = new Point(DistanceFromLeft, Location.Y);
                    }
                }
                if (Orientation == Orientation.Vertical)
                {
                    if (element.Size != null)
                    {
                        element.Location = new Point(Location.X, DistanceFromTop);
                        DistanceFromTop += ElementMargin.Top;
                        DistanceFromTop += element.ActualBounds.Height;
                        DistanceFromTop += ElementMargin.Bottom;
                    }
                    else
                    {
                        element.Location = new Point(Location.X, DistanceFromTop);
                    }
                }

                if (!(element is Control))
                {
                    continue;
                }

                Control modifiedElement = (Control)element;
                int newElementX = modifiedElement.Location.X;
                int newElementY = modifiedElement.Location.Y;

                if (Orientation == Orientation.Vertical)
                {
                    switch (modifiedElement.HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            break;
                        case HorizontalAlignment.Center:
                            if (modifiedElement.Size != null)
                            {
                                newElementX = ActualBounds.Center.X - (modifiedElement.ActualBounds.Width / 2);
                                break;
                            }
                            newElementX = ActualBounds.Center.X;
                            break;
                        case HorizontalAlignment.Right:
                            if (modifiedElement.Size != null)
                            {
                                newElementX = ActualBounds.Right - modifiedElement.ActualBounds.Width;
                                break;
                            }
                            newElementX = ActualBounds.Right;
                            break;
                        case HorizontalAlignment.Stretch:
                            break;
                    }
                }

                if (Orientation == Orientation.Horizontal)
                {
                    switch (modifiedElement.VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            newElementY = ActualBounds.Top;
                            break;
                        case VerticalAlignment.Center:
                            if (modifiedElement.Size != null)
                            {
                                newElementY = ActualBounds.Center.Y - (modifiedElement.ActualBounds.Height / 2);
                                break;
                            }
                            newElementY = ActualBounds.Center.Y;
                            break;
                        case VerticalAlignment.Bottom:
                            newElementY = ActualBounds.Bottom - modifiedElement.ActualBounds.Height;
                            break;
                        case VerticalAlignment.Stretch:
                            break;
                    }
                }

                modifiedElement.Location = new Point(newElementX, newElementY);
            }
        }

        public void UpdateSize()
        {
            // Don't continue on recomputing container width/height
            // if this flag is true
            if (OverrideContainerSize)
            {
                return;
            }

            int ComputedWidth = 0;
            int ComputedHeight = 0;

            foreach (BaseElement element in Children.Values)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    ComputedWidth += ElementMargin.Left;
                    ComputedWidth += element.ActualBounds.Width;
                    ComputedWidth += ElementMargin.Right;
                    if (element.ActualBounds.Height > ComputedHeight)
                    {
                        ComputedHeight = element.ActualBounds.Height;
                    }
                }

                if (Orientation == Orientation.Vertical)
                {
                    ComputedHeight += ElementMargin.Top;
                    ComputedHeight += element.ActualBounds.Height;
                    ComputedHeight += ElementMargin.Bottom;
                    if (element.ActualBounds.Width > ComputedWidth)
                    {
                        ComputedWidth = element.ActualBounds.Width;
                    }
                }
            }

            Size = new Point(ComputedWidth, ComputedHeight);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Children.ElementChanged -= Children_ElementChanged;
                Children.Clear(true);
                ElementChanged -= StackPanel_ElementChanged;
                DisabledStateChanged -= StackPanel_DisabledStateChanged;
                Application.Display.ScaleChanged -= Global_ScaleChanged;
            }
            base.Dispose(disposing);
        }
    }
}
