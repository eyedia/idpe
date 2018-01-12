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
using System.ComponentModel;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Services
{
    public class CSharpCodeInformation
    {
        public enum CodeTypes
        {
            Unknown,
            ExecuteAndReturnString,
            OutputWriter,
            DataTableGeneratorFromFileContent,
            DataTableGeneratorFromFileName,
            Execute
        }        

        public CSharpCodeInformation(string rawString = null)
        {
            this.RawString = rawString;
        }

        [DefaultValue(CodeTypes.DataTableGeneratorFromFileContent)]
        public CodeTypes CodeType { get; set; }

        public string Code { get; set; }

        public string HelperMethods { get; set; }

        public string AdditionalUsingNamespace { get; set; }

        public string AdditionalReferences { get; set; }
        
        public string CompleteCode
        {
            get
            {
                switch (CodeType)
                {
                    case CodeTypes.ExecuteAndReturnString:
                        return new DynamicCodeExecutor(AdditionalUsingNamespace, HelperMethods).GenerateCodeSingleStatementReturnString(Code);

                    case CodeTypes.OutputWriter:
                        return new DynamicCodeExecutor(AdditionalUsingNamespace, HelperMethods).GenerateCodeOutputWriter(Code);

                    case CodeTypes.DataTableGeneratorFromFileContent:
                        return new DynamicCodeExecutor(AdditionalUsingNamespace, HelperMethods).GenerateCodeGenerateDataTableFromString(Code);

                    case CodeTypes.DataTableGeneratorFromFileName:
                        return new DynamicCodeExecutor(AdditionalUsingNamespace, HelperMethods).GenerateCodeGenerateDataTableFromFileName(Code);

                    case CodeTypes.Execute:
                        return new DynamicCodeExecutor(AdditionalUsingNamespace, HelperMethods).GenerateCodeExecute(Code);

                    default:
                        return string.Empty;

                }
            }
        }

        public const string __RawStringFormat = "{0}°{1}°{2}°{3}°{4}";
        public string RawString
        {
            get
            {
                //return CodeType + "°" + Code + "°" + HelperMethods + "°" + AdditionalUsingNamespace + "°" + AdditionalReferences;
                return string.Format(__RawStringFormat, 
                    CodeType, Code, HelperMethods, AdditionalUsingNamespace, AdditionalReferences);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                string[] completeInformation = value.Split("°".ToCharArray());
                if (completeInformation.Length != 5)
                    return;

                CodeType = (CodeTypes)Enum.Parse(typeof(CodeTypes), completeInformation[0]);
                Code = completeInformation[1];
                HelperMethods = completeInformation[2];
                AdditionalUsingNamespace = completeInformation[3];
                AdditionalReferences = completeInformation[4];

            }
        }

        public string[] GetReferencedAssemblies()
        {
            List<string> referencedAssemblies = new List<string>();

            referencedAssemblies.Add("System.Data.dll");
            referencedAssemblies.Add("System.Xml.dll");
            referencedAssemblies.Add("Eyedia.Core.dll");
            referencedAssemblies.Add("Eyedia.IDPE.Common.dll");
            referencedAssemblies.Add("Eyedia.IDPE.Services.dll");
            referencedAssemblies.Add("Eyedia.IDPE.DataManager.dll");

            if (!string.IsNullOrEmpty(AdditionalReferences))
            {
                string[] references = AdditionalReferences.Split(";".ToCharArray());
                foreach (string reference in references)
                {
                    if (!string.IsNullOrEmpty(reference))
                        referencedAssemblies.Add(reference);
                }
            }
            return referencedAssemblies.ToArray();
        }
    }
}


