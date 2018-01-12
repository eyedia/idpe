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
using System.IO;
using System.Reflection;
using Eyedia.Core;
using Microsoft.VisualBasic;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using Eyedia.Core.Net;

namespace Eyedia.IDPE.Services
{
    public class PostMan
    {
        public Job Job { get; private set; }
        public DataSource DataSource { get; private set; }
        private List<string> FileAttachmentWarnings { get; set; }
        public const string __warningStartTag = "<w>";
        private const string __warningStartTagValue = "<span style=\"color: Maroon\">";
        public const string __warningEndTag = "</w>";
        private const string __warningEndTagValue = "</span>";

        public const string __errorStartTag = "<e>";
        private const string __errorStartTagValue = "<span style=\"color: Red; font-weight: bold\">";
        public const string __errorEndTag = "</e>";
        private const string __errorEndTagValue = "</span>";

        public string Subject 
        { 
            get 
            {
                try
                {
                    if(KeepErrorInSubject)
                        return string.Format("{0}.Error.{1}", EyediaCoreConfigurationSection.CurrentConfig.InstanceName, DataSource.Name);
                    else
                        return string.Format("{0}.{1}", EyediaCoreConfigurationSection.CurrentConfig.InstanceName, DataSource.Name);
                }
                catch
                {
                    return string.Format("{0}.Error.", EyediaCoreConfigurationSection.CurrentConfig.InstanceName);
                }
            } 
        }

        public string Body
        {
            get
            {
                if ((!string.IsNullOrEmpty(EmailTemplate))
                    && (Job != null))
                {                    
                    string strReturn = EmailTemplate;
                    strReturn = strReturn.Replace("{0}", DataSource.Name);
                    strReturn = strReturn.Replace("{1}", Job.TotalRowsToBeProcessed == 0 ? "0 (or not known)" : Job.TotalRowsToBeProcessed.ToString());
                    strReturn = strReturn.Replace("{2}", (Job.TotalRowsToBeProcessed - Job.TotalValid).ToString());
                    strReturn = strReturn.Replace("{3}", GetFormattedErrors());
                    strReturn = strReturn.Replace("{4}", GetFormattedWarnings());
                    strReturn = strReturn.Replace("{5}", GetFormattedAttachmentWarnings());
                    return strReturn + Eyedia.Core.Net.PostMan.GetDefaultInformation(true);
                }

                else
                {
                    return "<br />There were few validation errors while processing a cash matching file. <br /><br /><font color='red'>Please study the file, correct and re-process.</font><br /><br /><br />Thanks,<br />Supply Chain Finance – Cash Matching Service.<br />[Please do not reply to this automated email]";
                }
         
            }
        }
       
        public string EmailTemplate
        {
            get
            {
                string strContent = Cache.Instance.Bag["emailtemplate"] as string;

                if (string.IsNullOrEmpty(strContent))
                {
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.IDPE.Services.Utilities.PostMan.EmailTemplate.htm"))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        strContent = reader.ReadToEnd();
                    }
                }
                return strContent;
            }
        }

