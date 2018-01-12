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
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Data.Linq;
using System.Collections;

namespace Eyedia.Core
{
    public class PrintObject
    {
        public PrintObject()
        {
            sb = new StringBuilder();
        }
        StringBuilder sb;
        public void PrintProperties(object obj, string fileName)
        {
            PrintProperties(obj, 0);
            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(sb.ToString());
            sw.Close();
        }

        void PrintProperties(object obj, int indent)
        {
            if (obj == null) return;
            string indentString = new string(' ', indent);
            Type objType = obj.GetType();
            if (objType == typeof(string))
                return;

            PropertyInfo[] properties = objType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object propValue = property.GetValue(obj, null);
                if (propValue == null)
                    continue;

                if (property.PropertyType.Assembly == objType.Assembly)
                {
                    sb.AppendLine(string.Format("{0}{1}:", indentString, property.Name));
                    PrintProperties(propValue, indent + 2);
                }
                else if (property.PropertyType.IsGenericType
                    && ((property.PropertyType.GetGenericTypeDefinition() == typeof(List<>)) || (property.PropertyType.GetGenericTypeDefinition() == typeof(EntitySet<>))))
                {
                    sb.AppendLine(string.Format("{0}{1}:", indentString, property.Name));
                    IList collection = (IList)propValue;
                    foreach (var item in collection)
                    {
                        if (item is string)
                        {
                            sb.AppendLine(string.Format("{0}{1}", indentString, item.ToString()));
                        }
                        else
                        {
                            PrintProperties(item, indent + 4);
                        }
                    }
                }

                else
                {
                    sb.AppendLine(string.Format("{0}{1}: {2}", indentString, property.Name, propValue));
                }
            }
        }
    }
}



