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




#if WEB
namespace Eyedia.IDPE.Services
{
    #region Imports

    using System;
    using System.IO;
    using System.Globalization;
    using System.Reflection;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Configuration;
    using System.Web.UI.HtmlControls;
    using System.Collections;
    using System.Web;
    using Eyedia.IDPE.Common;
    using Eyedia.Core;    

    #endregion

    /// <summary>
    /// Renders an HTML page that presents information about the version,
    /// build configuration, source files as well as a method to check
    /// for updates.
    /// </summary>

    internal sealed class MonitorPage : BasePage
    {
        public MonitorPage()
        {
            PageTitle = "IDPE Monitor";
        }
       
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            //
            // Emit a script that emit version info and checks for updates.
            //

            writer.WriteLine(@"
                <script type='text/javascript' language='JavaScript'>
                                       var IDPE = {
                        info : {
                            version     : '" + GetVersion() + @"',
                            fileVersion : '" + GetFileVersion() + @"',
                            type        : '" + Build.TypeLowercase + @"',
                            status      : '" + Build.Status + @"',
                            framework   : '" + Build.Framework + @"',
                            imageRuntime: '" + Build.ImageRuntimeVersion + @"'
                        }
                    };
                </script>");

            //
            // Title
            //

            writer.AddAttribute(HtmlTextWriterAttribute.Id, "PageTitle");
            writer.RenderBeginTag(HtmlTextWriterTag.H1);
            writer.Write(PageTitle);
            writer.RenderEndTag(); // </h1>
            writer.WriteLine();

          
            //
            // Content...
            //

            WriteApplicationBaseVariables(writer);
            WriteOtherPullers(writer);
            WriteActiveJobs(writer);
            WriteLogs(writer);            
            //
            // Update...
            //
            //CheckForUpdates(writer);
         

            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.Write("This <strong>{0}</strong> ", Build.TypeLowercase);
            writer.Write("build was compiled from the following sources for CLR {0}:", Build.ImageRuntimeVersion);
            writer.Write("<br /><br />");

            string[] dllvers = Build.DLLVersions;
            foreach (string dllver in dllvers)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.RenderBeginTag(HtmlTextWriterTag.Code);
                writer.Write(dllver);
                writer.RenderEndTag(); // </code>
                writer.RenderEndTag(); // </li>
            }
            

            writer.RenderEndTag(); // </p>

            //
            // Stamps...
            //

            //writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            

            //writer.RenderEndTag(); // </ul>
        }

        #region WriteApplicationBaseVariables

