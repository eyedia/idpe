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
using Eyedia.IDPE.Common;
using System.Data;

namespace Eyedia.IDPE.Services
{
    public class ColumnMapInfo
    {
        private string _RawString;        
        public string RawString
        {
            get { return _RawString; }
            set { _RawString = value; BindData(); }
        }
        public string InputColumn { get; set; }
        public AttributeTypes OutputDataType { get; set; }
        public string OutputColumn { get; set; }
        public const string DatabaseDefault = "{Database-Default}";
        public const string CustomDefined = "{Custom-Defined}";
        public ColumnMapInfo(int dataSourceId, string rawString = null)
        {
            if (rawString != null)
            {
                _RawString = rawString;                
                BindData();
            }
        }

        #region Helpers
        private void BindData()
        {
            string[] info = RawString.Split(",".ToCharArray());
            string[] OutputColumninfo = info[0].Split("|".ToCharArray());
            string[] InputColumninfo = info[1].Split("|".ToCharArray());

            InputColumn = InputColumninfo[0];
            OutputDataType = (AttributeTypes)int.Parse(OutputColumninfo[1]);
            OutputColumn = OutputColumninfo[0];
        }
        #endregion Helpers


    }
}


