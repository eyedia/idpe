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
using System.Text;
using System.Configuration;
using System.Collections;
using System.Web.Caching;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;

namespace Eyedia.Core.Data
{
    #region OracleDal
    internal class OracleDal: IDal
	{
		
		#region Command

        public IDbCommand CreateCommand(string cmdText = null, IDbConnection connection = null, IDbTransaction transaction = null)
		{
            if (cmdText == null)
            {
                return (IDbCommand)Activator.CreateInstance(Type.GetType("Oracle.DataAccess.Client.OracleCommand, Oracle.DataAccess"));
            }
            else if ((cmdText != null) && (connection == null) && (transaction == null))
            {
                return (IDbCommand)Activator.CreateInstance(Type.GetType("Oracle.DataAccess.Client.OracleCommand, Oracle.DataAccess"), cmdText);
            }
            else if ((cmdText != null) && (connection != null) && (transaction == null))
            {
                return (IDbCommand)Activator.CreateInstance(Type.GetType("Oracle.DataAccess.Client.OracleCommand, Oracle.DataAccess"), cmdText, connection);
            }
            else if ((cmdText != null) && (connection != null) && (transaction != null))
            {
                IDbCommand command = (IDbCommand)Activator.CreateInstance(Type.GetType("Oracle.DataAccess.Client.OracleCommand, Oracle.DataAccess"), cmdText, connection);
                command.Transaction = transaction;
                return command;
            }
            else
            {
                return null;
            }
		}

		#endregion		
	
		#region Connection
        
        public IDbConnection CreateConnection(string connectionString)
		{            
            if (connectionString == null)
                return (IDbConnection)Activator.CreateInstance(Type.GetType("Oracle.DataAccess.Client.OracleConnection, Oracle.DataAccess"));
            else
                return (IDbConnection)Activator.CreateInstance(Type.GetType("Oracle.DataAccess.Client.OracleConnection, Oracle.DataAccess"), connectionString);
		}

        
		#endregion

		#region DataAdapter

        public IDbDataAdapter CreateDataAdapter(IDbCommand selectCommand = null)
        {
            if (selectCommand == null)
            {
                return (IDbDataAdapter)Activator.CreateInstance(Type.GetType("Oracle.DataAccess.Client.OracleDataAdapter, Oracle.DataAccess"));
            }
            else
            {
                return (IDbDataAdapter)Activator.CreateInstance(Type.GetType("Oracle.DataAccess.Client.OracleDataAdapter, Oracle.DataAccess"), selectCommand);
            }
        }

        public IDbDataAdapter CreateDataAdapter(string selectCommandText, string selectConnectionString)
        {
            return (IDbDataAdapter)Activator.CreateInstance(Type.GetType("Oracle.DataAccess.Client.OracleDataAdapter, Oracle.DataAccess"), selectCommandText, selectConnectionString);

        }

        public IDbDataAdapter CreateDataAdapter(string selectCommandText, IDbConnection selectConnection)
        {
            return (IDbDataAdapter)Activator.CreateInstance(Type.GetType("Oracle.DataAccess.Client.OracleDataAdapter, Oracle.DataAccess"), selectCommandText, selectConnection);
        }
		
		#endregion

        #region Transation

        public IDbTransaction CreateTransaction(IDbConnection connection, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return (IDbTransaction)connection.BeginTransaction(isolationLevel);
        }

        #endregion Transaction        
       
    }

    #endregion OracleDal

    #region DB2iSeriesDal

