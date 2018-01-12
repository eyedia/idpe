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
using System.Text;
using System.ComponentModel;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Interface.Data
{
    public class AttributeDataSource : Component, IListSource
    {
        BindingList<Attribute> _Attributes;
        public AttributeDataSource(BindingList<Attribute> attributes)
        {            
            _Attributes = attributes;
        }

        public AttributeDataSource(IContainer container)
        {
            container.Add(this);
        }

        public BindingList<Attribute> Attributes { get { return _Attributes; } }
         
        #region IListSource Members

        bool IListSource.ContainsListCollection
        {
            get { return false; }
        }

        System.Collections.IList IListSource.GetList()
        {            
            return _Attributes;
        }

        #endregion
    }

    #region Attribute Class
    public class Attribute : BusinessObjectBase
    {
        string _Name;
        public string Name
        {
            get
            {
                return _Name == null ? string.Empty : _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

      

        AttributeTypes _Type;
        public AttributeTypes Type
        {
            get
            {
                return _Type;
            }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        string _Minimum;
        public string Minimum
        {
            get
            {
                return _Minimum == null ? string.Empty : _Minimum;
            }
            set
            {
                if (_Minimum != value)
                {
                    _Minimum = value;
                    OnPropertyChanged("Minimum");
                }
            }
        }

        string _Maximum;
        public string Maximum
        {
            get
            {
                return _Maximum == null ? string.Empty : _Maximum;
            }
            set
            {
                if (_Maximum != value)
                {
                    _Maximum = value;
                    OnPropertyChanged("Maximum");
                }
            }
        }

        string _Formula;
        public string Formula
        {
            get
            {
                return _Formula == null ? string.Empty : _Formula;
            }
            set
            {
                if (_Formula != value)
                {
                    _Formula = value;
                    OnPropertyChanged("Formula");
                }
            }
        }

        bool _IsAcceptable;
        public bool IsAcceptable
        {
            get
            {
                return _IsAcceptable;
            }
            set
            {
                if (_IsAcceptable != value)
                {
                    _IsAcceptable = value;
                    OnPropertyChanged("IsAcceptable");
                }
            }
        }

        int _AttributePrintValueType;
        public int AttributePrintValueType
        {
            get
            {
                return _AttributePrintValueType;
            }
            set
            {
                if (_AttributePrintValueType != value)
                {
                    _AttributePrintValueType = value;
                    OnPropertyChanged("AttributePrintValueType");
                }
            }
        }

        string _AttributePrintValueCustom;
        public string AttributePrintValueCustom
        {
            get
            {
                return _AttributePrintValueCustom;
            }
            set
            {
                if (_AttributePrintValueCustom != value)
                {
                    _AttributePrintValueCustom = value;
                    OnPropertyChanged("AttributePrintValueCustom");
                }
            }
        }

        int _AttributeDataSourceId;
        public int AttributeDataSourceId
        {
            get
            {
                return _AttributeDataSourceId;
            }
            set
            {
                if (_AttributeDataSourceId != value)
                {
                    _AttributeDataSourceId = value;
                    OnPropertyChanged("AttributeDataSourceId");
                }
            }
        }

        int _DataSourceId;
        public int DataSourceId
        {
            get
            {
                return _DataSourceId;
            }
            set
            {
                if (_DataSourceId != value)
                {
                    _DataSourceId = value;
                    OnPropertyChanged("DataSourceId");
                }
            }
        }

        string _DataSourceName;
        public string DataSourceName
        {
            get
            {
                return _DataSourceName == null ? string.Empty : _DataSourceName;
            }
            set
            {
                if (_DataSourceName != value)
                {
                    _DataSourceName = value;
                    OnPropertyChanged("DataSourceName");
                }
            }
        }

        string _Position;
        public string Position
        {
            get
            {
                return _Position;
            }
            set
            {
                if (_Position != value)
                {
                    _Position = value;
                    OnPropertyChanged("Position");
                }
            }
        }

        int _Id;
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }
    }
    #endregion Attribute Class

}





