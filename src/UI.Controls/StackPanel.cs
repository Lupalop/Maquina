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
    public class StackPanel : Control, IEntityContainer
    {
        private Orientation _orientation;
        private Region _controlMargin;

        public StackPanel(string name) : base(name)
        {
            Children = new EntityCollection();
            _orientation = Orientation.Vertical;
            _controlMargin = new Region();
            Children.EntityChanged += OnLayoutChanged;
            Children.CollectionChanged += OnLayoutChanged;
            Application.Display.ScaleChanged += OnLayoutChanged;
        }

        public EntityCollection Children { get; protected set; }

        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                OnPropertyChanged(new PropertyChangedEventArgs(PropertyId.Orientation));
            }
        }

        public Region ControlMargin
        {
            get { return _controlMargin; }
            set
            {
                _controlMargin = value;
                OnPropertyChanged(new PropertyChangedEventArgs(PropertyId.Margin));
            }
        }

        public override bool IgnoreDisplayScale
        {
            get { return true; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Children.Draw(spriteBatch);
        }

        public override void Update()
        {
            Children.Update();
        }

        private void OnLayoutChanged(object sender, EventArgs e)
        {
            if (e is PropertyChangedEventArgs &&
                ((PropertyChangedEventArgs)e).Id == PropertyId.Location)
            {
                return;
            }

            UpdateLayout();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.Id == PropertyId.Orientation ||
                e.Id == PropertyId.Margin)
            {
                UpdateLayout();
            }

            if (e.Id == PropertyId.Location)
            {
                UpdateLayout(false);
            }

            if (e.Id == PropertyId.Disabled)
            {
                foreach (var item in Children)
                {
                    if (item is Control)
                    {
                        ((Control)(item)).Disabled = Disabled;
                    }
                }
            }
            base.OnPropertyChanged(e);
        }

        public void UpdateLayout(bool resizeContainer = true)
        {
            if (Children.Count <= 0)
            {
                return;
            }

            if (resizeContainer)
            {
                int ComputedWidth = 0;
                int ComputedHeight = 0;

                foreach (var item in Children)
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
                    else
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

            int CurrentX = Location.X;
            int CurrentY = Location.Y;

            foreach (var item in Children)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    CurrentY = Location.Y;
                    if (item is Control)
                    {
                        switch (((Control)item).VerticalAlignment)
                        {
                            case VerticalAlignment.Top:
                                CurrentY = ActualBounds.Top;
                                break;
                            case VerticalAlignment.Center:
                                CurrentY = ActualBounds.Center.Y - (item.ActualBounds.Height / 2);
                                break;
                            case VerticalAlignment.Bottom:
                                CurrentY = ActualBounds.Bottom - item.ActualBounds.Height;
                                break;
                        }
                    }

                    item.Location = new Point(CurrentX, CurrentY);
                    CurrentX += ControlMargin.Left;
                    CurrentX += item.ActualBounds.Width;
                    CurrentX += ControlMargin.Right;
                }
                else
                {
                    CurrentX = Location.X;
                    if (item is Control)
                    {
                        switch (((Control)item).HorizontalAlignment)
                        {
                            case HorizontalAlignment.Left:
                                break;
                            case HorizontalAlignment.Center:
                                CurrentX = ActualBounds.Center.X - (item.ActualBounds.Width / 2);
                                break;
                            case HorizontalAlignment.Right:
                                CurrentX = ActualBounds.Right - item.ActualBounds.Width;
                                break;
                        }
                    }

                    item.Location = new Point(CurrentX, CurrentY);
                    CurrentY += ControlMargin.Top;
                    CurrentY += item.ActualBounds.Height;
                    CurrentY += ControlMargin.Bottom;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Application.Display.ScaleChanged -= OnLayoutChanged;
                Children.EntityChanged -= OnLayoutChanged;
                Children.CollectionChanged -= OnLayoutChanged;
                Children.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
