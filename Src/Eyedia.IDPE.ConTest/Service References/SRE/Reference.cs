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

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Eyedia.IDPE.ConTest.SRE {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SRE.ISre")]
    public interface ISre {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISre/StartPullers", ReplyAction="http://tempuri.org/ISre/StartPullersResponse")]
        string StartPullers();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISre/StopPullers", ReplyAction="http://tempuri.org/ISre/StopPullersResponse")]
        string StopPullers();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISre/GetPullersStatus", ReplyAction="http://tempuri.org/ISre/GetPullersStatusResponse")]
        string GetPullersStatus();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISre/ProcessJob", ReplyAction="http://tempuri.org/ISre/ProcessJobResponse")]
        string ProcessJob(int dataSourceId, string dataSourceName, string processingBy, string inputData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISre/ProcessObjects", ReplyAction="http://tempuri.org/ISre/ProcessObjectsResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        object ProcessObjects(int dataSourceId, string dataSourceName, string processingBy, object[] inputData, string overridenMapping);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISre/ProcessXml", ReplyAction="http://tempuri.org/ISre/ProcessXmlResponse")]
        string ProcessXml(string xmlInput, string processingBy);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISre/ClearCache", ReplyAction="http://tempuri.org/ISre/ClearCacheResponse")]
        string ClearCache();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISreChannel : Eyedia.IDPE.ConTest.SRE.ISre, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SreClient : System.ServiceModel.ClientBase<Eyedia.IDPE.ConTest.SRE.ISre>, Eyedia.IDPE.ConTest.SRE.ISre {
        
        public SreClient() {
        }
        
        public SreClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SreClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SreClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SreClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string StartPullers() {
            return base.Channel.StartPullers();
        }
        
        public string StopPullers() {
            return base.Channel.StopPullers();
        }
        
        public string GetPullersStatus() {
            return base.Channel.GetPullersStatus();
        }
        
        public string ProcessJob(int dataSourceId, string dataSourceName, string processingBy, string inputData) {
            return base.Channel.ProcessJob(dataSourceId, dataSourceName, processingBy, inputData);
        }
        
        public object ProcessObjects(int dataSourceId, string dataSourceName, string processingBy, object[] inputData, string overridenMapping) {
            return base.Channel.ProcessObjects(dataSourceId, dataSourceName, processingBy, inputData, overridenMapping);
        }
        
        public string ProcessXml(string xmlInput, string processingBy) {
            return base.Channel.ProcessXml(xmlInput, processingBy);
        }
        
        public string ClearCache() {
            return base.Channel.ClearCache();
        }
    }
}


