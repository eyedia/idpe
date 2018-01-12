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
using System.IO;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Eyedia.Core
{
    public sealed class FileStreamWithBackup : FileStream
    {
        public FileStreamWithBackup(string path, long maxFileLength, int maxFileCount, FileMode mode)
            : base(path, BaseFileMode(mode), FileAccess.Write)
        {
            Init(path, maxFileLength, maxFileCount, mode);
        }

        public FileStreamWithBackup(string path, long maxFileLength, int maxFileCount, FileMode mode, FileShare share)
            : base(path, BaseFileMode(mode), FileAccess.Write, share)
        {
            Init(path, maxFileLength, maxFileCount, mode);
        }

        public FileStreamWithBackup(string path, long maxFileLength, int maxFileCount, FileMode mode, FileShare share, int bufferSize)
            : base(path, BaseFileMode(mode), FileAccess.Write, share, bufferSize)
        {
            Init(path, maxFileLength, maxFileCount, mode);
        }

        public FileStreamWithBackup(string path, long maxFileLength, int maxFileCount, FileMode mode, FileShare share, int bufferSize, bool isAsync)
            : base(path, BaseFileMode(mode), FileAccess.Write, share, bufferSize, isAsync)
        {
            Init(path, maxFileLength, maxFileCount, mode);
        }

        public override bool CanRead { get { return false; } }

        public override void Write(byte[] array, int offset, int count)
        {
            int actualCount = System.Math.Min(count, array.GetLength(0));
            if (Position + actualCount <= m_maxFileLength)
            {
                base.Write(array, offset, count);
            }
            else
            {
                if (CanSplitData)
                {
                    int partialCount = (int)(System.Math.Max(m_maxFileLength, Position) - Position);
                    base.Write(array, offset, partialCount);
                    offset += partialCount;
                    count = actualCount - partialCount;
                }
                else
                {
                    if (count > m_maxFileLength)
                        throw new ArgumentOutOfRangeException("Buffer size exceeds maximum file length");
                }
                BackupAndResetStream();
                Write(array, offset, count);
            }
        }

        public long MaxFileLength { get { return m_maxFileLength; } }
        public int MaxFileCount { get { return m_maxFileCount; } }
        public bool CanSplitData { get { return m_canSplitData; } set { m_canSplitData = value; } }

        private void Init(string path, long maxFileLength, int maxFileCount, FileMode mode)
        {
            if (maxFileLength <= 0)
                throw new ArgumentOutOfRangeException("Invalid maximum file length");
            if (maxFileCount <= 0)
                throw new ArgumentOutOfRangeException("Invalid maximum file count");

            m_maxFileLength = maxFileLength;
            m_maxFileCount = maxFileCount;
            m_canSplitData = true;

            string fullPath = Path.GetFullPath(path);
            m_fileDir = Path.GetDirectoryName(fullPath);
            m_fileBase = Path.GetFileNameWithoutExtension(fullPath);
            m_fileExt = Path.GetExtension(fullPath);

            m_fileDecimals = 1;
            int decimalBase = 10;
            while (decimalBase < m_maxFileCount)
            {
                ++m_fileDecimals;
                decimalBase *= 10;
            }

            switch (mode)
            {
                case FileMode.Create:
                case FileMode.CreateNew:
                case FileMode.Truncate:
                    // Delete old files
                    for (int iFile = 0; iFile < m_maxFileCount; ++iFile)
                    {
                        string file = GetBackupFileName(iFile);
                        if (File.Exists(file))
                            File.Delete(file);
                    }
                    break;

                default:
                    // Position file pointer to the last backup file
                    for (int iFile = 0; iFile < m_maxFileCount; ++iFile)
                    {
                        if (File.Exists(GetBackupFileName(iFile)))
                            m_nextFileIndex = iFile + 1;
                    }
                    if (m_nextFileIndex == m_maxFileCount)
                        m_nextFileIndex = 0;
                    Seek(0, SeekOrigin.End);
                    break;
            }
        }

        public void BackupAndResetStream()
        {
            Flush();
            File.Copy(Name, GetBackupFileName(m_nextFileIndex), true);
            SetLength(0);

            ++m_nextFileIndex;
            if (m_nextFileIndex >= m_maxFileCount)
                m_nextFileIndex = 0;
        }

        private string GetBackupFileName(int index)
        {
            StringBuilder format = new StringBuilder();
            format.AppendFormat("D{0}", m_fileDecimals);
            StringBuilder sb = new StringBuilder();
            if (m_fileExt.Length > 0)
                sb.AppendFormat("{0}{1}{2}", m_fileBase, index.ToString(format.ToString()), m_fileExt);
            else
                sb.AppendFormat("{0}{1}", m_fileBase, index.ToString(format.ToString()));
            return Path.Combine(m_fileDir, sb.ToString());
        }

        private static FileMode BaseFileMode(FileMode mode)
        {
            return mode == FileMode.Append ? FileMode.OpenOrCreate : mode;
        }

        private long m_maxFileLength;
        private int m_maxFileCount;
        private string m_fileDir;
        private string m_fileBase;
        private string m_fileExt;
        private int m_fileDecimals;
        private bool m_canSplitData;
        private int m_nextFileIndex;
    }

    public class TextWriterTraceListenerWithTime : TextWriterTraceListener
    {
        public TextWriterTraceListenerWithTime()
            : base()
        {
        }

        public TextWriterTraceListenerWithTime(Stream stream)
            : base(stream)
        {
        }

        public FileStreamWithBackup FileStream { get; private set; }
        public TextWriterTraceListenerWithTime(string path, int maxFileCount, FileStreamWithBackup fileStreamWithBackup)
            : base(fileStreamWithBackup)
        {
            FileName = path;
            FileStream = fileStreamWithBackup;
        }

        public TextWriterTraceListenerWithTime(TextWriter writer)
            : base(writer)
        {
        }

        public TextWriterTraceListenerWithTime(Stream stream, string name)
            : base(stream, name)
        {
        }

        public TextWriterTraceListenerWithTime(string path, string name)
            : base(path, name)
        {
        }

        public TextWriterTraceListenerWithTime(TextWriter writer, string name)
            : base(writer, name)
        {
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            base.TraceEvent(eventCache, source, eventType, id);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            TraceEventInMyOwnStyle(eventCache, source, eventType, id, message);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            TraceEventInMyOwnStyle(eventCache, source, eventType, id, string.Format(format, args));            
        }        
        
        public override void WriteLine(string message)
        {
            if (message != Environment.NewLine)
            {
                base.Write(string.Format("{0:s}", DateTime.Now));
                base.Write(" ");
                base.WriteLine(message);               
            }
            else
            {
                base.WriteLine("");
            }
        }

        public string FileName { get; private set; }


        protected override string[] GetSupportedAttributes()
        {
            return new string[] { "EmailErrors" };
        }

        /// <summary>
        /// Returns whatever have been set in the application configuration file.
        /// Set to 'true' if you want to send email notification in case of 'Error' or 'Critical'.
        /// </summary>
        public bool EmailErrors
        {
            get
            {
                foreach (DictionaryEntry de in this.Attributes)
                    if (de.Key.ToString().Equals("EmailErrors", StringComparison.OrdinalIgnoreCase))
                        return bool.Parse(de.Value.ToString());
                return false;
            }
        }

        /// <summary>
        /// Returns whatever have been set in the application configuration file.
        /// Set to 'true' if you want to send email notification in case of 'Error' or 'Critical'.
        /// </summary>
        public string SMTPServer
        {
            get
            {
                foreach (DictionaryEntry de in this.Attributes)
                    if (de.Key.ToString().Equals("SMTPServer", StringComparison.OrdinalIgnoreCase))
                        return de.Value.ToString();
                return string.Empty;
            }
        }       

        /// <summary>
        /// Returns whatever have been set in the application configuration file.
        /// Set to 'true' if you want to send email notification in case of 'Error' or 'Critical'.
        /// </summary>
        public string ToEmailIds
        {
            get
            {
                foreach (DictionaryEntry de in this.Attributes)
                    if (de.Key.ToString().Equals("ToEmailIds", StringComparison.OrdinalIgnoreCase))
                        return de.Value.ToString();
                return string.Empty;
            }
        }        
        

        #region Private Methods
        private void TraceEventInMyOwnStyle(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (message != Environment.NewLine)
            {
                EventTypeFilter filter = this.Filter as EventTypeFilter;

                if (filter == null)
                    WriteInMyOwnStyle(message, eventType);
                else if (((filter.EventType | SourceLevels.Critical) == SourceLevels.Critical)
                                  && (eventType != TraceEventType.Critical))
                    return;
                else if (((filter.EventType | SourceLevels.Error) == SourceLevels.Error)
                               && (!((eventType == TraceEventType.Critical) || (eventType == TraceEventType.Error))))
                    return;
                else if (((filter.EventType | SourceLevels.Warning) == SourceLevels.Warning)
                                && (!((eventType == TraceEventType.Critical) || (eventType == TraceEventType.Error) || (eventType == TraceEventType.Warning))))
                    return;
                else if (((filter.EventType | SourceLevels.Information) == SourceLevels.Information)
                                && (!((eventType == TraceEventType.Critical) || (eventType == TraceEventType.Error) || (eventType == TraceEventType.Warning) || (eventType == TraceEventType.Information))))
                    return;
                else if (((filter.EventType | SourceLevels.Verbose) == SourceLevels.Verbose)
                            && (!((eventType == TraceEventType.Critical) || (eventType == TraceEventType.Error) || (eventType == TraceEventType.Warning) || (eventType == TraceEventType.Information) || (eventType == TraceEventType.Verbose))))
                    return;
                else
                    WriteInMyOwnStyle(message, eventType);

            }
            else
            {
                base.WriteLine("");
            }
        }

        private void WriteInMyOwnStyle(string message, TraceEventType eventType)
        {
            base.Write(string.Format("{0:s}", DateTime.Now));
            base.Write(" ");
            base.WriteLine(message);


            if ((eventType == TraceEventType.Critical) || (eventType == TraceEventType.Error))
            {                
                if (this.EmailErrors)
                {
                    new Net.PostMan().Send(string.Format("{0} - Critical Error", EyediaCoreConfigurationSection.CurrentConfig.InstanceName), TrimEmailMessage(message));                    
                }
            }
            
        }

        private string TrimEmailMessage(string message)
        {
            try
            {
                if (message.Contains("at System."))
                    return message.Substring(0, message.IndexOf("at System.")).Trim();
                else if (message.Contains("--->"))
                    return message.Substring(0, message.IndexOf("--->")).Trim();
                else if (message.Length > 100)
                    return message.Substring(0, 100).Trim();
                else
                    return message;
            }
            catch
            {
                return message;
            }
        }

        public const string ErrorStart = "::ErrorStart::";
        public const string ErrorEnd = "::ErrorEnd::";

        #endregion Private Methods
    }
}





