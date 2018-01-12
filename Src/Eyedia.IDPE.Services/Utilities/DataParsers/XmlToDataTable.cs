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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using System.Data;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// Converts Xml to DataTable
    /// </summary>
    public class XmlToDataTable : DataParser
    {
        public string Xslt { get; private set; }

        /// <summary>
        /// Instantiate XmlToDataTable with datasource in demo mode
        /// </summary>
        public XmlToDataTable(){}

        /// <summary>
        /// Instantiate XmlToDataTable with datasource
        /// </summary>
        /// <param name="dataSource">The data source</param>        
        public XmlToDataTable(Job job)
            : base(job)
        {
            if ((job != null)
                && (job.DataSource != null))
                Xslt = job.DataSource.Keys.GetKeyValue(SreKeyTypes.Xslt);
        }       

        /// <summary>
        /// Instantiate XmlToDataTable with datasource
        /// </summary>
        /// <param name="dataSource">The data source</param>        
        public XmlToDataTable(DataSource dataSource)
            : base(dataSource)
        {
            if(dataSource != null)
                Xslt = dataSource.Keys.GetKeyValue(SreKeyTypes.Xslt);
        }       

        /// <summary>
        /// Converts xml file into data table
        /// </summary>        
        /// <param name="csvRows">List of string csv rows</param>
        /// <param name="emptyRowWarnings">List of warnings</param>
        /// <param name="columnCount">Total column count</param>        
        /// <returns></returns>
        public DataTable Parse(ref List<string> csvRows, ref List<string> emptyRowWarnings, ref int columnCount)
        {            
            CsvToDataTable csvToDataTable = new CsvToDataTable(Job);
            return csvToDataTable.ParseData(ref csvRows, ref emptyRowWarnings, ref columnCount);
            
        }        

        /// <summary>
        /// Transforms xml to delimited data based on Xslt. If Xslt not passed, data source xslt will be used
        /// </summary>        
        /// <param name="xslt">Optional xslt, if not passed then data source xslt will be used</param>
        /// <param name="xmlContent">xml content, generally called from tool just to test</param>
        /// <returns>Delimited data</returns>
        public StringBuilder Parse(string xslt = null, string xmlContent = null)
        {
            string myXslt = Xslt;
            if (xslt != null)
                myXslt = xslt;

            StringBuilder sb = new StringBuilder();
            var transform = new XslCompiledTransform();
            using (Stream stream = XsltToStream(myXslt))
            {
                transform.Load(XmlReader.Create(stream));
                var writer = new StringWriter();
                if(!string.IsNullOrEmpty(xmlContent))
                    transform.Transform(XmlReader.Create(new StringReader(xmlContent)), new XsltArgumentList(), writer);
                else
                    transform.Transform(XmlReader.Create(new StringReader(Job.FileContent.ToString())), new XsltArgumentList(), writer);
                sb = writer.GetStringBuilder();
            }

            return sb;
        }

        Stream XsltToStream(string xslt)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(xslt);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        public override void Dispose()
        {
            //todo
        }
    }
}


