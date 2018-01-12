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
using System.Data;
using Eyedia.Core;
using System.Text.RegularExpressions;

namespace Eyedia.IDPE.Services
{
    public class RuleReferenceExtractor
    {
        public RuleReferenceExtractor(string ruleAsXaml)
        {
            RuleAsXaml = ruleAsXaml;
            AttributeNames = new List<string>();
            SystemAttributeNames = new List<string>();
            ProcessVariablesIncorrectWay = new List<string>();
            ProcessVariables = new List<string>();
        }

        public string RuleAsXaml { get; private set; }
        public List<string> AttributeNames { get; private set; }
        public List<string> SystemAttributeNames { get; private set; }
        public List<string> ProcessVariablesIncorrectWay { get; private set; }
        public List<string> ProcessVariables { get; private set; }

        public void Parse()
        {
            using (StringReader reader = new StringReader(RuleAsXaml))
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

            AttributeNames = AttributeNames.DistinctEx();
            SystemAttributeNames = SystemAttributeNames.DistinctEx();
            ProcessVariablesIncorrectWay = ProcessVariablesIncorrectWay.DistinctEx();
            ProcessVariables = ProcessVariables.DistinctEx();
        }

        const string prefix = "srs";
        private void Collect(string line)
        {
            List<string> activityNames = new List<string>(new string[] { "AddError", "AddOrUpdateProcessVariable", "AssignAttribute" ,"WriteTraceToCurrentRow","PreValidationFailed" , "DuplicateCheck", 
                        "RestApiCall" ,"Lookup","PersistVariable", "GetPersistVariable","DeletePersistVariable" ,"ExecuteNonQuery"});
            string startedWith = "<{0}:{1}";
            string whatTypeOfActivity = string.Empty;

            foreach (string activityName in activityNames)
            {
                if (line.Trim().StartsWith(string.Format(startedWith, prefix, activityName)))
                {
                    whatTypeOfActivity = activityName;
                    break;
                }
            }

            //if (line.Trim().StartsWith("    <srs:AssignAttribute"))
            //    Debugger.Break();

            string attributeName = Extract(line, "Data.CurrentRow.Columns(&quot;", "&quot;");
            //if (attributeName.Contains("BookingCustomerId"))
            //    Debugger.Break();

            if(!string.IsNullOrEmpty(attributeName))
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

            if (whatTypeOfActivity == "Lookup")
            {
                string columName = Extract(line, "ColumnName=\"", "\"");
                string isSytemColumn =  Extract(line, "IsSystemColumn=\"" , "\"");
                if (!string.IsNullOrEmpty(columName))
                {
                    if(isSytemColumn.ToLower() == "true")
                        SystemAttributeNames.Add(columName);
                    else
                        AttributeNames.Add(columName);
                }
            }
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


