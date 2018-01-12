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
using System.Activities;
using System.ComponentModel;
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{
    [Designer(typeof(Eyedia.IDPE.Services.WorkflowActivities.RestApiCall))]
    public sealed class RestApiCall : CodeActivity
    {
        public InArgument<Job> Job { get; set; }
        public InArgument<WorkerData> Data { get; set; }
        public InArgument<string> RestUrl { get; set; }
        public InArgument<string> Json { get; set; }
        public InArgument<string> Method { get; set; }
        public InArgument<string> RestResource { get; set; }
        public InArgument<int> TimeOut { get; set; }
        public OutArgument<string> RestResponse { get; set; }
        public OutArgument<string> RestResponseStatusCode { get; set; }
        public OutArgument<string> RestResponseErrorMessage { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string restUrl = context.GetValue(this.RestUrl);
            string restResource = context.GetValue(this.RestResource);
            string method = context.GetValue(this.Method);
            string json = context.GetValue(this.Json);

            if (string.IsNullOrEmpty(method))
                method = "GET";

            int timeOut = context.GetValue(this.TimeOut);
            if (timeOut == 0)
                timeOut = 2 * 60 * 1000;//default is 2 minutes

            RestApiClient restClient = new RestApiClient(restUrl, timeOut);
            string responseStatusCode = restClient.Execute(restResource, method, json);

            ExtensionMethods.TraceInformation("Executing REST API {0}{1}. Return Code = {2}", restUrl, restResource, responseStatusCode);
            Trace.Flush();
            RestResponseStatusCode.Set(context, responseStatusCode);
            RestResponse.Set(context, restClient.ResponseContent);
            RestResponseErrorMessage.Set(context, restClient.ResponseErrorMessage);
        }
    }
}



