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
using System.Diagnostics;
using Eyedia.IDPE.DataManager;
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.DataManager
{
    public static class ExtensionMethods
    {
        public static bool IsConnectionStringType(this IdpeKeyTypes type)
        {
            if ((type == IdpeKeyTypes.ConnectionStringOracle)
                                || (type == IdpeKeyTypes.ConnectionStringSqlCe)                                
                                || (type == IdpeKeyTypes.ConnectionStringSqlServer)
                                || (type == IdpeKeyTypes.ConnectionStringDB2iSeries))
                return true;

            return false;
        }

        public static bool IsConnectionStringType(this int type)
        {
            if ((type == (int)IdpeKeyTypes.ConnectionStringOracle)
                                || (type == (int)IdpeKeyTypes.ConnectionStringSqlCe)                                
                                || (type == (int)IdpeKeyTypes.ConnectionStringSqlServer)
                                || (type == (int)IdpeKeyTypes.ConnectionStringDB2iSeries))
                return true;

            return false;
        }

        public static DatabaseTypes GetDatabaseType(this IdpeKey key)
        {
            IdpeKeyTypes idpeKeyType = (IdpeKeyTypes)key.Type;
            switch (idpeKeyType)
            {
                case IdpeKeyTypes.ConnectionStringSqlServer:
                    return DatabaseTypes.SqlServer;

                case IdpeKeyTypes.ConnectionStringOracle:
                    return DatabaseTypes.Oracle;

                case IdpeKeyTypes.ConnectionStringDB2iSeries:
                    return DatabaseTypes.DB2iSeries;

                case IdpeKeyTypes.ConnectionStringSqlCe:
                    return DatabaseTypes.SqlCe;

                default:
                   throw new Exception(idpeKeyType.ToString() + " is not database type key!");
            }

        }

        public static IdpeKeyTypes GetSreType(this DatabaseTypes databaseType)
        {            
            switch (databaseType)
            {
                case DatabaseTypes.SqlServer:
                    return IdpeKeyTypes.ConnectionStringSqlServer;

                case DatabaseTypes.Oracle:
                    return IdpeKeyTypes.ConnectionStringOracle;

                case DatabaseTypes.DB2iSeries:
                    return IdpeKeyTypes.ConnectionStringDB2iSeries;

                case DatabaseTypes.SqlCe:
                    return IdpeKeyTypes.ConnectionStringSqlCe;

                default:
                    throw new Exception(databaseType.ToString() + " is not sretype key!");
            }

        }

        public static string GetKeyValue(this List<IdpeKey> keys, string key)
        {
            if (keys == null)
                return string.Empty;

            List<IdpeKey> filteredKeys = (from k in keys
                                         where k.Name == key
                                         select k).ToList();

            IdpeKey idpekey = GetFirstItem(filteredKeys);

            if (idpekey == null)
                return string.Empty;

            return string.IsNullOrEmpty(idpekey.Value) ? string.Empty : idpekey.Value;
        }

        public static string GetKeyValue(this List<IdpeKey> keys, IdpeKeyTypes keyType)
        {
            if (keys == null)
                return string.Empty;

            List<IdpeKey> filteredKeys = (from k in keys
                              where k.Name == keyType.ToString()
                              select k).ToList();

            IdpeKey key = GetFirstItem(filteredKeys);
            if (key == null)
                return string.Empty;

            return string.IsNullOrEmpty(key.Value) ? string.Empty : key.Value;
        }

        public static IdpeKey GetKey(this List<IdpeKey> keys, string key)
        {
            if (keys == null)
                return null;

            List<IdpeKey> filteredKeys = (from k in keys
                                 where k.Name.Equals(key, StringComparison.OrdinalIgnoreCase)
                                 select k).ToList();

            return GetFirstItem(filteredKeys);

        }

        private static IdpeKey GetFirstItem(List<IdpeKey> keys)
        {
            if (keys.Count == 1)
            {
                return keys[0];
            }
            else if (keys.Count > 1)
            {
                Trace.TraceInformation("Duplicate key issue, found more than 1 key with same name.");
                return keys[0];
            }
            else
            {
                return null;
            }
        }

        public static string GetDefaultConnectionString(this List<IdpeKey> keys)
        {
            if (keys == null)
                return string.Empty;

            List<IdpeKey> srekeys = (from k in keys
                                    where k.Type.IsConnectionStringType() == true
                                    select k).ToList();

            return srekeys.Count > 0 ? srekeys[0].Value : string.Empty;
        }

        public static Type ConvertSreTypeIntoDotNetType(this IdpeAttribute attribute, DatabaseTypes databaseType)
        {
            switch (databaseType)
            {
                default:
                    switch (attribute.Type.ToUpper())
                    {
                        case "INT":
                            return typeof(int);

                        case "BIGINT":
                            return typeof(long);

                        case "DECIMAL":
                            return typeof(double);

                        case "BIT":
                            return typeof(bool);

                        case "GENERATED":
                        case "NOTREFERENCED":
                        case "REFERENCED":
                        case "CODESET":
                        case "STRING":
                            return typeof(string);

                        case "DATETIME":
                            return typeof(DateTime);

                        default:
                            return typeof(string);
                    }
            }
        }

        public static UserPreferences GetUserPreferences(this User user)
        {            
            return new UserPreferences(user.Preferences);
        }
        
    }
}




