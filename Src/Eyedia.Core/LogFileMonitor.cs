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
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace Eyedia.Core
{
    public class LogFileMonitorLineEventArgs : EventArgs
    {
        public string Line { get; set; }
    }

    public class LogFileMonitor
    {
        public EventHandler<LogFileMonitorLineEventArgs> OnLine;

        // file path + delimiter internals
        string _path = String.Empty;
        string _delimiter = String.Empty;

        // timer object
        Timer _t = null;

        // buffer for storing data at the end of the file that does not yet have a delimiter
        string _buffer = String.Empty;

        // get the current size
        long _currentSize = 0;

        // are we currently checking the log (stops the timer going in more than once)
        bool _isCheckingLog = false;

        protected bool StartCheckingLog()
        {
            lock (_t)
            {
                if (_isCheckingLog)
                    return true;

                _isCheckingLog = true;
                return false;
            }
        }

        protected void DoneCheckingLog()
        {
            lock (_t)
                _isCheckingLog = false;
        }

        public LogFileMonitor(string path, string delimiter = "\n")
        {
            _path = path;
            _delimiter = delimiter;
        }

        public void Start()
        {
            // get the current size
            _currentSize = new FileInfo(_path).Length;

            // start the timer
            _t = new Timer();
            _t.Elapsed += CheckLog;
            _t.AutoReset = true;
            _t.Start();
        }

        private void CheckLog(object s, ElapsedEventArgs e)
        {
            if (StartCheckingLog())
            {
                try
                {
                    // get the new size
                    var newSize = new FileInfo(_path).Length;

                    // if they are the same then continue.. if the current size is bigger than the new size continue
                    if (_currentSize >= newSize)
                        return;

                    // read the contents of the file
                    using (var stream = File.Open(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        // seek to the current file position
                        sr.BaseStream.Seek(_currentSize, SeekOrigin.Begin);

                        // read from current position to the end of the file
                        var newData = _buffer + sr.ReadToEnd();

                        // if we don't end with a delimiter we need to store some data in the buffer for next time
                        if (!newData.EndsWith(_delimiter))
                        {
                            // we don't have any lines to process so save in the buffer for next time
                            if (newData.IndexOf(_delimiter) == -1)
                            {
                                _buffer += newData;
                                newData = String.Empty;
                            }
                            else
                            {
                                // we have at least one line so store the last section (without lines) in the buffer
                                var pos = newData.LastIndexOf(_delimiter) + _delimiter.Length;
                                _buffer = newData.Substring(pos);
                                newData = newData.Substring(0, pos);
                            }
                        }

                        // split the data into lines
                        var lines = newData.Split(new string[] { _delimiter }, StringSplitOptions.RemoveEmptyEntries);

                        // send back to caller, NOTE: this is done from a different thread!
                        foreach (var line in lines)
                        {
                            if (OnLine != null)
                                OnLine(this, new LogFileMonitorLineEventArgs { Line = line });
                        }
                    }

                    // set the new current position
                    _currentSize = newSize;
                }
                catch (Exception)
                {
                }

                // we done..
                DoneCheckingLog();
            }
        }

        public void Stop()
        {
            if (_t == null)
                return;

            _t.Stop();
        }
    }
}


