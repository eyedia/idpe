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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Linq;
using System.Activities;
using System.Diagnostics;
using System.Collections.Generic;
using Eyedia.IDPE.Common;


namespace Eyedia.IDPE.Services
{
    public sealed class ValidateAttributes : CodeActivity<Int32>
    {
        public InArgument<Job> Job { get; set; }        
        public OutArgument<Int32> FixedLengthFormatTotalRowLength { get; set; }

        protected override Int32 Execute(CodeActivityContext context)
        {
            Job job = Job.Get(context);
            job.TraceInformation("Attributes Expected:{0}, found:{1}", job.DataSource.AcceptableAttributes.Count, job.ColumnCount);
            
            if (job.DataSource.AcceptableAttributes.Count == 0)
            {                
                string message = string.Empty;
                if (job.DataSource.Id > 0)
                    message = string.Format("No attribute defined for data source id {0}", job.DataSource.Id);
                else
                    message = string.Format("No attribute defined for data source name '{0}'", job.DataSource.Name);
                job.TraceError(message + ". Aborting...");                
                job.Errors.Add(message);
                return 1;
            }
            else if (job.DataSource.DataFormatType == DataFormatTypes.Delimited)
            {
                if (job.DataSource.AcceptableAttributes.Count == job.ColumnCount)
                {
                    return 100;
                }
                else
                {
                    job.Errors.Add(string.Format("{0}Attributes not recieved as expected. Expected {1}, received {2}", PrintRowColPosition(), job.DataSource.AcceptableAttributes.Count, job.ColumnCount));
                    return 2;
                }
            }
            else if (job.DataSource.DataFormatType == DataFormatTypes.FixedLength)
            {                
                return 100;
            }
            else if (job.DataSource.AcceptableAttributes.Count == job.ColumnCount)
            {
                
                return 100;
            }
            else
            {
                job.Errors.Add(string.Format("{0}Attributes not recieved as expected. Expected {1}, received {2}", PrintRowColPosition(), job.DataSource.AcceptableAttributes.Count, job.ColumnCount));
                return 2;
            }
            
        }
        private string PrintRowColPosition()
        {   
            return "Row[0][*]: ";
        }
    }
}