    /*
	internal class DB2iSeriesDal: IDal
	{

		#region Command
		
		public IDbCommand CreateCommand()
		{
			return new iDB2Command();
		}

		
		public IDbCommand CreateCommand(string cmdText)
		{
			return new iDB2Command(cmdText);
		}

		
		public IDbCommand CreateCommand(string cmdText, IDbConnection connection)
		{
			IDbCommand db2Command = null;
			
			try
			{
				db2Command = new iDB2Command(cmdText, (iDB2Connection)connection);
			}
			catch(iDB2Exception db2Exc)
			{
				if(db2Command != null)
					db2Command.Dispose();
				
				throw new Exception(db2Exc.Message);
			}

			return db2Command;
			
		}

		
		public IDbCommand CreateCommand(string cmdText, IDbConnection connection, IDbTransaction transaction)
		{
			IDbCommand db2Command = null;
			
			try
			{
				db2Command = new iDB2Command(cmdText, (iDB2Connection)connection, (iDB2Transaction)transaction);
			}
			catch(iDB2Exception db2Exc)
			{
				if(db2Command != null)
					db2Command.Dispose();
				
				throw new Exception(db2Exc.Message);
			}

			return db2Command;
		}
		
		
		#endregion
		
		#region DataReader
		
		public IDataReader CreateDataReader(IDbCommand dbCommand)
		{
			IDataReader dr = null;
			
			try
			{
				dr = dbCommand.ExecuteReader();
			}
			catch(iDB2Exception db2Exc)
			{
				if(dr != null)
				{
					if(!dr.IsClosed)
						dr.Close();
					
					dr.Dispose();
				}

				throw new Exception(db2Exc.Message);
			}

			return dr;
		}
		
		
		public IDataReader CreateDataReader(IDbCommand dbCommand, System.Data.CommandBehavior dbCommandBehavior)
		{
			IDataReader dr = null;
			
			try
			{
				dr = dbCommand.ExecuteReader(dbCommandBehavior);
			}
			catch(iDB2Exception db2Exc)
			{
				if(dr != null)
				{
					if(!dr.IsClosed)
						dr.Close();
					
					dr.Dispose();
				}

				throw new Exception(db2Exc.Message);
			}

			return dr;
		}

		
		#endregion
		
		#region Connection
		
		public IDbConnection CreateConnection()
		{
			return new iDB2Connection();
		}

		
		public IDbConnection CreateConnection(string connectionString)
		{
			IDbConnection db2Connection = null;
			
			try
			{
				db2Connection = new iDB2Connection(connectionString);
			}
			catch(iDB2Exception db2Exc)
			{
				if(db2Connection != null)
					db2Connection.Dispose();

				throw new Exception(db2Exc.Message);
			}
			
			return db2Connection;
		}
		

		#endregion

		#region DataAdapter
		
		public IDbDataAdapter CreateDataAdapter()
		{
			return new iDB2DataAdapter();
		}

		
		public IDbDataAdapter CreateDataAdapter(IDbCommand selectCommand)
		{
			IDbDataAdapter db2DataAdapter = null;
			 
			try
			{
				db2DataAdapter = new iDB2DataAdapter((iDB2Command)selectCommand);
			}
			catch(iDB2Exception db2Exc)
			{
				throw new Exception(db2Exc.Message);
			}
			
			return db2DataAdapter;
		}

		
		public IDbDataAdapter CreateDataAdapter(string selectCommandText, string selectConnectionString)
		{
			IDbDataAdapter db2DataAdapter = null;
			 
			try
			{
				db2DataAdapter = new iDB2DataAdapter(selectCommandText,selectConnectionString);
			}
			catch(iDB2Exception db2Exc)
			{
				throw new Exception(db2Exc.Message);
			}
			
			return db2DataAdapter;

		}

		
		public IDbDataAdapter CreateDataAdapter(string selectCommandText, IDbConnection selectConnection)
		{
			IDbDataAdapter db2DataAdapter = null;
			 
			try
			{
				db2DataAdapter = new iDB2DataAdapter(selectCommandText,(iDB2Connection)selectConnection);
			}
			catch(iDB2Exception db2Exc)
			{
				throw new Exception(db2Exc.Message);
			}
			
			return db2DataAdapter;
		}

		
		#endregion

     * #region Transation
        public IDbTransaction CreateTransaction(IDbConnection connection)
        {
            OracleTransaction oracleTrans = null;

            try
            {
                
                oracleTrans = (OracleTransaction)connection.BeginTransaction(IsolationLevel.ReadCommitted);                
            }
            catch (OracleException ex)
            {
                if (oracleTrans != null)
                    oracleTrans.Dispose();

                throw new Exception(ex.ToString(),ex);
            }

            return oracleTrans;
        }
        #endregion Transaction 
     
	}

	*/
    #endregion DB2iSeriesDal

    #region SqlServerDal
    internal class SqlServerDal: IDal
	{

		#region Command
	
