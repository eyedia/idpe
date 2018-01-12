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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using System.IO;
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{
    public class Attribute : INotifyPropertyChanged, IDisposable
    {
        int _Id;
        string _Name;
        string _Value;
        int _ValueEnumCode;
        string _ValueEnumValue;
        string _ValueEnumReferenceKey;
        SreMessage _Result;

        public event PropertyChangedEventHandler PropertyChanged;
 
        public Attribute(int id, string name, string value, string unknownMessage, SreTraceLogWriter traceWriter, bool isAcceptableOrPrintable, bool isAssociatedWithSystemRow = false, int? attributePrintValueType = 0, string attributePrintValueCustom = null)
        {
            this.IsAcceptableOrPrintable = isAcceptableOrPrintable;
            this.IsAssociatedWithSystemRow = isAssociatedWithSystemRow;
            OriginalValue = value;
            Init(id, name, value, unknownMessage, attributePrintValueType, attributePrintValueCustom);
            this.TraceWriter = traceWriter;
        }

        void Init(int id, string name, string value, string unknownMessage, int? attributePrintValueType = 0, string attributePrintValueCustom = null)
        {
            this._UpdateMatrix = new StringBuilder();
            this._Id = id;
            this._Name = name;
            this._Value = value;

            if ((attributePrintValueType == null)
                || (attributePrintValueType == 0))
            {
                this.AttributePrintValueType = AttributePrintValueTypes.Value;
                this.AttributePrintValueCustom = new StringBuilder();
            }
            else
            {
                this.AttributePrintValueType = (AttributePrintValueTypes)attributePrintValueType;
                if (!string.IsNullOrEmpty(attributePrintValueCustom))
                    this.AttributePrintValueCustom = new StringBuilder(attributePrintValueCustom);
                else
                    this.AttributePrintValueCustom = new StringBuilder();
            }


            if (EyediaCoreConfigurationSection.CurrentConfig.Tracking.UpdateMatrix)
                NotificationManager.Instance.BeforePerforming += new NotificationManager.NotificationHandler(NotificationManager_BeforePerforming);
            TrackUpdateMatrix("InitialValue", value);
        }

        #region Public Properties
        public int Id { get { return _Id; } set { _Id = value; } }
        public string Name
        {
            get { return _Name == null ? string.Empty : _Name; }
            set
            {
                if (value != this._Name)
                {
                    this._Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public string Value
        {
            get 
            {                
                return _Value == null ? string.Empty : _Value; 
            }
            set
            {
                OriginalValue = value == null ? "NULL" : value;
                if ((this.ValueUpdationNotPermitted) && (!this._Name.Equals("IsValid", StringComparison.OrdinalIgnoreCase)))
                {
                    this._Result = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_VALUE_UPDATION_NOT_PERMITTED);
                }
                else if (IsSreMessage(value))
                {
                    //no need to do anything
                }
                else if (value != this._Value)
                {
                    string oldValue = this._Value;
                    this._Value = value;

                    if (this.Type is SreString)
                        ((SreString)this.Type).Value = value;
                    else if (this.Type is SreInt)
                        ((SreInt)this.Type).Value = value;
                    else if (this.Type is SreBigInt)
                        ((SreBigInt)this.Type).Value = value;
                    else if (this.Type is SreDecimal)
                        ((SreDecimal)this.Type).Value = value;
                    else if (this.Type is SreDateTime)
                        ((SreDateTime)this.Type).Value = value;
                    else if (this.Type is SreGenerated)
                        ((SreGenerated)this.Type).Value = value;
                    else if (this.Type is SreCodeset)
                        ((SreCodeset)this.Type).Value = value;

                    TrackUpdateMatrix(GetCallerName(), value);
                    OnPropertyChanged("Value");
                    TraceWriter.WriteVerboseLine(string.Format("Value of {0} changed. Previous value was '{1}' new value is '{2}'", this._Name, oldValue, this._Value));
                }
            }
        }
        public bool ValueBoolean
        {
            get
            {
                //if (string.IsNullOrEmpty(Value)) return false;

                //if ((Value.ToLower().Equals("1"))
                //    || (Value.ToLower().Equals("true")))
                //    return true;
                //else
                //    return false;
                return Value.ParseBool();
            }

        }

        public int ValueInt
        {
            get
            {
                //try
                //{
                //    return int.Parse(Value);
                //}
                //catch 
                //{ 
                //    return 0; 
                //}

                return (int)Value.ParseInt();
            }
        }

        public long ValueLong
        {
            get
            {
                //try
                //{
                //    return long.Parse(Value);
                //}
                //catch
                //{
                //    return 0;
                //}

                return (long)Value.ParseLong();
            }
        }

        public Double ValueDouble
        {
            get
            {
                //try
                //{
                //    return Double.Parse(Value);
                //}
                //catch
                //{                    
                //    return 0;
                //}
                return (double)Value.ParseDouble();
            }
        }

        public decimal ValueDecimal
        {
            get
            {
                //try
                //{
                //    return decimal.Parse(Value);
                //}
                //catch
                //{
                //    return 0;
                //}
                return (decimal)Value.ParseDecimal();
            }
        }

        public DateTime ValueDate
        {
            get
            {
                ////this code is a replica of SREDateTime.TryExtractingSpecificType method
                //try
                //{
                //    if (!(Value.Contains("/")) && !(Value.Contains("-")) && !(Value.Contains(":")))
                //    {
                //        //special try 1
                //        if (Value.Length == 8)
                //        {
                //            return new DateTime(int.Parse(Value.Substring(0, 4)), int.Parse(Value.Substring(4, 2)), int.Parse(Value.Substring(6, 2)));
                //        }
                //    }
                //    else
                //    {                        
                //        return DateTime.Parse(Value);
                //    }
                //    return new DateTime();
                //}
                //catch { return new DateTime(); }

                return (DateTime)Value.ParseDateTime();
            }
        }


        public int ValueEnumCode
        {
            get { return _ValueEnumCode; }
            set
            {
                if (value != this._ValueEnumCode)
                {
                    int oldValue = this._ValueEnumCode;
                    this._ValueEnumCode = value;
                    OnPropertyChanged("ValueEnumCode");
                    TraceWriter.WriteVerboseLine(string.Format("ValueEnumCode of {0} changed. Previous value was '{1}' new value is '{2}'", this._Name, oldValue, this._ValueEnumCode));
                }
            }
        }
        public string ValueEnumValue
        {
            get { return _ValueEnumValue == null ? string.Empty : _ValueEnumValue; }
            set
            {
                if (value != this._ValueEnumValue)
                {
                    string oldValue = this._ValueEnumValue;
                    this._ValueEnumValue = value;
                    OnPropertyChanged("ValueEnumValue");
                    TraceWriter.WriteVerboseLine(string.Format("ValueEnumValue of {0} changed. Previous value was '{1}' new value is '{2}'", this._Name, oldValue, this._ValueEnumValue));
                }
            }
        }

        public string ValueEnumReferenceKey
        {
            get { return _ValueEnumReferenceKey == null ? string.Empty : _ValueEnumReferenceKey; }
            set
            {
                if (value != this._ValueEnumReferenceKey)
                {
                    string oldValue = this._ValueEnumReferenceKey;
                    this._ValueEnumReferenceKey = value;
                    OnPropertyChanged("ValueEnumReferenceKey");
                    TraceWriter.WriteVerboseLine(string.Format("ValueEnumReferenceKey of {0} changed. Previous value was '{1}' new value is '{2}'", this._Name, oldValue, this._ValueEnumReferenceKey));
                }
            }
        }

        /// <summary>
        /// Keeps original value
        /// </summary>
        public string OriginalValue { get; private set; }

        public bool IsNull { get; set; }

        public SreType Type { get; internal set; }

        public AttributePrintValueTypes AttributePrintValueType { get; private set; }
        public StringBuilder AttributePrintValueCustom { get; private set; }
        public bool IsAssociatedWithSystemRow { get; private set; }
        public bool IsAcceptableOrPrintable { get; private set; }

        public SreMessage Error
        {
            get
            { return _Result; }
            set
            {
                _Result = value;

                //Lets format the message and insert column name, as we have it
                if (!string.IsNullOrEmpty(_Result.Message)
                    && ((_Result.Message.Contains("Row[") == false) && (_Result.Message.Contains("RowSystem[") == false)))
                {
                    string pre = string.Format("{0}[{1}][{2}]:", IsAssociatedWithSystemRow ? "RowSystem" : "Row", _Result.RowPosition, Name);
                    _Result.Message = string.Format("{0} {1}", pre, _Result.Message);
                }
            }
        }

        public bool IgnoreParsing { get; set; }

        public bool ValueUpdationNotPermitted { get; internal set; }

        StringBuilder _UpdateMatrix;
        public string UpdateMatrix
        {
            get
            {
                string strUpdateMatrix = _UpdateMatrix.ToString();
                if ((strUpdateMatrix.Length >= 2) && (strUpdateMatrix.Substring(strUpdateMatrix.Length - 2) == ">>"))
                    return strUpdateMatrix.Substring(0, strUpdateMatrix.Length - 2);
                else
                    return strUpdateMatrix;
            }
            internal set
            {
                _UpdateMatrix.AppendLine(value);
            }
        }

        /// <summary>
        /// If an explicit error has been set using business rules
        /// </summary>
        public bool HasBusinessError { get; internal set; }

        public SreTraceLogWriter TraceWriter { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Assign(Attribute attribute)
        {
            //do not put this statement, let the error get thrown
            //if (attribute == null) return;

            this.Value = attribute.Value;
            this.IsNull = attribute.IsNull;
            this.Error = attribute.Error;
            this._Result = attribute._Result;
            this._ValueEnumCode = attribute.ValueEnumCode;
            this._ValueEnumValue = attribute.ValueEnumValue;

        }

        #endregion Public Methods

        #region Private Methods

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        string _LastExecutedRuleSetInformation;
        void NotificationManager_BeforePerforming(OperationInformation operationInformation)
        {

        }

        bool IsSreMessage(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;

            if (str.Contains(typeof(SreMessage).ToString()))
            {
                string[] sreMsgInfo = str.Split("|".ToCharArray());

                SreMessageCodes msgCode = (SreMessageCodes)Enum.Parse(typeof(SreMessageCodes), sreMsgInfo[1], true);
                _Result = new SreMessage(msgCode);
                _Result.Message = sreMsgInfo[2];
                return true;
            }
            return false;
        }

        void TrackUpdateMatrix(string callerName, string newValue)
        {
            if (EyediaCoreConfigurationSection.CurrentConfig.Tracking.UpdateMatrix)
                _UpdateMatrix.AppendFormat(EyediaCoreConfigurationSection.CurrentConfig.Tracking.UpdateMatrixFormat, callerName, string.IsNullOrEmpty(newValue) ? "NULL" : newValue);
        }
        string GetCallerName()
        {
            string callerName = string.Empty;
            StackTrace stackTrace = new StackTrace();
            callerName = stackTrace.GetFrame(2).GetMethod().Name;
            switch (callerName)
            {
                case "LoopPreset_Execute":
                    callerName = "By Default";
                    break;
                case "_InvokeMethodFast":
                    callerName = _LastExecutedRuleSetInformation;
                    break;
            }

            return callerName;
        }

        protected virtual string PrintRowColPosition()
        {
            return string.Format("{0}[{1}][{2}]: ", IsAssociatedWithSystemRow ? "RowSystem" : "Row", _Result.RowPosition, Name);
        }

        #endregion Private Methods

        #region IDisposable Members

        public void Dispose()
        {
            this.Error.Dispose();
            this._Result.Dispose();
            this._Name = null;
            this._Value = null;
            this._ValueEnumValue = null;
            this._ValueEnumReferenceKey = null;
        }

        #endregion
    }


}






