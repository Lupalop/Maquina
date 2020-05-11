using Maquina.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    public abstract class Control : Entity
    {
        // Constructor
        protected Control(string name) : base (name)
        {
            Id = "UI_CONTROL";
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            AutoPosition = false;
        }

        private HorizontalAlignment horizontalAlignment;
        public HorizontalAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set
            {
                horizontalAlignment = value;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.Custom));
            }
        }
        private VerticalAlignment verticalAlignment;
        public VerticalAlignment VerticalAlignment
        {
            get { return verticalAlignment; }
            set
            {
                verticalAlignment = value;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.Custom));
            }
        }


        private bool disabled;
        public bool Disabled
        {
            get { return disabled; }
            set
            {
                disabled = value;
                OnDisabledStateChanged();
            }
        }
        private bool focused;
        public bool Focused
        {
            get { return focused; }
            set
            {
                focused = value;
                OnFocusedStateChanged();
            }
        }

        public event EventHandler DisabledStateChanged;
        public event EventHandler FocusedStateChanged;

        protected virtual void OnDisabledStateChanged()
        {
            if (DisabledStateChanged != null)
            {
                DisabledStateChanged(this, EventArgs.Empty);
            }
        }
        protected virtual void OnFocusedStateChanged()
        {
            if (FocusedStateChanged != null)
            {
                FocusedStateChanged(this, EventArgs.Empty);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AutoPosition = false;
            }
            base.Dispose(disposing);
        }

        // Control Auto Position implementation
        private bool autoPosition;
        public bool AutoPosition
        {
            get { return autoPosition; }
            set
            {
                autoPosition = value;
                // Register to events necessary to update in time
                if (value)
                {
                    Application.Display.ResolutionChanged += Display_ResolutionChanged;
                    Application.Display.ScaleChanged += Global_ScaleChanged;
                    EntityChanged += Control_EntityChanged;
                }
                // Only unregister from these events if auto position was previously true
                if (autoPosition && !value)
                {
                    Application.Display.ResolutionChanged -= Display_ResolutionChanged;
                    Application.Display.ScaleChanged -= Global_ScaleChanged;
                    EntityChanged -= Control_EntityChanged;
                }
            }
        }

        public void UpdateAutoPositionedLayout()
        {
            // Ignore controls that are not requesting auto position
            if (!AutoPosition)
            {
                return;
            }

            int modifiedX = Location.X;
            int modifiedY = Location.Y;

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    modifiedX = Application.Display.WindowBounds.Left;
                    break;
                case HorizontalAlignment.Center:
                    if (ActualBounds.Width != 0)
                        modifiedX = Application.Display.WindowBounds.Center.X - (ActualBounds.Width / 2);
                    break;
                case HorizontalAlignment.Right:
                    modifiedX = Application.Display.WindowBounds.Right - ActualBounds.Width;
                    break;
                case HorizontalAlignment.Stretch:
                    break;
            }

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    modifiedY = Application.Display.WindowBounds.Top;
                    break;
                case VerticalAlignment.Center:
                    if (ActualBounds.Height != 0)
                        modifiedY = Application.Display.WindowBounds.Center.Y - (ActualBounds.Height / 2);
                    break;
                case VerticalAlignment.Bottom:
                    modifiedY = Application.Display.WindowBounds.Bottom - ActualBounds.Height;
                    break;
                case VerticalAlignment.Stretch:
                    break;
            }

            Location = new Point(modifiedX, modifiedY);
        }

        private void Display_ResolutionChanged(object sender, EventArgs e)
        {
            UpdateAutoPositionedLayout();
        }

        private void Global_ScaleChanged(object sender, EventArgs e)
        {
            UpdateAutoPositionedLayout();
        }

        private void Control_EntityChanged(object sender, EntityChangedEventArgs e)
        {
            if (e.Property == EntityChangedProperty.Location)
                return;

            UpdateAutoPositionedLayout();
        }
    }
}
