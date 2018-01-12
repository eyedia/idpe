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
using System.ComponentModel;
using System.Reflection;
using Eyedia.Core;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Interface
{
    public abstract class SrePropertyGrid
    {
        public SrePropertyGrid(SreDataSource datasource)
        {
            this.DataSource = datasource;
            //this.DataSourceOriginal = this.DataSource.Clone();
            Assign();
        }

        public SrePropertyGrid(SreAttribute attribute, int dataSourceId = 0, bool isAssociatedWithSystemDataSource = false)
        {
            this.Attribute = attribute;
            this.DataSourceId = dataSourceId;            
            this.IsAssociatedWithSystemDataSource = isAssociatedWithSystemDataSource;
            Assign();
        }

        [Browsable(false)]
        public int DataSourceId { get; internal set; }

        [Browsable(false)]       
        public bool IsAssociatedWithSystemDataSource { get; private set; }


        [Browsable(false)]
        public SreAttribute Attribute { get; private set; }


        [Browsable(false)]
        public SreDataSource DataSource { get; private set; }

        [Browsable(false)]
        public string ValidationError { get; protected set; }

        [Browsable(false)]
        public bool HasChanged { get; set; }

        /// <summary>
        /// Save the property grid configuration to the database
        /// </summary>
        public abstract void Assign();

        /// <summary>
        /// Save the property grid configuration to the database
        /// </summary>
        public abstract void Save();

        public void RevertChanges()
        {
            //this.DataSource = this.DataSourceOriginal;
            Assign();
        }

        public void ClearValidationError()
        {
            ValidationError = string.Empty;
        }

        #region Retrieve Property
        PropertyGridProperty GetReadOnlyProperty(string propertyName, bool browsable = true)
        {
            if (!browsable)
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())[propertyName];
                ReadOnlyAttribute attribute = (ReadOnlyAttribute)
                                              descriptor.Attributes[typeof(ReadOnlyAttribute)];
                FieldInfo fieldToChange = attribute.GetType().GetField("isReadOnly",
                                                 System.Reflection.BindingFlags.NonPublic |
                                                 System.Reflection.BindingFlags.Instance);

                return new PropertyGridProperty(attribute, fieldToChange);
            }
            else
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())[propertyName];
                BrowsableAttribute attribute = (BrowsableAttribute)
                                              descriptor.Attributes[typeof(BrowsableAttribute)];
                FieldInfo fieldToChange = attribute.GetType().GetField("browsable",
                                                 System.Reflection.BindingFlags.NonPublic |
                                                 System.Reflection.BindingFlags.Instance);

                return new PropertyGridProperty(attribute, fieldToChange);
            }
        }

        #endregion Retrieve Property
    }
}


