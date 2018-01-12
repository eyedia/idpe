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
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Eyedia.Core.Net
{

    /// <remarks>
    /// Template Parser is simple parser has been written on C#.
    /// It allows setup variables and conditions block in template.
    /// Also you can use some of variable's modificators.
    ///
    ///     Author: Alexander Kleshevnikov
    ///     E-mail: seigo@icconline.com
    ///
    /// <example>There is the simpl example of template for html page:
    /// <code>
    /// <html>
    /// <head><title>##Title##</title></head>
    /// <body><h1>##Title:upper##</h1>
    /// ##If--IsRegisteredUser##
    /// Hello, ##UserName##!
    /// ##Else--IsRegisteredUser##
    /// Please sign in.
    /// ##EndIf--IsRegisteredUser##
    /// </body>
    /// </html>
    /// </code>
    /// To parse this template you can use the following code:
    /// <code>
    /// ...
    /// Hashtable Variables = new Hashtable();
    /// Variables.Add("Title", "Login In");
    /// Variables.Add("IsRegisteredUser", true);
    /// Variables.Add("UserName", "seigo");
    /// TemplateParser tpl = new TemplateParser("template.htm", Variables);
    /// tpl.ParseToFile("result.htm");
    /// ...
    /// </code>
    /// </example>
    /// </remarks>
    public class EmailTemplateParser
    {
        private string      _strTemplateBlock;
        private Hashtable   _hstValues;
        private Hashtable   _ErrorMessage = new Hashtable();
        private string      _ParsedBlock;

        private Dictionary<string, EmailTemplateParser> _Blocks = new Dictionary<string, EmailTemplateParser>();

        private string VariableTagBegin        = "##";
        private string VariableTagEnd          = "##";

        private string ModificatorTag          = ":";
        private string ModificatorParamSep     = ",";

        private string ConditionTagIfBegin     = "##If--";
        private string ConditionTagIfEnd       = "##";
        private string ConditionTagElseBegin   = "##Else--";
        private string ConditionTagElseEnd     = "##";
        private string ConditionTagEndIfBegin  = "##EndIf--";
        private string ConditionTagEndIfEnd    = "##";

		private string BlockTagBeginBegin      = "##BlockBegin--";
		private string BlockTagBeginEnd        = "##";
		private string BlockTagEndBegin        = "##BlockEnd--";
		private string BlockTagEndEnd          = "##";

        /// <value>Template block</value>
        public string TemplateBlock
        {
            get { return this._strTemplateBlock; }
            set 
            { 
                this._strTemplateBlock = value;
                ParseBlocks();
            }
        }

        /// <value>Template Variables</value>
        public Hashtable Variables
        {
            get { return this._hstValues; }
            set { this._hstValues = value; }
        }

        /// <value>Error Massage</value>
        public Hashtable ErrorMessage
        {
            get { return _ErrorMessage; }
        }

        /// <value>Blocks inside template</value>
        public Dictionary<string, EmailTemplateParser> Blocks
        {
            get { return _Blocks; }
        }

        /// <summary>
        /// Creates a new instance of TemplateParser
        /// </summary>

        #region Contructors
        public EmailTemplateParser()
        {
            this._strTemplateBlock = "";
        }

        public EmailTemplateParser(string FilePath)
        {
            ReadTemplateFromFile(FilePath);
            ParseBlocks();
        }

        public EmailTemplateParser(Hashtable Variables)
        {
            this._hstValues = Variables;
        }

        public EmailTemplateParser(string FilePath, Hashtable Variables)
        {
            ReadTemplateFromFile(FilePath);
            this._hstValues = Variables;
            ParseBlocks();
        }
        #endregion

        /// <summary>
        /// Setup template from specified file
        /// </summary>
        /// <param name="FilePath">Full phisical path to template file</param>
        public void SetTemplateFromFile(string FilePath)
        {
            ReadTemplateFromFile(FilePath);
        }

        /// <summary>
        /// Setup template as string block
        /// </summary>
        /// <param name="TemplateBlock">String template block</param>
        public void SetTemplate(string TemplateBlock)
        {
            this.TemplateBlock = TemplateBlock;
        }

        /// <summary>
        /// Parse template after setuping Template and Variables
        /// </summary>
        /// <returns>
        /// Parsed Block for Whole Template
        /// </returns>
        public string Parse()
        {
            ParseConditions();
            ParseVariables();
            return this._ParsedBlock;
        }

        /// <summary>
        /// Parse Template Block
        /// </summary>
        /// <returns>
        /// Parsed Block for Specified BlockName
        /// </returns>
        public string ParseBlock(string BlockName, Hashtable Variables)
        {
            if (!this._Blocks.ContainsKey(BlockName))
            {
                throw new ArgumentException(String.Format("Could not find Block with Name '{0}'", BlockName));
            }

            this._Blocks[BlockName].Variables = Variables;
            return this._Blocks[BlockName].Parse();
        }

        /// <summary>
        /// Parse template and save result into specified file
        /// </summary>
        /// <param name="FilePath">Full physical path to file</param>
        /// <param name="ReplaceIfExists">If true file which already exists
        /// will be replaced</param>
        /// <returns>True if new content has been written</returns>
        public bool ParseToFile(string FilePath, bool ReplaceIfExists)
        {
            if (File.Exists(FilePath) && !ReplaceIfExists)
            {
                return false;
            }
            else
            {
                StreamWriter sr = File.CreateText(FilePath);
                sr.Write(Parse());
                sr.Close();
                return true;
            }
        }

        /// <summary>
        /// Read template content from specified file
        /// </summary>
        /// <param name="FilePath">Full physical path to template file</param>
        private void ReadTemplateFromFile(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                throw new ArgumentException("Template file does not exist.");
            }

            StreamReader reader = new StreamReader(FilePath);
            this.TemplateBlock = reader.ReadToEnd();
            reader.Close();
        }

        /// <summary>
        /// Parse all blocks in template
        /// </summary>
        private void ParseBlocks()
        {
            //int idxPrevious = 0;
            int idxCurrent = 0;
            while ((idxCurrent = this._strTemplateBlock.IndexOf(this.BlockTagBeginBegin, idxCurrent)) != -1)
            {
                string BlockName;
                int idxBlockBeginBegin, idxBlockBeginEnd, idxBlockEndBegin;

                idxBlockBeginBegin = idxCurrent;
                idxCurrent += this.BlockTagBeginBegin.Length;

                // Searching for BlockBeginEnd Index

                idxBlockBeginEnd = this._strTemplateBlock.IndexOf(this.BlockTagBeginEnd, idxCurrent);
                if (idxBlockBeginEnd == -1) throw new Exception("Could not find BlockTagBeginEnd");

                // Getting Block Name

                BlockName = this._strTemplateBlock.Substring(idxCurrent, (idxBlockBeginEnd - idxCurrent));
                idxCurrent = idxBlockBeginEnd + this.BlockTagBeginEnd.Length;

                // Getting End of Block index

                string EndBlockStatment = this.BlockTagEndBegin + BlockName + this.BlockTagEndEnd;
                idxBlockEndBegin = this._strTemplateBlock.IndexOf(EndBlockStatment, idxCurrent);
                if (idxBlockEndBegin == -1) throw new Exception("Could not find End of Block with name '" + BlockName + "'");

                // Add Block to Dictionary

                EmailTemplateParser block = new EmailTemplateParser();
                block.TemplateBlock = this._strTemplateBlock.Substring(idxCurrent, (idxBlockEndBegin - idxCurrent));
                this._Blocks.Add(BlockName, block);

                // Remove Block Declaration From Template

                this._strTemplateBlock = this._strTemplateBlock.Remove(idxBlockBeginBegin, (idxBlockEndBegin - idxBlockBeginBegin));

                idxCurrent = idxBlockBeginBegin;
            }
        }

        /// <summary>
        /// Parse all conditions in template
        /// </summary>
        private void ParseConditions()
        {
            int idxPrevious = 0;
            int idxCurrent = 0;
            this._ParsedBlock = "";
            while ((idxCurrent = this._strTemplateBlock.IndexOf(this.ConditionTagIfBegin, idxCurrent)) != -1)
            {
                string VarName;
                string TrueBlock, FalseBlock;
                string ElseStatment, EndIfStatment;
                int idxIfBegin, idxIfEnd, idxElseBegin, idxEndIfBegin;
                bool boolValue;

                idxIfBegin = idxCurrent;
                idxCurrent += this.ConditionTagIfBegin.Length;

                // Searching for EndIf Index

                idxIfEnd = this._strTemplateBlock.IndexOf(this.ConditionTagIfEnd, idxCurrent);
                if (idxIfEnd == -1) throw new Exception("Could not find ConditionTagIfEnd");

                // Getting Value Name

                VarName = this._strTemplateBlock.Substring(idxCurrent, (idxIfEnd-idxCurrent));

                idxCurrent = idxIfEnd + this.ConditionTagIfEnd.Length;

                // Compare ElseIf and EndIf Indexes

                ElseStatment = this.ConditionTagElseBegin+VarName+this.ConditionTagElseEnd;
                EndIfStatment = this.ConditionTagEndIfBegin+VarName+this.ConditionTagEndIfEnd;
                idxElseBegin = this._strTemplateBlock.IndexOf(ElseStatment, idxCurrent);
                idxEndIfBegin = this._strTemplateBlock.IndexOf(EndIfStatment, idxCurrent);
                if (idxElseBegin > idxEndIfBegin) throw new Exception("Condition Else Tag placed after Condition Tag EndIf for '"+VarName+"'");

                // Getting True and False Condition Blocks

                if (idxElseBegin != -1)
                {
                    TrueBlock = this._strTemplateBlock.Substring(idxCurrent, (idxElseBegin-idxCurrent));
                    FalseBlock = this._strTemplateBlock.Substring((idxElseBegin+ElseStatment.Length), (idxEndIfBegin-idxElseBegin-ElseStatment.Length));
                }
                else
                {
                    TrueBlock = this._strTemplateBlock.Substring(idxCurrent, (idxEndIfBegin-idxCurrent));
                    FalseBlock = "";
                }

                // Parse Condition

                try
                {
                    boolValue = Convert.ToBoolean(this._hstValues[VarName]);
                }
                catch
                {
                    boolValue = false;
                }

                string BeforeBlock = this._strTemplateBlock.Substring(idxPrevious, (idxIfBegin-idxPrevious));

                if (this._hstValues.ContainsKey(VarName) && boolValue)
                {
                    this._ParsedBlock += BeforeBlock + TrueBlock.Trim();
                }
                else
                {
                    this._ParsedBlock += BeforeBlock + FalseBlock.Trim();
                }

                idxCurrent = idxEndIfBegin + EndIfStatment.Length;
                idxPrevious = idxCurrent;
            }
            this._ParsedBlock += this._strTemplateBlock.Substring(idxPrevious);
        }

        /// <summary>
        /// Parse all variables in template
        /// </summary>
        private void ParseVariables()
        {
            int idxCurrent = 0;
            while ((idxCurrent = this._ParsedBlock.IndexOf(this.VariableTagBegin, idxCurrent)) != -1)
            {
                string VarName, VarValue;
                int idxVarTagEnd;

                idxVarTagEnd = this._ParsedBlock.IndexOf(this.VariableTagEnd, (idxCurrent+this.VariableTagBegin.Length));
                if (idxVarTagEnd == -1) throw new Exception(String.Format("Index {0}: could not find Variable End Tag", idxCurrent));

                // Getting Variable Name

                VarName = this._ParsedBlock.Substring((idxCurrent+this.VariableTagBegin.Length), (idxVarTagEnd-idxCurrent-this.VariableTagBegin.Length));

                // Checking for Modificators

                string[] VarParts = VarName.Split(this.ModificatorTag.ToCharArray());
                VarName = VarParts[0];

                // Getting Variable Value
                // If Variable doesn't exist in _hstValue then
                // Variable Value equal empty string

                // [added 6/6/2006] If variable is null than it will also has empty string

                VarValue = String.Empty;
                if (this._hstValues.ContainsKey(VarName) && this._hstValues[VarName] != null)
                {
                    VarValue = this._hstValues[VarName].ToString();
                }

                // Apply All Modificators to Variable Value

                for (int i = 1; i < VarParts.Length; i++)
                    this.ApplyModificator(ref VarValue, VarParts[i]);

                // Replace Variable in Template

                this._ParsedBlock = this._ParsedBlock.Substring(0, idxCurrent) + VarValue + this._ParsedBlock.Substring(idxVarTagEnd+this.VariableTagEnd.Length);

                // Add Length of added value to Current index 
                // to prevent looking for variables in the added value
                // Fixed Date: April 5, 2006
                idxCurrent += VarValue.Length;
            }
        }

        /// <summary>
        /// Method for applying modificators to variable value
        /// </summary>
        /// <param name="Value">Variable value</param>
        /// <param name="Modificator">Determination statment</param>
        private void ApplyModificator(ref string Value, string Modificator)
        {
            // Checking for parameters
            
            string strModificatorName = "";
            string strParameters = "";
            int idxStartBrackets, idxEndBrackets;
            if ((idxStartBrackets = Modificator.IndexOf("(")) != -1) {
                idxEndBrackets = Modificator.IndexOf(")", idxStartBrackets);
                if (idxEndBrackets == -1)
                {
                    throw new Exception("Incorrect modificator expression");
                }
                else
                {
                    strModificatorName = Modificator.Substring(0, idxStartBrackets).ToUpper();             
                    strParameters = Modificator.Substring(idxStartBrackets+1, (idxEndBrackets-idxStartBrackets-1));
                }
            }
            else
            {
                strModificatorName = Modificator.ToUpper();
            }
            string[] arrParameters = strParameters.Split(this.ModificatorParamSep.ToCharArray());
            for (int i = 0; i < arrParameters.Length; i++)
                arrParameters[i] = arrParameters[i].Trim();

            try
            {
                Type typeModificator = Type.GetType("Modificators." + strModificatorName);
                if (typeModificator.IsSubclassOf(Type.GetType("Modificator")))
                {
                    EmailModificator objModificator = (EmailModificator)Activator.CreateInstance(typeModificator);
                    objModificator.Apply(ref Value, arrParameters);
                }
            }
            catch
            {
                throw new Exception(String.Format("Could not find modificator '{0}'", strModificatorName));
            }
        }
    }
}





