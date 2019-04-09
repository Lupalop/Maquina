using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Maquina.Elements;
using System.Diagnostics.CodeAnalysis;

/*
 * Adapted from Progressbar Game Component
 * 2009 Luke Rymarz, www.lukerymarz.com
 */
namespace Maquina.UI
{
    /// <summary>
    /// This is a game component that implements IDisposable and inherits from GenericElement.
    /// </summary>
    public class ProgressBar : GuiElement, IDisposable
    {
        #region public members
        /// <summary>
        /// Minimum value of the progress bar.  Default is 0.0f.
        /// </summary>
        public float minimum
        {
            get
            {
                return m_minimum;
            }
            set
            {
                m_minimum = value;
                // causes progress to update, and rectangles to update
                this.value = m_progress;
            }
        }

        /// <summary>
        /// Maximum value of the progress bar.  Default is 100.0f.
        /// </summary>
        public float maximum
        {
            get
            {
                return m_maximum;
            }
            set
            {
                m_maximum = value;
                // causes progress to update, and rectangles to update
                this.value = m_progress;
            }
        }

        /// <summary>
        /// Current progress value.
        /// </summary>
        public float value
        {
            get
            {
                return m_progress;
            }
            set
            {
                m_progress = value;
                if (m_progress < m_minimum)
                    m_progress = m_minimum;
                else if (m_progress > m_maximum)
                    m_progress = m_maximum;
                UpdateRectangles();
            }
        }

