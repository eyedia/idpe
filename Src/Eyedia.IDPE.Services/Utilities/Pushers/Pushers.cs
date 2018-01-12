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
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// Pushers used to push output to another destination. Just override 'FileProcessed' method.
    /// In case of redirected output, this can be used to copy data back to original output folder.
    /// </summary>
	public class Pushers
	{
        /// <summary>
        /// Default constructor
        /// </summary>
        public Pushers() { }

        /// <summary>
        /// Depreciated. Do not use.
        /// </summary>
        public void Start()
        {
            //dummy method to initialize the class
        }

        /// <summary>
        /// This method will get called once a file has been processed. Override this method to push output to your desired destination.
        /// The base method deletes the file.
        /// </summary>
        /// <param name="e"></param>
        public virtual void FileProcessed(PullersEventArgs e)
        {            
            //File.Delete(e.FileName);
            Trace.Flush();
        }
	}

    /// <summary>
    /// Sample class for testing. Do not use.
    /// </summary>
    public class MyPusher : Pushers
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MyPusher() { }

        /// <summary>
        /// Overridden method to send output to desired destination. Just for testing, do not use.
        /// </summary>
        /// <param name="e"></param>
        public override void FileProcessed(PullersEventArgs e)
        {

            base.FileProcessed(e);
        }
    }
}





