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
using System.Linq;
using System.Text;
using Eyedia.Core;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;
using System.IO;
using System.Diagnostics;
using System.Activities;
using System.Text.RegularExpressions;

namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// Run time data source for the job
    /// </summary>
    public partial class DataSource : IDisposable
    {

        IdpeDataSource _dbApp;

        /// <summary>
        /// Data source object, generally intialized by a Job
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public DataSource(int id, string name)
        {
            Manager am = new Manager();
            _dbApp = new IdpeDataSource();
            _dbApp = Cache.Instance.Bag[id] as IdpeDataSource;
            if (_dbApp == null)
                _dbApp = Cache.Instance.Bag[name] as IdpeDataSource;

            if (_dbApp == null)
            {
                if (id > 0)
                {
                    _dbApp = am.GetDataSourceDetails(id);
                    Cache.Instance.Bag.Add(id, _dbApp);
                }
                else if ((!(string.IsNullOrEmpty(name))) && (_dbApp == null))
                {
                    _dbApp = am.GetApplicationDetails(name);
                    Cache.Instance.Bag.Add(name, _dbApp);
                }
            }

            if (_dbApp != null)
            {
                this.IsValid = true;
                this.Id = _dbApp.Id;
                this.Name = _dbApp.Name;
                this.DataFeederType = (DataFeederTypes)_dbApp.DataFeederType;
                this.DataFormatType = (DataFormatTypes)_dbApp.DataFormatType;

                this.Attributes = Cache.Instance.Bag[this.Id + ".attributes"] as List<IdpeAttribute>;
                if (this.Attributes == null)
                {
                    this.Attributes = am.GetAttributes(this.Id);
                    Cache.Instance.Bag.Add(this.Id + ".attributes", this.Attributes);
                }

                this.AttributesSystem = Cache.Instance.Bag[this.Id + ".attributessystem"] as List<IdpeAttribute>;
                if (this.AttributesSystem == null)
                {
                    this.AttributesSystem = am.GetAttributes((int)_dbApp.SystemDataSourceId);
                    Cache.Instance.Bag.Add(this.Id + ".attributessystem", this.AttributesSystem);
                }

                RefreshKeys();

                this.BusinessRules = Cache.Instance.Bag[this.Id + ".rules"] as BusinessRules;
                if (this.BusinessRules == null)
                {
                    InitRuleSets(am);
                    Cache.Instance.Bag.Add(this.Id + ".rules", this.BusinessRules);
                }
              
            }
            else
            {
                this.IsValid = false;
                this.Id = id;
                this.Name = name;
            }

        }

        public void RefreshKeys(bool clearCache = true)
        {
            if (clearCache)
            {
                if (Cache.Instance.Bag[Id + ".keys"] != null)
                    Cache.Instance.Bag.Remove(Id + ".keys");
            }
            this.Keys = LoadKeys(this.Id);
            string strIsFirstRowIsHeader = this.Keys.GetKeyValue(SreKeyTypes.IsFirstRowHeader);
            bool boolIsFirstRowIsHeader = false;
            if (strIsFirstRowIsHeader != null)
                bool.TryParse(strIsFirstRowIsHeader, out boolIsFirstRowIsHeader);
            IsFirstRowHeader = boolIsFirstRowIsHeader;

            #region Adding Derrived Keys

            IdpeKey dkey = Keys.GetKey(SreKeyTypes.EmailAfterFileProcessedAttachOtherFiles.ToString());
            if (dkey == null)
            {
                //this key is used to send any additional files to be sent along with email
                //this key value can be filled anywhere, including plugins
                //Expected value is comma separate file names
                dkey = new IdpeKey();
                dkey.Name = SreKeyTypes.EmailAfterFileProcessedAttachOtherFiles.ToString();
                dkey.Value = string.Empty;
                this.Keys.Add(dkey);
            }

            #endregion Adding Derrived Keys
        }

        public int Id { get; private set; }

        public int SystemDataSourceId
        {
            get
            {
                if (_dbApp == null)
                    return 0;

                return (int)_dbApp.SystemDataSourceId;
            }
        }

        public string Name
        {
            get;
            private set;
        }

        public string Delimiter
        {
            get
            {
                if (_dbApp == null)
                    return ",";
                return _dbApp.Delimiter == null ? "," : _dbApp.Delimiter;
            }
        }

        public OutputTypes OutputType
        {
            get
            {
                if (_dbApp == null)
                    return OutputTypes.Xml;
                return (OutputTypes)_dbApp.OutputType;
            }
        }

        public string OutputWriterTypeFullName
        {
            get 
            {
                if (_dbApp == null)
                    return string.Empty;

                if(string.IsNullOrEmpty(_dbApp.OutputWriterTypeFullName))
                {
                    //this is only fyi
                    switch (OutputType)
                    {
                        case OutputTypes.Xml: return "Xml";
                        case OutputTypes.Delimited: return "Delimited";
                        case OutputTypes.FixedLength: return "FixedLength";
                        case OutputTypes.CSharpCode: return "CSharpCode";
                        case OutputTypes.Database: return "Database";
                        default: return "Unknown";  //this should never be the case
                    }
                }
                else
                {
                    return _dbApp.OutputWriterTypeFullName;
                }
            }
        }
       

        public string PlugInsType
        {
            get
            {
                if (_dbApp == null)
                    return string.Empty;
                return _dbApp.PlugInsType;
            }
        }

        public PusherTypes PusherType
        {
            get
            {
                if (_dbApp == null)
                    return PusherTypes.None;
                return (PusherTypes)_dbApp.PusherType;
            }
        }

        public string PusherTypeFullName
        {
            get
            {
                if (_dbApp == null)
                    return string.Empty;
                return _dbApp.PusherTypeFullName;
            }
        }

        public bool IsActive
        {
            get
            {
                if (_dbApp == null)
                    return false;

                return _dbApp.IsActive;
            }
        }

        public OutputWriter OutputWriter { get; internal set; }
        public PlugIns PlugIns { get; internal set; }
        public Pushers Pusher { get; internal set; }
        public bool IsValid { get; private set; }
        public bool IsFirstRowHeader { get; internal set; }
        public DataFeederTypes DataFeederType { get; private set; }
        public DataFormatTypes DataFormatType { get; private set; }
        public List<IdpeKey> Keys { get; private set; }
        public BusinessRules BusinessRules { get; private set; }       

        /// <summary>
        /// Returns list of attributes
        /// </summary>
        public List<IdpeAttribute> Attributes { get; private set; }

        /// <summary>
        /// Returns only acceptable list of attributes
        /// </summary>
        public List<IdpeAttribute> AcceptableAttributes
        {
            get
            {
                List<IdpeAttribute> acceptableAttributes = new List<IdpeAttribute>();
                if (Attributes != null)
                    acceptableAttributes = Attributes.Where(a => a.IsAcceptable == true).ToList();
                return acceptableAttributes;
            }
        }

        /// <summary>
        /// Returns only acceptable list of system attributes
        /// </summary>
        public List<IdpeAttribute> AcceptableAttributesSystem
        {
            get
            {
                List<IdpeAttribute> acceptableAttributes = new List<IdpeAttribute>();
                if (AttributesSystem != null)
                    acceptableAttributes = AttributesSystem.Where(a => a.IsAcceptable == true).ToList();
                return acceptableAttributes;
            }
        }

        /// <summary>
        /// Returns true if local file system pull directory is overriden (does not use Global pull)
        /// </summary>
        public bool LocalFileSystemFoldersOverriden
        {
            get
            {
                return IsLocalFileSystemFoldersAreOverriden(this.Keys);
            }
        }

        /// <summary>
        /// Returns true if SRE to use default archive location (parallel to 'Pull')
        /// </summary>
        public bool LocalFileSystemFolderArchiveAuto
        {
            get
            {
                return IsLocalFileSystemFolderArchiveAuto(this.Keys);
            }
        }

        /// <summary>
        /// Returns pull folder path
        /// </summary>
        public string LocalFileSystemFolderPullFolder
        {
            get
            {
                return GetPullFolder(this.Id, this.Keys);
            }
        }

        /// <summary>
        /// Returns archive folder path
        /// </summary>
        public string LocalFileSystemFolderArchiveFolder
        {
            get
            {
                return GetArchiveFolder(this.Id, this.Keys);
            }
        }

        /// <summary>
        /// Returns output folder path
        /// </summary>
        public string LocalFileSystemFolderOutputFolder
        {
            get
            {
                return GetOutputFolder(this.Id, this.Keys);
            }
        }

        /// <summary>
        /// Returns system attributes (inherited from parent data source)
        /// </summary>
        public List<IdpeAttribute> AttributesSystem { get; private set; }


        /// <summary>
        /// returns specific key
        /// </summary>
        /// <param name="sreKeyType">specific key</param>
        /// <returns></returns>
        public IdpeKey Key(SreKeyTypes sreKeyType)
        {
            if ((sreKeyType == SreKeyTypes.Custom)
                || (sreKeyType.IsConnectionStringType()))
                return null;

            List<IdpeKey> keys = (from e in this.Keys
                                 where e.Type == (int)sreKeyType
                                 select e).ToList();
            if (keys.Count > 0)
            {
                if (keys.Count > 1)
                {
                    ExtensionMethods.TraceInformation("Warning:: More than one key of type '{0}' defined in data source '{1}'. Taking first key.",
                        sreKeyType.ToString(), this.Id);
                }
                return keys[0];
            }
            else
            {
                //throw new Exception(string.Format("No key of type '{0}' is defined in data source '{1}'",
                //    sreKeyType.ToString(), this.Id));                
                return null;
            }
        }

        /// <summary>
        /// returns all custom keys
        /// </summary>
        /// <returns></returns>
        public List<IdpeKey> KeyCustoms()
        {
            return (from e in this.Keys
                    where e.Type == (int)SreKeyTypes.Custom
                    select e).ToList();
        }

        /// <summary>
        /// returns all connection strings
        /// </summary>
        /// <returns></returns>
        public List<IdpeKey> KeyConnectionStrings()
        {
            return (from e in this.Keys
                    where ((SreKeyTypes)e.Type).IsConnectionStringType()
                    select e).ToList();
        }

        /// <summary>
        /// Adds additional attachments to the email when a file is processed.
        /// Basically adds a new comma separated value to the derrived key EmailAfterFileProcessedAttachOtherFiles
        /// </summary>
        /// <param name="fileName">The attachment name</param>
        public void AddOtherAttachments(string fileName)
        {
            IdpeKey key = this.Keys.GetKey(SreKeyTypes.EmailAfterFileProcessedAttachOtherFiles.ToString());
            if (key != null)
            {
                if (string.IsNullOrEmpty(key.Value))
                {
                    key.Value = fileName;
                }
                else
                {
                    key.Value += "," + fileName;
                }
            }
        }

        /// <summary>
        /// Clears additional attachments, this will be executed everytime a new job is created
        /// </summary>
        public void ClearAdditionalAttachments()
        {
            IdpeKey key = this.Keys.GetKey(SreKeyTypes.EmailAfterFileProcessedAttachOtherFiles.ToString());
            if (key != null)
                key.Value = string.Empty;
        }

        private void InitRuleSets(Manager am)
        {
            this.BusinessRules = new BusinessRules();
            List<IdpeRule> rules = am.GetRules(this.Id);
            foreach (IdpeRule rule in rules)
            {
                this.BusinessRules.Add(new BusinessRule(rule.Xaml, (int)rule.Priority, (RuleSetTypes)rule.RuleSetType));
            }

        }

        /// <summary>
        /// Returns pull folder path (when datasource object is not available)
        /// </summary>
        public static string GetPullFolder(int dataSourceId, List<IdpeKey> keys)
        {
            string folder = Path.Combine(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull, dataSourceId.ToString());
            if (!IsLocalFileSystemFoldersAreOverriden(keys))
                return folder;

            IdpeKey key = (from e in keys
                          where e.Type == (int)SreKeyTypes.LocalFileSystemFolderPull
                          select e).SingleOrDefault();

            folder = key.Value;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
        }
        /// <summary>
        /// Returns output folder path (when datasource object is not available)
        /// </summary>
        public static string GetOutputFolder(int dataSourceId, List<IdpeKey> keys)
        {
            string folder = Path.Combine(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryOutput, dataSourceId.ToString());
            folder = Path.Combine(folder, DateTime.Now.ToDBDateFormat());
            if (!IsLocalFileSystemFoldersAreOverriden(keys))
                return folder;

            IdpeKey key = (from e in keys
                          where e.Type == (int)SreKeyTypes.LocalFileSystemFolderOutput
                          select e).SingleOrDefault();

            folder = Path.Combine(key.Value, DateTime.Now.ToDBDateFormat());
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;

        }

        /// <summary>
        /// Returns output file name based on data source configuration 
        /// </summary>
        public static string GetOutputFileName(int dataSourceId, List<IdpeKey> keys, string outputFolder, string inputFileNameOnly)
        {
            if (keys == null)
                keys = LoadKeys(dataSourceId);

            string outputExtension = keys.GetKeyValue(SreKeyTypes.OutputFileExtension);
            if (string.IsNullOrEmpty(outputExtension))
                outputExtension = ".xml";

            string outputFileName = string.Empty;
            string outputFileNameKey = keys.GetKeyValue(SreKeyTypes.OutputFileName);
            if (string.IsNullOrEmpty(outputFileNameKey))
            {
                outputFileName = Path.Combine(outputFolder, inputFileNameOnly + outputExtension);
            }
            else
            {
                outputFileName = FormatOutputFileName(outputFileNameKey, inputFileNameOnly);
                outputFileName = Path.Combine(outputFolder, outputFileName + outputExtension);
            }
            Directory.CreateDirectory(Path.GetDirectoryName(outputFileName));
            return outputFileName;
        }

        private static string FormatOutputFileName(string fileName, string onlyFileName)
        {
            Regex regex = new Regex(@"\[(.*?)\]", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(fileName);
            foreach (var match in matches)
            {
                fileName = fileName.Replace(match.ToString(), DateTime.Now.ToString(match.ToString()));
            }
            if (fileName.Length > 0)
                fileName = fileName.Replace("[", string.Empty).Replace("]", string.Empty);

            fileName = Regex.Replace(fileName, "Same as input", onlyFileName, RegexOptions.IgnoreCase);
            return fileName;
        }

        /// <summary>
        /// Returns archive folder path (when datasource object is not available)
        /// </summary>
        public static string GetArchiveFolder(int dataSourceId, List<IdpeKey> keys)
        {
            string folder = Path.Combine(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryArchive, dataSourceId.ToString());
            folder = Path.Combine(folder, DateTime.Now.ToDBDateFormat());
            if (!IsLocalFileSystemFoldersAreOverriden(keys))
                return folder;

            if (IsLocalFileSystemFolderArchiveAuto(keys))
            {
                folder = Path.Combine(Directory.GetParent(GetPullFolder(dataSourceId, keys)).FullName, "Archive");
                folder = Path.Combine(folder, DateTime.Now.ToDBDateFormat());
                return folder;
            }

            IdpeKey key = (from e in keys
                          where e.Type == (int)SreKeyTypes.LocalFileSystemFolderArchive
                          select e).SingleOrDefault();

            folder = Path.Combine(key.Value, DateTime.Now.ToDBDateFormat());
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }

        static bool IsLocalFileSystemFoldersAreOverriden(List<IdpeKey> keys)
        {
            if (keys == null)
                return false;

            IdpeKey key = (from e in keys
                          where e.Type == (int)SreKeyTypes.LocalFileSystemFoldersOverriden
                          select e).SingleOrDefault();

            bool result = false;
            if (key != null)
                bool.TryParse(key.Value, out result);
            return result;

        }


        /// <summary>
        /// Returns true if SRE to use default archive location (parallel to 'Pull')
        /// </summary>
        public static bool IsLocalFileSystemFolderArchiveAuto(List<IdpeKey> keys)
        {

            IdpeKey key = (from e in keys
                          where e.Type == (int)SreKeyTypes.LocalFileSystemFolderArchiveAuto
                          select e).SingleOrDefault();

            bool result = false;
            if (key != null)
                bool.TryParse(key.Value, out result);
            return result;

        }

        public static List<IdpeKey> LoadKeys(int dataSourceId)
        {         
            List<IdpeKey> keys = new List<IdpeKey>();
            keys = Cache.Instance.Bag[dataSourceId + ".keys"] as List<IdpeKey>;
            if (keys == null)
            {
                Manager manager = new Manager();
                keys = manager.GetApplicationKeys(dataSourceId, true);

                //adding global
                List<IdpeKey> gKeys = manager.GetApplicationKeys(-99, true);
                foreach (IdpeKey key in gKeys)
                {
                    if (keys.Where(dKey => dKey.Name == key.Name).ToList().Count == 0)
                        keys.Add(key);
                }
                Cache.Instance.Bag.Add(dataSourceId + ".keys", keys);
            }
            return keys;
        }


        #region IDisposable Members

        public void Dispose()
        {
            this._dbApp = null;
            this.Attributes = null;
            this.AttributesSystem = null;
            //this.DataContainerValidator = null;
        }

        #endregion
    }
}






