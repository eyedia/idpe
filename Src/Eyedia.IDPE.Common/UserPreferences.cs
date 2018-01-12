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

using Eyedia.Core;

namespace Eyedia.IDPE.Common
{
    public class UserPreferences
    {
        public UserPreferences()
        {
        }

        public UserPreferences(string serializedUserPreferences)
        {
            LoadFromString(serializedUserPreferences);
        }

        public string ComparisonToolExecutablePath { get; set; }
        public bool CommunicateWithService { get; set; }
        public bool InputFeedPullFromDatabaseTestQuery { get; set; }
        public int DefaultDataSourceId { get; set; }       

        #region Helpers

        void LoadFromString(string serializedUserPreferences)
        {
            UserPreferences userPreferences = new UserPreferences();
            if (!string.IsNullOrEmpty(serializedUserPreferences))
            {
                userPreferences = serializedUserPreferences.Deserialize<UserPreferences>();
                this.ComparisonToolExecutablePath = userPreferences.ComparisonToolExecutablePath;
                this.CommunicateWithService = userPreferences.CommunicateWithService;
                this.InputFeedPullFromDatabaseTestQuery = userPreferences.InputFeedPullFromDatabaseTestQuery;
                this.DefaultDataSourceId = userPreferences.DefaultDataSourceId;
                
            }
        }

        #endregion
    }

 
}


