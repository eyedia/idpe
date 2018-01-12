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
using System.Collections.Generic;
using System.Configuration;

namespace Eyedia.Core.Encryption
{    

    public class ConfigEncryptor
    {
        const string configProtectionProvider = "SymplusRsaProtectedConfigurationProvider";
        public static void EncryptAppConfigSections(string exePath, string commaSeparatedSections)
        {            
            List<string> sectionNames = new List<string>(commaSeparatedSections.Split(",".ToCharArray()));
            Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
            SectionInformation sectionInformation = null;
            foreach (string sectionName in sectionNames)
            {
                sectionInformation = config.GetSection(sectionName.Trim()).SectionInformation;
                if (sectionInformation == null) continue;
                if (!sectionInformation.IsProtected)
                {                    
                    sectionInformation.ProtectSection(configProtectionProvider);
                    sectionInformation.ForceSave = true;
                }
            }
            config.Save(ConfigurationSaveMode.Full);
        }

        public static void DecryptAppConfigSections(string exePath, string commaSeparatedSections)
        {            
            List<string> sectionNames = new List<string>(commaSeparatedSections.Split(",".ToCharArray()));
            Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
            SectionInformation sectionInformation = null;
            
            foreach (string sectionName in sectionNames)
            {
                sectionInformation = config.GetSection(sectionName.Trim()).SectionInformation;
                if (sectionInformation == null) continue;
                if (sectionInformation.IsProtected)
                {
                    sectionInformation.UnprotectSection();
                    sectionInformation.ForceSave = true;
                }
            }
            config.Save(ConfigurationSaveMode.Full);
        }

        public static void CreateKey(string keyPath)
        {
            // Instantiate the custom provider.
            RsaProtectedConfigurationProvider
                provider =
                new RsaProtectedConfigurationProvider();

            // Create the encryption/decryption key.
            provider.CreateKey(keyPath);


        }

    }
}





