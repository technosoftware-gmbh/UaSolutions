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
#endregion

namespace SampleCompany.Common
{
    /// <summary>
    /// The error code why the application exit.
    /// </summary>
    public enum ExitCode : int
    {
        Ok = 0,
        ErrorNotStarted = 0x80,
        ErrorRunning = 0x81,
        ErrorException = 0x82,
        ErrorStopping = 0x83,
        ErrorCertificate = 0x84,
        ErrorInvalidCommandLine = 0x100
    };

    /// <summary>
    /// An exception that occured and caused an exit of the application.
    /// </summary>
    public class ErrorExitException : Exception
    {
        public ExitCode ExitCode { get; }

        public ErrorExitException(ExitCode exitCode)
        {
            ExitCode = exitCode;
        }

        public ErrorExitException()
        {
            ExitCode = ExitCode.Ok;
        }

        public ErrorExitException(string message) : base(message)
        {
            ExitCode = ExitCode.Ok;
        }

        public ErrorExitException(string message, ExitCode exitCode) : base(message)
        {
            ExitCode = exitCode;
        }

        public ErrorExitException(string message, Exception innerException) : base(message, innerException)
        {
            ExitCode = ExitCode.Ok;
        }

        public ErrorExitException(string message, Exception innerException, ExitCode exitCode) : base(message, innerException)
        {
            ExitCode = exitCode;
        }
    }
}

