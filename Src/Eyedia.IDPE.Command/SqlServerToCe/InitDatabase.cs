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
using System.IO;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Command
{
    public class InitDatabase
    {
        public void Initialize(string folder)
        {
            List<string> idpeps = new List<string>(Directory.GetFiles(folder, "*.idpep"));
            foreach (string idpep in idpeps)
            {
                DataSourcePatch dsp = new DataSourcePatch(idpep);
                dsp.Import();
                Console.WriteLine("{0} imported.", Path.GetFileName(idpep));
            }

            string[] systemDataSources = Directory.GetFiles(folder, "*system*.idpex");

            DataManager.Manager manager = new DataManager.Manager();
            foreach (string systemDataSource in systemDataSources)
            {
                DataSourceBundle dsb = new DataSourceBundle(systemDataSource);
                if (manager.ApplicationExists(dsb.DataSource.Name))
                {
                    Console.WriteLine("{0} already exists, skipped.", Path.GetFileName(systemDataSource));
                    continue;
                }

                dsb.Import();
                Console.WriteLine("{0} imported.", Path.GetFileName(systemDataSource));
            }

            List<string> dataSources = new List<string>(Directory.GetFiles(folder, "*.idpex"));
            dataSources.RemoveAll(f => f.Contains("System"));

            foreach (string dataSource in dataSources)
            {
                DataSourceBundle dsb = new DataSourceBundle(dataSource);
                if (manager.ApplicationExists(dsb.DataSource.Name))
                {
                    Console.WriteLine("{0} already exists, skipped.", Path.GetFileName(dataSource));
                    continue;
                }

                dsb.Import();
                Console.WriteLine("{0} imported.", Path.GetFileName(dataSource));
            }
        }
    }
}
