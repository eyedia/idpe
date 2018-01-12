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
    public class SreVersionManager
    {
        public void KeepVersion(VersionObjectTypes objectType, int referenceId)
        {
            Manager manager = new Manager();

            switch (objectType)
            {
                case VersionObjectTypes.Attribute:
                    throw new NotImplementedException();
                    break;

                case VersionObjectTypes.DataSource:
                    if (!string.IsNullOrEmpty(manager.GetApplicationName(referenceId)))
                        KeepVersionInternal(manager, VersionObjectTypes.DataSource, referenceId);
                    break;

                case VersionObjectTypes.Rule:
                    KeepVersionInternal(manager, VersionObjectTypes.Rule, referenceId);
                    break;
            }
        }

        public void KeepVersionInternal(Manager manager, VersionObjectTypes versionObjectType, int referenceId)
        {
            if (referenceId == 0)
                return;

            IdpeVersion version = GetCurrentVersion(versionObjectType, referenceId);
            if (version == null)
                return;

            version.Version = manager.GetLatestVersionNumber(versionObjectType, referenceId) + 1;
            version.Type = (int)versionObjectType;
            version.ReferenceId = referenceId;

            switch (versionObjectType)
            {
                case VersionObjectTypes.DataSource:
                    IdpeDataSource ds = manager.GetDataSourceDetails(referenceId);
                    version.Data = new Binary(GZipArchive.Compress(new DataSourceBundle().Export(referenceId, false).GetByteArray()));
                    version.CreatedTS = ds.CreatedTS;
                    break;

                case VersionObjectTypes.Rule:
                    IdpeRule rule = manager.GetRule(referenceId);
                    DataSourcePatch patch = new DataSourcePatch();
                    patch.Rules.Add(rule);
                    version.Data = new Binary(GZipArchive.Compress(patch.Export().GetByteArray()));
                    version.CreatedTS = rule.CreatedTS;
                    break;
            }
            manager.Save(version);
        }

        public IdpeVersion GetCurrentVersion(VersionObjectTypes versionObjectType, int referenceId)
        {
            IdpeVersion currentVersion = new IdpeVersion();
            currentVersion.Type = (int)versionObjectType;
            if (versionObjectType == VersionObjectTypes.DataSource)
            {
                DataSourceBundle dsb = new DataSourceBundle();
                string serialized = dsb.Export(referenceId, false);
                currentVersion.Data = new System.Data.Linq.Binary(GZipArchive.Compress(serialized.GetByteArray()));
                currentVersion.CreatedBy = dsb.DataSource.CreatedBy;
                currentVersion.CreatedTS = (DateTime)new Manager().GetDataSourceLastUpdatedTime(referenceId);
                currentVersion.Source = dsb.DataSource.Source;
            }
            else if (versionObjectType == VersionObjectTypes.Rule)
            {
                IdpeRule rule = new Manager().GetRule(referenceId);
                if (rule == null)
                    return null;

                DataSourcePatch patch = new DataSourcePatch();
                patch.Rules.Add(rule);
                currentVersion.Data = new System.Data.Linq.Binary(GZipArchive.Compress(patch.Export().GetByteArray()));
                currentVersion.CreatedBy = rule.CreatedBy;
                currentVersion.CreatedTS = rule.CreatedTS;
                currentVersion.Source = rule.Source;

            }
            return currentVersion;
        }
    }
}


