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
using System.Reflection;

namespace Eyedia.IDPE.Services
{
    public class RestApiClient
    {
        public string RestUrl { get; set; }        
        public string ResponseStatusCode { get; set; }
        public string ResponseContent { get; set; }
        public string ResponseErrorMessage { get; set; }
        public int TimeOut { get; set; }

        public RestApiClient(string restUrl, int timeOut = 2)
        {
            this.RestUrl = restUrl;
            this.TimeOut = TimeOut;
        }

        public string Execute(string restResource, string method, string json)
        {
            #region OriginalCode
            //var request = new RestRequest(RestResource, Method.GET);
            //var response = new RestClient(RestUrl).Execute(request);
            #endregion OriginalCode

            method = method.ToUpper();
            Type restRequestType = Type.GetType("RestSharp.RestRequest, RestSharp");
            Type restClientType = Type.GetType("RestSharp.RestClient, RestSharp");
            Type restEnumMethodType = Type.GetType("RestSharp.Method, RestSharp");
            Type restEnumRequestParameterType = Type.GetType("RestSharp.ParameterType, RestSharp");

            if ((restRequestType == null) || (restClientType == null) || (restEnumMethodType == null) || (restEnumRequestParameterType == null))
                throw new Exception("RestSharp.dll was not found!");

            object objEnumMethodType = null;

            if (!string.IsNullOrEmpty(json))
                objEnumMethodType = GetEnumType(restEnumMethodType, "POST");
            else
                objEnumMethodType = GetEnumType(restEnumMethodType, method);

            var request = Activator.CreateInstance(restRequestType, restResource, objEnumMethodType);
            //request.GetType().GetProperty("TimeOut").SetValue(request, TimeOut);

            if (!string.IsNullOrEmpty(json))
            {
                List<MethodInfo> addParameterMethods = request.GetType().GetMethods().Where(m => m.Name == "AddParameter" && m.IsGenericMethod == false && m.GetParameters().Length == 3).ToList();
                if (addParameterMethods.Count == 1)
                {
                    object objEnumParameterType = GetEnumType(restEnumRequestParameterType, "RequestBody");
                    var addParamResult = addParameterMethods[0].Invoke(request, new object[] { "application/json", json, objEnumParameterType });
                }
            }

            var client = Activator.CreateInstance(restClientType, RestUrl);
            MethodInfo executeMethod = client.GetType().GetMethods().First(m => m.Name == "Execute" && m.IsGenericMethod == false);
            var response = executeMethod.Invoke(client, new object[] { request });

            ResponseStatusCode = response.GetType().GetProperty("StatusCode").GetValue(response, null).ToString();
            ResponseContent = response.GetType().GetProperty("Content").GetValue(response, null).ToString();
            var objErrorMessage = response.GetType().GetProperty("ErrorMessage").GetValue(response, null);
            if (objErrorMessage != null)
                ResponseErrorMessage = objErrorMessage.ToString();
            else
                ResponseErrorMessage = string.Empty;

            return ResponseStatusCode;
        }

     

        private object GetEnumType(Type enumType, string enumValue)
        {
            object objEnumValue = null;
            if (Enum.IsDefined(enumType, enumValue))
                objEnumValue = Enum.Parse(enumType, enumValue);

            if (objEnumValue == null)
                throw new Exception(string.Format("The enum value is not defined in {0}", enumType.ToString()));

            return objEnumValue;
        }
    }
}


