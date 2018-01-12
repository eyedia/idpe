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
using System.Resources;
using System.Reflection;
using System.Text;
using System.Linq;
using Eyedia.Core;

namespace Eyedia.IDPE.Common
{
    public class SreMessage : IDisposable
    {
        public SreMessage(string message)
        {
            this.Code = SreMessageCodes.SRE_UNKNOWN_ERROR;
            this.Message = message;
        }

        public SreMessage(string message, int rowPoistion)
        {
            this.Code = SreMessageCodes.SRE_UNKNOWN_ERROR;
            this.Message = message;
            this.RowPosition = rowPoistion;
        }

        public SreMessage(SreMessageCodes code)
        {
            this.Code = code;
            string msg = (string)Cache.Instance.Bag[code];
            if (msg == null)    //empty string is valid msg
            {
                ResourceManager rm = new ResourceManager("Eyedia.IDPE.Common.SreMessageCodeDescriptions", Assembly.GetAssembly(typeof(SreKeyTypes)));
                msg = rm.GetString(code.ToString());
                Cache.Instance.Bag.Add(code, msg);
            }
            this.Message = msg;
            if (this.Message == null)
            {
                this.Code = SreMessageCodes.SRE_UNKNOWN_ERROR;
                this.Message = "An unknown error occurred. There is no error defined with code " + code.ToString();
            }
        }

        public SreMessageCodes Code { get; private set; }
        public string Message { get; set; }
        public int RowPosition { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}", this.GetType().ToString(), Code, Message);
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Message = null;
        }

        #endregion
    }

    
}




