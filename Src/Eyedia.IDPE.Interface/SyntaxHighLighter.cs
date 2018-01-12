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
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Eyedia.IDPE.Interface
{
    public class SyntaxHighLighter
    {
        [DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

      //  public static Regex KeyWords = new Regex("abstract|as|base|bool|break|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|do|double|else|enum|event|explicit|extern|false|finally|fixed|float|for|" +
      //"foreach|goto|if|implicit|in|int|interface|internal|is|lock|long|namespace|new|null|object|operator|out|override|params|private|protected|public|readonly|ref|return|sbyte|sealed|short|sizeof|stackalloc|static|" +
      //"string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|volatile|void|while|" +
      //"Environment|" +
      //"select|from|update|set|where");

        public static string __BlueKeyWords = "\\b(abstract|as|base|bool|break|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|do|double|else|enum|event|explicit|extern|false|finally|fixed|float|for|" +
      "foreach|goto|if|implicit|in|int|interface|internal|is|lock|long|namespace|new|null|object|operator|out|override|params|private|protected|public|readonly|ref|return|sbyte|sealed|short|sizeof|stackalloc|static|" +
      "string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|volatile|void|while|" +
      "Environment|" +
      "xml|xsl|template|stylesheet|" +
      "select|from|update|set|where)\\b";

        public static void HighLight(RichTextBox richTextBox, string blueKeyWords = null, string redKeyWords = null)
        {
            if (richTextBox == null)
                return;

            if (blueKeyWords == null)
                blueKeyWords = __BlueKeyWords;

            try
            {

                LockWindowUpdate(richTextBox.Handle);
                int selPos = richTextBox.SelectionStart;

                foreach (Match keyWordMatch in Regex.Matches(richTextBox.Text, blueKeyWords))
                {
                    richTextBox.Select(keyWordMatch.Index, keyWordMatch.Length);
                    richTextBox.SelectionColor = Color.Blue;
                    richTextBox.Select(selPos, 0);
                    richTextBox.SelectionColor = Color.Black;
                }

                if (redKeyWords != null)
                {
                    selPos = richTextBox.SelectionStart;

                    foreach (Match keyWordMatch in Regex.Matches(richTextBox.Text, redKeyWords))
                    {
                        richTextBox.Select(keyWordMatch.Index, keyWordMatch.Length);
                        richTextBox.SelectionColor = Color.Red;
                        richTextBox.Select(selPos, 0);
                        richTextBox.SelectionColor = Color.Black;
                    }
                }
            }
            finally { LockWindowUpdate(IntPtr.Zero); }

        }
    }
}


