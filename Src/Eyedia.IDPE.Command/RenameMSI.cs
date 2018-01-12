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
using System.Text;
using System.IO;
using WindowsInstaller;

namespace Eyedia.IDPE.Command
{
    public class RenameMSI
    {
        public static void Rename(string inputFile, string buildConfiguration)
        {   
            string productName = "[ProductName]";
            try
            {
                string version;

                if (inputFile.EndsWith(".msi", StringComparison.OrdinalIgnoreCase))
                {
                    // Read the MSI property
                    version = GetMsiProperty(inputFile, "ProductVersion");
                    productName = GetMsiProperty(inputFile, "ProductName");
                }
                else
                {
                    return;
                }
                string dir = Path.GetDirectoryName(inputFile);
                productName = productName.Replace(" ", "_");
                string dtTm = DateTime.Now.ToString().Replace("/", ".").Replace(":", ".").Replace(" ", "."); ;
                string newFileName = Path.Combine(dir, string.Format("{0}_{1}_{2}_{3}.msi", productName, version, buildConfiguration, dtTm));

                File.Copy(inputFile, newFileName);
                File.Delete(inputFile);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        static string GetMsiProperty(string msiFile, string property)
        {
            string retVal = string.Empty;

            // Create an Installer instance  
            Type classType = Type.GetTypeFromProgID("WindowsInstaller.Installer");
            Object installerObj = Activator.CreateInstance(classType);
            Installer installer = installerObj as Installer;

            // Open the msi file for reading  
            // 0 - Read, 1 - Read/Write  
            Database database = installer.OpenDatabase(msiFile, 0);

            // Fetch the requested property  
            string sql = String.Format(
                "SELECT Value FROM Property WHERE Property='{0}'", property);
            View view = database.OpenView(sql);
            view.Execute(null);

            // Read in the fetched record  
            Record record = view.Fetch();
            if (record != null)
            {
                retVal = record.get_StringData(1);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(record);
            }
            view.Close();
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(view);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(database);

            return retVal;
        }
    }
}




