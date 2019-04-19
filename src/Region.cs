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
    public struct Region : IEquatable<Region>
    {
        private int top;
        public int Top
        {
            get
            {
                return top;
            }
            set
            {
                top = value;
            }
        }
        private int right;
        public int Right
        {
            get
            {
                return right;
            }
            set
            {
                right = value;
            }
        }
        private int bottom;
        public int Bottom
        {
            get
            {
                return bottom;
            }
            set
            {
                bottom = value;
            }
        }
        private int left;
        public int Left
        {
            get
            {
                return left;
            }
            set
            {
                left = value;
            }
        }

        public Region(int top, int right, int bottom, int left)
        {
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.left = left;
        }

        public Region(Point height, Point width)
        {
            this.top = height.X;
            this.bottom = height.Y;
            this.right = width.X;
            this.left = width.Y;
        }

        public static Region Empty
        {
            get
            {
                return default(Region);
            }
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

        public bool Equals(Region other)
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
            if (obj is Region)
            {
                result = Equals((Region)obj);
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

        public static bool operator ==(Region a, Region b)
        {
            if (a.Top == b.Top && a.Bottom == b.Bottom &&
                a.Left == b.Left && a.Right == b.Right)
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(Region a, Region b)
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
