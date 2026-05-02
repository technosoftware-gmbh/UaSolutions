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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Technosoftware.ClientGateway;
#endregion Using Directives

namespace Technosoftware.Common
{
    /// <summary>
    /// Creates UserName.
    /// </summary>
    public class UserNameCreator
    {
        /// <summary>
        /// Triple DES Key
        /// </summary>
        private const string strKey = "h13h6m9F";

        /// <summary>
        /// Triple DES initialization vector
        /// </summary>
        private const string strIV = "Zse5";

        #region Constructors
        /// <summary>
        /// The default constructor.
        /// </summary>
        public UserNameCreator(string applicationName, ITelemetryContext telemetry)
        {
            m_telemetry = telemetry;
            m_logger = telemetry.CreateLogger<UserNameCreator>();
            m_UserNameIdentityTokens = LoadUserName(applicationName, m_logger);
        }
        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Add a User.
        /// </summary>
        /// <param name="applicationName">The Application Name.</param>
        /// <param name="userName">The UserName.</param>
        /// <param name="password">The Password.</param>
        public void Add(string applicationName, string userName, byte[] password)
        {
            lock (m_lock)
            {
                UserNameIdentityToken newUserNameToken = new UserNameIdentityToken()
                {
                    UserName = userName,
                    DecryptedPassword = password,
                };

                newUserNameToken.Password = newUserNameToken.DecryptedPassword;

                m_UserNameIdentityTokens.Add(newUserNameToken.UserName, newUserNameToken);

                SaveUserName(applicationName, newUserNameToken, m_logger);
            }
        }

        /// <summary>
        /// Delete a User.
        /// </summary>
        /// <param name="applicationName">The Application Name.</param>
        /// <param name="userName">The  UserName.</param>
        /// <returns>True if the item deleted from list.</returns>
        public bool Delete(string applicationName, string userName)
        {
            lock (m_lock)
            {
                string relativePath = Utils.Format("%CommonApplicationData%\\OPC Foundation\\Accounts\\{0}\\{1}.xml", applicationName, userName);
                string absolutePath = Utils.GetAbsoluteFilePath(relativePath, false, false, true);

                // oops - nothing found.
                if (absolutePath == null)
                {
                    absolutePath = Utils.GetAbsoluteFilePath(relativePath, true, false, true);
                }

                if (File.Exists(absolutePath))
                {   // delete a file.
                    File.Delete(absolutePath);
                }

                return m_UserNameIdentityTokens.Remove(userName);
            }
        }