        void WriteApplicationBaseVariables(HtmlTextWriter writer)
        {

            writer.Write("<center><input name='btn_refresh1' type='submit' id='btn_refresh1' value='Refresh' onclick=\"reloadme('')\" /></center>");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "hilite");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "1");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "hilite");

            writer.Write("<tr>");
            writer.Write("<th colspan='4'>IDPE Base Variables</th>");
            writer.Write("</tr>");

            writer.Write("<tr><td>InstanceName</td><td><strong>{0}</strong></td><td></td><td></td></tr>", EyediaCoreConfigurationSection.CurrentConfig.InstanceName);
            writer.Write("<tr><td>HostingEnvironment</td><td>{0}</td><td></td><td></td></tr>", EyediaCoreConfigurationSection.CurrentConfig);


            string str = new Idpe().GetPullersStatus();
            if (str.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                writer.Write("<tr><td>LocalWatcher</td><td>{0}</td><td align='center'><img src='{1}/running.png' height='16' width='16' border='0' /></td>", 
                    IdpeConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull, this.Context.Request.Path);
                writer.Write("<td align='center'><input name='btn_LocalFileWatcherFolderNamePull' type='submit' id='btn_LocalFileWatcherFolderNamePull' value='Stop' onclick=\"reloadme('pullers=1')\" /></td></tr>");
            }
            else
            {
                writer.Write("<tr><td>LocalWatcher</td><td>{0}</td><td align='center'><img src='{1}/stopped.png' height='16' width='16' border='0' /></td>",
                    IdpeConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull, this.Context.Request.Path);
                writer.Write("<td align='center'><input name='btn_LocalFileWatcherFolderNamePull' type='submit' id='btn_LocalFileWatcherFolderNamePull' value='Start' onclick=\"reloadme('pullers=0')\" /></td></tr>");

            }            

            if (EyediaCoreConfigurationSection.CurrentConfig.Cache)
            {
                writer.Write("<tr><td>Cache</td><td>true</td><td></td>");
                writer.Write("<td align='center'><input name='btn_SRECache' type='submit' id='btn_SRECache' value='Disable' onclick=\"reloadme('cache=0')\" /></td></tr>");
            }
            else
            {
                writer.Write("<tr><td>Cache</td><td>false</td><td></td>");
                writer.Write("<td align='center'><input name='btn_SRECache' type='submit' id='btn_SRECache' value='Enable' onclick=\"reloadme('cache=1')\" /></td></tr>");
            }            


            writer.RenderEndTag(); //</table>

        }
        #endregion WriteApplicationBaseVariables

        #region WriteOtherPullers

        void WriteOtherPullers(HtmlTextWriter writer)
        {

            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "hilite");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "1");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "hilite");

            writer.Write("<tr>");
            writer.Write("<th colspan='4'>Other Pullers</th>");
            writer.Write("</tr>");

            writer.Write("<tr><td colspan=4> <font color='grey'>There is no other pullers configured.<font></td></tr>");
            writer.RenderEndTag(); //</table>

        }
        #endregion WriteOtherPullers

        #region WriteActiveJobs

        void WriteActiveJobs(HtmlTextWriter writer)
        {
            writer.Write("<center><input name='btn_refresh2' type='submit' id='btn_refresh2' value='Refresh' onclick=\"reloadme('')\" /></center>");

            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "hilite");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "1");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "hilite");

            writer.Write("<tr>");
            writer.Write("<th colspan='6'>Active Jobs</th>");
            writer.Write("</tr>");            

            if (Registry.Instance.Entries.Count == 0)
            {
                writer.Write("<tr><td colspan=6> <font color='grey'>There is no active jobs.<font></td></tr>");
            }
            else
            {
                writer.Write("<tr><td><strong>Job Id</strong></td><td><strong>Started</strong></td><td><strong>Total Rows</strong></td><td><strong>Processed</strong></td><td><strong>Elapsed Time</strong> (hour.minutes.seconds.milliseconds)</td><td><strong>Per row</strong> (in miliseconds)</td></tr>");

                foreach (DictionaryEntry e in Registry.Instance.Entries)
                {
                    Job job = e.Value as Job;
                    TimeSpan timeTaken;
                    if (!job.IsFinished)
                        timeTaken = DateTime.Now.Subtract(job.StartedAt);
                    else
                        timeTaken = job.FinishedAt.Subtract(job.StartedAt);

                    double perRow = timeTaken.TotalMilliseconds / job.TotalRowsProcessed;

                    writer.Write("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>", e.Key,
                        job.StartedAt, job.TotalRowsToBeProcessed, job.TotalRowsProcessed, timeTaken.ToString(), perRow);
                }
            }


            writer.RenderEndTag(); //</table>

        }

        #endregion WriteActiveJobs

        #region WriteLogs

        void WriteLogs(HtmlTextWriter writer)
        {
                        
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "hilite");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "1");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "hilite");

            writer.Write("<tr>");
            writer.Write("<th colspan='4'>Logs</th>");
            writer.Write("</tr>"); 
            
            if (!EyediaCoreConfigurationSection.CurrentConfig.Trace.Enabled)
            {
                writer.Write("<tr><td colspan=4> <font color='grey'>Trace log is not enabled.<font></td></tr>");
            }
            else
            {                
                string logContent = string.Empty;
                try
                {
                    string ccFile = EyediaCoreConfigurationSection.CurrentConfig.Trace.File + ".web.log";
                    if (System.IO.File.Exists(ccFile))
                        System.IO.File.Delete(ccFile);

                    System.IO.File.Copy(EyediaCoreConfigurationSection.CurrentConfig.Trace.File, ccFile);
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(ccFile))
                    {
                        logContent = sr.ReadToEnd();
                        sr.Close();
                    }
                    System.IO.File.Delete(ccFile);
                }
                catch { }
                writer.Write("<table style=\"width:100%;table-layout:fixed\"><tr><td><textarea id=\"taLog\" name=\"taLog\"  style=\"width:99%;height:300px;background-color:#E6E6E6;\">");
                writer.Write(logContent);
                writer.Write("</textarea></td></tr></table>");                
                writer.Write("<input name='btn_ClearLog' type='submit' id='btn_ClearLog' value='Clear' onclick=\"reloadme('log=1')\" />");
                writer.Write("<input name='btn_refresh3' type='submit' id='btn_refresh3' value='Refresh' onclick=\"reloadme('')\" />");
                writer.Write("<input name='btnSelectAll' type='button' id='btnSelectAll' value='Select All' onclick=\"document.getElementById('taLog').select()\" />");
                writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                writer.Write("<input name='txtSearch' type='input' id='txtSearch' value='' size='80' />");
                writer.Write("<input name='btnSearch' type='button' id='btnSearch' value='Search' onclick=\"searchLog()\" />");
                writer.Write("<label name='lblSearch' id='lblSearch'></label>");

                #region Archived Log Files
                string onlyName = Path.GetFileNameWithoutExtension(EyediaCoreConfigurationSection.CurrentConfig.Trace.File);
                string ext = Path.GetExtension(EyediaCoreConfigurationSection.CurrentConfig.Trace.File);
                string dir = Path.GetDirectoryName(EyediaCoreConfigurationSection.CurrentConfig.Trace.File);
                string[] arFiles = Directory.GetFiles(dir, onlyName + "*" + ext);
                if (arFiles.Length > 1)
                {
                    writer.Write("<div class=\"scroll\">");
                    writer.Write("<p>Active log file is displayed above. Archived logs files are...</p>");
                    writer.Write("<p>");                   

                    int counter = 1;
                    foreach (string arFile in arFiles)
                    {
                        if (arFile.Equals(EyediaCoreConfigurationSection.CurrentConfig.Trace.File)) continue;

                        string url = Request.Url + "/archivelog?logname=" + arFile;
                        string mdtm = File.GetLastWriteTime(arFile).ToString();
                        long flsz = (new FileInfo(arFile).Length) / 1024;
                        writer.RenderBeginTag(HtmlTextWriterTag.Li);
                        writer.RenderBeginTag(HtmlTextWriterTag.Code);
                        writer.Write("{0}&nbsp;&nbsp;{1}KB&nbsp;&nbsp;{2}&nbsp;&nbsp;<a href=\"{3}\" target=\"_blank\">Open</a>", arFile, flsz.ToString("D4"), mdtm, url);
                        writer.RenderEndTag();
                        writer.RenderEndTag();
                        counter++;
                    }
                    writer.Write("</div>");
                }

                #endregion Archived Log Files

            }

            writer.RenderEndTag(); //</table>

        }
        #endregion WriteLogs

        #region CheckForUpdates
        void CheckForUpdates(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return onCheckForUpdate(this)");
            writer.AddAttribute(HtmlTextWriterAttribute.Title, "Checks if your IDPE version is up to date (requires Internet connection)");
            writer.RenderBeginTag(HtmlTextWriterTag.Button);
            writer.Write("Check for Update");
            writer.RenderEndTag(); // </button>
            writer.RenderEndTag(); // </p>            
        }
        #endregion CheckForUpdates

        #region Helper Methods

        private Version GetVersion()
        {
            return GetType().Assembly.GetName().Version;
        }
        
        private Version GetFileVersion()
        {
            //AssemblyFileVersionAttribute version = (AssemblyFileVersionAttribute) Attribute.GetCustomAttribute(GetType().Assembly, typeof(AssemblyFileVersionAttribute));
            //return version != null ? new Version(version.Version) : new Version();
            return new Version();
        }
        #endregion Helper Methods
    }
}
#endif





