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



//  Copyright (c) Microsoft Corporation. All rights reserved.

using System;
using System.Globalization;

namespace Symplus.Core.Workflow
{
    public class RuleSetInfo : IComparable<RuleSetInfo>
    {
        private string name;
        private int majorVersion;
        private int minorVersion;

        #region Constructors

        public RuleSetInfo()
        {
        }

        public RuleSetInfo(string ruleSetName)
        {
            name = ruleSetName;
        }

        public RuleSetInfo(string ruleSetName, int ruleSetMajorVersion, int rulesSetMinorVersion)
        {
            name = ruleSetName;
            majorVersion = ruleSetMajorVersion;
            minorVersion = rulesSetMinorVersion;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int MajorVersion
        {
            get { return majorVersion; }
            set { majorVersion = value; }
        }

        public int MinorVersion
        {
            get { return minorVersion; }
            set { minorVersion = value; }
        }

        #endregion

        #region IComparable<RuleSetInfo> Members

        public int CompareTo(RuleSetInfo other)
        {
            if (other != null)
            {
                int nameComparison = String.CompareOrdinal(this.Name, other.Name);
                if (nameComparison != 0)
                    return nameComparison;

                int majorVersionComparison = this.MajorVersion - other.MajorVersion;
                if (majorVersionComparison != 0)
                    return majorVersionComparison;

                int minorVersionComparison = this.MinorVersion - other.MinorVersion;
                if (minorVersionComparison != 0)
                    return minorVersionComparison;

                return 0;
            }
            else
            {
                return 1;
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}-{1}.{2}", name, majorVersion, minorVersion);
        }

        #endregion

    }
}




