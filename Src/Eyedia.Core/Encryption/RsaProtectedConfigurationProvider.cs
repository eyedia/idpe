#region Copyright Notice
/* Copyright (c) 2017, Deb'jyoti Das - debjyoti@debjyoti.com
 All rights reserved.
 Redistribution and use in source and binary forms, with or without
 modification, are not permitted.Neither the name of the 
 'Deb'jyoti Das' nor the names of its contributors may be used 
 to endorse or promote products derived from this software without 
 specific prior written permission.
 THIS SOFTWARE IS PROVIDED BY Deb'jyoti Das 'AS IS' AND ANY
 EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED. IN NO EVENT SHALL Debjyoti OR Deb'jyoti OR Debojyoti Das OR Eyedia BE LIABLE FOR ANY
 DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#region Developer Information
/*
Author  - Deb'jyoti Das
Created - 3/19/2013 11:14:16 AM
Description  - 
Modified By - 
Description  - 
*/
#endregion Developer Information

#endregion Copyright Notice


using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Security.Cryptography;
using System.Collections.Specialized;

namespace Eyedia.Core.Encryption
{
    public class RsaProtectedConfigurationProvider : ProtectedConfigurationProvider
    {
        private TripleDESCryptoServiceProvider des =
           new TripleDESCryptoServiceProvider();

        private string pKeyFilePath;
        private string pName;

        // Gets the path of the file 
        // containing the key used to 
        // encryption or decryption. 
        public string KeyFilePath
        {
            get { return pKeyFilePath; }
        }


        // Gets the provider name. 
        public override string Name
        {
            get { return pName; }
        }


        // Performs provider initialization. 
        public override void Initialize(string name,
            NameValueCollection config)
        {
            pName = name;
            pKeyFilePath = config["keyContainerName"];
            string onlyFileName = Path.GetFileName(pKeyFilePath);
            if (pKeyFilePath == onlyFileName)
                pKeyFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, onlyFileName);
                
            ReadKey(KeyFilePath);
        }


        // Performs encryption. 
        public override XmlNode Encrypt(XmlNode node)
        {
            string encryptedData = EncryptString(node.OuterXml);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.LoadXml("<EncryptedData>" +
                encryptedData + "</EncryptedData>");

            return xmlDoc.DocumentElement;
        }

        // Performs decryption. 
        public override XmlNode Decrypt(XmlNode encryptedNode)
        {
            string decryptedData =
                DecryptString(encryptedNode.InnerText);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.LoadXml(decryptedData);

            return xmlDoc.DocumentElement;
        }

        // Encrypts a configuration section and returns  
        // the encrypted XML as a string. 
        private string EncryptString(string encryptValue)
        {
            byte[] valBytes =
                Encoding.Unicode.GetBytes(encryptValue);

            ICryptoTransform transform = des.CreateEncryptor();

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,
                transform, CryptoStreamMode.Write);
            cs.Write(valBytes, 0, valBytes.Length);
            cs.FlushFinalBlock();
            byte[] returnBytes = ms.ToArray();
            cs.Close();

            return Convert.ToBase64String(returnBytes);
        }


        // Decrypts an encrypted configuration section and  
        // returns the unencrypted XML as a string. 
        private string DecryptString(string encryptedValue)
        {
            byte[] valBytes =
                Convert.FromBase64String(encryptedValue);

            ICryptoTransform transform = des.CreateDecryptor();

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,
                transform, CryptoStreamMode.Write);
            cs.Write(valBytes, 0, valBytes.Length);
            cs.FlushFinalBlock();
            byte[] returnBytes = ms.ToArray();
            cs.Close();

            return Encoding.Unicode.GetString(returnBytes);
        }

        // Generates a new TripleDES key and vector and  
        // writes them to the supplied file path. 
        public void CreateKey(string filePath)
        {
            des.GenerateKey();
            des.GenerateIV();

            StreamWriter sw = new StreamWriter(filePath, false);
            sw.WriteLine(ByteToHex(des.Key));
            sw.WriteLine(ByteToHex(des.IV));
            sw.Close();
        }


        // Reads in the TripleDES key and vector from  
        // the supplied file path and sets the Key  
        // and IV properties of the  
        // TripleDESCryptoServiceProvider. 
        private void ReadKey(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            string keyValue = sr.ReadLine();
            string ivValue = sr.ReadLine();
            des.Key = HexToByte(keyValue);
            des.IV = HexToByte(ivValue);
        }


        // Converts a byte array to a hexadecimal string. 
        private string ByteToHex(byte[] byteArray)
        {
            string outString = "";

            foreach (Byte b in byteArray)
                outString += b.ToString("X2");

            return outString;
        }

        // Converts a hexadecimal string to a byte array. 
        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] =
                    Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

    }
}



