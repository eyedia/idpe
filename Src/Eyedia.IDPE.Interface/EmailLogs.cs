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
using System.IO;
using Eyedia.Core.Net;
using System.Security.Permissions;
using System.Configuration;
using Eyedia.Core;

namespace Eyedia.IDPE.Interface
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public partial class EmailLogs : SREBaseFormNew
    {
        public EmailLogs()
        {
            InitializeComponent();
            Init();
            Bind();
        }
        public string EmailDirectory { get; set; }
        private void Init()
        {
            string exeName = "sre.exe";
            if (ConfigurationManager.AppSettings["exeName"] != null)
                exeName = ConfigurationManager.AppSettings["exeName"];

            string confFileName = AppDomain.CurrentDomain.BaseDirectory + exeName;
            if (File.Exists(confFileName))
            {
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(confFileName);
                EyediaCoreConfigurationSection sreSection = (EyediaCoreConfigurationSection)configuration.GetSection("eyediaCoreConfigurationSection");
                EmailDirectory = Path.GetDirectoryName(sreSection.Trace.File);
                EmailDirectory += "\\Emails";

                if (!Directory.Exists(EmailDirectory))
                    Directory.CreateDirectory(EmailDirectory);
           
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = EmailDirectory;
                watcher.Filter = "*.html";

                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);
                watcher.Deleted += new FileSystemEventHandler(OnChanged);

                watcher.EnableRaisingEvents = true;
            }
        }
        private static readonly object _lock = new object();
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            lock (_lock)
            {
                RefreshFiles();
                SetStatusText("New errors!", true, true);
            }
        }
        //private readonly object _lock = new object();
        private void RefreshFiles()
        {
            if (listView.InvokeRequired)
            {
                listView.Invoke(new MethodInvoker(delegate
                   {
                       Bind();  
                   }));
            }
        }

        private void Bind()
        {
            webBrowser.Navigate("about:blank");
            listView.Columns.Clear();
            listView.Columns.Add("Errors");
            listView.Columns.Add("Date & Time");
            listView.Columns[0].Width = (int)(listView.Width * 0.6);
            listView.Columns[1].Width = (int)(listView.Width * 0.3);

            listView.Items.Clear();
            if (string.IsNullOrEmpty(EmailDirectory))
                return;

            try
            {
                DirectoryInfo di = new DirectoryInfo(EmailDirectory);
                FileSystemInfo[] files = di.GetFileSystemInfos();
                var orderedFiles = files.Where(f => f.Name.EndsWith(".html"))
                                        .OrderByDescending(f => f.CreationTime)
                                        .ToList();

                foreach (FileSystemInfo file in orderedFiles)
                {
                    ListViewItem item = new ListViewItem(ReadSubject(file));
                    item.SubItems.Add(file.CreationTime.ToString());
                    item.Tag = file.FullName;
                    listView.Items.Add(item);
                }

            }
            catch { }
            if (listView.Items.Count > 0)
                webBrowser.Url = new Uri("file:///" + listView.Items[0].Tag.ToString());
            toolStripStatusLabel1.Text = string.Format("{0} Items", listView.Items.Count);
            listView.Focus();
        }

        private string ReadSubject(FileSystemInfo file)
        {
            string subject = file.Name;
            List<string> lines = new List<string>(File.ReadLines(file.FullName));
            if (lines.Count > 0)
            {
                subject = lines[0];
                subject = subject.Replace(PostMan.BodyFontStart, string.Empty);
                subject = subject.Replace(PostMan.BodyFontEnd, string.Empty);
            }
            return subject;
        }


        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
                webBrowser.Url = new Uri("file:///" + listView.SelectedItems[0].Tag.ToString());            
        }

        private void listView_Resize(object sender, EventArgs e)
        {
            if (listView.Columns.Count > 1)
            {
                listView.Columns[0].Width = (int)(listView.Width * 0.6);
                listView.Columns[1].Width = (int)(listView.Width * 0.3);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            miDelete.Enabled = listView.SelectedItems.Count > 0 ? true : false;
        }

        private void miRefresh_Click(object sender, EventArgs e)
        {
            Bind();
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                File.Delete(item.Tag as string);
            }
            Bind();
            if (listView.Items.Count > 0)
                webBrowser.Url = new Uri("file:///" + listView.Items[0].Tag.ToString());
            else
                webBrowser.Navigate("about:blank");

        
        }

        private void listView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                miDelete_Click(sender, e);
            }
            else if ((e.Control) && (e.KeyCode == Keys.A))
            {
                foreach (ListViewItem item in listView.Items)
                {
                    item.Selected = true;
                }
            }
        }
    }
}


