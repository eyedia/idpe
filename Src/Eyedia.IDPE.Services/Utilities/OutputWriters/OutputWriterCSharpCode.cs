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
using System.Xml;
using System.Diagnostics;
using Eyedia.Core;

using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;


namespace Eyedia.IDPE.Services
{
    public class OutputWriterCSharpCode : OutputWriter
    {
        public OutputWriterCSharpCode() : base() { }
        public OutputWriterCSharpCode(Job job) : base(job) { }

        /// <summary>
        /// Executes CSharp code and returns string for output
        /// </summary>
        /// <returns></returns>
        public override StringBuilder GetOutput()
        {
            if (_Job.IsErrored)
                return new StringBuilder();

            CSharpCodeInformation csharpCodeInformation = new CSharpCodeInformation(_Job.DataSource.Keys.GetKeyValue(IdpeKeyTypes.CSharpCodeOutputWriter));
            ExecuteCSharpCode executeCSharpcode = new ExecuteCSharpCode("Eyedia.IDPE.Common.CSharpCode",
                ExecuteCSharpCode.CompilerVersions.v40, csharpCodeInformation.GetReferencedAssemblies());
            return new StringBuilder(executeCSharpcode.Execute(csharpCodeInformation.CompleteCode, "ExecuteAndReturnString", new object[] { _Job }).ToString());

        }

        public override object GetCustomOutput()
        {
            return null;
        }
    }
}


