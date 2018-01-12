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
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Services
{
    public class EnvironmentServiceDispatcherFactory
    {
        public static EnvironmentServiceDispatcher GetInstance(bool wcfClient)
        {
            //SreEnvironments envs = SreEnvironmentServiceDispatcherFactory.GetEnvironments();
            if (wcfClient)
                return new EnvironmentServiceDispatcherUsingWcf();
            else
                return new EnvironmentServiceDispatcherUsingFileSystem();
        }

        public static SreEnvironment GetEnvironmentLocal()
        {
            List<SreEnvironment> envs = GetEnvironments();
            return envs.Where(e => e.Name == "<local>").SingleOrDefault();
        }

        public static SreEnvironments GetEnvironments()
        {
            SreEnvironments envs = new SreEnvironments();
            SreKey key = new Manager().GetKey(SreKeyTypes.Environments);
            if (key != null)
            {
                try
                {
                    //envs = key.Value.Deserialize<SreEnvironments>();
                    envs = GZipArchive.Decompress(key.ValueBinary.ToArray()).GetString().Deserialize<SreEnvironments>();
                    if (envs.Count ==0)
                    {
                        envs = new SreEnvironments(true);
                    }
                    foreach(SreEnvironment env in envs)
                    {
                        foreach(SreEnvironmentConfig config in env.EnvironmentConfigs)
                        {
                            config.Environment = env;
                        }
                    }
                }
                catch
                {
                    envs = new SreEnvironments(true);
                    SaveEnvironment(envs);
                    return envs;
                }
            }
            else
            {
                envs = new SreEnvironments(true);
            }

            return envs;
        }

        public static void SaveEnvironment(SreEnvironments envs)
        {
            SreKey key = new SreKey();
            key.Name = SreKeyTypes.Environments.ToString();
            key.Type = (int)SreKeyTypes.Environments;           
            key.ValueBinary = new Binary(GZipArchive.Compress(envs.Serialize().GetByteArray()));
            new Manager().Save(key);
        }

    }
}


