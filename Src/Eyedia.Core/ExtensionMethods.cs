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
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;


namespace Eyedia.Core
{
    public static class ExtensionMethods
    {

        /// <summary>     /// Perform a deep Copy of the object.     
        /// </summary>     
        /// <typeparam name="T">The type of object being copied.</typeparam>     
        /// <param name="source">The object instance to copy.</param>     
        /// <returns>The copied object.</returns>     
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("The type must be serializable.", "source");

            // Don't serialize a null object, simply return the default for that object         
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Extension method to serialize any object (except List)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(this T obj)
        {
            var serializer = new DataContractSerializer(obj.GetType());
            using (var writer = new StringWriter())
            using (var stm = new XmlTextWriter(writer))
            {
                serializer.WriteObject(stm, obj);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Extension method to deserialize any object (except list)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serialized"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this string serialized)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var reader = new StringReader(serialized))
            using (var stm = new XmlTextReader(reader))
            {
                return (T)serializer.ReadObject(stm);
            }
        }

        /// <summary>
        /// Extension method to serialize a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeList<T>(this T list)
        {
            var serializer = new XmlSerializer(list.GetType());

            using (var ms = new MemoryStream())
            {
                serializer.Serialize(ms, list);
                return new ASCIIEncoding().GetString(ms.ToArray());
            }

        }

        public static T DeserializeList<T>(this string xml)
        {
            var xs = new XmlSerializer(typeof(T));
            return (T)xs.Deserialize(new StringReader(xml));
        }


        public static SourceLevels TraceLevel(this TraceListener traceListener)
        {
            return ((EventTypeFilter)traceListener.Filter).EventType;
        }

        public static string ToLine(this List<string> listOfStrings)
        {
            if (listOfStrings == null) return "NULL";
            return String.Join(Environment.NewLine, listOfStrings.ToArray());
        }

        public static string ToLine(this List<string> listOfStrings, bool degreeDelimiter)
        {
            if (listOfStrings == null) return "NULL";
            if (!degreeDelimiter)
                return ToLine(listOfStrings);

            return String.Join("°", listOfStrings.ToArray());
        }

        public static string ToLine(this List<string> listOfStrings, string delimiter)
        {
            if (listOfStrings == null) return "NULL";

            string returnString = string.Empty;
            foreach (string line in listOfStrings)
            {
                returnString = returnString + line + delimiter;
            }
            return returnString.Length > delimiter.Length ? returnString.Substring(0, returnString.Length - delimiter.Length) : returnString;
        }

        public static List<string> DistinctEx(this List<string> listOfStrings)
        {
            if (listOfStrings == null) return new List<string>();
            List<string> noDupes = new List<string>(new HashSet<string>(listOfStrings));
            listOfStrings.Clear();
            listOfStrings.AddRange(noDupes);
            return listOfStrings;
        }

        public static List<string> RemoveEmptyStrings(this List<string> listOfStrings)
        {
            if (listOfStrings == null) return new List<string>();
            List<string> noEmpties = (from s in listOfStrings
                                      where s != string.Empty
                                      select s).ToList<string>();

            listOfStrings.Clear();
            listOfStrings.AddRange(noEmpties);
            return listOfStrings;
        }

        public static string ToDBDateFormat(this DateTime date)
        {
            if (date == null) return "19000101";
            return date.ToString("yyyyMMdd");
        }

        public static string ToDBDateTimeFormat(this DateTime date)
        {
            if (date == null) return "01/01/1900 00:00:00";
            return date.ToShortDateString() + " " + date.ToLongTimeString();
        }

        public static string YYYYDDMMPrefix(this string str)
        {
            try
            {
                if (str.Length == 8)
                {
                    int year = int.Parse(str.Substring(0, 4));
                    int month = int.Parse(str.Substring(4, 2));
                    int day = int.Parse(str.Substring(6, 2));

                    return new DateTime(year, month, day).ToString();
                }
            }
            catch { }
            return DateTime.Now.ToString();
        }

        public static bool IsNumeric(this string str)
        {
            try
            {
                char[] chars = str.ToCharArray();
                foreach (char chr in chars)
                {
                    if (!Char.IsNumber(chr))
                        return false;
                }
                return true;
            }
            catch { return false; }
        }




        /// <summary>
        /// Returns longest string from a list of strings.
        /// </summary>
        /// <param name="listOfStrings"></param>
        /// <returns></returns>
        public static string GetLongestString(this string[] listOfStrings)
        {
            return listOfStrings.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
        }

        public static bool ParseBool(this string strBool)
        {
            bool result = false;
            return ParseBool(strBool, ref result);
        }

        public static bool ParseBool(this string strBool, ref bool result)
        {
            if (string.IsNullOrEmpty(strBool))
                return false;
            else if (strBool == "1")
                return true;
            else if (strBool == "0")
                return false;

            bool boolResult = false;
            result = bool.TryParse(strBool, out boolResult);
            return boolResult;
        }

        /// <summary>
        /// These methods are created to avoid using try parse whenever we need integer value for sure. If you intention is to have null value, avoid using these extension methods
        /// </summary>
        /// <param name="strInt"></param>
        /// <returns></returns>
        public static int? ParseInt(this string strInt)
        {
            bool result = false;
            return ParseInt(strInt, ref result);
        }

        public static int? ParseInt(this string strInt, ref bool result)
        {
            int intResult = 0;
            result = int.TryParse(strInt, out intResult);
            return intResult;
        }

        public static long? ParseLong(this string strLong)
        {
            bool result = false;
            return ParseLong(strLong, ref result);
        }

        public static long? ParseLong(this string strLong, ref bool result)
        {
            long longResult = 0;
            result = long.TryParse(strLong, out longResult);
            return longResult;
        }

        public static double? ParseDouble(this string strDouble)
        {
            bool result = false;
            return ParseDouble(strDouble, ref result);
        }

        public static double? ParseDouble(this string strDouble, ref bool result)
        {
            double doubleResult = 0;
            result = double.TryParse(strDouble, out doubleResult);
            return doubleResult;
        }

        public static decimal? ParseDecimal(this string strDecimal)
        {
            bool result = false;
            return ParseDecimal(strDecimal, ref result);
        }

        public static decimal? ParseDecimal(this string strDecimal, ref bool result)
        {
            decimal decimalResult = 0;
            result = decimal.TryParse(strDecimal, out decimalResult);
            return decimalResult;
        }

        public static DateTime? ParseDateTime(this string strDateTime)
        {
            bool result = false;
            return ParseDateTime(strDateTime, ref result);
        }

        public static DateTime? ParseDateTime(this string strDateTime, ref bool result)
        {
            if (string.IsNullOrEmpty(strDateTime)) return DateTime.MinValue;

            DateTime dateTimeResult = DateTime.MinValue;
            if (!(strDateTime.Contains("/")) && !(strDateTime.Contains("-")) && !(strDateTime.Contains(":")))
            {
                if (strDateTime.Length == 8)
                {
                    result = true;
                    return new DateTime(int.Parse(strDateTime.Substring(0, 4)), int.Parse(strDateTime.Substring(4, 2)), int.Parse(strDateTime.Substring(6, 2)));
                }
            }
            else
            {
                result = DateTime.TryParse(strDateTime, out dateTimeResult);
            }
            return dateTimeResult;
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static bool Filter(this LastDurations duringLast, DateTime dtTime)
        {
            switch (duringLast)
            {
                case LastDurations.Last15Minutes:
                    return dtTime > DateTime.Now.AddMinutes(-15);
                case LastDurations.Last30Minutes:
                    return dtTime > DateTime.Now.AddMinutes(-30);
                case LastDurations.Last45Minutes:
                    return dtTime > DateTime.Now.AddMinutes(-45);
                case LastDurations.Last1Hour:
                    return dtTime > DateTime.Now.AddHours(-1);
                case LastDurations.Last2Hours:
                    return dtTime > DateTime.Now.AddHours(-2);
                case LastDurations.Last3Hours:
                    return dtTime > DateTime.Now.AddHours(-3);
                case LastDurations.Last4Hours:
                    return dtTime > DateTime.Now.AddHours(-4);
                case LastDurations.Last5Hours:
                    return dtTime > DateTime.Now.AddHours(-5);
                case LastDurations.Last6Hours:
                    return dtTime > DateTime.Now.AddHours(-6);
                case LastDurations.Last7Hours:
                    return dtTime > DateTime.Now.AddHours(-7);
                case LastDurations.Last8Hours:
                    return dtTime > DateTime.Now.AddHours(-8);
                case LastDurations.Last9Hours:
                    return dtTime > DateTime.Now.AddHours(-9);
                case LastDurations.Last10Hours:
                    return dtTime > DateTime.Now.AddHours(-10);
                case LastDurations.Last11Hours:
                    return dtTime > DateTime.Now.AddHours(-11);
                case LastDurations.Last12Hours:
                    return dtTime > DateTime.Now.AddHours(-12);
                case LastDurations.Last24Hours:
                    return dtTime > DateTime.Now.AddHours(-24);
                case LastDurations.Last1Week:
                    return dtTime > DateTime.Now.AddDays(-7);
                case LastDurations.Last2Weeks:
                    return dtTime > DateTime.Now.AddDays(-14);
                case LastDurations.Last3Weeks:
                    return dtTime > DateTime.Now.AddDays(-21);
                case LastDurations.Last1Month:
                    return dtTime > DateTime.Now.AddMonths(-1);
                case LastDurations.Last2Months:
                    return dtTime > DateTime.Now.AddMonths(-2);
                case LastDurations.Last3Months:
                    return dtTime > DateTime.Now.AddMonths(-3);
                case LastDurations.Last4Months:
                    return dtTime > DateTime.Now.AddMonths(-4);
                case LastDurations.Last5Months:
                    return dtTime > DateTime.Now.AddMonths(-5);
                case LastDurations.Last6Months:
                    return dtTime > DateTime.Now.AddMonths(-6);
                case LastDurations.Last7Months:
                    return dtTime > DateTime.Now.AddMonths(-7);
                case LastDurations.Last8Months:
                    return dtTime > DateTime.Now.AddMonths(-8);
                case LastDurations.Last9Months:
                    return dtTime > DateTime.Now.AddMonths(-9);
                case LastDurations.Last10Months:
                    return dtTime > DateTime.Now.AddMonths(-10);
                case LastDurations.Last11Months:
                    return dtTime > DateTime.Now.AddMonths(-11);
                case LastDurations.Last12Months:
                    return dtTime > DateTime.Now.AddYears(-1);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Tries to find closest match against list of strings
        /// </summary>
        /// <param name="source">The source string</param>
        /// <param name="targetList">List of target strings</param>
        /// <param name="threshold">Threshold, higher the threshold lesser match. 0 = Exact match</param>
        /// <returns></returns>
        public static string TryMatchUsingLevenshteinDistance(this string source, List<string> targetList, int threshold = 3)
        {
            List<LevenshteinDistanceInfo> distances = new List<LevenshteinDistanceInfo>();

            foreach (string target in targetList)
            {
                LevenshteinDistanceInfo info = new LevenshteinDistanceInfo();
                info.Text = target;
                info.Distance = LevenshteinDistance.Compute(source, info.Text);
                distances.Add(info);
            }

            LevenshteinDistanceInfo minDistance = distances.Aggregate((c, d) => c.Distance < d.Distance ? c : d);
            return (minDistance.Distance <= threshold) ? minDistance.Text : string.Empty;
        }

        public static string FormatMemorySize(this double memoryLength)
        {
            return FormatMemorySizeInternal(memoryLength);
        }

        public static string FormatMemorySize(this long memoryLength)
        {
            return FormatMemorySizeInternal(memoryLength);
        }

        private static string FormatMemorySizeInternal(double memoryLength)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };

            int order = 0;
            while (memoryLength >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                memoryLength = memoryLength / 1024;
            }

            return (Math.Round(memoryLength, 2) + sizes[order]);
        }

        public static byte[] GetByteArray(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(this byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static T? GetValueOrNull<T>(this string valueAsString) where T : struct
        {
            if (string.IsNullOrEmpty(valueAsString))
                return null;
            return (T)Convert.ChangeType(valueAsString, typeof(T));
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValueProvider)
        {
            TValue value;
            if (defaultValueProvider == null)
            {
                if (dictionary.TryGetValue(key, out value))
                    return value;
                else
                    return default(TValue);
            }
            else
            {
                return dictionary.TryGetValue(key, out value) ? value
                     : defaultValueProvider();
            }
        }

        public static string GetSdfFileName(this System.Configuration.ConnectionStringSettings connectionStringSetting)
        {
            string sdfFileName = string.Empty;
            var m = new System.Text.RegularExpressions.Regex("='(.*)';").Match(connectionStringSetting.ConnectionString);
            if (m.Groups.Count > 0)
                sdfFileName = m.Groups[1].ToString();
            return sdfFileName;
        }
        public static string GetSqlServerDatabaseName(this System.Configuration.ConnectionStringSettings connectionStringSetting)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.ConnectionString = connectionStringSetting.ConnectionString;
            return builder.InitialCatalog;
        }

        public static string GetSqlServerName(this System.Configuration.ConnectionStringSettings connectionStringSetting)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.ConnectionString = connectionStringSetting.ConnectionString;
            return builder.DataSource;
        }

    }
}





