using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Elements
{
    public class StackPanel : GuiElement, IContainerElement
    {
        public StackPanel(string name) : base(name)
        {
            Background = new Sprite();
            Children = new ObservableDictionary<string, BaseElement>();
            Orientation = Orientation.Vertical;
            ElementMargin = new Region();
            OverrideContainerSize = false;
            Children.CollectionChanged += Children_CollectionChanged;
            ElementChanged += StackPanel_ElementChanged;
            Global.ScaleChanged += Global_ScaleChanged;
        }

        // General
        private ObservableDictionary<string, BaseElement> children;
        public ObservableDictionary<string, BaseElement> Children
        {
            get { return children; }
            set
            {
                children = value;
                UpdateSize();
                UpdateLayout();
            }
        }
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
        public override string Id
        {
            get { return "GUI_STACKPANEL"; }
        }

        // Child elements
        public Sprite Background { get; set; }

        // Alias
        public Texture2D ContainerBackground
        {
            get { return Background.Graphic; }
            set { Background.Graphic = value; }
        }

        // This flag prevents automatic updating of the container size
        // based on the elements inside it.
        // Useful when you want to use a custom width/height for the control.
        public bool OverrideContainerSize { get; set; }

        // Draw and update methods
        public override void Draw(GameTime gameTime)
        {
            foreach (BaseElement element in Children.Values)
            {
                element.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (BaseElement element in Children.Values)
            {
                element.Update(gameTime);
            }

            base.Update(gameTime);
        }

        // Listeners
        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BaseElement elem;
            UpdateSize();
            UpdateLayout();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    elem = (BaseElement)e.NewItems[0];
                    elem.ElementChanged += ChildElement_ElementChanged;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    elem = (BaseElement)e.OldItems[0];
                    elem.ElementChanged -= ChildElement_ElementChanged;
                    break;
            }
        }

        private void ChildElement_ElementChanged(object sender, ElementChangedEventArgs e)
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

        private void Global_ScaleChanged(object sender, float e)
        {
            UpdateLayout();
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

                // TODO: Add considerations for other control alignments
                if (!(element is GuiElement))
                {
                    continue;
                }

                GuiElement newElement = (GuiElement)element;
                int newElementX = newElement.Location.X;
                int newElementY = newElement.Location.Y;

                if (Orientation == Orientation.Vertical)
                {
                    switch (newElement.HorizontalAlignment)
                    {
                        case Alignment.Left:
                            break;
                        case Alignment.Center:
                            if (newElement.Size != null)
                            {
                                newElementX = ActualBounds.Center.X - (newElement.ActualBounds.Width / 2);
                                break;
                            }
                            newElementX = ActualBounds.Center.X;
                            break;
                        case Alignment.Right:
                            break;
                        case Alignment.Fixed:
                            break;
                    }
                    switch (newElement.VerticalAlignment)
                    {
                        case Alignment.Left:
                            break;
                        case Alignment.Center:
                            break;
                        case Alignment.Right:
                            break;
                        case Alignment.Fixed:
                            break;
                    }
                }

                if (Orientation == Orientation.Horizontal)
                {
                    switch (newElement.HorizontalAlignment)
                    {
                        case Alignment.Left:
                            break;
                        case Alignment.Center:
                            break;
                        case Alignment.Right:
                            break;
                        case Alignment.Fixed:
                            break;
                    }
                    switch (newElement.VerticalAlignment)
                    {
                        case Alignment.Left:
                            break;
                        case Alignment.Center:
                            if (newElement.Size != null)
                            {
                                newElementY = ActualBounds.Center.Y - (newElement.ActualBounds.Height / 2);
                                break;
                            }
                            newElementY = ActualBounds.Center.Y;
                            break;
                        case Alignment.Right:
                            break;
                        case Alignment.Fixed:
                            break;
                    }
                }

                newElement.Location = new Point(newElementX, newElementY);
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

            Size = new Point(ComputedWidth, ComputedHeight);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Children.CollectionChanged -= Children_CollectionChanged;
                ElementChanged -= StackPanel_ElementChanged;
                Global.ScaleChanged -= Global_ScaleChanged;
                foreach (var element in Children.Values)
                {
                    element.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
