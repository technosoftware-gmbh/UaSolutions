#region Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Technosoftware.UaConfiguration;
#endregion Using Directives

namespace SampleCompany.Common
{
    /// <summary>
    /// A dialog which asks for user input.
    /// </summary>
    public class ApplicationMessageDlg : IUaApplicationMessageDlg
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Generic Message Dialog for issues during loading of an application
        /// </summary>
        /// <param name="output">The TextWriter to use for the output</param>
        public ApplicationMessageDlg(TextWriter output)
        {
            m_output = output;
        }
        #endregion Constructors, Destructor, Initialization

        #region Overridden Methods
        /// <inheritdoc/>
        public override void Message(string text, bool ask = false)
        {
            m_message = text;
            m_ask = ask;
        }

        /// <inheritdoc/>
        public override async Task<bool> ShowAsync()
        {
            if (m_ask)
            {
                var message = new StringBuilder(m_message);
                _ = message.Append(" (y/n, default y): ");
                await m_output.WriteAsync(message.ToString()).ConfigureAwait(false);

                try
                {
                    ConsoleKeyInfo result = Console.ReadKey();
                    await m_output.WriteLineAsync().ConfigureAwait(false);
                    return await Task.FromResult(result.KeyChar is 'y' or
                        'Y' or '\r').ConfigureAwait(false);
                }
                catch
                {
                    // intentionally fall through
                }
            }
            else
            {
                await m_output.WriteLineAsync(m_message).ConfigureAwait(false);
            }

            return await Task.FromResult(true).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public override bool Show()
        {
            return ShowAsync().GetAwaiter().GetResult();
        }
        #endregion Overridden Methods

        #region Private Fields
        private readonly TextWriter m_output;
        private string m_message = string.Empty;
        private bool m_ask;
        #endregion Private Fields
    }
}

