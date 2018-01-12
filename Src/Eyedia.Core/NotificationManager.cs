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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Eyedia.Core
{
    public interface INotificationManager
    {
        string GetManagerName();
        string GetPoissibleOperationNames();
        string FireBeforePerforming(object thisObject, string operationName, Dictionary<string, object> parameters);
        void FireAfterPerforming(object thisObject, string operationId, Dictionary<string, object> parameters);
    }

    public class NotificationManager
    {
        public delegate void NotificationHandler(OperationInformation operationInformation);        
        public event NotificationHandler BeforePerforming;
        public event NotificationHandler AfterPerforming;

        Dictionary<string,object> _CurrentOperationsInformation;        

        
        NotificationManager()
        {
            Trace.TraceInformation("Notification manager has been initiated");
        }

        static NotificationManager _instance;
        public static NotificationManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NotificationManager();

                return _instance;
            }
        }

        bool _Initiated;
        public void Init()
        {
            this._CurrentOperationsInformation = new Dictionary<string, object>();
            this._Initiated = true;
        }

        public string FireBeforePerforming(object caller, string operationName, Dictionary<string, object> parameters)
        {
            IsInitiated();
            INotificationManager icaller = caller as INotificationManager;

            if (icaller == null)
                throw new InvalidOperationException("Caller is not INotificationManager type. Implement INotificationManager to use methods.");

            if (!(icaller.GetPoissibleOperationNames().Contains(operationName)))
                throw new InvalidOperationException(string.Format("Caller does not support '{0}' operation.", operationName));

            //add operation information
            OperationInformation opInfo = new OperationInformation(caller, operationName, parameters);
            this._CurrentOperationsInformation.Add(opInfo.OperationId, opInfo);

            //fire event
            if (this.BeforePerforming != null)
                this.BeforePerforming(opInfo);

            return opInfo.OperationId;
        }

        public void FireAfterPerforming(object caller, string operationId, Dictionary<string, object> parameters)
        {
            IsInitiated();

            INotificationManager icaller = caller as INotificationManager;

            if (icaller == null)
                throw new InvalidOperationException("Caller is not INotificationManager type. Implement INotificationManager to use methods.");


            if (this._CurrentOperationsInformation.ContainsKey(operationId))
            {

                OperationInformation opInfo = (OperationInformation)this._CurrentOperationsInformation[operationId];

                if (opInfo.Parameters != null)
                {
                    //Lets have all the available parameters
                    Dictionary<string, object> modifiedParam = new Dictionary<string, object>();
                    foreach (KeyValuePair<string, object> param in opInfo.Parameters)
                    {
                        if (!opInfo.Parameters.ContainsKey(param.Key))
                            opInfo.Parameters.Add(param.Key, param.Value);
                        else
                            modifiedParam.Add(param.Key, param.Value);
                    }

                    foreach (KeyValuePair<string, object> param in modifiedParam)
                    {
                        opInfo.Parameters[param.Key] = param.Value;
                    }
                }
                else
                {
                    opInfo.Parameters = parameters;
                }

                if (this.AfterPerforming != null)
                    this.AfterPerforming(opInfo);

                this._CurrentOperationsInformation.Remove(operationId);
            }
            else
            {
                throw new ArgumentException(string.Format("There is no operation initiated with id {0}. Or the operation has been expired", operationId));
            }
        }

        private void IsInitiated()
        {
            if (!this._Initiated)
                throw new InvalidOperationException("Notification manager is not been initiated");
        }        

        private void IsValidOperation(object caller, string operationName)
        {          
            INotificationManager icaller = caller as INotificationManager;

            if(icaller == null)
                throw new InvalidOperationException("Caller is not INotificationManager type. Implement INotificationManager to use methods.");

            if (!(icaller.GetPoissibleOperationNames().Contains(operationName)))
                throw new InvalidOperationException( string.Format("Caller does not support '{0}' operation.", operationName));
        }
    }

    public class NotificationArgs :EventArgs
    {
        public NotificationArgs(string operationName, Dictionary<string, object> parameters)
        {
            this._OperationName = operationName;
            this._Parameters = parameters;
        }

        string _OperationName;
        public string OperationName
        {
            get { return _OperationName;}
        }

        Dictionary<string, object> _Parameters;
        public Dictionary<string, object> Parameters
        {
            get { return _Parameters; }
        }
    }
    
    public struct OperationInformation
    {
        public OperationInformation(object raisedBy, string operationName, Dictionary<string, object> parameters)
        {
            this._RaisedBy = raisedBy;
            this._OperationId = Guid.NewGuid();
            this._OperationName = operationName;
            this._Parameters = parameters;
        }

        object _RaisedBy;
        public object RaisedBy
        {
            get { return _RaisedBy; }
        }
        
        Guid _OperationId;
        public string OperationId
        {
            get { return _OperationId.ToString(); }
        }

        string _OperationName;
        public string OperationName
        {
            get { return _OperationName; }
        }

        Dictionary<string, object> _Parameters;
        public Dictionary<string, object> Parameters
        {
            get { return _Parameters; }
            internal set { _Parameters = value; }
        }
    }
}