        public string EmailTemplateGeneric
        {
            get
            {
                string strContent = Cache.Instance.Bag["emailtemplategeneric"] as string;

                if (string.IsNullOrEmpty(strContent))
                {
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.IDPE.Services.Utilities.PostMan.EmailTemplateGeneric.htm"))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        strContent = reader.ReadToEnd();
                    }
                }
                return strContent;
            }
        }       

        private bool KeepErrorInSubject;

        private List<SreKey> Keys
        {
            get
            {
                if (Job != null)
                    return Job.DataSource.Keys;
                else if (DataSource != null)
                    return DataSource.Keys;
                else
                    return new List<SreKey>();
            }
        }

        private EmailTracker _EmailTracker;

        /// <summary>
        /// Postman sends emails in standard format(body, subject, attachment) unless explicitly passed
        /// </summary>
        /// <param name="job">The job object from where it takes information to construct body</param>
        /// <param name="error">If false then subject will not contain [Error] word</param>
        public PostMan(Job job, bool error = true)
        {
            this.Job = job;
            this.DataSource = job.DataSource;
            this.KeepErrorInSubject = error;
            this.FileAttachmentWarnings = new List<string>();
            this._EmailTracker = new EmailTracker();
        }

        /// <summary>
        /// Postman sends emails in standard format(body, subject, attachment) unless explicitly passed
        /// </summary>
        /// <param name="dataSource">The DataSource object from where it takes information to construct body</param>
        /// <param name="error">If false then subject will not contain [Error] word</param>
        public PostMan(DataSource dataSource, bool error = true)
        {            
            this.DataSource = dataSource;
            this.KeepErrorInSubject = error;
            this.FileAttachmentWarnings = new List<string>();
            this._EmailTracker = new EmailTracker();
        }

        string FormatExternalEmailBody(string body)
        {
            body = body.Replace(Environment.NewLine, "<br />");
            body = body.Replace(__errorStartTag, __errorStartTagValue);
            body = body.Replace(__errorEndTag, __errorEndTagValue);
            body = body.Replace(__warningStartTag, __warningStartTagValue);
            body = body.Replace(__warningEndTag, __warningEndTagValue);
            string strReturn = EmailTemplateGeneric;
            strReturn = strReturn.Replace("{0}", body);
            return strReturn + Eyedia.Core.Net.PostMan.GetDefaultInformation(true);
        }

        string FormatExternalEmailSubject(string subject)
        {
            return string.Format("{0}.{1}.{2}", EyediaCoreConfigurationSection.CurrentConfig.InstanceName, DataSource.Name, subject);
        }

        public void Send(string body = null, string subject = null, bool noDefaultAttachment = false, List<string> additionalAttachments = null)
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Email.Enabled)
                return;

            MailMessage oMail = new MailMessage();
            oMail.IsBodyHtml = true;
            oMail.From = new MailAddress(EyediaCoreConfigurationSection.CurrentConfig.Email.FromEmail);
            if (EyediaCoreConfigurationSection.CurrentConfig.Email.ToEmails.Contains(";"))
            {
                List<string> toEmails = new List<string>(EyediaCoreConfigurationSection.CurrentConfig.Email.ToEmails.Split(";".ToCharArray()));
                foreach (string toEmail in toEmails)
                {
                    oMail.To.Add(toEmail);
                }
            }
            else
            {
                oMail.To.Add(EyediaCoreConfigurationSection.CurrentConfig.Email.ToEmails);
            }
            AddCcAndBcc(oMail);
            oMail.Subject = subject == null ? Subject : FormatExternalEmailSubject(subject);
            oMail.Priority = MailPriority.Normal;


            if (Job != null)
            {
                if (noDefaultAttachment == false)
                {
                    if (!(string.IsNullOrEmpty(Job.FileName))
                        && (File.Exists(Job.FileName)))
                    {
                        if (GetFileSize(Job.FileName) < EyediaCoreConfigurationSection.CurrentConfig.Email.MaxAttachmentSize)
                            oMail.Attachments.Add(new Attachment(Job.FileName));
                        else
                            FileAttachmentWarnings.Add(string.Format("File '{0}' was not attached to the email as it was greater than allowed size ({1} MB)", Path.GetFileName(Job.FileName), EyediaCoreConfigurationSection.CurrentConfig.Email.MaxAttachmentSize));

                    }
                }
            }

            if (additionalAttachments != null)
            {
                foreach (string file in additionalAttachments)
                {
                    if (!(string.IsNullOrEmpty(file))
                    && (File.Exists(file)))
                    {
                        if (new FileUtility().WaitTillFileIsFree(file) == true)
                        {
                            if (GetFileSize(file) < EyediaCoreConfigurationSection.CurrentConfig.Email.MaxAttachmentSize)
                                oMail.Attachments.Add(new Attachment(file));
                            else
                                FileAttachmentWarnings.Add(string.Format("File '{0}' was not attached to the email as it was greater than allowed size ({1} MB)", Path.GetFileName(file), EyediaCoreConfigurationSection.CurrentConfig.Email.MaxAttachmentSize));
                        }
                    }
                }
            }


            oMail.Body = body == null ? Body : FormatExternalEmailBody(body);
            if (_EmailTracker.IsAllowed(subject, body, DataSource.Id))
            {
                SmtpClient smtpmail = new SmtpClient(EyediaCoreConfigurationSection.CurrentConfig.Email.SmtpServer);
                if (!string.IsNullOrEmpty(EyediaCoreConfigurationSection.CurrentConfig.Email.UserName))
                    smtpmail.Credentials = new System.Net.NetworkCredential(EyediaCoreConfigurationSection.CurrentConfig.Email.UserName,
                        EyediaCoreConfigurationSection.CurrentConfig.Email.Password);
                else
                    smtpmail.Credentials = (ICredentialsByHost)CredentialCache.DefaultNetworkCredentials;

                smtpmail.EnableSsl = EyediaCoreConfigurationSection.CurrentConfig.Email.EnableSsl;
                //smtpmail.DeliveryMethod = SmtpDeliveryMethod.Network;
                try
                {
                    smtpmail.Send(oMail);                    
                    if (oMail != null)
                    {
                        oMail.Dispose();
                        oMail = null;
                    }
                }
                catch (Exception ex)
                {
                    Eyedia.Core.Net.PostMan.WriteHtml(oMail);
                }

                _EmailTracker.Track(subject, body, DataSource.Id);
            }
            else
            {
                Trace.TraceInformation("Postman: Repeated email with subject = '{0}', Body = '{1}", subject, body);
            }
        }
      
        private void AddCcAndBcc(MailMessage oMail)
        {
            
            string emailCc = Keys.GetKeyValue(SreKeyTypes.EmailCc);
            if (!string.IsNullOrEmpty(emailCc))
                oMail.CC.Add(emailCc.Replace(";",","));

            string emailBcc = Keys.GetKeyValue(SreKeyTypes.EmailBcc);
            if (!string.IsNullOrEmpty(emailBcc))
                oMail.Bcc.Add(emailBcc.Replace(";", ","));
            
        }

        private string GetFormattedErrors()
        {
            if (Job.Errors.Count <= 100)
            {
                return Job.Errors.ToLine("<br / >");
            }
            else
            {
                string errors = Job.Errors.Take(100).ToList().ToLine("<br / >");
                errors += string.Format("...and {0} more!<br / >", (Job.Errors.Count - 100));
                return errors;
            }

        }

        private string GetFormattedWarnings()
        {
            if (Job.Warnings.Count <= 100)
            {
                return Job.Warnings.ToLine("<br / >");
            }
            else
            {
                string warnings = Job.Warnings.Take(100).ToList().ToLine("<br / >");
                warnings += string.Format("...and {0} more!<br / >", (Job.Warnings.Count - 100));
                return warnings;
            }
        }

        private string GetFormattedAttachmentWarnings()
        {
            if (FileAttachmentWarnings.Count <= 100)
            {
                return FileAttachmentWarnings.ToLine("<br / >");
            }
            else
            {
                string warnings = FileAttachmentWarnings.Take(100).ToList().ToLine("<br / >");
                warnings += string.Format("...and {0} more!<br / >", (FileAttachmentWarnings.Count - 100));
                return warnings;
            }
        }

        private double GetFileSize(string fileName)
        {
            double fileLength = new FileInfo(fileName).Length;
            return Math.Round((fileLength / 1024) / 1024, 2);
        }
       
    }
}


