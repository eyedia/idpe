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
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using Eyedia.Core;
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Globalization;

namespace Eyedia.IDPE.Interface.Data
{
    public class UtilDataManager
    {
        private string _ConnectionString;
      
        public UtilDataManager()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings[Constants.ConnectionStringName].ToString();       
        }

        public BindingList<Attribute> GetAttributes(string attributeName = null)
        {
            List<Attribute> returnList = new List<Attribute>();
            List<IdpeAttribute> SreAttributes = new Manager().GetAttributes();
            if (attributeName == null)
            {
                var result = from a in SreAttributes
                             orderby a.Name
                             select new Attribute
                             {
                                 Id = a.AttributeId,
                                 Name = a.Name,
                                 Type = (AttributeTypes)Enum.Parse(typeof(AttributeTypes), a.Type, true),
                                 Minimum = a.Minimum,
                                 Maximum = a.Maximum,
                                 Formula = a.Formula,
                                 IsAcceptable = a.IsAcceptable == null ? false : (bool)a.IsAcceptable
                             };
                returnList = result.ToList<Attribute>();
            }
            else
            {
                attributeName = attributeName.ToLower();
                var result = from a in SreAttributes
                             where a.Name.ToLower().Contains(attributeName)
                             orderby a.Name
                             select new Attribute
                             {
                                 Id = a.AttributeId,
                                 Name = a.Name,
                                 Type = (AttributeTypes)Enum.Parse(typeof(AttributeTypes), a.Type, true),
                                 Minimum = a.Minimum,
                                 Maximum = a.Maximum,
                                 Formula = a.Formula,
                                 IsAcceptable = a.IsAcceptable == null ? false : (bool)a.IsAcceptable
                             };
                returnList = result.ToList<Attribute>();
            }
            
            return new BindingList<Attribute>(returnList);
        }

        public BindingList<Attribute> GetAttributes(int dataSourceId)
        {
            List<Attribute> returnList = new List<Attribute>();
            string commandText = "select a.[AttributeId],a.[Name],a.[Type],a.[Minimum],a.[Maximum],a.[Formula],aes.[IsAcceptable],aes.[AttributePrintValueType],aes.[AttributePrintValueCustom],a.[CreatedTS],a.[CreatedBy],a.[ModifiedTS],a.[ModifiedBy],a.[Source],AttributeDataSourceId,Position,es.Name as [DataSourceName] " +
                " from  idpeAttribute a Inner join idpeAttributeDataSource aes  on a.AttributeId = aes.AttributeId " +
                         " inner join idpeDataSource es on aes.DataSourceId = es.Id where es.Id =  " + dataSourceId + " order by aes.Position";

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return new BindingList<Attribute>(returnList);

            foreach (DataRow row in table.Rows)
            {
                Attribute attrb = new Attribute();
                attrb.Id = (int)row["AttributeId"].ToString().ParseInt();
                attrb.Name = row["Name"] != DBNull.Value ? row["Name"].ToString() : null;
                if (row["Type"] != DBNull.Value)
                    attrb.Type = (AttributeTypes)Enum.Parse(typeof(AttributeTypes), row["Type"].ToString(), true);

                attrb.Minimum = row["Minimum"] != DBNull.Value ? row["Minimum"].ToString() : null;
                attrb.Maximum = row["Maximum"] != DBNull.Value ? row["Maximum"].ToString() : null;
                attrb.Formula = row["Formula"] != DBNull.Value ? row["Formula"].ToString() : null;
                attrb.IsAcceptable = row["IsAcceptable"] != DBNull.Value ? row["IsAcceptable"].ToString().ParseBool() : false;
                attrb.AttributePrintValueType = row["AttributePrintValueType"] != DBNull.Value ? (int)row["AttributePrintValueType"].ToString().ParseInt() : 0;
                attrb.AttributePrintValueCustom = row["AttributePrintValueCustom"] != DBNull.Value ? row["AttributePrintValueCustom"].ToString() : string.Empty;
                attrb.AttributeDataSourceId = (int)row["AttributeDataSourceId"].ToString().ParseInt();
                attrb.DataSourceId = dataSourceId;
                attrb.DataSourceName = row["DataSourceName"] != DBNull.Value ? row["DataSourceName"].ToString() : null;
                attrb.Position = (row["Position"] != DBNull.Value) ? row["Position"].ToString() : string.Empty;

                returnList.Add(attrb);
            }
            table.Dispose();
            return new BindingList<Attribute>(returnList);

        }

