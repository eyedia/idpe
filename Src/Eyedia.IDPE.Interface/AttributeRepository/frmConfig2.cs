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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Eyedia.IDPE.Common;
using Data = Eyedia.IDPE.Interface.Data;
using Eyedia.IDPE.DataManager;
using System.Windows.Forms.Design;

namespace Eyedia.IDPE.Interface
{
    public partial class frmConfig2 : Form
    {
        public IWindowsFormsEditorService _WindowsFormsEditorService;
        public frmConfig2(int dataSourceId, string dataSourceName, DataFormatTypes dataFormatType, IWindowsFormsEditorService windowsFormsEditorService = null)
        {
            _WindowsFormsEditorService = windowsFormsEditorService;
            if (_WindowsFormsEditorService != null)
                TopLevel = false;

            InitializeComponent();           

            ;

            headerFooterConfiguration1.DataFormatType = dataFormatType;
            headerFooterConfiguration1.DataSourceId = dataSourceId;

            switch (dataFormatType)
            {
                case DataFormatTypes.Delimited:
                    this.Text = "Delimited Configuration";
                    break;

                case DataFormatTypes.FixedLength:
                    this.Text = "FixedLength Configuration";
                    break;
            }
        }

        private void frmConfig2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(_WindowsFormsEditorService != null)
                _WindowsFormsEditorService.CloseDropDown();
        }       
    
    }
}





