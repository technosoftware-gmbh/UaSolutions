#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on https://github.com/junian/Standard.Licensing. 
// The complete license agreement for that can be found in this directore in the LICENSE.txt file.
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Timers;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    internal class EvaluationTimer : IDisposable
    {
        #region Constructors, Destructor, Initialization

        // Removed static constructor per CA1810. All static fields are initialized inline.

        /// <summary>
        /// The finalizer implementation.
        /// </summary>
        ~EvaluationTimer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Implements <see cref="IDisposable.Dispose"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // Take yourself off the Finalization queue
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!m_disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing && s_expireTimer != null)
                {
                    s_expireTimer.Dispose();
                }
                // Release unmanaged resources. If disposing is false,
                // only the following code is executed.
            }
            m_disposed = true;
        }
        #endregion Constructors, Destructor, Initialization

        #region Properties
        /// <summary>
        /// True if the evaluation period has expired.
        /// </summary>
        public bool IsExpired
        {
            get
            {
                if (!s_isExpired)
                {
                    TimeSpan timeSpan = DateTime.Now - s_time;
                    if (timeSpan > s_timeSpan)
                    {
                        s_isExpired = true;
                        s_expireTimer.Enabled = false;
                        return true;
                    }
                }
                return s_isExpired;
            }
        }
        #endregion Properties

        #region Internal Methods
        internal static void Start(int runtimePeriod = 0)
        {
            if (runtimePeriod > 0)
            {
                s_timeSpan = new TimeSpan(0, 0, runtimePeriod, 0, 0);
            }
            s_expireTimer = new Timer(60000); // 60,000 ms = 1 minute
            s_expireTimer.Elapsed += TimerElapsed;
            s_expireTimer.AutoReset = true;
            s_expireTimer.Enabled = true;
        }

        internal static void Stop()
        {
            s_expireTimer.Stop();
        }
        #endregion Internal Methods

        #region Private Methods
        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!s_isExpired)
            {
                TimeSpan timeSpan = DateTime.Now - s_time;
                if (timeSpan > s_timeSpan)
                {
                    s_isExpired = true;
                    s_expireTimer.Enabled = false;
                }
            }
        }
        #endregion Private Methods

        #region Private Fields
        private static readonly DateTime s_time = DateTime.Now;
        private static TimeSpan s_timeSpan = new(0, 2, 00, 0, 0);
        private static bool s_isExpired;
        private static Timer s_expireTimer;
        private bool m_disposed;
        #endregion Private Fields
    }
}