        public IDbCommand CreateCommand(string cmdText = null, IDbConnection connection = null, IDbTransaction transaction = null)
		{
            if ((cmdText == null) && (connection == null) && (transaction == null))
			{
				return new SqlCommand();
			}
            else if ((cmdText != null) && (connection == null) && (transaction == null))
            {
                return new SqlCommand(cmdText);
            }
            else if ((cmdText != null) && (connection != null) && (transaction == null))
            {
                return new SqlCommand(cmdText, (SqlConnection)connection);
            }
            else if ((cmdText != null) && (connection != null) && (transaction != null))
            {
                return new SqlCommand(cmdText, (SqlConnection)connection, (SqlTransaction)transaction);
            }
            else
            {
                return null;
            }			
		}

		#endregion
		
		#region DataReader
		
        //public IDataReader CreateDataReader(IDbCommand dbCommand)
        //{
        //    IDataReader dr = null;
			
        //    try
        //    {
        //        dr = dbCommand.ExecuteReader();
        //    }
        //    catch(SqlException sqlExc)
        //    {
        //        if(dr != null)
        //        {
        //            if(!dr.IsClosed)
        //                dr.Close();
					
        //            dr.Dispose();
        //        }

        //        throw new Exception(sqlExc.Message);
        //    }

        //    return dr;
        //}
		
		
        //public IDataReader CreateDataReader(IDbCommand dbCommand, System.Data.CommandBehavior dbCommandBehavior)
        //{
        //    IDataReader dr = null;
			
        //    try
        //    {
        //        dr = dbCommand.ExecuteReader(dbCommandBehavior);
        //    }
        //    catch(SqlException sqlExc)
        //    {
        //        if(dr != null)
        //        {
        //            if(!dr.IsClosed)
        //                dr.Close();
					
        //            dr.Dispose();
        //        }

        //        throw new Exception(sqlExc.Message);
        //    }

        //    return dr;
        //}

		
		#endregion
		
		#region Connection
		
		public IDbConnection CreateConnection(string connectionString = null)
		{
			if(connectionString == null)
				return new SqlConnection();
			else
                return new SqlConnection(connectionString);
		}		

		#endregion

		#region DataAdapter
		
		public IDbDataAdapter CreateDataAdapter(IDbCommand selectCommand = null)
		{
			if(selectCommand == null)
                return new SqlDataAdapter();
            else 
				return new SqlDataAdapter((SqlCommand)selectCommand);			
		}

		
		public IDbDataAdapter CreateDataAdapter(string selectCommandText, string selectConnectionString)
		{
			return new SqlDataAdapter(selectCommandText,selectConnectionString);

		}

		
		public IDbDataAdapter CreateDataAdapter(string selectCommandText, IDbConnection selectConnection)
		{
			return new SqlDataAdapter(selectCommandText,(SqlConnection)selectConnection);
		}

		
		#endregion

        #region Transation

        public IDbTransaction CreateTransaction(IDbConnection connection, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return (SqlTransaction)connection.BeginTransaction(isolationLevel);
        }

        #endregion Transaction      

    }
    #endregion SqlServerDal

    #region SqlCeDal
    internal class SqlCeDal : IDal
    {

        #region Command
       
        public IDbCommand CreateCommand(string cmdText = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if ((cmdText == null) && (connection == null) & (transaction == null))
                return new SqlCeCommand(cmdText, (SqlCeConnection)connection, (SqlCeTransaction)transaction);
            else if ((cmdText != null) && (connection == null) & (transaction == null))
                return new SqlCeCommand(cmdText);
            else if ((cmdText != null) && (connection != null) & (transaction == null))
                return new SqlCeCommand(cmdText, (SqlCeConnection)connection);
            else if ((cmdText != null) && (connection != null) & (transaction != null))
                return new SqlCeCommand(cmdText, (SqlCeConnection)connection, (SqlCeTransaction)transaction);
            else
                return null;
        }


        #endregion

        #region DataReader

        //public IDataReader CreateDataReader(IDbCommand dbCommand)
        //{
        //    IDataReader dr = null;

        //    try
        //    {
        //        dr = dbCommand.ExecuteReader();
        //    }
        //    catch (SqlCeException sqlExc)
        //    {
        //        if (dr != null)
        //        {
        //            if (!dr.IsClosed)
        //                dr.Close();

