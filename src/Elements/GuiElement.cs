using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Elements
{
    public abstract class GuiElement : BaseElement
    {
        // Constructor
        protected GuiElement(string name) : base (name)
        {
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
                OnElementChanged(new ElementChangedEventArgs(ElementChangedProperty.Custom));
            }
        }
        private VerticalAlignment verticalAlignment;
        public VerticalAlignment VerticalAlignment
        {
            get { return verticalAlignment; }
            set
            {
                verticalAlignment = value;
                OnElementChanged(new ElementChangedEventArgs(ElementChangedProperty.Custom));
            }
        }

        public override string Id
        {
            get { return "GENERIC_GUI"; }
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

        protected void OnDisabledStateChanged()
        {
            if (DisabledStateChanged != null)
            {
                DisabledStateChanged(this, EventArgs.Empty);
            }
        }
        protected void OnFocusedStateChanged()
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

        // Element Auto Position implementation
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
                    Global.Display.ResolutionChanged += Display_ResolutionChanged;
                    Global.Display.ScaleChanged += Global_ScaleChanged;
                    ElementChanged += GuiElement_ElementChanged;
                }
                // Only unregister from these events if auto position was previously true
                if (autoPosition && !value)
                {
                    Global.Display.ResolutionChanged -= Display_ResolutionChanged;
                    Global.Display.ScaleChanged -= Global_ScaleChanged;
                    ElementChanged -= GuiElement_ElementChanged;
                }
            }
        }

        public void UpdateAutoPositionedLayout()
        {
            // Ignore elements that are not requesting auto position
            if (!AutoPosition)
            {
                return;
            }

            int modifiedElementX = Location.X;
            int modifiedElementY = Location.Y;

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    modifiedElementX = Global.Display.WindowBounds.Left;
                    break;
                case HorizontalAlignment.Center:
                    if (ActualBounds.Width != 0)
                        modifiedElementX = Global.Display.WindowBounds.Center.X - (ActualBounds.Width / 2);
                    break;
                case HorizontalAlignment.Right:
                    modifiedElementX = Global.Display.WindowBounds.Right - ActualBounds.Width;
                    break;
                case HorizontalAlignment.Stretch:
                    break;
            }

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    modifiedElementY = Global.Display.WindowBounds.Top;
                    break;
                case VerticalAlignment.Center:
                    if (ActualBounds.Height != 0)
                        modifiedElementY = Global.Display.WindowBounds.Center.Y - (ActualBounds.Height / 2);
                    break;
                case VerticalAlignment.Bottom:
                    modifiedElementY = Global.Display.WindowBounds.Bottom - ActualBounds.Height;
                    break;
                case VerticalAlignment.Stretch:
                    break;
            }

            Location = new Point(modifiedElementX, modifiedElementY);
        }

        private void Display_ResolutionChanged(object sender, EventArgs e)
        {
            UpdateAutoPositionedLayout();
        }

        private void Global_ScaleChanged(object sender, EventArgs e)
        {
            UpdateAutoPositionedLayout();
        }

        private void GuiElement_ElementChanged(object sender, ElementChangedEventArgs e)
        {
            if (e.Property == ElementChangedProperty.Location)
                return;

            UpdateAutoPositionedLayout();
        }
    }
}
