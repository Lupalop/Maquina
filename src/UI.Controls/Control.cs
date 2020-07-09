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
                if (_horizontalAlignment == value)
                {
                    return;
                }
                _horizontalAlignment = value;
                OnPropertyChanged(new PropertyChangedEventArgs(PropertyId.HorizontalAlignment));
            }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return _verticalAlignment; }
            set
            {
                if (_verticalAlignment == value)
                {
                    return;
                }
                _verticalAlignment = value;
                OnPropertyChanged(new PropertyChangedEventArgs(PropertyId.VerticalAlignment));
            }
        }

        public bool Disabled
        {
            get { return _disabled; }
            set
            {
                if (_disabled == value)
                {
                    return;
                }
                _disabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs(PropertyId.Disabled));
            }
        }

        public bool Focused
        {
            get { return _focused; }
            set
            {
                if (_focused == value)
                {
                    return;
                }
                _focused = value;
                OnPropertyChanged(new PropertyChangedEventArgs(PropertyId.Focused));
            }
        }

        public bool AutoPosition { get; set; }
    }
}
