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
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using Eyedia.Core.Data;
using System.Diagnostics;

namespace Eyedia.Core.Net
{
    public enum AuthenticationResultTypes
    {
        Eyedia,
        ActiveDirectory,
        ActiveDirectoryWithImpersonation,
        ActiveDirectoryGroup

    }

    public enum AuthenticationTypes
    {
        ActiveDirectory,
        Eyedia,        
        ActiveDirectoryGroup,
        Auto
    }


    public class Authenticator
    {
        public Authenticator(AuthenticationTypes authenticationType)
        {
            this.AuthenticationType = authenticationType;
            this.Result = new Dictionary<AuthenticationResultTypes, bool>();
        }

        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Domain { get; private set; }
        public bool IsAuthenticated { get; private set; }
        public Dictionary<AuthenticationResultTypes, bool> Result { get; private set; }
        public AuthenticationTypes AuthenticationType { get; private set; }
        public string GroupName { get; private set; }
        public int GroupId { get; private set; }

        public delegate void CallbackEventHandler(string info);
        public event CallbackEventHandler Callback;

        public User Authenticate()
        {            
            IsAuthenticated = true;
            return GetSymplusUser();
        }

        public User Authenticate(string groupNames)
        {            
            Trace.TraceInformation("Active directory group authentication {0}", groupNames);
            List<string> lstGroups = null;
            if (groupNames.Contains(","))
            {
                lstGroups = new List<string>(groupNames.Split(",".ToCharArray()));                                
            }
            else
            {
                lstGroups = new List<string>();
                lstGroups.Add(groupNames);
            }

            try
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, Environment.UserDomainName);
                
                if (ctx != null)
                {
                    UserPrincipal user = UserPrincipal.FindByIdentity(ctx, Environment.UserName);
                    SetData(System.Security.Principal.WindowsIdentity.GetCurrent().Name, string.Empty, groupNames); //1st level, if fails, still we have something
                    
                    if (user != null)
                    {
                        foreach (string groupName in lstGroups)
                        {                           
                            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, groupName);
                            if ((group != null) && (user.IsMemberOf(group)))
                            {                               
                                IsAuthenticated = true;
                                SetData(System.Security.Principal.WindowsIdentity.GetCurrent().Name, string.Empty, groupName); //final level
                                SetAuthenticationResult(AuthenticationResultTypes.ActiveDirectoryGroup, true);                                
                                return GetSymplusUser();
                            }
                        }
                    }
                }
                if(Callback != null)
                    Callback(IsAuthenticated ? "success" : "Failed");
            }
            catch (Exception ex)
            {
                if (Callback != null)
                    Callback("Failed - " + ex.Message);
                SetAuthenticationResult(AuthenticationResultTypes.ActiveDirectoryGroup, false);

            }
          
            return null;
        }

        public User Authenticate(string userName, string password, string groupName = null)
        {
            SetData(userName, password, groupName);           
            switch (AuthenticationType)
            {
                case AuthenticationTypes.Eyedia:
                    AuthenticateSymplus(userName, password);
                    break;

                case AuthenticationTypes.ActiveDirectory:
                    AuthenticateDomain(ContextType.Machine, userName, password);
                    if (!IsAuthenticated)
                        AuthenticateDomain(ContextType.Domain, userName, password);
                    break;

                //case AuthenticationTypes.ActiveDirectoryGroup:
                //    AuthenticateDomainGroup(userName, password, groupName);
                //    break;

                case AuthenticationTypes.Auto:
                    AuthenticateDomain(ContextType.Machine, userName, password);
                    if (!IsAuthenticated)
                    {
                        AuthenticateDomain(ContextType.Domain, userName, password);
                        if (!IsAuthenticated)
                            AuthenticateSymplus(userName, password);

                    }
                    break;
            }
            return IsAuthenticated ? GetSymplusUser(userName) : null;
        }

        public bool AuthenticateSymplus(string userName, string password)
        {
            if (Callback != null)
                Callback("Trying Eyedia authentication...");
            try
            {
                SetData(userName, password);
                IsAuthenticated = (CoreDatabaseObjects.Instance.Authenticate(UserName, Password) != null);
                SetAuthenticationResult(AuthenticationResultTypes.Eyedia, IsAuthenticated);
                if (Callback != null)
                    Callback(IsAuthenticated ? "success" : "Failed");
            }
            catch (Exception ex)
            {
                if (Callback != null)
                    Callback("Failed - " + ex.Message);
                SetAuthenticationResult(AuthenticationResultTypes.Eyedia, false);
            }
            GroupId = IsAuthenticated ? -100 : 0;
            return IsAuthenticated;

        }

        public bool AuthenticateDomain(ContextType contextType, string userName, string password)
        {
            if (Callback != null)
                Callback("Trying active directory" + (contextType == ContextType.Machine?"(+Machine)":string.Empty) + " authentication...");
            try
            {
                SetData(userName, password);
                IsAuthenticated = new PrincipalContext(contextType, Domain).ValidateCredentials(UserName, Password);
                SetAuthenticationResult(AuthenticationResultTypes.ActiveDirectory, IsAuthenticated);
                if (Callback != null)
                    Callback(IsAuthenticated ? "success" : "Failed");
            }
            catch (Exception ex)
            {
                if (Callback != null)
                    Callback("Failed - " + ex.Message);
                SetAuthenticationResult(AuthenticationResultTypes.ActiveDirectory, false);
            }

            if (!IsAuthenticated)
            {

                if (Callback != null)
                    Callback("Trying active directory" + (contextType == ContextType.Machine ? "(+Machine)" : string.Empty) + " impersonation...");

                try
                {
                    new Impersonator(UserName, Domain, Password);
                    SetAuthenticationResult(AuthenticationResultTypes.ActiveDirectoryWithImpersonation, true);
                }
                catch
                {
                    SetAuthenticationResult(AuthenticationResultTypes.ActiveDirectoryWithImpersonation, false);
                }
            }
            GroupId = IsAuthenticated ? -99 : 0;
            return IsAuthenticated;
        }

        
        public override string ToString()
        {
            string result = string.Empty;
            foreach(var r in Result)
            {
                result += string.Format("{0}:{1}{2}", r.Key.ToString(), r.Value.ToString(), Environment.NewLine);
            }

            return result;
        }

        private void SetData(string userName, string password, string groupName = null)
        {
            this.UserName = userName;
            this.Password = password;
            this.Domain = userName.Contains("@") ? userName.Split("@".ToCharArray())[1] : string.Empty;
            this.GroupName = groupName;
        }

        private void SetAuthenticationResult(AuthenticationResultTypes authenticationType, bool isAuthenticated)
        {
            if (Result.ContainsKey(authenticationType))
                Result[authenticationType] = isAuthenticated;
            else
                Result.Add(authenticationType, isAuthenticated);
        }

        private User GetSymplusUser(string userName = null)
        {
            if (userName == null)
                userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            User user = CoreDatabaseObjects.Instance.GetUser(userName);

            if (user == null)
            {
                user = new User();
                user.UserName = userName;
                user.FullName = user.UserName;
                user.IsDebugUser = true;
                CoreDatabaseObjects.Instance.Save(user);
            }            
            return user;
        }


    }
}


