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
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using Eyedia.Core.Data;
using System.Text.RegularExpressions;

namespace Eyedia.IDPE.Services
{

    #region SreType
    public abstract class SreType
    {
        string _ColumnName;
        public string ColumnName { get { return _ColumnName; } }

        protected string _Formula;
        public string Formula { get { return _Formula; } }

        AttributeTypes _Type;
        public AttributeTypes Type { get { return _Type; } }

        protected string _Value;
        public string Value { get { return _Value; } protected set { _Value = value; } }

        string _Minimum;
        public string Minimum { get { return _Minimum; } }

        string _Maximum;
        public string Maximum { get { return _Maximum; } }

        protected bool _IsParsed;
        public bool IsParsed { get { return _IsParsed; } }

        protected bool _IsHavingSqlQuery;
        public bool IsHavingSqlQuery { get { return _IsHavingSqlQuery; } }

        string _ConnectionStringKeyName;
        public string ConnectionStringKeyName { get { return _ConnectionStringKeyName; } internal set { _ConnectionStringKeyName = value; } }

        string _ConnectionString;
        public string ConnectionString { get { return _ConnectionString; } internal set { _ConnectionString = value; } }

        SreKeyTypes _DatabaseType;
        public SreKeyTypes DatabaseType { get { return _DatabaseType; } internal set { _DatabaseType = value; } }

        List<SreKey> _DataSourceKeys;
        public List<SreKey> DataSourceKeys { get { return _DataSourceKeys; } }

        protected int _RecordPosition;
        public int RecordPosition { get { return _RecordPosition; } }

        protected SreMessage _ParseResult;
        public SreMessage ParseResult { get { return _ParseResult; } }

        public bool IsAssociatedWithSystemRow { get; private set; }

        protected int? ValueInt;
        protected long? ValueLong;
        protected double? ValueDouble;
        protected DateTime? ValueDateTime;
        protected bool? ValueBool;
        protected SqlClientManager SqlClientManager;

        public bool IsNull { get; private set; }

        public SreType(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum,
            bool isAssociatedWithSystemRow, int recordPosition, SqlClientManager sqlClientManager, List<SreKey> dataSourceKeys)
        {
            _ConnectionStringKeyName = Constants.DefaultConnectionStringKeyName;
            SqlClientManager = sqlClientManager;
            _DataSourceKeys = dataSourceKeys;
            _ConnectionString = DataSourceKeys.GetDefaultConnectionString();
            bool result = Init(columnName, value, type, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition);
            if (result)
                _ParseResult = new SreMessage(SreMessageCodes.SRE_SUCCESS);
            else
                _ParseResult = new SreMessage(SreMessageCodes.SRE_FAILED);
        }

        public SreType(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum,
            bool isAssociatedWithSystemRow, int recordPosition)
        {
            _ConnectionStringKeyName = Constants.DefaultConnectionStringKeyName;
            bool result = Init(columnName, value, type, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition);
            if (result)
                _ParseResult = new SreMessage(SreMessageCodes.SRE_SUCCESS);
            else
                _ParseResult = new SreMessage(SreMessageCodes.SRE_FAILED);
        }

        bool Init(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum,
            bool isAssociatedWithSystemRow, int recordPosition)
        {
            this._ColumnName = columnName;
            this._Value = value;
            this._Type = type;
            this._Formula = formula;
            this._Minimum = minimum;
            this._Maximum = maximum;
            this.IsAssociatedWithSystemRow = isAssociatedWithSystemRow;
            this._RecordPosition = recordPosition;

            if ((string.IsNullOrEmpty(_Value))
                && (_Type == AttributeTypes.Bit))
            {
                _Value = "False";
                return true;
            }
            else if (_Value.ToUpper() == "NULL")
            {                
                IsNull = true;
                return true;
            }
          
            bool result = true;
     
            switch (_Type)
            {
                case AttributeTypes.Int:
                    int? tempInt = _Value.ParseInt(ref result);
                    if (result)
                        ValueInt = tempInt;
                    break;

                case AttributeTypes.BigInt:
                    long? tempLong = _Value.ParseLong(ref result);
                    if (result)
                        ValueLong = tempLong;
                    break;

                case AttributeTypes.Decimal:
                    double? tempDouble = _Value.ParseDouble(ref result);
                    if (result)
                        ValueDouble = tempDouble;
                    break;

                case AttributeTypes.Datetime:
                    DateTime? tempDateTime = _Value.ParseDateTime();
                    if (result)
                        ValueDateTime = tempDateTime;
                    break;

                case AttributeTypes.Bit:
                    bool? tempBool = _Value.ParseBool(ref result);
                    if (result)
                        ValueBool = tempBool;
                    break;

            }
           

            return result;
        }

