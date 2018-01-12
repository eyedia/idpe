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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Eyedia.IDPE.Services;
using System.Collections.Concurrent;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Interface
{
    public partial class DataSourceValidator : Form
    {
        public DataSourceValidator(bool dataSource, int id, string title, Icon icon = null)
        {
            InitializeComponent();
            this.Text = title;
            if (dataSource)
            {
                DataSourceReferenceExtractor dsf = new DataSourceReferenceExtractor(id);
                dsf.Parse();

                Bind(dsf.MissingAttributes, "Missing Attributes");
                Bind(dsf.MissingSystemAttributes, "Missing System Attributes");
                Bind(dsf.ReferencesProcessVariables, "Process Variables");
                Bind(dsf.ReferencesProcessVariablesIncorrectWay, "Process Variables - Incorrect Way");
            }
            else
            {
                IdpeRule rule = new Manager().GetRule(id);
                RuleReferenceExtractor rrf = new RuleReferenceExtractor(rule.Xaml);
                rrf.Parse();
                Bind(rrf.AttributeNames, "Attributes");
                Bind(rrf.SystemAttributeNames, "System Attributes");
                Bind(rrf.ProcessVariables, "Process Variables");
                Bind(rrf.ProcessVariablesIncorrectWay, "Process Variables - Incorrect Way");
            }

            if (icon != null)
                this.Icon = icon;
        }

        private void Bind(ConcurrentDictionary<string, List<string>> info, string caption)
        {
            TreeNode rootNode = new TreeNode(caption);
            int count = 0;
            foreach (var r in info)
            {
                TreeNode nodeRuleName = new TreeNode(r.Key + string.Format("({0})", r.Value.Count));
                foreach (string name in r.Value)
                {
                    nodeRuleName.Nodes.Add(name);
                    count++;
                }
                nodeRuleName.ForeColor = Color.DarkBlue;
                rootNode.Nodes.Add(nodeRuleName);
            }
            rootNode.ForeColor = Color.DarkBlue;
            rootNode.Text = caption + string.Format("({0})", count);
            treeView.Nodes.Add(rootNode);
        }

        private void Bind(List<string> list, string caption)
        {
            TreeNode rootNode = new TreeNode(caption + string.Format("({0})", list.Count));
            
            foreach (var item in list)
            {
                TreeNode nodeRuleName = new TreeNode(item);               
                nodeRuleName.ForeColor = Color.DarkBlue;
                rootNode.Nodes.Add(nodeRuleName);
            }
            rootNode.ForeColor = Color.DarkBlue;           
            treeView.Nodes.Add(rootNode);
        }
    }
}


