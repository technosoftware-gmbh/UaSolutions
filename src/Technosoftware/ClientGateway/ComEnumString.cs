#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
// Web: http://www.technosoftware.com
//
// The Software is based on the OPC Foundation’s software and is subject to 
// the OPC Foundation MIT License 1.00, which can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//
// The Software is subject to the Technosoftware GmbH Software License Agreement,
// which can be found here:
// https://technosoftware.com/license-agreement/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved


#region Using Directives

using System;
using System.Runtime.InteropServices;

using Technosoftware.Common;
using Technosoftware.Rcw;

#endregion Using Directives

namespace Technosoftware.ClientGateway
{
    /// <summary>
    /// A wrapper for the COM IEnumString interface.
    /// </summary>
    /// <exclude />
    internal class EnumString : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes the object with an enumerator.
        /// </summary>
        public EnumString(object enumerator, int bufferLength)
        {
            m_enumerator = enumerator as IEnumString;

            if (m_enumerator == null)
            {
                throw new ArgumentNullException("enumerator", "Object does not support IEnumString interface.");
            }

            m_buffer = new string[bufferLength];
            m_position = 0;
            m_fetched = 0;
        }
        #endregion Constructors

        #region IDisposable Members
        /// <summary>
        /// The finializer implementation.
        /// </summary>
        ~EnumString()
        {
            Dispose(false);
        }

        /// <summary>
        /// Frees any unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            object enumerator = System.Threading.Interlocked.Exchange(ref m_enumerator, null);

            if (enumerator != null)
            {
                ComUtils.ReleaseServer(enumerator);
            }
        }
        #endregion IDisposable Members

        #region IEnumString Members
        /// <summary>
        /// Fetches the next string (returns null if no more data).
        /// </summary>
        public string Next()
        {
            // return null if at end of list.
            if (m_fetched == -1)
            {
                return null;
            }

            // see if next item is available.
            if (m_position < m_fetched)
            {
                return m_buffer[m_position++];
            }

            m_position = 0;

            try
            {
                // fetch next batch.
                m_fetched = 0;

                IntPtr pBuffer = Marshal.AllocCoTaskMem(IntPtr.Size * m_buffer.Length);

                try
                {
                    int error = m_enumerator.RemoteNext(m_buffer.Length, pBuffer, out m_fetched);

                    if (error < 0 || m_fetched == 0)
                    {
                        return null;
                    }

                    IntPtr[] pStrings = new IntPtr[m_fetched];
                    Marshal.Copy(pBuffer, pStrings, 0, m_fetched);

                    for (int ii = 0; ii < m_fetched; ii++)
                    {
                        m_buffer[ii] = Marshal.PtrToStringUni(pStrings[ii]);
                        Marshal.FreeCoTaskMem(pStrings[ii]);
                    }
                }
                finally
                {
                    Marshal.FreeCoTaskMem(pBuffer);
                }

                // check if end of list.
                if (m_fetched == 0)
                {
                    m_fetched = -1;
                    return null;
                }

                // return first item.
                return m_buffer[m_position++];
            }
            catch (Exception e)
            {
                if (ComUtils.IsUnknownError(e, ResultIds.E_FAIL))
                {
                    throw ComUtils.CreateException(e, "IEnumString.RemoteNext");
                }

                // some (incorrect) implementations return E_FAIL at the end of the list.
                m_fetched = -1;
                return null;
            }
        }

        /// <summary>
        /// Fetches the next group of strings. 
        /// </summary>
        public int Next(string[] buffer, int count)
        {
            // can't use simple interface after calling this method.
            m_fetched = -1;

            try
            {
                int fetched = 0;

                IntPtr pBuffer = Marshal.AllocCoTaskMem(IntPtr.Size * count);

                try
                {
                    int error = m_enumerator.RemoteNext(
                        count,
                        pBuffer,
                        out fetched);

                    if (error >= 0 && fetched > 0)
                    {
                        IntPtr[] pStrings = new IntPtr[m_fetched];
                        Marshal.Copy(pBuffer, pStrings, 0, fetched);

                        for (int ii = 0; ii < fetched; ii++)
                        {
                            m_buffer[ii] = Marshal.PtrToStringUni(pStrings[ii]);
                            Marshal.FreeCoTaskMem(pStrings[ii]);
                        }
                    }

                    return fetched;
                }
                finally
                {
                    Marshal.FreeCoTaskMem(pBuffer);
                }
            }
            catch (Exception e)
            {
                // some (incorrect) implementations return E_FAIL at the end of the list.
                if (Marshal.GetHRForException(e) == ResultIds.E_FAIL)
                {
                    return 0;
                }

                throw ComUtils.CreateException(e, "IEnumString.RemoteNext");
            }
        }

        /// <summary>
        /// Skips a number of strings.
        /// </summary>
        public void Skip(int count)
        {
            m_enumerator.Skip(count);

            // can't use simple interface after calling this method.
            m_fetched = -1;
        }

        /// <summary>
        /// Sets pointer to the start of the list.
        /// </summary>
        public void Reset()
        {
            m_enumerator.Reset();

            // can't use simple interface after calling this method.
            m_fetched = -1;
        }

        /// <summary>
        /// Clones the enumerator.
        /// </summary>
        public EnumString Clone()
        {
            IEnumString enumerator = null;
            m_enumerator.Clone(out enumerator);
            return new EnumString(enumerator, m_buffer.Length);
        }
        #endregion IEnumString Members

        #region Private Members
        private Technosoftware.Rcw.IEnumString m_enumerator;
        private string[] m_buffer;
        private int m_position;
        private int m_fetched;
        #endregion Private Members
    }
}
