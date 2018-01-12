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
using System.Threading;
using System.Reflection;
using WeifenLuo.WinFormsUI.Docking;

namespace Eyedia.IDPE.Interface
{
    public partial class MonitorLogSearchResult : SREBaseFormNew
    {
        public MonitorLogSearchResult()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
        }

        private void MonitorLogResult_Resize(object sender, EventArgs e)
        {
            lvSearchResult.Columns[2].Width = (lvSearchResult.Width - lvSearchResult.Columns[0].Width) - 100;
        }

        public void ShowResult()
        {
            lvSearchResult.Items.Clear();
            SetStatusText("Matching lines: " + lvSearchResult.Items.Count);
        }

        private void lvSearchResult_DoubleClick(object sender, EventArgs e)
        {
            if (lvSearchResult.SelectedItems.Count > 0)
            {
                this.Cursor = Cursors.WaitCursor;
                string fileContent = string.Empty;
                try
                {
                    fileContent = File.ReadAllText(lvSearchResult.SelectedItems[0].SubItems[0].Text);
                }
                catch
                {
                    string ccFile = Monitor.GetCCFile(lvSearchResult.SelectedItems[0].SubItems[0].Text);
                    if (System.IO.File.Exists(ccFile))
                        System.IO.File.Delete(ccFile);

                    File.Copy(lvSearchResult.SelectedItems[0].SubItems[0].Text, ccFile, true);
                    Thread.Sleep(500);
                    fileContent = File.ReadAllText(ccFile);
                }

                Log log = new Log(fileContent,
                    int.Parse(lvSearchResult.SelectedItems[0].SubItems[1].Text));
                log.Icon = this.Icon;
                this.Cursor = Cursors.Default;
                log.ShowDialog();
            }
        }

        public void SearchInFile(string fileName, int searchOption, string searchString, DateTime dateTime)
        {
            int lineNumber = 1;
            if (string.IsNullOrEmpty(searchString))
            {
                searchString = dateTime.Year
                      + "-" + dateTime.Month.ToString("00")
                      + "-" + dateTime.Day.ToString("00");
                searchOption = -1;
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Open,
                                  FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        SearchInLine(fileName, line, searchOption, searchString, lineNumber);
                        lineNumber++;
                    }
                }
            }
        }

        private void SearchInLine(string fileName, string line, int searchOption, string searchString, int lineNumber)
        {
            bool found = false;
            switch (searchOption)
            {
                case 0: //WholeWord                           
                    if ((searchString.Length > 0)
                        && (searchString.Substring(searchString.Length - 1, 1) == " "))
                        searchString = searchString.Trim();

                    if (line.StartsWith(searchString + " ", StringComparison.OrdinalIgnoreCase))
                        found = true;
                    else
                        found = line.IndexOf(" " + searchString + " ", StringComparison.OrdinalIgnoreCase) > -1;
                    break;

                case 1: //MatchCase
                    found = line.IndexOf(searchString, StringComparison.Ordinal) > -1;
                    break;

                case 2: //None
                    found = line.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) > -1;
                    break;

                case -1:
                    if (line.StartsWith(searchString))
                        found = true;
                    break;
            }

            if (found)
            {
                ListViewItem item = new ListViewItem(fileName);
                item.SubItems.Add(lineNumber.ToString());
                item.SubItems.Add(line);
                lvSearchResult.Items.Add(item);
            }

            // return found;
        }
    }
}


