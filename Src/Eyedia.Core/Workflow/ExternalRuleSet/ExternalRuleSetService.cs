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
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Workflow.Activities.Rules;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Runtime.Configuration;
using System.ServiceModel.Configuration;
using System.Diagnostics;
using Symplus.Core.Data;


namespace Symplus.Core.Workflow
{
    public class ExternalRuleSetService : WorkflowRuntimeService
    {       

        public ExternalRuleSetService()
        {            
        }        

        string _ConnectionStringName;
        
        public string ConnectionStringName
        {            
            get
            {   
                return "cs";    //todo
            }
            set
            {
                _ConnectionStringName = value;
            }
        }

        public string ConnectionString
        {            
            get
            {
                ConnectionStringSettingsCollection connectionStringSettingsCollection = ConfigurationManager.ConnectionStrings;

                foreach (ConnectionStringSettings connectionStringSettings in connectionStringSettingsCollection)
                {
                    if (string.CompareOrdinal(connectionStringSettings.Name, this.ConnectionStringName) == 0)
                        return connectionStringSettings.ConnectionString;
                }               

                return string.Empty;
            }
        }

        public RuleSet GetRuleSet(RuleSetInfo ruleSetInfo)
        {
            if (ruleSetInfo != null)
            {
                if (string.IsNullOrEmpty(this.ConnectionString))
                    throw new ConfigurationErrorsException("Connection string not available for the (should be provided in the config file).");

                IDal _dal = new DataAccessLayer().Instance;
                IDbConnection conn = _dal.CreateConnection(this.ConnectionString);
                conn.Open();
                string commandString;

                // If both the major and minor are 0, it is assumed that a specific version is not being requested.
                bool specificVersionRequested = !(ruleSetInfo.MajorVersion == 0 && ruleSetInfo.MinorVersion == 0);

                if (specificVersionRequested)
                {
                    //commandString = String.Format(CultureInfo.InvariantCulture, "SELECT TOP 1 * FROM {0}SRE_RuleSet WHERE Name=@name AND MajorVersion={1} AND MinorVersion={2} ORDER BY MajorVersion DESC, MinorVersion DESC", Information.SchemaPrefix, ruleSetInfo.MajorVersion, ruleSetInfo.MinorVersion);
                    commandString = String.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0}SRE_RuleSet WHERE Name=@name AND MajorVersion={1} AND MinorVersion={2} ORDER BY MajorVersion DESC, MinorVersion DESC", Information.SchemaPrefix, ruleSetInfo.MajorVersion, ruleSetInfo.MinorVersion);
                }
                else
                {
                    //commandString = String.Format(CultureInfo.InvariantCulture, "SELECT TOP 1 * FROM {0}SRE_RuleSet WHERE Name=@name ORDER BY MajorVersion DESC , MinorVersion DESC", Information.SchemaPrefix);
                    commandString = String.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0}SRE_RuleSet WHERE Name=@name ORDER BY MajorVersion DESC , MinorVersion DESC", Information.SchemaPrefix);

                    //seems like SQL CE does not support 'TOP'
                }
                IDbCommand command = _dal.CreateCommand(commandString, conn);                
                command.AddParameterWithValue("@name", ruleSetInfo.Name);

                IDataReader reader = command.ExecuteReader();
                RuleSetData data = null;
                reader.Read();

                try
                {
                    data = new RuleSetData();
                    data.Name = reader.GetString(1);
                    data.OriginalName = data.Name; // will be used later to see if one of these key values changed                       
                    data.MajorVersion = reader.GetInt32(2);
                    data.OriginalMajorVersion = data.MajorVersion;
                    data.MinorVersion = reader.GetInt32(3);
                    data.OriginalMinorVersion = data.MinorVersion;

                    data.RuleSetDefinition = reader.GetString(4);
                    data.Status = reader.GetInt16(5);
                    data.AssemblyPath = reader.GetString(6);
                    data.ActivityName = reader.GetString(7);
                    data.ModifiedDate = reader.GetDateTime(8);
                    data.Dirty = false;
                }
                catch (InvalidCastException)
                {
                    Trace.TraceError("Error parsing table row - RuleSet Open Error");
                }


                conn.Close();

                if (data != null)
                    return data.RuleSet;
                else if (specificVersionRequested)
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Specified RuleSet version does not exist: '{0}'", ruleSetInfo.ToString())); //could use a custom exception type here
                else
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No RuleSets exist with this name: '{0}'", ruleSetInfo.Name));
            }
            else
            {
                return null;
            }
        }
    }
}




