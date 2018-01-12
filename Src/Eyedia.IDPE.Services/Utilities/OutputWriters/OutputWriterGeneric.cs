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
using System.Xml;
using System.Diagnostics;
using Eyedia.Core;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Services
{
	public class OutputWriterGeneric: OutputWriter
	{
        public OutputWriterGeneric() : base() { }
        public OutputWriterGeneric(Job job) : base(job) { }


        /// <summary>
        /// Generates XML string based on pre-determined format
        /// </summary>
        /// <returns></returns>
        public override StringBuilder GetOutput()
        {
            if (_Job.IsErrored)
                return new StringBuilder();
            else
                return GenerateGenericOutput(this._Job);
        }

        public override object GetCustomOutput()
        {
            return null;
        }

        public static StringBuilder GenerateGenericOutput(Job processedJob)
        {            
            string root = "ValidatorResult";
            string child1 = "DataSource";
            string child2 = "Rows";            
            string child2Child1 = "Row";
            string child2Child1Child1 = "Attributes";
            string child2Child1Child1Child1 = "Attribute";
            string child2Child1Child2 = "SystemAttributes";
            string child2Child1Child2Child1 = "Attribute";

            string child3 = "Summary";            
            int totalValid = 0;
            

            StringBuilder sb = new StringBuilder();
            //XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            //xmlWriterSettings.Encoding = Encoding.UTF8;
            XmlWriter writer = XmlWriter.Create(sb);

            writer.WriteStartElement(root);

            writer.WriteStartElement(child1);
            if (processedJob.DataSource.Id > 0)
                writer.WriteElementString("Id", processedJob.DataSource.Id.ToString());             
            if (processedJob.DataSource.Name != null)
                writer.WriteElementString("Name", processedJob.DataSource.Name);
            writer.WriteEndElement();


            //List<string> allErrors = new List<string>();
            writer.WriteStartElement(child2);   //<Rows>
            foreach (var row in processedJob.Rows)
            {                
                if (row == null) break;
                writer.WriteStartElement(child2Child1);   //<Row>
                writer.WriteStartElement(child2Child1Child1); //<Attributes>
                foreach (var col in row.Columns)
                {                    
                    writer.WriteStartElement(child2Child1Child1Child1); //<Attribute>
                    writer.WriteElementString("Name", col.Name);
                    writer.WriteStartElement("Value");
                    if (!string.IsNullOrEmpty(col.ValueEnumValue))
                    {
                        writer.WriteStartAttribute("Text");
                        writer.WriteValue(col.ValueEnumValue);
                        writer.WriteEndAttribute();
                    }
                    if (!string.IsNullOrEmpty(col.ValueEnumReferenceKey))
                    {
                        writer.WriteStartAttribute("ReferenceKey");
                        writer.WriteValue(col.ValueEnumReferenceKey);
                        writer.WriteEndAttribute();
                    }

                    if (col.Type is SreBit)
                        writer.WriteValue(col.Value.ParseBool());    //print true/false
                    else if (col.Type is SreCodeset)
                        writer.WriteValue(col.ValueEnumCode);    //Enum values are always enum code
                    else
                        writer.WriteValue(col.Value);
                    writer.WriteEndElement();
                    if (EyediaCoreConfigurationSection.CurrentConfig.Tracking.UpdateMatrix)
                        writer.WriteElementString("UpdateMatrix", col.UpdateMatrix);
                    writer.WriteElementString("Error", col.Error.Message);                    
                    
                    writer.WriteEndElement();//</Attribute>
                }
                writer.WriteEndElement();//</Attributes>
                writer.WriteStartElement(child2Child1Child2); //<SystemAttributes>
                foreach (var col in row.ColumnsSystem)
                {
                    if (col.Name.Equals("IsValid", StringComparison.OrdinalIgnoreCase))
                    {
                        bool isValid = col.Value.ParseBool();
                        if (isValid)
                            totalValid++;
                    }
                    writer.WriteStartElement(child2Child1Child2Child1); //<Attribute>                    
                    writer.WriteElementString("Name", col.Name);
                    writer.WriteStartElement("Value");
                    if (!string.IsNullOrEmpty(col.ValueEnumValue))
                    {
                        writer.WriteStartAttribute("Text");
                        writer.WriteValue(col.ValueEnumValue);
                        writer.WriteEndAttribute();
                    }
                    if (!string.IsNullOrEmpty(col.ValueEnumReferenceKey))
                    {
                        writer.WriteStartAttribute("ReferenceKey");
                        writer.WriteValue(col.ValueEnumReferenceKey);
                        writer.WriteEndAttribute();
                    }

                    if(col.Type is SreBit)
                        writer.WriteValue(col.Value.ParseBool());
                    else if (col.Type is SreCodeset)
                        writer.WriteValue(col.ValueEnumCode);    //Enum values are always enum code
                    else
                        writer.WriteValue(col.Value);
                    writer.WriteEndElement();
                    if (EyediaCoreConfigurationSection.CurrentConfig.Tracking.Enabled)
                        writer.WriteElementString("UpdateMatrix", col.UpdateMatrix);
                    writer.WriteElementString("Error", col.Error.Message);                 
                    writer.WriteEndElement();   //</Attribute>
                }
                writer.WriteEndElement();//</SystemAttributes>
                writer.WriteEndElement();   //</Row>
            }           
            writer.WriteEndElement();   //</Rows>

            //common errors + all row level errors            
            writer.WriteStartElement(child3);   //<Summary>
            TimeSpan timeTaken;
            if (!processedJob.IsFinished)
                timeTaken = DateTime.Now.Subtract(processedJob.StartedAt);
            else
                timeTaken = processedJob.FinishedAt.Subtract(processedJob.StartedAt);

            double perRow = timeTaken.TotalMilliseconds / processedJob.TotalRowsProcessed;

            if(!string.IsNullOrEmpty(processedJob.FileName))   //we will not have file name when request comes through WCF
                writer.WriteElementString("FileName", processedJob.FileName);

            writer.WriteElementString("StartedAt", processedJob.StartedAt.ToString());
            writer.WriteElementString("IsFinished", processedJob.IsFinished.ToString());
            writer.WriteElementString("TotalRequested", processedJob.TotalRowsToBeProcessed.ToString());
            writer.WriteElementString("TotalRowsProcessed", processedJob.TotalRowsProcessed.ToString());            
            writer.WriteElementString("TotalValid", totalValid.ToString());
            writer.WriteElementString("Errors", processedJob.Errors.ToLine());
            writer.WriteElementString("Warnings", processedJob.Warnings.ToLine());
            writer.WriteElementString("BadData", processedJob.BadDataInCsvFormat.Count >1? processedJob.BadDataInCsvFormat.ToLine(true): string.Empty);
            writer.WriteEndElement();   //</Summary>
            writer.WriteEndElement();

            writer.Close();
            
            return sb;  //.ToString().Replace("utf-16", "utf-8");
        }

	}
}





