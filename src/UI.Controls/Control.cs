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
                //TODO: event
            }
        }

        private VerticalAlignment verticalAlignment;
        public VerticalAlignment VerticalAlignment
        {
            get { return verticalAlignment; }
            set
            {
                verticalAlignment = value;
                //TODO: event
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
            base.Dispose(disposing);
        }

        public bool AutoPosition { get; set; }
    }
}
