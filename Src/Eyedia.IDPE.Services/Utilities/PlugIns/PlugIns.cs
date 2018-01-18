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
using System.Diagnostics;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Services
{
    public abstract class PlugIns
    {
        protected Job _Job;
        public PlugIns() {}
        public PlugIns(Job job) { _Job = job; }

        public virtual object Execute(string method)
        {
            try
            {
                Object instance = null;
                MethodInfo thisMethod = GetMethod(method, ref instance);
                if ((thisMethod != null) && (instance != null))
                    return thisMethod.Invoke(instance, null);
            }
            catch (Exception ex)
            {
                result.Message = string.Format(result.Message, LogError(ex));
            }
            return result;
        }

        public virtual object Execute(string method, object param1)
        {            
            try
            {
                Object instance = null;
                MethodInfo thisMethod = GetMethod(method, ref instance);
                if ((thisMethod != null) && (instance != null))
                    return thisMethod.Invoke(instance, new object[] { param1 });
            }
            catch (Exception ex)
            {
                result.Message = string.Format(result.Message, LogError(ex));
            }
            return result;
        }

        public virtual object Execute(string method, object param1, object param2)
        {
            try
            {
                Object instance = null;
                MethodInfo thisMethod = GetMethod(method, ref instance);
                if ((thisMethod != null) && (instance != null))
                    return thisMethod.Invoke(instance, new object[] { param1, param2 });
            }
            catch (Exception ex)
            {
                result.Message = string.Format(result.Message, LogError(ex));
            }
            return result;
        }

        public virtual object Execute(string method, object param1, object param2, object param3)
        {
            try
            {
                Object instance = null;
                MethodInfo thisMethod = GetMethod(method, ref instance);
                if ((thisMethod != null) && (instance != null))
                    return thisMethod.Invoke(instance, new object[] { param1, param2, param3 });
            }
            catch (Exception ex)
            {
                result.Message = string.Format(result.Message, LogError(ex));
            }
            return result;
        }

        public virtual object Execute(string method, object param1, object param2, object param3, object param4)
        {
            try
            {
                Object instance = null;
                MethodInfo thisMethod = GetMethod(method, ref instance);
                if ((thisMethod != null) && (instance != null))
                    return thisMethod.Invoke(instance, new object[] { param1, param2, param3, param4 });
            }
            catch (Exception ex)
            {
                result.Message = string.Format(result.Message, LogError(ex));
            }
            return result;
        }

        public virtual object Execute(string method, params object[] parameters)
        {

            try
            {
                Object instance = null;
                MethodInfo thisMethod = GetMethod(method, ref instance);
                if ((thisMethod != null) && (instance != null))
                    return thisMethod.Invoke(instance, parameters);
            }
            catch (Exception ex)
            {
                result.Message = string.Format(result.Message, LogError(ex));
            }
            return result;
        }

        IdpeMessage result;
        private MethodInfo GetMethod(string method, ref object instance)
        {
            result = new IdpeMessage(IdpeMessageCodes.IDPE_PLUGIN_METHOD_EXECUTION_FAILED);            
            MethodInfo thisMethod = null;
            Type thisType = this.GetType();
            Type[] types = new Type[1];
            types[0] = typeof(Job);
            ConstructorInfo constructor = thisType.GetConstructor(BindingFlags.Instance | BindingFlags.Public,
                null, CallingConventions.HasThis, types, null);

            if (constructor == null)
                goto Error;
            instance = constructor.Invoke(new object[] { _Job });
            thisMethod = thisType.GetMethod(method);
            Error:
            if (thisMethod == null)
            {                
                result = new IdpeMessage(IdpeMessageCodes.IDPE_PLUGIN_METHOD_EXECUTION_FAILED_METHOD_NOT_DEFINED);
                string traceErrMsg = string.Format("'{0}' is not implemented in {1}", method, thisType.FullName);
                if (_Job != null)
                {
                    traceErrMsg += string.Format(". Job Id = '{0}'", _Job.JobIdentifier);
                    traceErrMsg += string.Format(". Data Source Id = '{0}'", _Job.DataSource.Name);
                }
                traceErrMsg += Environment.NewLine + new StackTrace().ToString();            
                _Job.TraceError(traceErrMsg);
                return null;
            }
            return thisMethod;
        }
        
        private string LogError(Exception ex)
        {
            string errorId = Guid.NewGuid().ToString();
            string errorMessage = errorId + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);
            _Job.TraceError(errorMessage);
            return errorId;
        }
    }
}





