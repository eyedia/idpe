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
using System.Activities;
using System.ComponentModel;
using Eyedia.Core;

namespace Eyedia.IDPE.Services
{
    [Designer(typeof(WorkflowActivities.ExecuteCSCode))]
    public sealed class ExecuteCSCode : CodeActivity
    {
        public InArgument<Job> Job { get; set; }
        public InArgument<WorkerData> Data { get; set; }
        public InArgument<string> Code { get; set; }
        public InArgument<object> Object { get; set; }
        public InArgument<string> ObjectType { get; set; }
        public InArgument<string> AdditionalUsingNamespace { get; set; }
        public InArgument<string> AdditionalReferences { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            Job job = context.GetValue(this.Job);
            if (job == null)
            {
                WorkerData data = context.GetValue(this.Data);
                data.ThrowErrorIfNull(this.DisplayName);
            }

            Object obj = context.GetValue(this.Object);
            string objectType = context.GetValue(this.ObjectType);
            string code = context.GetValue(this.Code);
            string additionalUsingNamespace = context.GetValue(this.AdditionalUsingNamespace);
            string additionalReferences = context.GetValue(this.AdditionalReferences);


            CSharpCodeInformation csharpCodeInformation = new CSharpCodeInformation();
            csharpCodeInformation.CodeType = CSharpCodeInformation.CodeTypes.Execute;
            csharpCodeInformation.Code = string.Format("(({0})obj).{1}", objectType, code);
            if (string.IsNullOrEmpty(additionalUsingNamespace))
                csharpCodeInformation.AdditionalUsingNamespace = additionalUsingNamespace;

            if (string.IsNullOrEmpty(additionalReferences))
                csharpCodeInformation.AdditionalReferences = "System.Linq.dll";

            ExecuteCSharpCode cSharpCodeExecutor = new ExecuteCSharpCode("Eyedia.IDPE.Common.CSharpCode", null,
                    ExecuteCSharpCode.CompilerVersions.v40, csharpCodeInformation.GetReferencedAssemblies());
            cSharpCodeExecutor.Execute(csharpCodeInformation.CompleteCode, "Execute", new object[] { obj });
        }
    }
}