        public BindingList<ExternalSystem> GetAllExternalSystems(bool? onlySystemType)
        {
            List<ExternalSystem> returnList = new List<ExternalSystem>();
       
            string commandText = string.Empty;
            if (onlySystemType == null) //get all
                commandText = "select [Id],[Name],[IsSystem] from IdpeDataSource order by [Id], [IsSystem] desc";
            else
                commandText = "select [Id],[Name],[IsSystem] from IdpeDataSource where [IsSystem] = " + (onlySystemType == true ? "1" : "0") + " order by [Id], [IsSystem] desc";

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return new BindingList<ExternalSystem>(returnList);

            foreach (DataRow row in table.Rows)
            {
                ExternalSystem es = new ExternalSystem();
                es.ExternalSystemId = (int)row["Id"].ToString().ParseInt();
                es.ExternalSystemName = row["Name"] != DBNull.Value ? row["Name"].ToString() : null;
                es.IsSystemType = (bool)row["IsSystem"].ToString().ParseBool();
                es.IsHavingVersion = IsHavingVersion(VersionObjectTypes.DataSource, es.ExternalSystemId);

                returnList.Add(es);
            }
            return new BindingList<ExternalSystem>(returnList);
        }

        public BindingList<ExternalSystem> GetAllExternalSystems(bool? onlySystemType, string dataSourceName)
        {
            if (string.IsNullOrEmpty(dataSourceName))
                return GetAllExternalSystems(onlySystemType);

            List<ExternalSystem> returnList = new List<ExternalSystem>();

            string commandText = string.Empty;
            if (onlySystemType == null) //get all
                commandText = "select [Id],[Name],[IsSystem] from IdpeDataSource where [Name] like '%" + dataSourceName + "%' order by [Id], [IsSystem] desc";
            else
                commandText = "select [Id],[Name],[IsSystem] from IdpeDataSource where [IsSystem] = " + (onlySystemType == true ? "1" : "0") + "and [Name] like '%" + dataSourceName + "%' order by [Id], [IsSystem] desc";

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return new BindingList<ExternalSystem>(returnList);

            foreach (DataRow row in table.Rows)
            {
                ExternalSystem es = new ExternalSystem();
                es.ExternalSystemId = (int)row["Id"].ToString().ParseInt();
                es.ExternalSystemName = row["Name"] != DBNull.Value ? row["Name"].ToString() : null;
                es.IsSystemType = (bool)row["IsSystem"].ToString().ParseBool();
                es.IsHavingVersion = IsHavingVersion(VersionObjectTypes.DataSource, es.ExternalSystemId);
                returnList.Add(es);
            }
            return new BindingList<ExternalSystem>(returnList);
        }

        public bool IsHavingVersion(VersionObjectTypes versionObjectType, int referenceId)
        {
            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand( string.Format("select count(*) from IdpeVersion where [Type] = {0} and [ReferenceId] = {1}",
                (int)versionObjectType, referenceId));

            return table.Rows[0][0].ToString().ParseInt() > 0 ? true : false;
        }
    

