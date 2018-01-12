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
using System.Data;

namespace Eyedia.Core.Data
{
	public interface IDal
	{
		//IDbCommand CreateCommand();
		//IDbCommand CreateCommand(string cmdText);
		//IDbCommand CreateCommand(string cmdText, IDbConnection connection);
		IDbCommand CreateCommand(string cmdText = null, IDbConnection connection = null, IDbTransaction transaction = null);
		
		//IDbConnection CreateConnection();
		IDbConnection CreateConnection(string connectionString = null);
		
		//IDbDataAdapter CreateDataAdapter();
		IDbDataAdapter CreateDataAdapter(IDbCommand selectCommand = null);
		IDbDataAdapter CreateDataAdapter(string selectCommandText, string selectConnectionString);
		IDbDataAdapter CreateDataAdapter(string selectCommandText, IDbConnection selectConnection);
		
		//IDataReader CreateDataReader(IDbCommand dbCommand);
		//IDataReader CreateDataReader(IDbCommand dbCommand, CommandBehavior dbCommandBehavior = CommandBehavior.Default);

        //IDbTransaction CreateTransaction(IDbConnection connection);
        IDbTransaction CreateTransaction(IDbConnection connection, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

	}


	public interface IDalFactory
	{
        IDal CreateDAL(DatabaseTypes databaseType);	
	}
	
	
	public interface IStoredProcedure
	{
		
		#region Methods
		
		int ExecuteNonQuery();
		object ExecuteScalar();
		void ClearParameters();

		DataSet GetResultSet();
		
		IDbDataParameter CreateParameter();
		void AddParameter(IDbDataParameter parameter);
		void AddParameters(IDbDataParameter[] parameter);
		
		IDataReader CreateDataReader();
		
		#endregion
		
		#region Properties
		
		string	Name				{get;set;}
		string	ConnectionString	{get;set;}
		bool	CacheDisconnectData {get;set;} 
		
		#endregion
	
	}

    public enum DatabaseTypes 
    {
        SqlCe, 
        SqlServer,
        Oracle, 
        DB2iSeries       
        
    }

}





