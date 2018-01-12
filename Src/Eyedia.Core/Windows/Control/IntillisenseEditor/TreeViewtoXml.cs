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
using System.Windows.Forms;
using System.Xml;
using System.Reflection;

namespace Eyedia.Core.Windows.Control
{

    /// <summary>
    /// Mithilfe dieser kleinen Klasse, kann der aufbau des Treeviews in eine Xml Datei exportiert
    /// und natürlich auch wieder in einen TreeView Importiert werden.
    /// </summary>
    /// <example>
    /// <code lang="C#">
    /// TreeView tmpTreeview = new TreeView();
    /// 
    /// XmlHandler xmlHandler = new XmlHandler();
    /// //treeview TO Xml
    /// xmlHandler.ExportTreeToXmlFile(tmpTreeview, "C:\\temp\\tmpTreeView.xml");
    /// </code>
    /// </example>
    public class TreeViewToXml
    {
        XmlDocument xmlDocument;

        /// <summary>
        /// Initialisiert eine neue Instanz der MultiClipboard Klasse.
        /// </summary>
        public TreeViewToXml()
        {
        }

        /// <summary>
        /// Den inhalt des TreeViews in eine xml Datei exportieren
        /// </summary>
        /// <param name="treeView">Der TreeView der exportiert werden soll</param>
        /// <param name="path">Ein  Pfad unter dem die Xml Datei entstehen soll</param>
        public void Extract(TreeView treeView, String path)
        {
            xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateElement("ROOT"));
            XmlExport(xmlDocument.DocumentElement, treeView.Nodes);
            xmlDocument.Save(path);
        }

        /// <summary>
        /// Eine vorher Exportierte Xml Datei wieder in ein TreeView importieren
        /// </summary>
        /// <param name="path">Der Quellpfad der Xml Datei</param>
        /// <param name="treeView">Ein TreeView in dem der Inhalt der Xml Datei wieder angezeigt werden soll</param>
        /// <exception cref="FileNotFoundException">gibt an das die Datei nicht gefunden werden konnte</exception>
        public void Load(String path, TreeView treeView)
        {
            xmlDocument = new XmlDocument();

            xmlDocument.Load(path);
            treeView.Nodes.Clear();
            XmlRekursivImport(treeView.Nodes, xmlDocument.DocumentElement.ChildNodes);
        }

        private XmlNode XmlExport(XmlNode nodeElement, TreeNodeCollection treeNodeCollection)
        {
            XmlNode xmlNode = null;
            foreach (TreeNode treeNode in treeNodeCollection)
            {
                xmlNode = xmlDocument.CreateElement("TreeViewNode");

                xmlNode.Attributes.Append(xmlDocument.CreateAttribute("value"));
                xmlNode.Attributes["value"].Value = treeNode.Text;

                xmlNode.Attributes.Append(xmlDocument.CreateAttribute("memberType"));
                if (treeNode.Tag != null)
                  //  && (treeNode.Tag is MemberTypes))
                {
                    //MemberTypes memberType = (MemberTypes)treeNode.Tag;
                    //if (memberType == MemberTypes.Custom)
                    //    xmlNode.Attributes["memberType"].Value = "1";
                    //else if (memberType == MemberTypes.Property)
                    //    xmlNode.Attributes["memberType"].Value = "3";
                    //else if (memberType == MemberTypes.Event)
                    xmlNode.Attributes["memberType"].Value = treeNode.Tag.GetType().ToString();

                }
                else
                {
                    xmlNode.Attributes["memberType"].Value = "4";
                }

                if (nodeElement != null)
                    nodeElement.AppendChild(xmlNode);

                if (treeNode.Nodes.Count > 0)
                {
                    XmlExport(xmlNode, treeNode.Nodes);
                }
            }
            return xmlNode;
        }

        private void XmlRekursivImport(TreeNodeCollection elem, XmlNodeList xmlNodeList)
        {
            TreeNode treeNode;
            foreach (XmlNode myXmlNode in xmlNodeList)
            {
                treeNode = new TreeNode(myXmlNode.Attributes["value"].Value);

                if (myXmlNode.ChildNodes.Count > 0)
                {
                    XmlRekursivImport(treeNode.Nodes, myXmlNode.ChildNodes);
                }
                elem.Add(treeNode);
            }
        }
    }
}



