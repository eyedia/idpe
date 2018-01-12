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
using Eyedia.Core.Data;
using System.Data;
using Eyedia.Core;
using System.Globalization;
using System.Diagnostics;

namespace Eyedia.Core.Data
{
    public partial class CoreDatabaseObjects
    {

        /// <summary>
        /// Saves new email tracker object into the database
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>       
        /// <param name="referenceId"></param>
        public bool Save(string subject, string body, int? referenceId = null)
        {
            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(subject))
                errorMessage = "Subject is mandatory!" + Environment.NewLine;
            if (string.IsNullOrEmpty(subject))
                errorMessage += "Body is mandatory!" + Environment.NewLine;

            if (!string.IsNullOrEmpty(errorMessage))
                return false;

            SymplusEmailTracker emailTracker = new SymplusEmailTracker();
            emailTracker.ReferenceId = referenceId;
            emailTracker.Subject = subject;
            emailTracker.Body = body;

            return Save(emailTracker);
        }

        /// <summary>
        /// Saves new email tracker object into the database
        /// </summary>
        /// <param name="emailTracker"></param>    
        public bool Save(SymplusEmailTracker emailTracker)
        {
            if (GetNumberOfTodaysEmail(emailTracker) >= EyediaCoreConfigurationSection.CurrentConfig.Email.MaxNumberOfEmails)
                return false;

            emailTracker.Subject = HandleMaxLengthAndQoutes(emailTracker.Subject, 150);
            emailTracker.Body = HandleMaxLengthAndQoutes(emailTracker.Body, 150);

            IDal dal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection connection = dal.CreateConnection(_ConnectionString);
            connection.Open();
            IDbTransaction transaction = dal.CreateTransaction(connection);
            IDbCommand command = dal.CreateCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            try
            {
                string userName = "Eyedia";
                if (userName.Contains("'"))
                    userName = userName.Replace("'", "''");
                if ((emailTracker.ReferenceId != null) && (emailTracker.ReferenceId != 0))
                {
                    command.CommandText = string.Format(CultureInfo.InvariantCulture, "INSERT INTO SymplusEmailTracker (Subject, Body, ReferenceId, CreatedTS, CreatedBy, Source) VALUES('{0}','{1}',{2},'{3}','{4}','{5}')"
                            , emailTracker.Subject, emailTracker.Body, emailTracker.ReferenceId, DateTime.Now, userName, GetSource());
                }
                else
                {
                    command.CommandText = string.Format(CultureInfo.InvariantCulture, "INSERT INTO SymplusEmailTracker (Subject, Body, CreatedTS, CreatedBy, Source) VALUES('{0}','{1}','{2}','{3}','{4}')"
                            , emailTracker.Subject, emailTracker.Body, DateTime.Now, userName, GetSource());
                }

                command.ExecuteNonQuery();
                transaction.Commit();
                return true;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Trace.TraceInformation(ex.ToString()); //donot use TraceError
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Dispose();
                transaction.Dispose();

            }

        }

        public int GetNumberOfTodaysEmail(SymplusEmailTracker emailTracker)
        {
            return GetNumberOfTodaysEmail(emailTracker.Subject, emailTracker.Body, emailTracker.ReferenceId);
        }

        public int GetNumberOfTodaysEmail(string subject, string body, int? referenceId = null)
        {
            subject = HandleMaxLengthAndQoutes(subject, 150);
            body = HandleMaxLengthAndQoutes(body, 150);
            int emails = 0;

            string commandText = string.Format("select Count(*) as [Count] from SymplusEmailTracker where Subject = '{0}' and Body = '{1}'",
                subject, body);
            if ((referenceId != null) && (referenceId != 0))
                commandText += " and referenceId =" + referenceId;
                

            if (EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType == DatabaseTypes.SqlCe)
                commandText += " group by CONVERT(nvarchar(10), CreatedTS, 120)";
            else if (EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType == DatabaseTypes.SqlServer)
                commandText += " group by CAST(CreatedTS AS DATE)";

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table != null)
            {
                if (table.Rows.Count > 0)
                    int.TryParse(table.Rows[0][0].ToString(), out emails);

            }

            return emails;
        }

        public void ClearTrackedEmails()
        {
            CoreDatabaseObjects.Instance.ExecuteStatement("delete from [SymplusEmailTracker]");
        }

        private string HandleMaxLengthAndQoutes(string data, int maxLength = 4000)
        {
            if (data == null)
                return string.Empty;

            return data.Length > 4000 ? HandleSingleQoute(data.Substring(0, 3999)) : HandleSingleQoute(data);
        }

        private string HandleSingleQoute(string data)
        {
            if (data == null)
                return string.Empty;
            return data.Contains("'") ? data.Replace("'", "''") : data;
        }
    }
}


