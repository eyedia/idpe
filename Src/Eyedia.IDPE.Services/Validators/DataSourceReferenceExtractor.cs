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
using Eyedia.IDPE.Services;
using Eyedia.IDPE.DataManager;
using System.Diagnostics;
using System.Collections.Concurrent;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Services
{
    public class DataSourceReferenceExtractor
    {
        public DataSourceReferenceExtractor(int dataSourceId)
        {
            this.DataSourceId = dataSourceId;
            ReferencesAttributes = new ConcurrentDictionary<string, List<string>>();
            ReferencesSystemAttributes = new ConcurrentDictionary<string, List<string>>();
            ReferencesProcessVariablesIncorrectWay = new ConcurrentDictionary<string, List<string>>();
            ReferencesProcessVariables = new ConcurrentDictionary<string, List<string>>();

            MissingAttributes = new ConcurrentDictionary<string, List<string>>();
            MissingSystemAttributes = new ConcurrentDictionary<string, List<string>>();
        }

        public int DataSourceId { get; set; }
        public ConcurrentDictionary<string, List<string>> ReferencesAttributes { get; private set; }
        public ConcurrentDictionary<string, List<string>> ReferencesSystemAttributes { get; private set; }
        public ConcurrentDictionary<string, List<string>> ReferencesProcessVariablesIncorrectWay { get; private set; }
        public ConcurrentDictionary<string, List<string>> ReferencesProcessVariables { get; private set; }

        public List<IdpeAttribute> Attributes { get; private set; }
        public List<IdpeAttribute> SystemAttributes { get; private set; }
        public List<IdpeRule> Rules { get; private set; }

        public ConcurrentDictionary<string, List<string>> MissingAttributes { get; private set; }
        public ConcurrentDictionary<string, List<string>> MissingSystemAttributes { get; private set; }       
       
        public void Parse()
        {
            ParseRuleReference();
            ParseCSharpCodes();
            Manager manager = new Manager();
            Attributes = manager.GetAttributes(this.DataSourceId);
            SystemAttributes = manager.GetAttributes(manager.GetDataSourceParentId(this.DataSourceId));

            foreach (var i in ReferencesAttributes)
            {
                List<string> missingattrb = i.Value.Except(Attributes.Select(a => a.Name).ToList(), StringComparer.OrdinalIgnoreCase).ToList();
                if (missingattrb.Count > 0)
                    MissingAttributes.TryAdd(i.Key, missingattrb);
            }

            foreach (var i in ReferencesSystemAttributes)
            {
                List<string> missingsysattrb = i.Value.Except(SystemAttributes.Select(a => a.Name).ToList(), StringComparer.OrdinalIgnoreCase).ToList();
                if (missingsysattrb.Count > 0)
                    MissingSystemAttributes.TryAdd(i.Key, missingsysattrb);
            }

            //ReferencesAttributes.OrderBy(a => a.Key);
            //ReferencesSystemAttributes.OrderBy(a => a.Key);
            //ReferencesProcessVariablesIncorrectWay.OrderBy(a => a.Key);
            //ReferencesProcessVariables.OrderBy(a => a.Key);
            //MissingAttributes.OrderBy(a => a.Key);
            //MissingSystemAttributes.OrderBy(a => a.Key);
           
        }

        private void ParseRuleReference()
        {
            Rules = new Manager().GetRules(DataSourceId);
            foreach (IdpeRule rule in Rules)
            {
                RuleReferenceExtractor ruleReferenceExtractor = new RuleReferenceExtractor(rule.Xaml);

                //if (rule.Name == "DRM Fx Data Load - Mapper")
                //    Debugger.Break();
                ruleReferenceExtractor.Parse();
                ReferencesAttributes.TryAdd(rule.Name, ruleReferenceExtractor.AttributeNames);
                ReferencesSystemAttributes.TryAdd(rule.Name, ruleReferenceExtractor.SystemAttributeNames);
                ReferencesProcessVariablesIncorrectWay.TryAdd(rule.Name, ruleReferenceExtractor.ProcessVariablesIncorrectWay);
                ReferencesProcessVariables.TryAdd(rule.Name, ruleReferenceExtractor.ProcessVariables);
            }
        }

        private void ParseCSharpCodes()
        {
            DataSourceCSharpCodeReferenceExtractor dsccre = new DataSourceCSharpCodeReferenceExtractor(DataSourceId, SreKeyTypes.CSharpCodeGenerateTable);
            dsccre.Parse();
            ReferencesAttributes.TryAdd("CSharp Code - Input Writer", dsccre.AttributeNames);
            ReferencesSystemAttributes.TryAdd("CSharp Code - Input Writer", dsccre.SystemAttributeNames);
            ReferencesProcessVariablesIncorrectWay.TryAdd("CSharp Code - Input Writer", dsccre.ProcessVariablesIncorrectWay);
            ReferencesProcessVariables.TryAdd("CSharp Code - Input Writer", dsccre.ProcessVariables);


            dsccre = new DataSourceCSharpCodeReferenceExtractor(DataSourceId, SreKeyTypes.CSharpCodeOutputWriter);
            dsccre.Parse();
            ReferencesAttributes.TryAdd("CSharp Code - Output Writer", dsccre.AttributeNames);
            ReferencesSystemAttributes.TryAdd("CSharp Code - Output Writer", dsccre.SystemAttributeNames);
            ReferencesProcessVariablesIncorrectWay.TryAdd("CSharp Code - Output Writer", dsccre.ProcessVariablesIncorrectWay);
            ReferencesProcessVariables.TryAdd("CSharp Code - Output Writer", dsccre.ProcessVariables);

        }
    }
}


