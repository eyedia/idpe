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
using Eyedia.Core;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;
using System.Text.RegularExpressions;

namespace Eyedia.IDPE.Services
{
    public class DataSourceCSharpCodeReferenceExtractor
    {
        public DataSourceCSharpCodeReferenceExtractor(int dataSourceId, SreKeyTypes sreKeyType)
        {
            this.DataSourceId = dataSourceId;
            Key = new Manager().GetKey(DataSourceId, sreKeyType.ToString());

            AttributeNames = new List<string>();
            SystemAttributeNames = new List<string>();
            ProcessVariablesIncorrectWay = new List<string>();
            ProcessVariables = new List<string>();
        }

        public SreKey Key { get; private set; }
        public int DataSourceId { get; set; }
        public List<string> AttributeNames { get; private set; }
        public List<string> SystemAttributeNames { get; private set; }
        public List<string> ProcessVariablesIncorrectWay { get; private set; }
        public List<string> ProcessVariables { get; private set; }

        public void Parse()
        {
            ParseKey();
            //ParseKey(new Manager().GetKey(DataSourceId, SreKeyTypes.CSharpCodeOutputWriter.ToString()));

            AttributeNames = AttributeNames.DistinctEx();
            SystemAttributeNames = SystemAttributeNames.DistinctEx();
            ProcessVariablesIncorrectWay = ProcessVariablesIncorrectWay.DistinctEx();
            ProcessVariables = ProcessVariables.DistinctEx();
        }

        private void ParseKey()
        {            
            if (Key != null)
            {
                using (StringReader reader = new StringReader(Key.Value))
                {
                    string line = string.Empty;
                    do
                    {
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            Collect(line);
                        }

                    } while (line != null);
                }

            }
        }

         const string prefix = "srs";
         private void Collect(string line)
         {
             string attributeName = Extract(line, "Data.CurrentRow.Columns(&quot;", "&quot;");
   
             if (!string.IsNullOrEmpty(attributeName))
                 AttributeNames.Add(attributeName);

             string systemAttributeName = Extract(line, "Data.CurrentRow.ColumnsSystem(&quot;", "&quot;");
             if (!string.IsNullOrEmpty(systemAttributeName))
                 SystemAttributeNames.Add(systemAttributeName);

             string processVariableName = Extract(line, "Job.ProcessVariables(\"", "\"");
             if (!string.IsNullOrEmpty(processVariableName))
                 ProcessVariablesIncorrectWay.Add(processVariableName);

             processVariableName = Extract(line, "Job.ProcessVariables(&quot;", "&quot;");
             if (!string.IsNullOrEmpty(processVariableName))
                 ProcessVariablesIncorrectWay.Add(processVariableName);

             processVariableName = Extract(line, "Job.GetProcessVariableValue(\"", "\"");
             if (!string.IsNullOrEmpty(processVariableName))
                 ProcessVariables.Add(processVariableName);

             processVariableName = Extract(line, "Job.GetProcessVariableValue(&quot;", "&quot;");
             if (!string.IsNullOrEmpty(processVariableName))
                 ProcessVariables.Add(processVariableName);
         }


         private string Extract(string line, string startPattern, string endPattern)
         {
             startPattern = Regex.Escape(startPattern);
             endPattern = Regex.Escape(endPattern);
             Regex regex = new Regex(startPattern + @"(.*?)" + endPattern, RegexOptions.IgnoreCase);
             var v = regex.Match(line);
             return v.Groups[1].Value;

         }

    }
}


