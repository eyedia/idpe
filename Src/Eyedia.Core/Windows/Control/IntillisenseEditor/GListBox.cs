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
using System.Drawing;
using System.Windows.Forms;

namespace Eyedia.Core.Windows.Control
{	
	// GListBox class 
	public class GListBox : ListBox
	{
		private ImageList _myImageList;
		public ImageList ImageList
		{
			get {return _myImageList;}
			set {_myImageList = value;}
		}
		public GListBox()
		{
			// Set owner draw mode
			this.DrawMode = DrawMode.OwnerDrawFixed;
		}
		protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
		{
			e.DrawBackground();
			e.DrawFocusRectangle();
			GListBoxItem item;
			Rectangle bounds = e.Bounds;
			Size imageSize = _myImageList.ImageSize;
			try
			{
				item = (GListBoxItem) Items[e.Index];
				if (item.ImageIndex != -1)
				{
					_myImageList.Draw(e.Graphics, bounds.Left,bounds.Top,item.ImageIndex); 
					e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor), 
						bounds.Left+imageSize.Width, bounds.Top);
				}
				else
				{
					e.Graphics.DrawString(item.Text, e.Font,new SolidBrush(e.ForeColor),
						bounds.Left, bounds.Top);
				}
			}
			catch
			{
                //design time it throws error, this try/catch was placed for that
                try
                {
                    if (e.Index != -1)
                    {
                        e.Graphics.DrawString(Items[e.Index].ToString(), e.Font,
                            new SolidBrush(e.ForeColor), bounds.Left, bounds.Top);
                    }
                    else
                    {
                        e.Graphics.DrawString(Text, e.Font, new SolidBrush(e.ForeColor),
                            bounds.Left, bounds.Top);
                    }
                }
                catch { }
			}
			base.OnDrawItem(e);
		}
	}//End of GListBox class

    // GListBoxItem class 
    public class GListBoxItem
    {
        private string _myText;
        private int _myImageIndex;
        // properties 
        public string Text
        {
            get { return _myText; }
            set { _myText = value; }
        }
        public int ImageIndex
        {
            get { return _myImageIndex; }
            set { _myImageIndex = value; }
        }
        //constructor
        public GListBoxItem(string text, int index)
        {
            _myText = text;
            _myImageIndex = index;
        }
        public GListBoxItem(string text) : this(text, -1) { }
        public GListBoxItem() : this("") { }
        public override string ToString()
        {
            return _myText;
        }
    }//End of GListBoxItem class

}


