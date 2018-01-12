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
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;

namespace Eyedia.IDPE.Interface.RuleEditor
{
    class SyntaxHighLighterWPF
    {
        static string[] ListOfDarkRedWords;
        static string[] ListOfRedWords;
        static string[] ListOfBlueWords;

        #region ctor

        static SyntaxHighLighterWPF()
        {
            ListOfDarkRedWords = new string[] { 
                "Activity",
                "/Activity",
                "x:Members",
                "/x:Members",
                "x:Property",
                "sap:VirtualizedContainerService.HintSize",
                "/sap:VirtualizedContainerService.HintSize",
                "sap:VirtualizedContainerService.HintSize=",
                "mva:VisualBasic.Settings",
                "If",
                "/If>",
                "If.Then",
                "/If.Then",
                "srs:AddOrUpdateProcessVariable"
            };

            ListOfRedWords = new string[] {                
                "xmlns", 
                "xmlns:mc",                
                 "xmlns:mv",
                 "xmlns:mva",
                 "xmlns:s",
                 "xmlns:s1",
                 "xmlns:s2",
                 "xmlns:s3",
                 "xmlns:s4",
                 "xmlns:sa",
                 "xmlns:sad",
                 "xmlns:sads",
                 "xmlns:sap",
                 "xmlns:scg",
                 "xmlns:scg1",
                 "xmlns:scg2",
                 "xmlns:scg3",
                 "xmlns:sd",
                 "xmlns:sl",
                 "xmlns:srs",
                 "xmlns:st",
                 "xmlns:x",
                 "xmlns:mva",
                 "x:Class",
                 "Name",
                 "Condition",
                 "Type",
                 "sad:XamlDebuggerXmlReader.FileName",
                 "Data=",
                 "Job=",
                 "Value=",
                 "Key"
            };

            ListOfBlueWords = new string[] 
            { 
                "http://schemas.microsoft.com/netfx/2009/xaml/activities",
                 "http://schemas.openxmlformats.org/markup-compatibility/2006",
                 "clr-namespace:Microsoft.VisualBasic;assembly=System",
                 "clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities",
                 "clr-namespace:System;assembly=mscorlib",
                 "clr-namespace:System;assembly=System",
                 "clr-namespace:System;assembly=System.Xml",
                 "clr-namespace:System;assembly=System.Core",
                 "clr-namespace:System;assembly=System.ServiceModel",
                 "clr-namespace:System.Activities;assembly=System.Activities",
                 "clr-namespace:System.Activities.Debugger;assembly=System.Activities",
                 "http://schemas.microsoft.com/netfx/2010/xaml/activities/debugger",
                 "http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation",
                 "clr-namespace:System.Collections.Generic;assembly=System",
                 "clr-namespace:System.Collections.Generic;assembly=System.ServiceModel",
                 "clr-namespace:System.Collections.Generic;assembly=System.Core",
                 "clr-namespace:System.Collections.Generic;assembly=mscorlib",
                 "clr-namespace:System.Data;assembly=System.Data",
                 "clr-namespace:System.Linq;assembly=System.Core",
                 "clr-namespace:Eyedia.IDPE.Services;assembly=Eyedia.IDPE.Services",
                 "clr-namespace:System.Text;assembly=mscorlib",
                 "http://schemas.microsoft.com/winfx/2006/xaml"
            };
        }

        #endregion

        static TextRange FindWordFromPosition(TextPointer position, string word)
        {
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = position.GetTextInRun(LogicalDirection.Forward);

                    // Find the starting index of any substring that matches "word".
                    int indexInRun = textRun.IndexOf(word);
                    if (indexInRun >= 0)
                    {
                        TextPointer start = position.GetPositionAtOffset(indexInRun);
                        TextPointer end = start.GetPositionAtOffset(word.Length);
                        return new TextRange(start, end);
                    }
                }

                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }

            // position will be null if "word" is not found.
            return null;
        }

        public static void Highlight(RichTextBox richTextBox)
        {
            if (richTextBox.Document == null)
                return;

            for (int j = 0; j < ListOfDarkRedWords.Length; j++)
            {
                string Word = ListOfDarkRedWords[j].ToString();
                TextRange myRange = FindWordFromPosition(richTextBox.Document.ContentStart, Word);
                if (myRange != null)
                    myRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.DarkRed));
            }


            for (int j = 0; j < ListOfRedWords.Length; j++)
            {
                string Word = ListOfRedWords[j].ToString();
                TextRange myRange = FindWordFromPosition(richTextBox.Document.ContentStart, Word);
                if (myRange != null)
                    myRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red));
            }


            for (int j = 0; j < ListOfBlueWords.Length; j++)
            {
                string Word = ListOfBlueWords[j].ToString();
                TextRange myRange = FindWordFromPosition(richTextBox.Document.ContentStart, Word);
                if (myRange != null)
                    myRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
            }
        }
    }
}


