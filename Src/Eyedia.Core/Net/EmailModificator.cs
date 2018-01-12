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
using System.Collections;

namespace Eyedia.Core.Net
{

    /// <summary>
    /// Abstract class for Modificators
    /// </summary>
    public abstract class EmailModificator
    {
        protected Hashtable _parameters = new Hashtable();

        public Hashtable Parameters
        { 
            get { return _parameters; }
        }

        public abstract void Apply(ref string Value, params string[] Parameters);
    }

    class NL2BR : EmailModificator
    {
        public override void Apply(ref string Value, params string[] Parameters)
        {
            Value = Value.Replace("\n", "<br>");
        }
    }

    class HTMLENCODE : EmailModificator
    {
        public override void Apply(ref string Value, params string[] Parameters)
        {
            Value = Value.Replace("&", "&amp;");
            Value = Value.Replace("<", "&lt;");
            Value = Value.Replace(">", "&gt;");
        }
    }

    class UPPER : EmailModificator
    {
        public override void Apply(ref string Value, params string[] Parameters)
        {
            Value = Value.ToUpper();
        }
    }

    class LOWER : EmailModificator
    {
        public override void Apply(ref string Value, params string[] Parameters)
        {
            Value = Value.ToLower();
        }
    }

    class TRIM : EmailModificator
    {
        public override void Apply(ref string Value, params string[] Parameters)
        {
            Value = Value.Trim();
        }
    }

    class TRIMEND : EmailModificator
    {
        public override void Apply(ref string Value, params string[] Parameters)
        {
            Value = Value.TrimEnd();
        }
    }

    class TRIMSTART : EmailModificator
    {
        public override void Apply(ref string Value, params string[] Parameters)
        {
            Value = Value.TrimStart();
        }
    }

    class DEFAULT : EmailModificator
    {
        public override void Apply(ref string Value, params string[] Parameters)
        {
            if (Value == null || Value.Trim() == string.Empty)
                Value = Parameters[0];
        }
    }
}





