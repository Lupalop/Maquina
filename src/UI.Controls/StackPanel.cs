using Maquina.Entities;
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
    public class StackPanel : Control, IContainer
    {
        public StackPanel(string name) : base(name)
        {
            Children = new EntityDictionary();
            Orientation = Orientation.Vertical;
            ControlMargin = new Region();
            Children.EntityChanged += Children_EntityChanged;
            Application.Display.ScaleChanged += Global_ScaleChanged;
        }

        // General
        public EntityDictionary Children { get; protected set; }
        private Orientation orientation;
        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                orientation = value;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.DestinationRectangle));
            }
        }
        private Region controlMargin;
        public Region ControlMargin
        {
            get { return controlMargin; }
            set
            {
                controlMargin = value;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.DestinationRectangle));
            }
        }

        // Draw and update methods
        public override void Draw(SpriteBatch spriteBatch)
        {
            Children.Draw(spriteBatch);
        }

        public override void Update()
        {
            Children.Update();
        }

        // Listeners
        private void Children_EntityChanged(object sender, EntityChangedEventArgs e)
        {
            if (e.Property != EntityChangedProperty.Location)
            {
                UpdateSize();
                UpdateLayout();
            }
        }

        private void Global_ScaleChanged(object sender, EventArgs e)
        {
            UpdateSize();
            UpdateLayout();
        }

        protected override void OnEntityChanged(EntityChangedEventArgs e)
        {
            if (e.Property != EntityChangedProperty.Location)
            {
                UpdateSize();
            }
            UpdateLayout();

            base.OnEntityChanged(e);
        }

        protected override void OnDisabledStateChanged()
        {
            foreach (var item in Children.Values)
            {
                if (item is Control)
                {
                    ((Control)(item)).Disabled = Disabled;
                }
                if (Children.IsModified)
                {
                    break;
                }
            }

            base.OnDisabledStateChanged();
        }

        public void UpdateLayout()
        {
            if (Children == null)
            {
                return;
            }

            int DistanceFromLeft = Location.X;
            int DistanceFromTop = Location.Y;

            foreach (var item in Children.Values)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    if (item.Size != null)
                    {
                        item.Location = new Point(DistanceFromLeft, Location.Y);
                        DistanceFromLeft += ControlMargin.Left;
                        DistanceFromLeft += item.ActualBounds.Width;
                        DistanceFromLeft += ControlMargin.Right;
                    }
                    else
                    {
                        item.Location = new Point(DistanceFromLeft, Location.Y);
                    }
                }
                if (Orientation == Orientation.Vertical)
                {
                    if (item.Size != null)
                    {
                        item.Location = new Point(Location.X, DistanceFromTop);
                        DistanceFromTop += ControlMargin.Top;
                        DistanceFromTop += item.ActualBounds.Height;
                        DistanceFromTop += ControlMargin.Bottom;
                    }
                    else
                    {
                        item.Location = new Point(Location.X, DistanceFromTop);
                    }
                }

                if (!(item is Control))
                {
                    continue;
                }

                Control modifiedEntity = (Control)item;
                int newX = modifiedEntity.Location.X;
                int newY = modifiedEntity.Location.Y;

                if (Orientation == Orientation.Vertical)
                {
                    switch (modifiedEntity.HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            break;
                        case HorizontalAlignment.Center:
                            if (modifiedEntity.Size != null)
                            {
                                newX = ActualBounds.Center.X - (modifiedEntity.ActualBounds.Width / 2);
                                break;
                            }
                            newX = ActualBounds.Center.X;
                            break;
                        case HorizontalAlignment.Right:
                            if (modifiedEntity.Size != null)
                            {
                                newX = ActualBounds.Right - modifiedEntity.ActualBounds.Width;
                                break;
                            }
                            newX = ActualBounds.Right;
                            break;
                        case HorizontalAlignment.Stretch:
                            break;
                    }
                }

                if (Orientation == Orientation.Horizontal)
                {
                    switch (modifiedEntity.VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            newY = ActualBounds.Top;
                            break;
                        case VerticalAlignment.Center:
                            if (modifiedEntity.Size != null)
                            {
                                newY = ActualBounds.Center.Y - (modifiedEntity.ActualBounds.Height / 2);
                                break;
                            }
                            newY = ActualBounds.Center.Y;
                            break;
                        case VerticalAlignment.Bottom:
                            newY = ActualBounds.Bottom - modifiedEntity.ActualBounds.Height;
                            break;
                        case VerticalAlignment.Stretch:
                            break;
                    }
                }

                modifiedEntity.Location = new Point(newX, newY);
            }
        }

        public void UpdateSize()
        {
            if (Children == null)
            {
                return;
            }

            int ComputedWidth = 0;
            int ComputedHeight = 0;

            foreach (var item in Children.Values)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    ComputedWidth += ControlMargin.Left;
                    ComputedWidth += item.ActualBounds.Width;
                    ComputedWidth += ControlMargin.Right;
                    if (item.ActualBounds.Height > ComputedHeight)
                    {
                        ComputedHeight = item.ActualBounds.Height;
                    }
                }

                if (Orientation == Orientation.Vertical)
                {
                    ComputedHeight += ControlMargin.Top;
                    ComputedHeight += item.ActualBounds.Height;
                    ComputedHeight += ControlMargin.Bottom;
                    if (item.ActualBounds.Width > ComputedWidth)
                    {
                        ComputedWidth = item.ActualBounds.Width;
                    }
                }
            }

            Size = new Point(ComputedWidth, ComputedHeight);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Children.EntityChanged -= Children_EntityChanged;
                Children.Clear(true);
                Application.Display.ScaleChanged -= Global_ScaleChanged;
            }
            base.Dispose(disposing);
        }
    }
}