        public void SaveAssociations(int applicationId, List<object> externalSystemAttributes)
        {
            string cmdText = string.Empty;
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            IDbCommand command = myDal.CreateCommand(cmdText, conn);
            int position = 1;
            conn.Open();

            foreach (Attribute attribute in externalSystemAttributes)
            {
                cmdText = "SELECT [AttributeDataSourceId] FROM IdpeAttributeDataSource WHERE [AttributeDataSourceId] = @AttributeDataSourceId";
                command.Parameters.Clear();
                command.AddParameterWithValue("AttributeDataSourceId", attribute.AttributeDataSourceId);
                command.CommandText = cmdText;
                IDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    cmdText = "UPDATE [IdpeAttributeDataSource] SET [Position] = @Position, [IsAcceptable] = @IsAcceptable WHERE [AttributeDataSourceId] = @AttributeDataSourceId";
                    command.Parameters.Clear();
                    if(attribute.IsAcceptable)
                        command.AddParameterWithValue("Position", position);
                    else
                        command.AddParameterWithValue("Position", DBNull.Value);

                    command.AddParameterWithValue("IsAcceptable", attribute.IsAcceptable);
                    command.AddParameterWithValue("AttributeDataSourceId", attribute.AttributeDataSourceId);
                }
                else
                {
                    cmdText = "INSERT INTO [IdpeAttributeDataSource] ([DataSourceId],[AttributeId],[Position],[IsAcceptable],[AttributePrintValueType],[AttributePrintValueCustom],[CreatedTS],[CreatedBy],[Source]) VALUES ";
                    cmdText += "(@DataSourceId,@AttributeId,@Position,@IsAcceptable,@AttributePrintValueType,@AttributePrintValueCustom,@CreatedTS,@CreatedBy,@Source)";
                    command.Parameters.Clear();
                    command.AddParameterWithValue("DataSourceId", applicationId);
                    command.AddParameterWithValue("AttributeId", attribute.Id);
                    if (attribute.IsAcceptable)
                        command.AddParameterWithValue("Position", position);
                    else
                        command.AddParameterWithValue("Position", DBNull.Value);

                    command.AddParameterWithValue("IsAcceptable", attribute.IsAcceptable);

                    if ((attribute.AttributePrintValueType != null)
                        || (attribute.AttributePrintValueType != 0))
                        command.AddParameterWithValue("AttributePrintValueType", attribute.AttributePrintValueType);
                    else
                        command.AddParameterWithValue("AttributePrintValueType", DBNull.Value);

                    if (attribute.AttributePrintValueCustom != null)
                        command.AddParameterWithValue("AttributePrintValueCustom", attribute.AttributePrintValueCustom);
                    else
                        command.AddParameterWithValue("AttributePrintValueCustom", DBNull.Value);

                    command.AddParameterWithValue("CreatedTS", DateTime.Now);
                    command.AddParameterWithValue("CreatedBy", Information.LoggedInUser == null ? "Debugger" : Information.LoggedInUser.UserName);
                    command.AddParameterWithValue("Source", "SRE Util");

                }
                reader.Close();
                command.CommandText = cmdText;
                command.ExecuteNonQuery();
                if (attribute.IsAcceptable)
                    position++;
            }
        }

        public int AttributeExists(Attribute attribute)
        {
            List<IdpeAttribute> idpeAttributes = new Manager().GetAttributes();
            var record = idpeAttributes.FirstOrDefault(a => a.AttributeId == attribute.Id);
            return (record != null) ? record.AttributeId : 0;
        }
        
        public int Save(Attribute attribute)
        {
            string cmdText = string.Empty;
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            IDbCommand command = myDal.CreateCommand(cmdText, conn);

            if (AttributeExists(attribute) == 0)
            {
                cmdText = "INSERT INTO [IdpeAttribute] ([Name],[Type],[Minimum],[Maximum],[Formula],[IsAcceptable],[CreatedTS],[CreatedBy],[Source]) VALUES ";
                cmdText += "(@Name,@Type,@Minimum,@Maximum,@Formula,@IsAcceptable,@CreatedTS,@CreatedBy,@Source)";


                command.AddParameterWithValue("Name", attribute.Name);
                command.AddParameterWithValue("Type", attribute.Type.ToString().ToUpper());
                if (!(string.IsNullOrEmpty(attribute.Minimum)))
                    command.AddParameterWithValue("Minimum", attribute.Minimum);
                else
                    command.AddParameterWithValue("Minimum", DBNull.Value);

                if (!(string.IsNullOrEmpty(attribute.Maximum)))
                    command.AddParameterWithValue("Maximum", attribute.Maximum);
                else
                    command.AddParameterWithValue("Maximum", DBNull.Value);

                if (!(string.IsNullOrEmpty(attribute.Formula)))
                    command.AddParameterWithValue("Formula", attribute.Formula);
                else
                    command.AddParameterWithValue("Formula", DBNull.Value);

                command.AddParameterWithValue("IsAcceptable", attribute.IsAcceptable);
                command.AddParameterWithValue("CreatedTS",DateTime.Now);
                command.AddParameterWithValue("CreatedBy", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                command.AddParameterWithValue("Source", "SRE Util");

            }
            else
            {
                cmdText = "UPDATE [IdpeAttribute] SET [Name] = @Name,[Type] = @Type,[Minimum] = @Minimum,[Maximum] = @Maximum,[Formula] = @Formula,[IsAcceptable] = @IsAcceptable,[ModifiedTS] = @ModifiedTS,[ModifiedBy] = @ModifiedBy ";
                cmdText += "WHERE [AttributeId] = @AttributeId";


                command.AddParameterWithValue("Name", attribute.Name);
                command.AddParameterWithValue("Type", attribute.Type.ToString().ToUpper());
                if (!(string.IsNullOrEmpty(attribute.Minimum)))
                    command.AddParameterWithValue("Minimum", attribute.Minimum);
                else
                    command.AddParameterWithValue("Minimum", DBNull.Value);

                if (!(string.IsNullOrEmpty(attribute.Maximum)))
                    command.AddParameterWithValue("Maximum", attribute.Minimum);
                else
                    command.AddParameterWithValue("Maximum", DBNull.Value);

                if (!(string.IsNullOrEmpty(attribute.Formula)))
                    command.AddParameterWithValue("Formula", attribute.Formula);
                else
                    command.AddParameterWithValue("Formula", DBNull.Value);

                command.AddParameterWithValue("IsAcceptable", attribute.IsAcceptable);
                command.AddParameterWithValue("ModifiedTS", DateTime.Now);
                command.AddParameterWithValue("ModifiedBy", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                command.AddParameterWithValue("AttributeId", attribute.Id);
            }
            conn.Open();
            command.CommandText = cmdText;
            return command.ExecuteNonQuery();

        }
        public void CopyAttributes(string fromDataSource, int toDataSourceId)
        {
            Manager manager = new Manager();
            List<IdpeAttribute> fromAttributes = manager.GetAttributes(fromDataSource);
            List<IdpeAttributeDataSource> appAttributes = new List<IdpeAttributeDataSource>();
            int position = 1;
            foreach (IdpeAttribute attribute in fromAttributes)
            {
                IdpeAttributeDataSource aes = new IdpeAttributeDataSource();
                aes.DataSourceId = toDataSourceId;
                aes.AttributeId = attribute.AttributeId;
                aes.Position = position;
                aes.CreatedTS = DateTime.Now;
                aes.CreatedBy = "SRE Util";
                aes.Source = "SRE Util";
                appAttributes.Add(aes);
                position++;
            }
            manager.SaveAssociations(toDataSourceId, appAttributes);
        }
        
    }
}