        public abstract SreMessage Parse(bool onlyConstraints);
        public abstract void CheckConstraints();

        protected virtual string PrintRowColPosition()
        {
            return string.Format("{0}[{1}][{2}]: ", IsAssociatedWithSystemRow ? "RowSystem" : "Row", _RecordPosition, ColumnName);
        }
        public virtual void OverrideFormula(string formattedSQLQuery)
        {
            this._Formula = formattedSQLQuery;
        }

    }

    #endregion SreType

    #region SreTypeFactory
    public static class SreTypeFactory
    {
        public static SreType GetInstance(string columnName, string value, string type, string formula, string minimum, string maximum,
            bool isAssociatedWithSystemRow, int recordPosition, SqlClientManager sqlClientManager, List<SreKey> dataSourceKeys)
        {
            AttributeTypes thisType = (AttributeTypes)Enum.Parse(typeof(AttributeTypes), type, true);

            switch (type.ToUpper())
            {
                case "INT":
                    return new SreInt(columnName, value, thisType, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition);

                case "BIGINT":
                    return new SreBigInt(columnName, value, thisType, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition);

                case "DECIMAL":
                    return new SreDecimal(columnName, value, thisType, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition);

                case "BIT":
                    return new SreBit(columnName, value, thisType, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition);

                case "STRING":
                    return new SreString(columnName, value, thisType, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition);

                case "DATETIME":
                    return new SreDateTime(columnName, value, thisType, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition);

                case "CODESET":
                    return new SreCodeset(columnName, value, thisType, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition, sqlClientManager, dataSourceKeys);

                case "REFERENCED":
                    return new SreReferenced(columnName, value, thisType, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition, sqlClientManager, dataSourceKeys);

                case "NOTREFERENCED":
                    return new SreNotReferenced(columnName, value, thisType, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition, sqlClientManager, dataSourceKeys);

                case "GENERATED":
                    return new SreGenerated(columnName, value, thisType, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition, sqlClientManager, dataSourceKeys);

                default:
                    return new SreUnknown(columnName, value, thisType, formula, minimum, maximum, recordPosition);
            }

        }

    }
    #endregion SRETypeFactory

    #region SreInt
    public class SreInt : SreType
    {
        public SreInt(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum, bool isAssociatedWithSystemRow, int recordPosition)
            : base(columnName, value, type, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition)
        {
        }

