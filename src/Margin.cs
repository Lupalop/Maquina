using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    [Serializable]
    public struct Margin : IEquatable<Margin>
    {
        private int _top;
        private int _right;
        private int _bottom;
        private int _left;

        private int _width;
        private int _height;

        public int Top
        {
            get { return _top; }
            set
            {
                _top = value;
                _height = _top + _bottom;
            }
        }

        public int Right
        {
            get { return _right; }
            set
            {
                _right = value;
                _width = _left + _right;
            }
        }

        public int Bottom
        {
            get { return _bottom; }
            set
            {
                _bottom = value;
                _height = _top + _bottom;
            }
        }

        public int Left
        {
            get { return _left; }
            set
            {
                _left = value;
                _width = _left + _right;
            }
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public Margin(int top, int right, int bottom, int left)
        {
            _top = top;
            _right = right;
            _bottom = bottom;
            _left = left;
            _width = left + right;
            _height = top + bottom;
        }

        public Margin(int value)
            : this(value, value, value, value)
        {
        }

        public Margin(Point height, Point width)
            : this(height.X, width.X, height.Y, width.Y)
        {
        }

        public static Margin Empty
        {
            get { return default(Margin); }
        }

        public bool IsEmpty
        {
            get
            {
                if (Top == 0 && Right == 0 && Bottom == 0 && Left == 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool Equals(Margin other)
        {
            if (Top == other.Top && Right == other.Right &&
                Bottom == other.Bottom && Left == other.Left)
            {
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is Margin)
            {
                result = Equals((Margin)obj);
            }
            return result;
        }

        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return string.Format(currentCulture,
                "{{Top:{0} Right:{1} Bottom:{2} Left:{3}}}",
                Top.ToString(currentCulture), Right.ToString(currentCulture),
                Bottom.ToString(currentCulture), Left.ToString(currentCulture));
        }

        public override int GetHashCode()
        {
            return Top.GetHashCode() + Right.GetHashCode() + Bottom.GetHashCode() + Left.GetHashCode();
        }

        public static bool operator ==(Margin a, Margin b)
        {
            if (a.Top == b.Top && a.Bottom == b.Bottom &&
                a.Left == b.Left && a.Right == b.Right)
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(Margin a, Margin b)
        {
            if (a.Top == b.Top && a.Bottom == b.Bottom &&
                a.Left == b.Left && a.Right == b.Right)
            {
                return false;
            }
            return true;
        }
    }
}
