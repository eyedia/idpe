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
using System.Text;
using System.Configuration;
using System.Xml;
using System.Diagnostics;
using System.IO;
using CultureInfo = System.Globalization.CultureInfo;
using Eyedia.Core.Data;

namespace Eyedia.Core
{

    #region EyediaConfigElements

    public class EyediaConfigElements : ConfigurationElementCollection
    {
        public EyediaConfigElements()
         {
             EyediaConfigElement details = (EyediaConfigElement)CreateNewElement();
             if (details.Name != "")
             {
                 Add(details);
             }
         }

         public override ConfigurationElementCollectionType CollectionType
         {
             get
             {
                 return ConfigurationElementCollectionType.BasicMap;
             }
         }

         protected override ConfigurationElement CreateNewElement()
         {
             return new EyediaConfigElement();
         }

         protected override Object GetElementKey(ConfigurationElement element)
         {
             return ((EyediaConfigElement)element).Name;
         }

         public EyediaConfigElement this[int index]
         {
             get
             {
                 return (EyediaConfigElement)BaseGet(index);
             }
             set
             {
                 if (BaseGet(index) != null)
                 {
                     BaseRemoveAt(index);
                 }
                 BaseAdd(index, value);
             }
         }

         new public EyediaConfigElement this[string name]
         {
             get
             {
                 return (EyediaConfigElement)BaseGet(name);
             }
         }

         public int IndexOf(EyediaConfigElement details)
         {
             return BaseIndexOf(details);
         }

         public void Add(EyediaConfigElement details)
         {
             BaseAdd(details);
         }

         protected override void BaseAdd(ConfigurationElement element)
         {
             EyediaConfigElement elem = element as EyediaConfigElement;
             Type elemType = Type.GetType(elem.Type);
             if (elemType == null)
             {

                 throw new Exception(string.Format("Could not initialize SymplusAcceleratorConfigElement of type '{0}'. The type was not found. Verify that the worker type has been defined correctly and required dlls exists in run time directory"
                     , elemType));
             }             
             EyediaConfigElement implementedElement = (EyediaConfigElement)Activator.CreateInstance(elemType, elem.Name, elem.Type);             
             BaseAdd(implementedElement, false);
         }

         public void Remove(EyediaConfigElement details)
         {
             if (BaseIndexOf(details) >= 0)
                 BaseRemove(details.Name);
         }

         public void RemoveAt(int index)
         {
             BaseRemoveAt(index);
         }

         public void Remove(string name)
         {
             BaseRemove(name);
         }

         public void Clear()
         {
             BaseClear();
         }

         protected override string ElementName
         {
             get { return "accelerator"; }
         }

    }

    #endregion EyediaConfigElements

    #region EyediaConfigElement

    public class EyediaConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        [StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;�\"|\\")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]        
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        protected override string GetTransformedAssemblyString(string assemblyName)
        {
            return base.GetTransformedAssemblyString(assemblyName);
        }
    }
    #endregion EyediaConfigElement

}



