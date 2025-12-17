#region Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is subject to the Technosoftware GmbH Software License 
// Agreement, which can be found here:
// https://technosoftware.com/documents/Source_License_Agreement.pdf
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
#endregion

namespace Technosoftware.UaServer.UserDatabase
{
    /// <summary>
    /// An abstract interface to the user database which stores logins with associated roles.
    /// </summary>
    public interface IUaUserDatabase
    {
        /// <summary>
        /// Create or update user password or roles.
        /// </summary>
        /// <param name="userName">The username</param>
        /// <param name="password">The password</param>
        /// <param name="roles">The roles assigned to the new user</param>
        /// <returns>true if registration was successful</returns>
        bool CreateUser(string userName, ReadOnlySpan<byte> password, ICollection<Role> roles);

        /// <summary>
        /// Delete existing user.
        /// </summary>
        /// <param name="userName">The user to delete</param>
        /// <returns>true if successfully removed.</returns>
        bool DeleteUser(string userName);

        /// <summary>
        /// Checks if the provided user credentials pass for a user.
        /// </summary>
        /// <param name="userName">The username</param>
        /// <param name="password">The password</param>
        /// <returns>true if userName + PW combination is correct.</returns>
        bool CheckCredentials(string userName, ReadOnlySpan<byte> password);

        /// <summary>
        /// Returns the roles assigned to the user.
        /// </summary>
        /// <param name="userName">The username</param>
        /// <returns>the Role of the provided users</returns>
        /// <exception cref="ArgumentException">When the user is not found</exception>
        ICollection<Role> GetUserRoles(string userName);

        /// <summary>
        /// Changes the password of an existing users.
        /// </summary>
        /// <returns>true if change was successfull</returns>
        bool ChangePassword(
            string userName,
            ReadOnlySpan<byte> oldPassword,
            ReadOnlySpan<byte> newPassword);
    }
}
