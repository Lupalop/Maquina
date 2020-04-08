using System;
using Microsoft.Xna.Framework;

namespace Maquina
{
    /// <summary>
    /// Generates an event after a set interval, with an option to generate recurring events.
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Maquina.Timer" />
        /// class, and sets all the properties to their initial values.
        /// </summary>
        public Timer()
        {
            Interval = 100.0;
            Enabled = false;
            AutoReset = true;
            TimerManager.Add(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Maquina.Timer" />
        /// class, and sets the <see cref="P:Maquina.Timer.Interval" />
        /// property to the specified number of milliseconds.
        /// </summary>
        /// <param name="interval">
        /// The time, in milliseconds, between events.
        /// The value must be greater than zero and less than or equal to
        /// <see cref="F:System.Int32.MaxValue" />.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        /// The value of the <paramref name="interval" /> parameter is less
        /// than or equal to zero, or greater than
        /// <see cref="F:System.Int32.MaxValue" />.
        /// </exception>
        public Timer(double interval) : this()
        {
            double num = Math.Ceiling(interval);
            if (num > Int32.MaxValue || num <= 0.0)
            {
                throw new ArgumentException();
            }
            Interval = interval;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the
        /// <see cref="T:Maquina.Timer" /> should raise the
        /// <see cref="E:Maquina.Timer.Elapsed" /> event each time the
        /// specified interval elapses or only after the first time it elapses.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:Maquina.Timer" /> should raise the
        /// <see cref="E:Maquina.Timer.Elapsed" /> event each time the
        /// interval elapses; false if it should raise the
        /// <see cref="E:Maquina.Timer.Elapsed" /> event only once, after
        /// the first time the interval elapses. The default is true.
        /// </returns>
        public bool AutoReset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the
        /// <see cref="T:Maquina.Timer" /> should raise the
        /// <see cref="E:Maquina.Timer.Elapsed" /> event.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:Maquina.Timer" /> should raise
        /// the <see cref="E:Maquina.Timer.Elapsed" /> event; otherwise,
        /// false. The default is false.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// The <see cref="P:Maquina.Timer.Interval" /> property was set
        /// to a value greater than <see cref="F:System.Int32.MaxValue" />
        /// before the timer was enabled.
        /// </exception>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the interval at which to raise the
        /// <see cref="E:Maquina.Timer.Elapsed" /> event.
        /// </summary>
        /// <returns>
        /// The time, in milliseconds, between
        /// <see cref="E:Maquina.Timer.Elapsed" /> events. The value must
        /// be greater than zero, and less than or equal to
        /// <see cref="F:System.Int32.MaxValue" />. The default is 100 milliseconds.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// The interval is less than or equal to zero.
        /// -or-
        /// The interval is greater than 
        /// <see cref="F:System.Int32.MaxValue" />, and the timer is currently
        /// enabled.
        /// (If the timer is not currently enabled, no exception is thrown
        /// until it becomes enabled.)
        /// </exception>
        public double Interval
        {
            get
            {
                return interval;
            }
            set
            {
                if (value <= 0.0)
                {
                    throw new ArgumentException();
                }
                interval = value;
            }
        }

        /// <summary>
        /// Occurs when the interval elapses.
        /// </summary>
        public event EventHandler Elapsed;

        private double interval;
        private double timeElapsed;

        public void Update()
        {
            if (!Enabled)
            {
                return;
            }

            timeElapsed += Application.GameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeElapsed >= Interval)
            {
                Elapsed(this, EventArgs.Empty);
                if (AutoReset)
                {
                    Reset();
                }
                else
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// Starts raising the <see cref="E:Maquina.Timer.Elapsed" />
        /// event by setting <see cref="P:Maquina.Timer.Enabled" /> to true.
        /// </summary>
        public void Start()
        {
            Enabled = true;
        }

        /// <summary>
        /// Stops raising the <see cref="E:Maquina.Timer.Elapsed" />
        /// event by setting <see cref="P:Maquina.Timer.Enabled" /> to false.
        /// </summary>
        public void Stop()
        {
            Enabled = false;
        }

        /// <summary>
        /// Resets the timer's elapsed time to 0.
        /// </summary>
        public void Reset()
        {
            timeElapsed = 0;
        }

        /// <summary>
        /// Releases the resources used by the <see cref="T:Maquina.Timer" />.
        /// </summary>
        public void Close()
        {
            TimerManager.Remove(this);
        }
    }
}
