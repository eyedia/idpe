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
using System.Runtime.Serialization;
using Eyedia.Core;

namespace Eyedia.IDPE.Common
{
    [DataContract]
    public class UserCookie
    {
        public UserCookie()
        {                
            UserNames = new List<string>();
        }

        public UserCookie(string serialized)
        {
            if(!string.IsNullOrEmpty(serialized))
            {
                UserCookie uc = null;
                try
                {
                    uc = serialized.Deserialize<UserCookie>();
                }
                catch { }
                if (uc != null)
                {                    
                    this.UserNames = uc.UserNames;
                    this.UserName = uc.UserName;
                    this.SdfName = uc.SdfName;
                }
                else
                {   
                    UserNames = new List<string>();
                }
            }
        }


        [DataMember]
        public List<string> UserNames { get; private set; }

        [DataMember]
        public string UserName { get; private set; }

        [DataMember]
        public string SdfName { get; private set; }

        public void Save(string userName, string sdfName)
        {
            UserNames.Add(userName);
            UserNames = UserNames.Distinct().ToList();
            UserName = userName;
            SdfName = sdfName;

        }
    }


}