        public new string Value
        {
            get { return base.ValueInt.ToString(); }
            set
            {
                base.Value = value;
                TryExtractingSpecificType();
            }
        }
        public override SreMessage Parse(bool onlyConstraints)
        {
            try
            {
                if (base.Value.ToUpper() != "NULL")
                {
                    this._ParseResult = new SreMessage(SreMessageCodes.SRE_SUCCESS);
                    if (ValueInt == null)
                    {
                        TryExtractingSpecificType();
                        if (ValueInt == null)  //still null, then throw 
                        {
                            this._ParseResult = new SreMessage(SreMessageCodes.SRE_INT_TYPE_DATA_VALIDATION_FAILED);
                            this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Type, base.Value);
                            return this._ParseResult;
                        }
                    }

                    Value = ValueInt.ToString();
                    CheckConstraints();
                }
               

            }
            catch (Exception ex)
            {                
                ExtensionMethods.TraceError(ex.ToString());
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_INT_TYPE_DATA_VALIDATION_FAILED);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Type, base.Value);

            }
            _IsParsed = true;
            return this._ParseResult;
        }

        public override void CheckConstraints()
        {
            long min = 0;
            long max = 0;
            if (!string.IsNullOrEmpty(Minimum))
                min = (long)Minimum.ParseLong();
            if (!string.IsNullOrEmpty(Maximum))
                max = (long)Maximum.ParseLong();

            SreMessage result = null;
            if ((!string.IsNullOrEmpty(Minimum)) && (ValueInt < min))
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MINIMUM);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), ColumnName, Minimum);
            }
            else if ((!string.IsNullOrEmpty(Maximum)) && (ValueInt > max))
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MAXIMUM);
                this._ParseResult.Message = string.Format(result.Message, PrintRowColPosition(), ColumnName, Maximum);
            }

        }

        private void TryExtractingSpecificType()
        {
            //try
            //{
            //    base.ValueInt = int.Parse(base.Value);
            //}
            //catch { }

            bool result = false;
            int tryInt = (int)base.Value.ParseInt(ref result);
            if (result)
                base.ValueInt = tryInt;
        }
    }
    #endregion SreInt

    #region SreBigInt
    public class SreBigInt : SreType
    {
        public SreBigInt(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum, bool isAssociatedWithSystemRow, int recordPosition)
            : base(columnName, value, type, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition)
        {
        }

        public new string Value
        {
            get { return base.ValueDouble.ToString(); }
            set
            {
                base.Value = value;
                TryExtractingSpecificType();
            }
        }

        public override SreMessage Parse(bool onlyConstraints)
        {

            try
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_SUCCESS);
                if (base.Value.ToUpper() != "NULL")
                {
                    if (ValueLong == null)
                    {
                        TryExtractingSpecificType();
                        if (ValueLong == null)  //still null, then throw 
                        {
                            this._ParseResult = new SreMessage(SreMessageCodes.SRE_BIGINT_TYPE_DATA_VALIDATION_FAILED);
                            this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Type, base.Value);
                            return this._ParseResult;
                        }
                    }

                    Value = ValueLong.ToString();
                    CheckConstraints();
                }

            }
            catch (Exception ex)
            {
                ExtensionMethods.TraceError(ex.ToString());
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_BIGINT_TYPE_DATA_VALIDATION_FAILED);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Type, base.Value);
            }
            _IsParsed = true;
            return this._ParseResult;
        }

        public override void CheckConstraints()
        {
            long min = 0;
            long max = 0;
            if (!string.IsNullOrEmpty(Minimum))
                min = (long)Minimum.ParseLong();
            if (!string.IsNullOrEmpty(Maximum))
                max = (long)Maximum.ParseLong();

            if ((!string.IsNullOrEmpty(Minimum)) && (ValueLong < min))
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MINIMUM);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), ColumnName, Minimum);
            }
            else if ((!string.IsNullOrEmpty(Maximum)) && (ValueLong > max))
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MAXIMUM);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), ColumnName, Maximum);
            }

        }

        private void TryExtractingSpecificType()
        {        
            bool result = false;
            long trylong = (long)base.Value.ParseLong(ref result);
            if (result)
                base.ValueLong = trylong;
        }
    }
    #endregion SreBigInt

    #region SreDecimal
    public class SreDecimal : SreType
    {
        public SreDecimal(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum, bool isAssociatedWithSystemRow, int recordPosition)
            : base(columnName, value, type, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition)
        {
        }

        public new string Value
        {
            get { return base.ValueDouble.ToString(); }
            set
            {
                base.Value = value;

            }
        }

        public override SreMessage Parse(bool onlyConstraints)
        {
            try
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_SUCCESS);
                if (base.Value.ToUpper() != "NULL")
                {
                    if (ValueDouble == null)
                    {
                        TryExtractingSpecificType();
                        if (ValueDouble == null)        //still null
                        {
                            this._ParseResult = new SreMessage(SreMessageCodes.SRE_DECIMAL_TYPE_DATA_VALIDATION_FAILED);
                            this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Type, base.Value);
                        }
                    }
                    else
                    {

                        Value = ValueDouble.ToString();
                        CheckConstraints();
                    }
                }
            }
            catch (Exception ex)
            {
                ExtensionMethods.TraceError(ex.ToString());
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_DECIMAL_TYPE_DATA_VALIDATION_FAILED);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Type, Value);
            }
            _IsParsed = true;
            return this._ParseResult;
        }

        public override void CheckConstraints()
        {
            double min = 0;
            double max = 0;
            if (!string.IsNullOrEmpty(Minimum))
                min = (double)Minimum.ParseDouble();
            if (!string.IsNullOrEmpty(Maximum))
                max = (double)Maximum.ParseDouble();

            if ((!string.IsNullOrEmpty(Minimum)) && (ValueDouble < min))
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MINIMUM);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), ColumnName, Minimum);
            }
            else if ((!string.IsNullOrEmpty(Maximum)) && (ValueDouble > max))
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MAXIMUM);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), ColumnName, Maximum);
            }

        }

        private void TryExtractingSpecificType()
        {
            bool result = false;
            double trydouble = (double)base.Value.ParseDouble(ref result);
            if (result)
                base.ValueDouble = trydouble;
        }
    }
    #endregion SreDecimal

    #region SreBit
    public class SreBit : SreType
    {
        public SreBit(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum, bool isAssociatedWithSystemRow, int recordPosition)
            : base(columnName, value, type, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition)
        {
        }

        public new string Value
        {
            get { return base.ValueDouble.ToString(); }
            set
            {
                base.Value = value;
                TryExtractingSpecificType();
            }
        }

        public override SreMessage Parse(bool onlyConstraints)
        {

            try
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_SUCCESS);
                if (base.Value.ToUpper() != "NULL")
                {
                    if (ValueBool == null)
                    {
                        this._ParseResult = new SreMessage(SreMessageCodes.SRE_BIT_TYPE_DATA_VALIDATION_FAILED);
                        this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Type, Value);
                        return this._ParseResult;
                    }

                    CheckConstraints();

                }
            }
            catch (Exception ex)
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_BIT_TYPE_DATA_VALIDATION_FAILED);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Type, Value);
                ExtensionMethods.TraceError(ex.ToString());
            }
            _IsParsed = true;
            return this._ParseResult;
        }

        private void TryExtractingSpecificType()
        {
            //try
            //{
            //    base.ValueBool = base.Value.ParseBool();
            //}
            //catch { }

            bool result = false;
            bool trybool = base.Value.ParseBool(ref result);
            if (result)
                base.ValueBool = trybool;
        }

        public override void CheckConstraints() { }
    }
    #endregion SreBit

    #region SreString
    public class SreString : SreType
    {
        public SreString(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum, bool isAssociatedWithSystemRow, int recordPosition)
            : base(columnName, value, type, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition)
        {
        }

        public new string Value
        {
            get { return base.Value.ToString(); }
            set
            {
                base.Value = value;
            }
        }

        public override SreMessage Parse(bool onlyConstraints)
        {
            try
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_SUCCESS);
                CheckConstraints();
            }
            catch (Exception ex)
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_STRING_TYPE_DATA_VALIDATION_FAILED);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Type, Value);
                ExtensionMethods.TraceError(ex.ToString());
            }
            _IsParsed = true;
            return this._ParseResult;
        }

        public override void CheckConstraints()
        {
            long min = 0;
            long max = 0;
            if (!string.IsNullOrEmpty(Minimum))
                min = (long)Minimum.ParseLong();
            if (!string.IsNullOrEmpty(Maximum))
                max = (long)Maximum.ParseLong();

            if ((!string.IsNullOrEmpty(Minimum)) && (Value.Length < min))
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MINIMUM_STRING);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), ColumnName, Minimum);
            }
            else if ((!string.IsNullOrEmpty(Maximum)) && (Value.Length > max))
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MAXIMUM_STRING);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), ColumnName, Maximum);
            }
        }
    }
    #endregion SreString

    #region SreDateTime
    public class SreDateTime : SreType
    {
        public SreDateTime(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum, bool isAssociatedWithSystemRow, int recordPosition)
            : base(columnName, value, type, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition)
        {
        }

        public new string Value
        {
            get { return base.ValueDateTime.ToString(); }
            set
            {
                base.Value = value;
                TryExtractingSpecificType();
            }
        }
        public override SreMessage Parse(bool onlyConstraints)
        {
            try
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_SUCCESS);
                if (base.Value.ToUpper() != "NULL")
                {
                    if (ValueDateTime == null)
                    {
                        TryExtractingSpecificType();
                        if (ValueDateTime == null)  //still null, then throw 
                        {
                            this._ParseResult = new SreMessage(SreMessageCodes.SRE_DATE_TYPE_DATA_VALIDATION_FAILED);
                            this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Type, base.Value);
                            return this._ParseResult;
                        }
                    }

                    CheckConstraints();
                }
            }
            catch (Exception ex)
            {
                ExtensionMethods.TraceError(ex.ToString());
            }
            _IsParsed = true;
            return this._ParseResult;
        }
        private void TryExtractingSpecificType()
        {
            //this code is a replica of Attribute.ValueDate property
            //try
            //{
            //    if (!(base.Value.Contains("/")) && !(base.Value.Contains("-")) && !(base.Value.Contains(":")))
            //    {
            //        //special try 1
            //        if (base.Value.Length == 8)
            //        {
            //            base.ValueDateTime = new DateTime(int.Parse(base.Value.Substring(0, 4)), int.Parse(base.Value.Substring(4, 2)), int.Parse(base.Value.Substring(6, 2)));
            //        }
            //    }
            //    else
            //    {
            //        //standard try
            //        base.ValueDateTime = DateTime.Parse(base.Value);
            //    }
            //}
            //catch { }

            bool result = false;
            DateTime tryDateTime = (DateTime)base.Value.ParseDateTime(ref result);
            if (result)
                base.ValueDateTime = tryDateTime;
        }

        public override void CheckConstraints()
        {
            DateTime value = (DateTime)ValueDateTime;
            this._Value = value.ToDBDateTimeFormat();

            DateTime min = DateTime.Now;
            if (string.IsNullOrEmpty(Minimum)) goto CheckMax;

            #region Checking Minimum

            if (!DateTime.TryParse(Minimum, out min))
                min = DateTime.Now;

            if (Minimum.ToUpper().Equals("CURRENTDATETIME+"))
            {
                if (value < min)
                {
                    this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MINIMUM_DATETIME_SERVER);
                    this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Value, min);
                }
            }
            else if (Minimum.ToUpper().Equals("CURRENTDATETIME-"))
            {
                if (value > min)
                {
                    this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MAXIMUM_DATETIME_SERVER);
                    this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Value, min);
                }
            }
            else if (Minimum.ToUpper().Equals("CURRENTDATE+"))
            {
                DateTime d1 = (DateTime)value.ToShortDateString().ParseDateTime();
                DateTime d2 = (DateTime)min.ToShortDateString().ParseDateTime();

                if (d1 < d2)
                {
                    this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MINIMUM_DATE_SERVER);
                    this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Value, d2);
                }
            }
            else if (Minimum.ToUpper().Equals("CURRENTDATE-"))
            {
                DateTime d1 = (DateTime)value.ToShortDateString().ParseDateTime();
                DateTime d2 = (DateTime)min.ToShortDateString().ParseDateTime();

                if (d1 > d2)
                {
                    this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MAXIMUM_DATE_SERVER);
                    this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Value, d2);
                }
            }
            #endregion Checking Minimum

        CheckMax:
            if (string.IsNullOrEmpty(Maximum)) return; //no need to check further

            #region Checking Maximum
            DateTime max = DateTime.Now;
            if (!DateTime.TryParse(Maximum, out max))
                max = DateTime.Now;

            //As we have both Minimum & Maximum, lets try a logical validation
            //logical validation starts
            if ((!string.IsNullOrEmpty(Minimum)) && (max <= min))
                throw new Exception(string.Format("Attribute {0} has some invalid configuration. Please check Minimum and Maximum, else contact administrator or read manual for more details.", ColumnName));
            //logical validation ends

            if (Maximum.ToUpper().Equals("CURRENTDATETIME+"))
            {
                if (value < max)
                {
                    this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MINIMUM_DATETIME_SERVER);
                    this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Value, max);
                }
            }
            else if (Maximum.ToUpper().Equals("CURRENTDATETIME-"))
            {
                if (value > max)
                {
                    this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MAXIMUM_DATETIME_SERVER);
                    this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Value, min);
                }
            }
            else if (Maximum.ToUpper().Equals("CURRENTDATE+"))
            {
                DateTime d1 = (DateTime)value.ToShortDateString().ParseDateTime();
                DateTime d2 = (DateTime)max.ToShortDateString().ParseDateTime();

                if (d1 < d2)
                {
                    this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MINIMUM_DATE_SERVER);
                    this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Value, d2);
                }
            }
            else if (Maximum.ToUpper().Equals("CURRENTDATE-"))
            {
                DateTime d1 = (DateTime)value.ToShortDateString().ParseDateTime();
                DateTime d2 = (DateTime)max.ToShortDateString().ParseDateTime();

                if (d1 > d2)
                {
                    this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MAXIMUM_DATE_SERVER);
                    this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Value, d2);
                }
            }
            #endregion Checking Maximum

            if (!((value >= min) && (value <= max)))
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MINIMUM_MAXIMUM_DATE);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), value, min, max);
            }
        }
    }
    #endregion SreDateTime

    #region SreCodeset
    public class SreCodeset : SreType
    {
        public SreCodeset(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum, bool isAssociatedWithSystemRow, int recordPosition, SqlClientManager sqlClientManager, List<SreKey> dataSourceKeys)
            : base(columnName, value, type, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition, sqlClientManager, dataSourceKeys)
        {
            //prevent null error;            
            _ValueEnumValue = string.Empty;
            _ReferenceKey = string.Empty;
        }

        public new string Value
        {
            get { return base.Value; }
            set
            {
                base.Value = value;
            }
        }

        int _ValueEnumCode;
        public int ValueEnumCode
        {
            get { return _ValueEnumCode; }
        }

        string _ValueEnumValue;
        public string ValueEnumValue
        {
            get { return _ValueEnumValue; }
        }

        string _ReferenceKey;
        public string ReferenceKey
        {
            get { return _ReferenceKey; }
        }

        public override SreMessage Parse(bool onlyConstraints)
        {
            try
            {
                string code = ParseFormulaGetCode(Formula);

                _ReferenceKey = string.Empty;
                string sqlErrorMessage = string.Empty;

                if (Registry.Instance.CodeSets != null)
                {
                    CodeSet thisCodeSet = (from cs in Registry.Instance.CodeSets
                                                  where ((cs.Code.Equals(code, StringComparison.OrdinalIgnoreCase)) && (cs.Value.Equals(Value, StringComparison.OrdinalIgnoreCase)))
                                                  select cs).SingleOrDefault();
                    if (thisCodeSet != null)
                    {
                        _Value = thisCodeSet.Value;
                        _ValueEnumCode = thisCodeSet.EnumCode;
                        _ReferenceKey = thisCodeSet.ReferenceKey;
                    }
                    else
                    {
                        //check one more time with EnumCode, as SRE supports Value OR EnumCode
                        int enumCode = 0;
                        if (int.TryParse(Value, out enumCode))
                        {
                            thisCodeSet = (from cs in Registry.Instance.CodeSets
                                           where ((cs.Code.Equals(code, StringComparison.OrdinalIgnoreCase)) && (cs.EnumCode == enumCode))
                                           select cs).SingleOrDefault();
                            if (thisCodeSet != null)
                            {
                                _Value = thisCodeSet.Value;
                                _ValueEnumCode = thisCodeSet.EnumCode;
                                _ReferenceKey = thisCodeSet.ReferenceKey;
                            }
                            else
                            {
                                _ValueEnumCode = -1;
                            }
                        }
                        else
                        {
                            _ValueEnumCode = -1;
                        }
                    }
                }

                if ((_ValueEnumCode == -1) || (sqlErrorMessage != string.Empty))
                {
                    this._ParseResult = new SreMessage(SreMessageCodes.SRE_CODESET_TYPE_DATA_VALIDATION_FAILED);
                    this._ParseResult.Message = string.Format(_ParseResult.Message, PrintRowColPosition(), Value);
                }
                else
                {
                    _ValueEnumValue = _Value;
                    return new SreMessage(SreMessageCodes.SRE_SUCCESS); //when got value, return success
                }
            }
            catch (Exception ex)
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_CODESET_TYPE_DATA_VALIDATION_FAILED);
                this._ParseResult.Message = string.Format(_ParseResult.Message, PrintRowColPosition(), Value);
                ExtensionMethods.TraceError(ex.ToString());
            }
            _IsParsed = true;
            return this._ParseResult;
        }

        public static string ParseFormulaGetConnectionString(string codeSetFormula)
        {
            if (!string.IsNullOrEmpty(codeSetFormula))
            {
                string csAndTableName = codeSetFormula.Substring(0, codeSetFormula.LastIndexOf("."));
                return csAndTableName.Split(".".ToCharArray())[0];
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ParseFormulaGetTableName(string codeSetFormula)
        {
            if (!string.IsNullOrEmpty(codeSetFormula))
            {
                string csAndTableName = codeSetFormula.Substring(0, codeSetFormula.LastIndexOf("."));
                return csAndTableName.Split(".".ToCharArray())[1];
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ParseFormulaGetCode(string codeSetFormula)
        {
            if (!string.IsNullOrEmpty(codeSetFormula))
                return codeSetFormula.Substring(codeSetFormula.IndexOf("(\"") + 2, (codeSetFormula.LastIndexOf("\")") - codeSetFormula.IndexOf("(\"")) - 2);
            else
                return string.Empty;
        }

        public override void CheckConstraints() { }
    }
    #endregion SreCodeset

    #region SreReferenced
    public class SreReferenced : SreType
    {
        public SreReferenced(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum, bool isAssociatedWithSystemRow, int recordPosition, SqlClientManager sqlClientManager, List<SreKey> dataSourceKeys)
            : base(columnName, value, type, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition, sqlClientManager, dataSourceKeys)
        {
            _IsHavingSqlQuery = true;
        }

        public override void OverrideFormula(string formattedSQLQuery)
        {
            base.OverrideFormula(formattedSQLQuery);
        }

        public override SreMessage Parse(bool onlyConstraints)
        {
            this._ParseResult = new SreMessage(SreMessageCodes.SRE_FAILED);
            try
            {
                if (!onlyConstraints)
                {
                    _IsParsed = true;
                    ExtensionMethods.TraceInformation("Validating 'Referenced' type '{0}' value '{1}', executing query '{1}' with '{2}'", Value, Formula, ConnectionString);

                    if (!SqlClientManager.CheckReferenceKey(ConnectionString, DatabaseType, Formula, Value))
                    {
                        this._ParseResult = new SreMessage(SreMessageCodes.SRE_REFERENCED_TYPE_DATA_RESULT_NOT_FOUND);
                        this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Value, ColumnName);
                        return this._ParseResult;
                    }
                    else
                    {
                        this._ParseResult = new SreMessage(SreMessageCodes.SRE_REFERENCED_TYPE_DATA_RESULT_FOUND);
                        return this._ParseResult;
                    }
                }
            }
            catch (Exception ex)
            {
                ExtensionMethods.TraceError(ex.ToString());
            }
            return this._ParseResult;
        }

        public override void CheckConstraints() { }
    }
    #endregion SreReferenced

    #region SreNotReferenced
    public class SreNotReferenced : SreType
    {
        public SreNotReferenced(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum, bool isAssociatedWithSystemRow, int recordPosition, SqlClientManager sqlClientManager, List<SreKey> dataSourceKeys)
            : base(columnName, value, type, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition, sqlClientManager, dataSourceKeys)
        {
            _IsHavingSqlQuery = true;
        }

        public override void OverrideFormula(string formattedSQLQuery)
        {
            base.OverrideFormula(formattedSQLQuery);
        }

        public override SreMessage Parse(bool onlyConstraints)
        {
            try
            {
                if (!onlyConstraints)
                {
                    _IsParsed = true;
                    ExtensionMethods.TraceInformation("Validating 'Not Referenced' type '{0}' value '{1}', executing query '{1}' with '{2}'", Value, Formula, ConnectionString);
                    if (SqlClientManager.CheckReferenceKey(ConnectionString, DatabaseType, Formula, Value))
                    {
                        this._ParseResult = new SreMessage(SreMessageCodes.SRE_NOT_REFERENCED_TYPE_DATA_RESULT_FOUND);
                        this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Value, ColumnName);
                        return this._ParseResult;
                    }
                    else
                    {
                        this._ParseResult = new SreMessage(SreMessageCodes.SRE_NOT_REFERENCED_TYPE_DATA_RESULT_NOT_FOUND);
                        return this._ParseResult;
                    }
                }
            }
            catch (Exception ex)
            {
                ExtensionMethods.TraceError(ex.ToString());
            }

            return this._ParseResult;
        }

        public override void CheckConstraints() { }
    }
    #endregion SreNotReferenced

    #region SreGenerated
    public class SreGenerated : SreType
    {
        public SreGenerated(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum, bool isAssociatedWithSystemRow, int recordPosition, SqlClientManager sqlClientManager, List<SreKey> dataSourceKeys)
            : base(columnName, value, type, formula, minimum, maximum, isAssociatedWithSystemRow, recordPosition, sqlClientManager, dataSourceKeys)
        {
            if ((formula.Length >= 5)
                && (formula.Substring(0, 5).Equals("=SQL(")))
                _IsHavingSqlQuery = true;

            if ((formula.Length >= 11)
                && (formula.Substring(0, 11).Equals("=STOREPROC(")))
                _IsHavingSqlQuery = true;
        }

        public new string Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }

        public override void OverrideFormula(string formattedSQLQuery)
        {
            base.OverrideFormula(formattedSQLQuery);
        }

        public override SreMessage Parse(bool onlyConstraints)
        {
            try
            {
                if (_IsHavingSqlQuery && !onlyConstraints)
                {
                    ExtensionMethods.TraceInformation("Generating 'Generated' type '{0}', executing query '{1}'", ColumnName, Formula);
                    bool isErrored = false;

                    _Value = SqlClientManager.ExecuteQuery(ConnectionString, DatabaseType, Formula, ref isErrored);
                    if (isErrored)
                    {
                        this._ParseResult = new SreMessage(SreMessageCodes.SRE_GENERATED_TYPE_DATA_VALIDATION_FAILED);
                        this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), string.Empty, ColumnName);
                    }
                }
                CheckConstraints();
            }
            catch (Exception ex)
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_GENERATED_TYPE_DATA_VALIDATION_FAILED);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), Formula, ColumnName);
                ExtensionMethods.TraceError(ex.ToString());
            }
            _IsParsed = true;
            return this._ParseResult;
        }

        public override void CheckConstraints()
        {
            long min = 0;
            long max = 0;
            if (!string.IsNullOrEmpty(Minimum))
                min = (long)Minimum.ParseLong();
            if (!string.IsNullOrEmpty(Maximum))
                max = (long)Maximum.ParseLong();

            if ((!string.IsNullOrEmpty(Minimum)) && (Value.Length < min))
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MINIMUM);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), ColumnName, Minimum);
            }
            else if ((!string.IsNullOrEmpty(Maximum)) && (Value.Length > max))
            {
                this._ParseResult = new SreMessage(SreMessageCodes.SRE_TYPE_DATA_VALIDATION_FAILED_MAXIMUM);
                this._ParseResult.Message = string.Format(this._ParseResult.Message, PrintRowColPosition(), ColumnName, Maximum);
            }

        }
    }
    #endregion SreGenerated

    #region SreUnknown
    public class SreUnknown : SreType
    {
        public SreUnknown(string columnName, string value, AttributeTypes type, string formula, string minimum, string maximum, int recordPosition)
            : base(columnName, value, type, formula, minimum, maximum, false, recordPosition)
        {
        }

        public override SreMessage Parse(bool onlyConstraints)
        {
            return new SreMessage(SreMessageCodes.SRE_SUCCESS);
        }

        public override void CheckConstraints() { }
    }
    #endregion SreUnknown
}