        //            dr.Dispose();
        //        }

        //        throw new Exception(sqlExc.Message);
        //    }

        //    return dr;
        //}


        //public IDataReader CreateDataReader(IDbCommand dbCommand, System.Data.CommandBehavior dbCommandBehavior)
        //{
        //    IDataReader dr = null;

        //    try
        //    {
        //        dr = dbCommand.ExecuteReader(dbCommandBehavior);
        //    }
        //    catch (SqlCeException sqlExc)
        //    {
        //        if (dr != null)
        //        {
        //            if (!dr.IsClosed)
        //                dr.Close();

        //            dr.Dispose();
        //        }

        //        throw new Exception(sqlExc.Message);
        //    }

        //    return dr;
       //}


        #endregion

        #region Connection

        public IDbConnection CreateConnection(string connectionString = null)
        {
            if (connectionString == null)
                return new SqlCeConnection();
            else
                return new SqlCeConnection(connectionString);
        }

        #endregion

        #region DataAdapter

        
        public IDbDataAdapter CreateDataAdapter(IDbCommand selectCommand = null)
        {
            if( selectCommand == null)
                return new SqlCeDataAdapter();
            else
                return new SqlCeDataAdapter((SqlCeCommand)selectCommand);            
        }


        public IDbDataAdapter CreateDataAdapter(string selectCommandText, string selectConnectionString)
        {
            return new SqlCeDataAdapter(selectCommandText, selectConnectionString);  

        }

        public IDbDataAdapter CreateDataAdapter(string selectCommandText, IDbConnection selectConnection)
        {
           return new SqlCeDataAdapter(selectCommandText, (SqlCeConnection)selectConnection);            
        }


        #endregion

        #region Transation
       
        public IDbTransaction CreateTransaction(IDbConnection connection, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
           return (SqlCeTransaction)connection.BeginTransaction(isolationLevel);            
        }

        #endregion Transaction       

    }
    #endregion SqlCeDal


    public class DataAccessLayer: MarshalByRefObject, IDalFactory
	{
        bool _ExplicitType;
        public IDal Instance = null;      

        public DataAccessLayer()
        {
            Instance = CreateDAL(DatabaseTypes.SqlCe);
            _ExplicitType = false;
        }

        public DataAccessLayer(DatabaseTypes databaseType)
        {
            Instance = CreateDAL(databaseType);

        }

        public IDal CreateDAL(DatabaseTypes databaseType)
        {
            IDal myDal = null;
            switch (databaseType)
            {
                case DatabaseTypes.SqlCe:
                    myDal = new SqlCeDal();
                    break;
                case DatabaseTypes.SqlServer:
                    myDal = new SqlServerDal();
                    break;
                case DatabaseTypes.Oracle:
                    myDal = new OracleDal();
                    break;
                //case databaseType.DB2iSeries 
                //    myDal = new SqlServer2000Dal();
                //    break;

            }
            return myDal;
        } 
        
        public static void CompactSqlCe(string connectionString)
        {
            SqlCeEngine engine = new SqlCeEngine(connectionString);
            engine.Compact(connectionString);
        }       
		
	}


	public class StoredProcedure: MarshalByRefObject, IStoredProcedure, IDisposable 
	{
		private IDal m_dal = null;
		private bool m_disposed = false;
		private IDbCommand m_cmd = null;
		private string m_name = String.Empty;
		private bool m_cacheDisconnectData = false;
		private string m_connectionString = String.Empty;
		private SortedList m_parameters = new SortedList();
		
		#region Public
		
		#region Public Methods
		
		#region Overrides

		public override int GetHashCode()
		{
			controlIfDisposed();
			return base.GetHashCode ();
		}


		public override string ToString()
		{
			controlIfDisposed();
			return base.ToString ();
		}

		
		public override bool Equals(object obj)
		{
			controlIfDisposed();
			return base.Equals (obj);
		}
		
		
		#endregion
		
		#region Parameters
		
