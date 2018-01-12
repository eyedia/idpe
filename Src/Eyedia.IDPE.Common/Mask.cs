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
using System.Text;
using CultureInfo = System.Globalization.CultureInfo;

/// <summary>
/// Mask class to provide various utility static helper methods to help most common activities
/// like NullString, EmptyString, SQLDateTime, NullableToGeneric, etc.
/// This class is made partial so that developer can add additional methods in to the same class.
/// </summary>
public partial class Mask
{
    /// <summary>
    /// Converts Nullable data type to Generic data type
    /// </summary>
    /// <typeparam name="TVal"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TVal NullableToGeneric<TVal>(object value)
    {
        TVal r = default(TVal);
        if (value == null)
        {
            if (typeof(TVal).Equals(typeof(DateTime)))
            {
                return (TVal)Convert.ChangeType(new DateTime(1753, 1, 1), typeof(DateTime));
            }

            else if (typeof(TVal).Equals(typeof(string)))
            {
                return (TVal)Convert.ChangeType("", typeof(string));
            }


            return r;
        }
        if (value != DBNull.Value)
        {
            r = (TVal)Convert.ChangeType(value, typeof(TVal));
        }
        return r;
    }
    

    /// <summary>
    /// Masks nullable string to empty string, else returns string itself.
    /// </summary>
    /// <param name="str">String to check</param>
    /// <returns></returns>
    public static string NullString(string str)
    {
        return str == null ? string.Empty : str;
    }

    /// <summary>
    /// Fills string with filler if string is NOT null or is NOT empty or string length is NOT 0    
    /// </summary>
    /// <param name="str">String to check</param>
    /// <param name="filler">Filler to return</param>
    /// <returns>string</returns>
    public static string EmptyString(string str, string filler)
    {
        return Mask.NullString(str).Length == 0 ? filler : str;
    }

    /// <summary>
    /// Checks if DateTime is a valid MS SQL DateTime.
    /// Returns 1/1/1753 12:00:00 AM if DateTime is equal to 1/1/0001 12:00:00 AM
    /// else returns passed DateTime
    /// </summary>
    /// <param name="DateTime">DateTime to Check</param>
    /// <returns>DateTime</returns>
    public static DateTime SQLDateTime(DateTime DateTime)
    {
        if (DateTime == DateTime.Parse("1/1/0001 12:00:00 AM"))
            return DateTime.Parse("1/1/1753 12:00:00 AM");
        return DateTime;
    }

    /// <summary>
    /// Converts strDateTime to valid DateTime.
    /// Returns 1/1/1753 12:00:00 AM if strDateTime is equal to 1/1/0001 12:00:00 AM
    /// else returns strDateTime
    /// </summary>
    /// <param name="strDateTime">DateTime as string to check</param>
    /// <returns>DateTime</returns>
    public static DateTime SQLDateTime(string strDateTime)
    {
        DateTime DtTm = new DateTime(1753, 1, 1);
        DtTm = DateTime.ParseExact(strDateTime, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

        if (DtTm == DateTime.Parse("1/1/0001 12:00:00 AM"))
            return DateTime.Parse("1/1/1753 12:00:00 AM");

        return DtTm;
    }

    /// <summary>
    /// Returns valid MS SQL DateTime 1/1/1753 12:00:00 AM
    /// </summary>
    /// <returns>DateTime</returns>
    public static DateTime SQLDateTime()
    {
        return DateTime.Parse("1/1/1753 12:00:00 AM");
    }
    
}






