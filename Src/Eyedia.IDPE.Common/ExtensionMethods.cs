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
using Symplus.RuleEngine.DataManager;
using Symplus.Core.Data;

namespace Symplus.RuleEngine.Common
{
    public static class ExtensionMethods
    {
        public static bool IsConnectionStringType(this SreKeyTypes type)
        {
            if ((type == SreKeyTypes.ConnectionStringOracle)
                                || (type == SreKeyTypes.ConnectionStringSqlCe)                                
                                || (type == SreKeyTypes.ConnectionStringSqlServer)
                                || (type == SreKeyTypes.ConnectionStringDB2iSeries))
                return true;

            return false;
        }

        public static bool IsConnectionStringType(this int type)
        {
            if ((type == (int)SreKeyTypes.ConnectionStringOracle)
                                || (type == (int)SreKeyTypes.ConnectionStringSqlCe)                                
                                || (type == (int)SreKeyTypes.ConnectionStringSqlServer)
                                || (type == (int)SreKeyTypes.ConnectionStringDB2iSeries))
                return true;

            return false;
        }

        public static DatabaseTypes GetDatabaseType(this SreKey key)
        {
            SreKeyTypes sreKeyType = (SreKeyTypes)key.Type;
            switch (sreKeyType)
            {
                case SreKeyTypes.ConnectionStringSqlServer:
                    return DatabaseTypes.SqlServer;

                case SreKeyTypes.ConnectionStringOracle:
                    return DatabaseTypes.Oracle;

                case SreKeyTypes.ConnectionStringDB2iSeries:
                    return DatabaseTypes.DB2iSeries;

                case SreKeyTypes.ConnectionStringSqlCe:
                    return DatabaseTypes.SqlCe;

                default:
                   throw new Exception(sreKeyType.ToString() + " is not database type key!");
            }

        }

        public static string GetKeyValue(this List<SreKey> keys, string key)
        {

            SreKey srekey = (from k in keys
                              where k.Name == key
                              select k).SingleOrDefault();
            if (srekey == null)
                return string.Empty;

            return string.IsNullOrEmpty(srekey.Value) ? string.Empty : srekey.Value;
        }

        public static string GetKeyValue(this List<SreKey> keys, SreKeyTypes keyType)
        {

            SreKey srekey = (from k in keys
                              where k.Name == keyType.ToString()
                              select k).SingleOrDefault();
            if (srekey == null)
                return string.Empty;

            return string.IsNullOrEmpty(srekey.Value) ? string.Empty : srekey.Value;
        }

        public static SreKey GetKey(this List<SreKey> keys, string key)
        {

            SreKey srekey = (from k in keys
                              where k.Name == key
                              select k).SingleOrDefault();

            return srekey;
        }

        public static string GetDefaultConnectionString(this List<SreKey> keys)
        {

            List<SreKey> srekeys = (from k in keys
                                    where k.Type.IsConnectionStringType() == true
                                    && k.IsDefault == true
                                    select k).ToList();

            return srekeys.Count > 0 ? srekeys[0].Value : string.Empty;
        }

        public static Type ConvertSreTypeIntoDotNetType(this SreAttribute attribute, DatabaseTypes databaseType)
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

        public static UserPreferences GetUserPreferences(this SymplusUser user)
        {
            return new UserPreferences(user.Preferences);
        }
        
    }
}




