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
using System.Net;

namespace Eyedia.IDPE.Interface
{
    public partial class frmFtpConfiguration : Form
    {
        public frmFtpConfiguration()
        {
            InitializeComponent();
            ;
        }

        public string FtpRemoteLocation
        {
            get { return txtFtpRemoteLocation.Text; }
            set { txtFtpRemoteLocation.Text = value; }
        }

        public string FtpUserName
        {
            get { return txtFtpUserName.Text; }
            set { txtFtpUserName.Text = value; }
        }

        public string FtpPassword
        {
            get { return txtFtpPassword.Text; }
            set { txtFtpPassword.Text = value; }
        }        

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFtpTest_Click(object sender, EventArgs e)
        {
            try
            {
                FtpWebRequest Request = (FtpWebRequest)FtpWebRequest.Create(new Uri(txtFtpRemoteLocation.Text));
                Request.Credentials = new NetworkCredential(txtFtpUserName.Text, txtFtpPassword.Text);
                Request.Method = WebRequestMethods.Ftp.ListDirectory;
                Request.Proxy = null;
                Request.UseBinary = true;
                FtpWebResponse Response = (FtpWebResponse)Request.GetResponse();
                MessageBox.Show("Successful!", "FTP Connectivity Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Failed!", "FTP Connectivity Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmFtpConfiguration_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

    }
}


