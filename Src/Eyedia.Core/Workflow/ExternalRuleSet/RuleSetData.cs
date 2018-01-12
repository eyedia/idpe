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



//  Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Globalization;
using System.IO;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;

namespace Symplus.Core.Workflow
{
    public class RuleSetData : IComparable<RuleSetData>
    {
        #region Variables and constructor 

        public RuleSetData()
        {
        }

        private string name;
        private string originalName;
        private int majorVersion;
        private int originalMajorVersion;
        private int minorVersion;
        private int originalMinorVersion;
        private string ruleSetDefinition;
        private RuleSet ruleSet;
        private short status;
        private string assemblyPath;
        private string activityName;
        private DateTime modifiedDate;
        private string modifiedBy;
        private bool dirty;
        private Type activity;

        private WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (this.RuleSet != null)
                    this.RuleSet.Name = name;
            }
        }
        public string OriginalName
        {
            get { return originalName; }
            set { originalName = value; }
        }
        public int MajorVersion
        {
            get { return majorVersion; }
            set { majorVersion = value; }
        }
        public int OriginalMajorVersion
        {
            get { return originalMajorVersion; }
            set { originalMajorVersion = value; }
        }
        public int MinorVersion
        {
            get { return minorVersion; }
            set { minorVersion = value; }
        }
        public int OriginalMinorVersion
        {
            get { return originalMinorVersion; }
            set { originalMinorVersion = value; }
        }
        public string RuleSetDefinition
        {
            get { return ruleSetDefinition; }
            set { ruleSetDefinition = value; }
        }
        public RuleSet RuleSet
        {
            get
            {
                if (ruleSet == null)
                {
                    ruleSet = this.DeserializeRuleSet(ruleSetDefinition);
                }
                return ruleSet;
            }
            set
            {
                ruleSet = value;
                name = ruleSet.Name; 
            }
        }
        public short Status
        {
            get { return status; }
            set { status = value; }
        }
        public string AssemblyPath
        {
            get { return assemblyPath; }
            set { assemblyPath = value; }
        }
        public string ActivityName
        {
            get { return activityName; }
            set { activityName = value; }
        }
        public DateTime ModifiedDate
        {
            get { return modifiedDate; }
            set { modifiedDate = value; }
        }
        public string ModifiedBy
        {
            get { return modifiedBy; }
            set { modifiedBy = value; }
        }
        public bool Dirty
        {
            get { return dirty; }
            set { dirty = value; }
        }

        public Type Activity
        {
            get { return activity; }
            set
            {
                activity = value;
                if (activity != null)
                    activityName = activity.ToString();
            }
        }

        #endregion

        #region Methods

        private RuleSet DeserializeRuleSet(string ruleSetXmlDefinition)
        {
            if (!String.IsNullOrEmpty(ruleSetXmlDefinition))
            {
                StringReader stringReader = new StringReader(ruleSetXmlDefinition);
                XmlTextReader reader = new XmlTextReader(stringReader);
                return serializer.Deserialize(reader) as RuleSet;
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} - {1}.{2}", name, majorVersion, minorVersion);
        }

        public RuleSetData Clone()
        {
            RuleSetData newData = new RuleSetData();
            newData.Activity = this.Activity;
            //newData.ActivityName = activityName; //Set by setting Activity
            newData.AssemblyPath = this.AssemblyPath;
            newData.Dirty = true;
            newData.MajorVersion = this.MajorVersion;
            newData.MinorVersion = this.MinorVersion;
            newData.Name = name;
            newData.RuleSet = this.RuleSet.Clone();
            newData.Status = 0;

            return newData;
        }

        public RuleSetInfo GetRuleSetInfo()
        {
            return new RuleSetInfo(name, majorVersion, minorVersion);
        }

        #endregion

        #region IComparable<RuleSetData> Members

        public int CompareTo(RuleSetData other)
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
    }
}




