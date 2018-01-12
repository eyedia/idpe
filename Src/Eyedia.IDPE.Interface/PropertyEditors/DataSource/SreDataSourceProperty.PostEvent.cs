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
using Eyedia.IDPE.Common;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing.Design;

namespace Eyedia.IDPE.Interface
{
    //[DefaultPropertyAttribute("SaveOnClose")]
    public partial class SreDataSourceProperty
    {

        PusherTypes mPusherType;

        [Browsable(true)]
        [DefaultValue(PusherTypes.None)]
        [RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        [TypeConverter(typeof(PusherTypeEnumTypeConverter))]
        [DisplayName("Post Event")]
        [CategoryAttribute("Output.PostEvents")]
        public PusherTypes PostEvent
        {
            get
            {
                return mPusherType;
            }
            set
            {
                mPusherType = value;
                PropertyGridProperty prop_PostEventCustomInterfaceName = GetReadOnlyProperty("PostEventCustomInterfaceName");
                PropertyGridProperty prop_PostEventConfigure = GetReadOnlyProperty("PostEventConfigure");

                switch (mPusherType)
                {
                    case PusherTypes.DosCommands:
                    case PusherTypes.Ftp:
                    case PusherTypes.SqlQuery:
                        prop_PostEventCustomInterfaceName.FieldToChange.SetValue(prop_PostEventCustomInterfaceName.BrowsableAttribute, false);
                        prop_PostEventConfigure.FieldToChange.SetValue(prop_PostEventConfigure.BrowsableAttribute, true);
                        break;

                    case PusherTypes.Custom:
                        prop_PostEventCustomInterfaceName.FieldToChange.SetValue(prop_PostEventCustomInterfaceName.BrowsableAttribute, true);
                        prop_PostEventConfigure.FieldToChange.SetValue(prop_PostEventConfigure.BrowsableAttribute, false);
                        break;

                    default:
                        prop_PostEventCustomInterfaceName.FieldToChange.SetValue(prop_PostEventCustomInterfaceName.BrowsableAttribute, false);
                        break;
                }
            }
        }

        [Browsable(false)]
        [CategoryAttribute("Output.PostEvents")]
        [Editor(typeof(EditorPostEvents), typeof(UITypeEditor))]
        [DisplayName("Configure")]
        public string PostEventCustomInterfaceName { get; set; }

        [Browsable(false)]
        [Editor(typeof(EditorPostEvents), typeof(UITypeEditor))]
        [CategoryAttribute("Output.PostEvents")]
        [DisplayName("Configure")]
        public string PostEventConfigure { get; set; }
    }

   
}