        /// <summary>
        /// Outer border color.  Default is Gray.
        /// </summary>
        public Color borderColorOuter
        {
            get
            {
                return m_borderColorOuter;
            }
            set
            {
                if (m_borderColorOuter != value)
                {
                    m_borderColorOuter = value;
                    outerData[0] = m_borderColorOuter;
                    outerTexture = new Texture2D(Global.Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    outerTexture.SetData(outerData);
                }
            }
        }

        /// <summary>
        /// Outer border thickness.  This is drawn within the bounds of the progress bar.  Default is 3.
        /// </summary>
        public Int32 borderThicknessOuter
        {
            get
            {
                return m_borderThicknessOuter;
            }
            set
            {
                m_borderThicknessOuter = value;
            }
        }

        /// <summary>
        /// Inner border color.  For situations where you will have multiple colors behind the progress bar.  
        /// Set this to something complementary to borderColorOuter.  Default is Black
        /// </summary>
        public Color borderColorInner
        {
            get
            {
                return m_borderColorInner;
            }
            set
            {
                if (m_borderColorInner != value)
                {
                    m_borderColorInner = value;
                    innerData[0] = m_borderColorInner;
                    innerTexture = new Texture2D(Global.Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    innerTexture.SetData(innerData);
                }
            }
        }

        /// <summary>
        /// Inner border thickness.  This is drawn within the bounds of the progress bar.  Default is 2.
        /// </summary>
        public Int32 borderThicknessInner
        {
            get
            {
                return m_borderThicknessInner;
            }
            set
            {
                m_borderThicknessInner = value;
            }
        }

        /// <summary>
        /// Color of the progress section of the bar.  Default is Dark Blue.
        /// </summary>
        public Color fillColor
        {
            get
            {
                return m_fillColor;
            }
            set
            {
                if (m_fillColor != value)
                {
                    m_fillColor = value;
                    fillData[0] = m_fillColor;
                    fillTexture.Dispose();
                    fillTexture = new Texture2D(Global.Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    fillTexture.SetData(fillData);
                }
            }
        }

        /// <summary>
        /// Color of the background (unfilled) section of the progress bar.  Default is White.
        /// </summary>
        public Color backgroundColor
        {
            get
            {
                return m_backgroundColor;
            }
            set
            {
                if (m_backgroundColor != value)
                {
                    m_backgroundColor = value;
                    backgroundData[0] = m_backgroundColor;
                    backgroundTexture = new Texture2D(Global.Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    backgroundTexture.SetData(backgroundData);
                }
            }
        }

        public enum BarOrientation
        {
            HORIZONTAL_LR, // default, horizontal Orientation, left to right fill
            HORIZONTAL_RL, // horizontal Orientation, right to left fill
            VERTICAL_TB, // vertical Orientation, top to bottom fill
            VERTICAL_BT, // vertical Orientation, bottom to top fill
        }

        /// <summary>
        /// Gets the Orientation of this progress bar.  Set at creation time.
        /// </summary>
        public BarOrientation Orientation
        {
            get
            {
                return m_orientation;
            }
        }
        #endregion

        #region protected members
        protected float m_minimum = 0.0f;
        protected float m_maximum = 100.0f;
        protected float m_progress = 0;

        protected Rectangle m_borderOuterRect;
        protected Rectangle m_borderInnerRect;
        protected Rectangle m_backgroundRect;
        protected Rectangle m_fillRect;

        protected Color m_borderColorOuter;
        protected Int32 m_borderThicknessOuter;

        protected Color m_borderColorInner;
        protected Int32 m_borderThicknessInner;

        protected Color m_fillColor;
        protected Color m_backgroundColor;

        protected Color[] outerData;
        protected Color[] innerData;
        protected Color[] fillData;
        protected Color[] backgroundData;
        protected Texture2D outerTexture;
        protected Texture2D innerTexture;
        protected Texture2D backgroundTexture;
        protected Texture2D fillTexture;

        protected BarOrientation m_orientation;

        #endregion

        /// <summary>
        /// Construct a progress bar in the given rectangle with default HORIZONTAL_LR Orientation.
        /// </summary>
        /// <exFonts name="game">Your game.  Most likely, "this"</exFonts>
        /// <exFonts name="rect">The rectangle you wish the progress bar to occupy (includes borders).</exFonts>
        public ProgressBar(string ObjectName, Rectangle rect)
            : base (ObjectName)
        {
            m_borderOuterRect = rect;
            m_orientation = BarOrientation.HORIZONTAL_LR;

            Initialize();
        }

        /// <summary>
        /// Construct a progress bar in the given rectangle with the given Orientation
        /// </summary>
        /// <exFonts name="rect">The rectangle you wish the progress bar to occupy (includes borders).</exFonts>
        /// <exFonts name="Orientation">The Orientation of the progress bar.</exFonts>
        public ProgressBar(string ObjectName, Rectangle rect,
            BarOrientation orientation)
            : base (ObjectName)
        {
            m_borderOuterRect = rect;
            m_orientation = orientation;

            Initialize();
        }

        /// <summary>
        /// Construct a progress bar with x,y, width and height and with default HORIZONTAL_LR Orientation.
        /// </summary>
        /// <exFonts name="game">Your game.  Most likely, "this"</exFonts>
        /// <exFonts name="x">X-position of the top left corner of the progress bar.</exFonts>
        /// <exFonts name="y">Y-position of the top left corner of the progress bar.</exFonts>
        /// <exFonts name="width">Width of the progress bar.</exFonts>
        /// <exFonts name="height">Height of the progress bar.</exFonts>
        public ProgressBar(string ObjectName, Int32 x, Int32 y, Int32 width,
            Int32 height)
            : base (ObjectName)
        {
            m_borderOuterRect = new Rectangle(x, y, width, height);
            m_orientation = BarOrientation.HORIZONTAL_LR;

            Initialize();
        }

        /// <summary>
        /// Construct a progress bar with x,y, width and height and the given Orientation.
        /// </summary>
        /// <exFonts name="x">X-position of the top left corner of the progress bar.</exFonts>
        /// <exFonts name="y">Y-position of the top left corner of the progress bar.</exFonts>
        /// <exFonts name="width">Width of the progress bar.</exFonts>
        /// <exFonts name="height">Height of the progress bar.</exFonts>
        /// <exFonts name="Orientation">The Orientation of the progress bar.</exFonts>
        public ProgressBar(string ObjectName, Int32 x, Int32 y, Int32 width,
            Int32 height, BarOrientation orientation)
            : base (ObjectName)
        {
            m_borderOuterRect = new Rectangle(x, y, width, height);
            m_orientation = orientation;

            Initialize();
        }

        /// <summary>
        /// Initialize the Progress bar.  Called automatically from the constructor.
        /// </summary>
        public void Initialize()
        {
            // create some textures.  These will actually be overwritten when colors are set below.
            outerTexture = new Texture2D(Global.Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            innerTexture = new Texture2D(Global.Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            backgroundTexture = new Texture2D(Global.Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            fillTexture = new Texture2D(Global.Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);

            // initialize data arrays for building textures
            outerData = new Color[1];
            innerData = new Color[1];
            fillData = new Color[1];
            backgroundData = new Color[1];

            // initialize colors
            borderColorOuter = Color.Transparent;
            borderColorInner = Color.Transparent;
            fillColor = new Color(0, 0, 0, 150);
            backgroundColor = new Color(0, 0, 0, 50);

            // set border thickness
            m_borderThicknessInner = 0;
            m_borderThicknessOuter = 0;

            // calculate the rectangles for displaying the progress bar
            UpdateRectangles();
        }

        /// <summary>
        /// Calculates the rectangles for displaying the progress bar.  
        /// Assumes m_borderOuterRect is already initialized.
        /// </summary>
        protected void UpdateRectangles()
        {
            // figure out inner border
            m_borderInnerRect = m_borderOuterRect;
            m_borderInnerRect.Inflate(m_borderThicknessOuter * -1, m_borderThicknessOuter * -1);

            // figure out background rectangle
            m_backgroundRect = m_borderInnerRect;
            m_backgroundRect.Inflate(m_borderThicknessInner * -1, m_borderThicknessInner * -1);

            // figure out fill rectangle based on progress.
            m_fillRect = m_backgroundRect;
            float percentProgress = (m_progress - m_minimum) / (m_maximum - m_minimum);
            // calculate fill properly according to Orientation
            switch (m_orientation)
            {
                case BarOrientation.HORIZONTAL_LR:
                    m_fillRect.Width = (int)((float)m_fillRect.Width * percentProgress); break;
                case BarOrientation.HORIZONTAL_RL:
                    // right to left means short the fill rect as usual, but it must justified to the right
                    m_fillRect.Width = (int)((float)m_fillRect.Width * percentProgress);
                    m_fillRect.X = m_backgroundRect.Right - m_fillRect.Width;
                    break;
                case BarOrientation.VERTICAL_BT:
                    //justify the fill to the bottom
                    m_fillRect.Height = (int)((float)m_fillRect.Height * percentProgress);
                    m_fillRect.Y = m_backgroundRect.Bottom - m_fillRect.Height;
                    break;
                case BarOrientation.VERTICAL_TB:
                    m_fillRect.Height = (int)((float)m_fillRect.Height * percentProgress); break;
                default:// default is HORIZONTAL_LR
                    m_fillRect.Width = (int)((float)m_fillRect.Width * percentProgress); break;
            }

        }

        /// <summary>
        /// Draws the progress bar.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // draw the outer border
            SpriteBatch.Draw(outerTexture, m_borderOuterRect, null, Tint, Rotation, RotationOrigin, GraphicEffects, LayerDepth);
            // draw the inner border
            SpriteBatch.Draw(innerTexture, m_borderInnerRect, null, Tint, Rotation, RotationOrigin, GraphicEffects, LayerDepth);
            // draw the background color
            SpriteBatch.Draw(backgroundTexture, m_backgroundRect, null, Tint, Rotation, RotationOrigin, GraphicEffects, LayerDepth);
            // draw the progress
            SpriteBatch.Draw(fillTexture, m_fillRect, null, Tint, Rotation, RotationOrigin, GraphicEffects, LayerDepth);

            if (OnDraw != null)
            {
                OnDraw(this);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Bounds = m_borderOuterRect;
            if (OnUpdate != null)
            {
                OnUpdate(this);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                innerTexture.Dispose();
                outerTexture.Dispose();
                fillTexture.Dispose();
                backgroundTexture.Dispose();
            }
        }

        public override string ID
        {
            get { return "GUI_PROGRESSBAR"; }
        }
    }
}
