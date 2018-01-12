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
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Configuration;
using System.IO;

namespace Eyedia.Core.Net
{   
    public class PostMan
    {
        private EmailTracker _EmailTracker;
        public PostMan()
        {
            this._EmailTracker = new EmailTracker();
        }

        public void Send(string subject, string body, bool isBodyHtml = true, string[] attachedFiles = null)
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Email.Enabled)
                return;
            if (!_EmailTracker.IsAllowed(subject, body))
            {
                Trace.TraceInformation("Postman: Repeated email with subject = '{0}', Body = '{1}", subject, body);
                return;
            }

            string originalsubject = subject;
            string originalbody = body;

            if ((isBodyHtml)
            && (!(body.Contains("<span"))))
                body = string.Format("{0}{1}{2}", BodyFontStart, body, BodyFontEnd);


            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = EyediaCoreConfigurationSection.CurrentConfig.Email.SmtpServer;
            if (EyediaCoreConfigurationSection.CurrentConfig.Email.SmtpPort != 0)
                smtpClient.Port = EyediaCoreConfigurationSection.CurrentConfig.Email.SmtpPort;
            if (!string.IsNullOrEmpty(EyediaCoreConfigurationSection.CurrentConfig.Email.UserName))
                smtpClient.Credentials = new System.Net.NetworkCredential(EyediaCoreConfigurationSection.CurrentConfig.Email.UserName,
                    EyediaCoreConfigurationSection.CurrentConfig.Email.Password);
            smtpClient.EnableSsl = EyediaCoreConfigurationSection.CurrentConfig.Email.EnableSsl;
            MailMessage mail = new MailMessage();
            if (string.IsNullOrEmpty(EyediaCoreConfigurationSection.CurrentConfig.Email.FromDisplayName))
                mail.From = new MailAddress(EyediaCoreConfigurationSection.CurrentConfig.Email.FromEmail);
            else
                mail.From = new MailAddress(EyediaCoreConfigurationSection.CurrentConfig.Email.FromEmail,
                    EyediaCoreConfigurationSection.CurrentConfig.Email.FromDisplayName);

            if (EyediaCoreConfigurationSection.CurrentConfig.Email.ToEmails.Contains(";"))
            {
                List<string> toEmails = new List<string>(EyediaCoreConfigurationSection.CurrentConfig.Email.ToEmails.Split(";".ToCharArray()));
                foreach (string toEmail in toEmails)
                {
                    mail.To.Add(toEmail);
                }
            }
            else
            {
                mail.To.Add(EyediaCoreConfigurationSection.CurrentConfig.Email.ToEmails);
            }
            mail.Subject = subject;
            mail.Body = body + GetDefaultInformation(isBodyHtml);
            mail.IsBodyHtml = isBodyHtml;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            if (attachedFiles != null)
            {
                foreach (string attachedFile in attachedFiles)
                {
                    mail.Attachments.Add(new Attachment(attachedFile));
                }
            }
            try
            {
                smtpClient.Send(mail);
                _EmailTracker.Track(originalsubject, originalbody);
            }
            catch
            {
                //purposefully Trace.TraceError() was not used               
                Trace.TraceInformation(mail.Body);
                WriteHtml(mail);
            }

        }

        #region Helpers

        public static string BodyFontStart = "<span style='font-family:Verdana;font-size:70%;background-color:#fff;'>";
        public static string BodyFontEnd = "</span>";
        public static string GetDefaultInformation(bool isBodyHtml)
        {
            string information = string.Format("{0}, {1} | Machine Name = {2} | IP Address = {3}"
                , DateTime.Now, TimeZone.CurrentTimeZone.StandardName, Environment.MachineName,
                Dns.GetLocalIpAddress());

            if (isBodyHtml)                
                information = string.Format("<br /><br /><span style='font-size:xx-small;color:#FFFFFF;'>{0}</span>", information);
                //information = string.Format("<br /><br /><span style='font-size:xx-small;color:#848484;'>{0}</span>", information);
            
            return information;
        }

        static string GetCalledBy()
        {
            try
            {
                System.Reflection.MemberInfo mi = new StackTrace().GetFrame(4).GetMethod();
                return string.Format("Called by: {0}->{1}", mi.DeclaringType, mi.Name);
            }
            catch { return "Called by: ?"; }
        }

        #endregion Helpers

        public static void WriteHtml(MailMessage mailMessage)
        {
            try
            {
                string htmlPath = GetEmailHtmlFileName();
                StreamWriter sw = new StreamWriter(htmlPath);
                sw.WriteLine(string.Format("{0}{1}{2}", BodyFontStart, mailMessage.Subject, BodyFontEnd));
                sw.WriteLine("<BR />");
                sw.Write(mailMessage.Body);
                sw.Close();
            }
            catch { }
        }

        public static string GetEmailDirectoryName()
        {
            string dir = Path.GetDirectoryName(EyediaCoreConfigurationSection.CurrentConfig.Trace.File);
            dir = Path.Combine(dir, "Emails");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }

        public static string GetEmailHtmlFileName()
        {
            string dir = GetEmailDirectoryName();
            string file = string.Empty;
            int counter = 1;
            while (true)
            {
                file = string.Format(dir + "\\{0}.html", counter);
                if (!File.Exists(file))
                    break;
                counter++;
            }
            return file;
        }

    }
    
}