		public void ClearParameters()
		{
			controlIfDisposed();
			
			m_parameters.Clear();
			m_cmd.Parameters.Clear();
		}
		
		
		public IDbDataParameter CreateParameter()
		{
			controlIfDisposed();

			return m_cmd.CreateParameter();
		}
		
		
		public void AddParameter(IDbDataParameter parameter)
		{
			controlIfDisposed();

			m_parameters.Add(parameter.ParameterName + parameter.Value.ToString(),parameter);
		}

		
		public void AddParameters(IDbDataParameter[] parameters)
		{
			for(int i = 0; i < parameters.Length; i++)
				AddParameter(parameters[i]);
		}
		
		
		#endregion

		
		/// <summary>
		/// Enable to dispose itself
		/// </summary>
		public void Dispose() 
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		
		public int ExecuteNonQuery()
		{
			int result = 0;
			
			coreSettings();
			
			try
			{
				if(m_cmd.Connection.State != ConnectionState.Open)
					m_cmd.Connection.Open();
				
				result = m_cmd.ExecuteNonQuery();
			}
			catch
			{
				result = -1;
			}
			finally
			{
				if(m_cmd.Connection != null && m_cmd.Connection.State == ConnectionState.Open)
					m_cmd.Connection.Close();
			}

			return result;
		}

		
		public object ExecuteScalar()
		{
			return manageCaching(operationType.GetScalar);
		}

		
		public IDataReader CreateDataReader()
		{
			IDataReader dr = null;
			
			coreSettings();
			
			try
			{
				if(m_cmd.Connection.State != ConnectionState.Open)
					m_cmd.Connection.Open();
				
				dr = m_cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch(Exception exc)
			{
				Dispose(true);

				throw new Exception(exc.Message);
			}
			
			return dr;
		}

		
		/// <summary>
		/// Create a ResultSets.
		/// </summary>
		/// <returns>Return DataSet<./returns>
		public DataSet GetResultSet()
		{
			return (DataSet)manageCaching(operationType.GetResultSet);
			
			//DataSet ds = new DataSet("DS");
			
			//			coreSettings();
			//
			//			try
			//			{
			//				IDbDataAdapter da = m_dal.CreateDataAdapter(m_cmd);
			//				da.Fill(ds);
			//			}
			//			catch(Exception exc)
			//			{
			//				Dispose(true);
			//
			//				throw new Exception(exc.Message);
			//			}
			//			finally
			//			{
			//				if(m_cmd.Connection != null && m_cmd.Connection.State == ConnectionState.Open)
			//					m_cmd.Connection.Close();
			//			}
			//			
			//			return ds;
		}		
		
		#endregion

		#region Public Properties

		public string Name
		{
			get
			{
				controlIfDisposed();
				return m_name;
			}
			set
			{
				controlIfDisposed();
				m_name = value;
			}
		}

		
		public string ConnectionString
		{
			get
			{
				controlIfDisposed();
				return m_connectionString;
			}
			set
			{
				controlIfDisposed();
				m_connectionString = value;
			}
		}

		
		public bool CacheDisconnectData
		{
			get
			{
				controlIfDisposed();
				return m_cacheDisconnectData;
			}
			set
			{
				controlIfDisposed();
				m_cacheDisconnectData = value;
			}
		}

		
		#endregion

		#endregion

		#region Private
		
		#region Private Methods
		
		/// <summary>
		/// Set core stored procedure settings.
		/// </summary>
		private void coreSettings()
		{
			controlIfDisposed();
			
			try
			{
				if(m_cmd.Connection.State == ConnectionState.Closed)
					m_cmd.Connection.ConnectionString = m_connectionString;
			}
			catch
			{
				if(m_cmd.Connection != null)
					m_cmd.Connection.Dispose();

				m_cmd.Dispose();
			
				throw new Exception("Wrong connection string !!!");	
			}

			// Set stored procedure's name. 
			m_cmd.CommandText = m_name;

			// Add parameters from m_parameters (SortedList) to command.
			addParameters(m_parameters);	
		}

		
		/// <summary>
		/// If object is disposed throw an exception. 
		/// </summary>
		private void controlIfDisposed()
		{
			if(m_disposed)
				throw new Exception("You can't use StoredProcedure object because it has been disposed !!!");
		}
		
		
		/// <summary>
		/// Dispose StoredProcedures; resources.
		/// </summary>
		/// <param name="disposing">If false dispose unmanaged resources else dispose managed resources.</param>
		protected void Dispose(bool disposing) 
		{
			if(!m_disposed)
			{
				if(m_cmd != null)
				{
					// If disposing is true dispose managed resources
					if(disposing)
						if(m_cmd.Connection != null)
						{
							if(m_cmd.Connection.State == ConnectionState.Open)
								m_cmd.Connection.Close();
				
							m_cmd.Connection.Dispose();	
						}
				
					// Dispose unmanaged resources
					
					try
					{
						m_cmd.Dispose();
					}
					catch(Exception exc)
					{
						string excep = exc.Message;
					}

					m_cmd = null;
				}
			}

			m_disposed = true;
		}
		
		
		/// <summary>
		/// Create a unique identifier.
		/// </summary>
		/// <param name="oType">Operation Type (GetDataAdapter or GetScalar).</param>
		/// <returns>Return a unique identifier.</returns>
		private string getKey(operationType oType)
		{
			StringBuilder key = new StringBuilder(100);
			
			// Add stored procedure name to key.
			key.Append(m_name);

			foreach(DictionaryEntry parameter in m_parameters)
			{
				// Add parameter name - value to key.
				key.Append(parameter.Key.ToString());
			}

			
			// Add operation type to key.
			key.Append(oType.ToString());
			
			return key.ToString();
		}


		/// <summary>
		/// Add stored procedure's parameters to IDataParameterCollection
		/// </summary>
		/// <param name="parameters">IDbDataParameter's sorted list</param>
		private void addParameters(SortedList parameters)
		{
			m_cmd.Parameters.Clear();
			
			foreach(DictionaryEntry parameter in parameters)
				m_cmd.Parameters.Add(parameter.Value);
		}


		/// <summary>
		/// Get a disconnected object from cache if caching has been required, from db otherwise. 
		/// </summary>
		/// <param name="oType">Operation Type (GetDataAdapter or GetScalar).</param>
		/// <returns>Return a disconnected object.</returns>
		private object manageCaching(operationType oType)
		{
			string key = String.Empty;
			object returnedObject = null;

			coreSettings();
			
			//Is caching required?
			if(m_cacheDisconnectData == true)
			{
				//Get unique identifier.
				key = getKey(oType);
				
				//Get object from cache.
				returnedObject = System.Web.HttpRuntime.Cache.Get(key);
				
				//Is object found ?
				if(returnedObject == null)
				{
					//Create and cache a new disconnected object.
					returnedObject = getDisconnectObject(oType);
					System.Web.HttpRuntime.Cache.Insert(key, returnedObject, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(1800));
				}
			}
			else
				returnedObject = getDisconnectObject(oType);

			return returnedObject; 
		}


		/// <summary>
		/// Get a disconnected object.
		/// </summary>
		/// <param name="oType">Operation Type (GetDataAdapter or GetScalar).</param>
		/// <returns>Return a disconnected object.</returns>
		private object getDisconnectObject(operationType oType)
		{
			object returnedObject = null;
			
			try
			{
				if(m_cmd.Connection.State != ConnectionState.Open)
					m_cmd.Connection.Open();

				switch(oType)
				{
					case operationType.GetScalar: // Return a new Scalar object.
						returnedObject = m_cmd.ExecuteScalar(); 
						break;
					case operationType.GetResultSet: // Return a new ResultSet.
						returnedObject = new DataSet();
						m_dal.CreateDataAdapter(m_cmd).Fill((DataSet)returnedObject);
						break;
				}
			}
			finally
			{
				if(m_cmd.Connection != null && m_cmd.Connection.State == ConnectionState.Open)
					m_cmd.Connection.Close();
			}

			return returnedObject;
		}
		

		#endregion

		private enum operationType{GetScalar,GetResultSet}
		
		
		#endregion
		
		#region Constructor and Destructor
		
		public StoredProcedure(IDal dal)
		{
			if(dal == null)
				throw new Exception("Savet.Framework.Data.StoredProcedure - IDal parameter cannot be null.");
			
			m_dal = dal;
			m_cmd = m_dal.CreateCommand();
			m_cmd.Connection = m_dal.CreateConnection();
			m_cmd.CommandType = CommandType.StoredProcedure;
		}


		~StoredProcedure()
		{
			Dispose(false);
		}

		
		#endregion
		
	}
		
}





