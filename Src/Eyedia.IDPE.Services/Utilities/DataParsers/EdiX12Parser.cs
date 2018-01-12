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


namespace Eyedia.IDPE.Services
{
    public class EdiX12Parser : IDisposable
    {
        public string FileName { private set; get; }
        public string FileContent { private set; get; }        
        public List<string> Warnings { private set; get; }

        const string X12DLLName = "OopFactory.X12.dll";
        public EdiX12Parser(string fileName, string fileContent)
        {
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, X12DLLName)))
                throw new Exception(X12DLLName + " file does not exist!");

            FileName = fileName;
            FileContent = fileContent;
            Warnings = new List<string>();
        }

        public StringBuilder Parse(string xslt)
        {
            StringBuilder sb = new StringBuilder();
            string xml = GenerateXml();
            var transform = new XslCompiledTransform();
            using (Stream stream = XsltToStream(xslt))
            {
                transform.Load(XmlReader.Create(stream));
                var writer = new StringWriter();
                transform.Transform(XmlReader.Create(new StringReader(xml)), new XsltArgumentList(), writer);
                sb = writer.GetStringBuilder();
            }

            return sb;
        }

        public string GenerateXml()
        {
            string xml = string.Empty;
            using (Stream stream = FileContentToStream())
            {

                object x12Parser = Activator.CreateInstance(Type.GetType("OopFactory.X12.Parsing.X12Parser, OopFactory.X12"));
                MethodInfo miParseMultiple = x12Parser.GetType().GetMethod("ParseMultiple", new[] { typeof(Stream) });

                if (miParseMultiple != null)
                {
                    AddEventHandler(x12Parser);
                    object interchangeList = miParseMultiple.Invoke(x12Parser, new object[] { stream });

                    int interchangeListCount = 0;
                    int.TryParse(GetPropValue(interchangeList, "Count").ToString(), out interchangeListCount);

                    if (interchangeListCount == 1)
                    {
                        IEnumerable interchangeListItems = (IEnumerable)interchangeList;
                        object interchangeItemFirst = interchangeListItems.Cast<object>().FirstOrDefault();
                        MethodInfo miSerialize = interchangeItemFirst.GetType().GetMethod("Serialize", new Type[] { });
                        xml = miSerialize.Invoke(interchangeItemFirst, null).ToString();

                    }
                    else
                    {
                        throw new Exception(string.Format("{0} number of interchange(s) found in the EDI X12 file {1}! Only one interchange is expected.",
                            interchangeListCount, FileName));
                    }
                }
            }
            return xml;

        }

        private void AddEventHandler(object x12Parser)
        {
            var eventInfo = x12Parser.GetType().GetEvent("ParserWarning");
            var methodInfo = this.GetType().GetMethod("parser_ParserWarning");
            Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, methodInfo);
            eventInfo.AddEventHandler(x12Parser, handler);

        }

        public static object GetPropValue(object src, string propertyName)
        {
            return src.GetType().GetProperty(propertyName).GetValue(src, null);
        }

        Stream FileContentToStream()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(FileContent);
            writer.Flush();
            stream.Position = 0;
            return stream;
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

        public void parser_ParserWarning(object sender, EventArgs args)
        {
            Warnings.Add(GetPropValue(args, "Message").ToString());
        }

        public void Dispose()
        {

        }
    }
}