        /// <summary>
        /// Load UserNameIdentityToken.
        /// </summary>
        /// <returns>UserNameIdentityToken list.</returns>
        public static Dictionary<string, UserNameIdentityToken> LoadUserName(string applicationName, ILogger logger)
        {
            Dictionary<string, UserNameIdentityToken> resultTokens = new Dictionary<string, UserNameIdentityToken>();

            try
            {
                string relativePath = Utils.Format("%CommonApplicationData%\\OPC Foundation\\Accounts\\{0}", applicationName);
                string absolutePath = Utils.GetAbsoluteDirectoryPath(relativePath, false, false, false);

                if (string.IsNullOrEmpty(absolutePath))
                {
                    return resultTokens;
                }

                foreach (string filePath in Directory.GetFiles(absolutePath))
                {
                    // oops - nothing found.
                    if (filePath == null)
                    {
                        continue;
                    }

                    // open the file.
                    using (FileStream istrm = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        TextReader tr = new StreamReader(istrm);

                        using (XmlReader reader = XmlReader.Create(tr, new XmlReaderSettings() { XmlResolver = null }))
                        {
                            DataContractSerializer serializer = new DataContractSerializer(typeof(UserNameIdentityToken));
                            UserNameIdentityToken userNameToken = (UserNameIdentityToken)serializer.ReadObject(reader, false);

                            if (userNameToken.UserName == null || userNameToken.Password == null)
                            {  // The configuration file has problem.
                                logger.LogError("Unexpected error saving user configuration for COM Wrapper.");
                                continue;
                            }

                            if (resultTokens.ContainsKey(userNameToken.UserName))
                            {   // When I already exist, I ignore it.
                                logger.LogInformation("When I already exist, I ignore it. UserName={0}", userNameToken.UserName);
                                continue;
                            }

                            userNameToken.Password = DecryptPassword(userNameToken.Password);
                            userNameToken.DecryptedPassword = userNameToken.Password;

                            resultTokens.Add(userNameToken.UserName, userNameToken);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unexpected error saving user configuration for COM Wrapper.");
            }

            return resultTokens;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Save UserNameIdentityToken.
        /// </summary>
        private static void SaveUserName(string applicationName, UserNameIdentityToken userNameToken, ILogger logger)
        {
            try
            {
                string relativePath = Utils.Format("%CommonApplicationData%\\OPC Foundation\\Accounts\\{0}\\{1}.xml", applicationName, userNameToken.UserName);
                string absolutePath = Utils.GetAbsoluteFilePath(relativePath, false, false, true);

                // oops - nothing found.
                if (absolutePath == null)
                {
                    absolutePath = Utils.GetAbsoluteFilePath(relativePath, true, false, true);
                }

                UserNameIdentityToken outputToken = new UserNameIdentityToken()
                {
                    UserName = userNameToken.UserName,
                    Password = EncryptPassword(userNameToken.Password),
                    EncryptionAlgorithm = "Triple DES",
                };

                // open the file.
                FileStream ostrm = File.Open(absolutePath, FileMode.Create, FileAccess.ReadWrite);

                using (XmlTextWriter writer = new XmlTextWriter(ostrm, System.Text.Encoding.UTF8))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(UserNameIdentityToken));
                    serializer.WriteObject(writer, outputToken);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unexpected error saving user configuration for COM Wrapper with UserName={0}.", userNameToken.UserName);
            }
        }

        /// <summary>
        /// Encrypt Password.
        /// </summary>
        /// <param name="srcPassword">The Source Password.</param>
        /// <returns>Encrypted Password.</returns>
        private static byte[] EncryptPassword(byte[] srcPassword)
        {
            byte[] encryptedPassword;
            TripleDESCryptoServiceProvider tdes; // Triple DES service provider
            MemoryStream outStream = null;
            CryptoStream encStream = null;
            string dst = string.Empty;

            // Create Triple DES service provider.
            tdes = new TripleDESCryptoServiceProvider();
            // Get encrypt key and initialization vector.
            byte[] key = Encoding.Unicode.GetBytes(strKey);
            byte[] IV = Encoding.Unicode.GetBytes(strIV);

            // Create result stream and encrypt stream.
            using (outStream = new MemoryStream())
            using (encStream = new CryptoStream(outStream, tdes.CreateEncryptor(key, IV), CryptoStreamMode.Write))
            {
                // Encrypt
                encStream.Write(srcPassword, 0, srcPassword.Length);
                encStream.Close();
                encryptedPassword = outStream.ToArray();
            }

            return encryptedPassword;
        }

        /// <summary>
        /// Decrypt Password.
        /// </summary>
        /// <param name="srcPassword">The Source Password.</param>
        /// <returns>Decrypted Password.</returns>
        private static byte[] DecryptPassword(byte[] srcPassword)
        {
            byte[] decryptedPassword;
            TripleDESCryptoServiceProvider tdes; // Triple DES service provider
            MemoryStream outStream = null;
            CryptoStream decStream = null;
            string dst = string.Empty;

            // Create Triple DES service provider.
            tdes = new TripleDESCryptoServiceProvider();
            // Get encrypt key and initialization vector.
            byte[] key = Encoding.Unicode.GetBytes(strKey);
            byte[] IV = Encoding.Unicode.GetBytes(strIV);

            // Create result stream and decrypt stream.
            using (outStream = new MemoryStream())
            using (decStream = new CryptoStream(outStream, tdes.CreateDecryptor(key, IV), CryptoStreamMode.Write))
            {
                // Decrypt
                decStream.Write(srcPassword, 0, srcPassword.Length);
                decStream.Close();
                decryptedPassword = outStream.ToArray();
            }

            return decryptedPassword;
        }

        #endregion Private Methods

        #region Private Fields
        private object m_lock = new object();
        private Dictionary<string, UserNameIdentityToken> m_UserNameIdentityTokens = new Dictionary<string, UserNameIdentityToken>();
        private readonly ILogger m_logger;
        private readonly ITelemetryContext m_telemetry;
        #endregion Private Fields
    }
}
