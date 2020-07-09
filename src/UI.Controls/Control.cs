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
        private HorizontalAlignment _horizontalAlignment;
        private VerticalAlignment _verticalAlignment;
        private bool _disabled;
        private bool _focused;

        protected Control(string name) : base (name)
        {
            _horizontalAlignment = HorizontalAlignment.Center;
            _verticalAlignment = VerticalAlignment.Center;
            AutoPosition = false;
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get { return _horizontalAlignment; }
            set
            {
                _horizontalAlignment = value;
                OnEntityChanged(this, new EntityChangedEventArgs(EntityChangedProperty.HorizontalAlignment));
            }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return _verticalAlignment; }
            set
            {
                _verticalAlignment = value;
                OnEntityChanged(this, new EntityChangedEventArgs(EntityChangedProperty.VerticalAlignment));
            }
        }

        public bool Disabled
        {
            get { return _disabled; }
            set
            {
                _disabled = value;
                OnEntityChanged(this, new EntityChangedEventArgs(EntityChangedProperty.Disabled));
            }
        }

        public bool Focused
        {
            get { return _focused; }
            set
            {
                _focused = value;
                OnEntityChanged(this, new EntityChangedEventArgs(EntityChangedProperty.Focused));
            }
        }

        public bool AutoPosition { get; set; }
    }
}
