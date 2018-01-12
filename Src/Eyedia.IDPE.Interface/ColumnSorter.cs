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
using System.Text.RegularExpressions;	
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;

namespace Eyedia.IDPE.Interface
{
	/// <summary>
	/// This class is an implementation of the 'IComparer' interface.
	/// </summary>
	public class ListViewColumnSorter : IComparer
	{
		/// <summary>
		/// Specifies the column to be sorted
		/// </summary>
		private int ColumnToSort;
		/// <summary>
		/// Specifies the order in which to sort (i.e. 'Ascending').
		/// </summary>
		private SortOrder OrderOfSort;
		/// <summary>
		/// Case insensitive comparer object
		/// </summary>
		//private CaseInsensitiveComparer ObjectCompare;
		private NumberCaseInsensitiveComparer ObjectCompare;
		private ImageTextComparer FirstObjectCompare;
        private DateComparer DateTimeComparer;
		/// <summary>
		/// Class constructor.  Initializes various elements
		/// </summary>
		public ListViewColumnSorter()
		{
			// Initialize the column to '0'
			ColumnToSort = 0;

			// Initialize the sort order to 'none'
			//OrderOfSort = SortOrder.None;
			OrderOfSort = SortOrder.Ascending;

			// Initialize the CaseInsensitiveComparer object
			ObjectCompare = new NumberCaseInsensitiveComparer();//CaseInsensitiveComparer();
			FirstObjectCompare = new ImageTextComparer();
            DateTimeComparer = new DateComparer();
		}

		/// <summary>
		/// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
		/// </summary>
		/// <param name="x">First object to be compared</param>
		/// <param name="y">Second object to be compared</param>
		/// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
		public int Compare(object x, object y)
		{
			int compareResult;
			ListViewItem listviewX, listviewY;

			// Cast the objects to be compared to ListViewItem objects
			listviewX = (ListViewItem)x;
			listviewY = (ListViewItem)y;
            DateTime datetime = DateTime.MinValue;

			if (ColumnToSort == 0)
			{
				compareResult = FirstObjectCompare.Compare(x,y);
			}
            else if (DateTime.TryParse(listviewX.SubItems[ColumnToSort].Text, out datetime))
            {
                compareResult = DateTimeComparer.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
            }
			else
			{
				// Compare the two items
                compareResult = ObjectCompare.Compare(string.IsNullOrEmpty(listviewX.SubItems[ColumnToSort].Text) ? string.Empty : listviewX.SubItems[ColumnToSort].Text,
                    string.IsNullOrEmpty(listviewY.SubItems[ColumnToSort].Text) ? string.Empty : listviewY.SubItems[ColumnToSort].Text);
			}

			// Calculate correct return value based on object comparison
			if (OrderOfSort == SortOrder.Ascending)
			{
				// Ascending sort is selected, return normal result of compare operation
				return compareResult;
			}
			else if (OrderOfSort == SortOrder.Descending)
			{
				// Descending sort is selected, return negative result of compare operation
				return (-compareResult);
			}
			else
			{
				// Return '0' to indicate they are equal
				return 0;
			}
		}
    
		/// <summary>
		/// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
		/// </summary>
		public int SortColumn
		{
			set
			{
				ColumnToSort = value;
			}
			get
			{
				return ColumnToSort;
			}
		}

		/// <summary>
		/// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
		/// </summary>
		public SortOrder Order
		{
			set
			{
				OrderOfSort = value;
			}
			get
			{
				return OrderOfSort;
			}
		}
    
	}

	public class ImageTextComparer : IComparer
	{
		//private CaseInsensitiveComparer ObjectCompare;
		private NumberCaseInsensitiveComparer ObjectCompare;
        
		public ImageTextComparer()
		{
			// Initialize the CaseInsensitiveComparer object
			ObjectCompare = new NumberCaseInsensitiveComparer();//CaseInsensitiveComparer();
		}

		public int Compare(object x, object y)
		{
			//int compareResult;
			int image1, image2;
			ListViewItem listviewX, listviewY;

			// Cast the objects to be compared to ListViewItem objects
			listviewX = (ListViewItem)x;
			image1 = listviewX.ImageIndex;
			listviewY = (ListViewItem)y;
			image2 = listviewY.ImageIndex;

			if (image1 < image2)
			{
				return -1;
			}
			else if (image1 == image2)
			{
				return ObjectCompare.Compare(listviewX.Text,listviewY.Text);
			}
			else
			{
				return 1;
			}
		}
	}

	public class NumberCaseInsensitiveComparer : CaseInsensitiveComparer
	{
		public NumberCaseInsensitiveComparer ()
		{
			
		}

		public new int Compare(object x, object y)
		{
			if ((x is System.String) && IsWholeNumber((string)x) && (y is System.String) && IsWholeNumber((string)y))
			{
                if (string.IsNullOrEmpty(x.ToString()))
                    x = "0";
                if (string.IsNullOrEmpty(y.ToString()))
                    y = "0";

				return base.Compare(System.Convert.ToInt32(x),System.Convert.ToInt32(y));
			}
			else
			{
				return base.Compare(x,y);
			}
		}

		private bool IsWholeNumber(string strNumber)
		{
			Regex objNotWholePattern=new Regex("[^0-9]");
			return !objNotWholePattern.IsMatch(strNumber);
		}  
	}

    public class DateComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            DateTime dtTm_x = DateTime.MinValue;
            DateTime.TryParse(x.ToString(), out dtTm_x);

            DateTime dtTm_y = DateTime.MinValue;
            DateTime.TryParse(y.ToString(), out dtTm_y);

            return DateTime.Compare(dtTm_x, dtTm_y);
        }
    }




}


