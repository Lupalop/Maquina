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
    public class StackPanel : BaseElement, IContainerElement
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
        }

        private bool IsFirstUpdateDone = false;
        // General
        public ObservableDictionary<string, BaseElement> Children { get; set; }
        public Orientation Orientation { get; set; }
        public Region ElementMargin { get; set; }
        public override string Id
        {
            get { return "GENERIC_STACKPANEL"; }
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
            if (!IsFirstUpdateDone)
            {
                return;
            }

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

            if (!IsFirstUpdateDone)
            {
                UpdateLayout();
                IsFirstUpdateDone = true;
            }

            base.Update(gameTime);
        }

        // Listeners
        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateLayout();
        }
        private void StackPanel_ElementChanged(object sender, ElementChangedEventArgs e)
        {
            UpdateLayout();
        }

        public void UpdateLayout()
        {
            int DistanceFromLeft = Location.X;
            int DistanceFromTop = Location.Y;

            foreach (BaseElement element in Children.Values)
            {
                switch (Orientation)
                {
                    case Orientation.Horizontal:
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
                        break;
                    case Orientation.Vertical:
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
                        break;
                }

                // TODO: Add considerations for other control alignments
                if (element is GuiElement)
                {
                    GuiElement newElement = (GuiElement)element;
                    if (Orientation == Orientation.Vertical)
                    {
                        switch (newElement.ControlAlignment)
                        {
                            case Alignment.Left:
                                break;
                            case Alignment.Center:
                                if (newElement.Size != null)
                                {
                                    newElement.Location = new Point(
                                        ActualBounds.Center.X -
                                        (newElement.ActualBounds.Width / 2),
                                        newElement.Location.Y);
                                    return;
                                }
                                newElement.Location = new Point(
                                    ActualBounds.Center.X,
                                    newElement.Location.Y);
                                break;
                            case Alignment.Right:
                                break;
                            case Alignment.Fixed:
                                break;
                        }
                    }
                    if (Orientation == Orientation.Horizontal)
                    {
                        switch (newElement.ControlAlignment)
                        {
                            case Alignment.Left:
                                break;
                            case Alignment.Center:
                                if (newElement.Size != null)
                                {
                                    newElement.Location = new Point(
                                        newElement.Location.X,
                                        ActualBounds.Center.Y -
                                        (newElement.ActualBounds.Height / 2));
                                }
                                newElement.Location = new Point(
                                    newElement.Location.X,
                                    ActualBounds.Center.Y);
                                break;
                            case Alignment.Right:
                                break;
                            case Alignment.Fixed:
                                break;
                        }
                    }
                }
            }

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
    }
}
