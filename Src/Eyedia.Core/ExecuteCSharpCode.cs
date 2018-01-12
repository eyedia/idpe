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
using System.Linq;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace Eyedia.Core
{
    public class ExecuteCSharpCode
    {
        #region Enums

        public enum CompilerVersions
        {
            Unknown,
            v35,
            v40,
        }

        #endregion Enums

        public ExecuteCSharpCode(string classFullName, CompilerVersions compilerVersion, string[] referencedAssemblies)
        {
            this.ClassFullName = classFullName;
            this.CompilerVersion = compilerVersion;
            this.ReferencedAssemblies = referencedAssemblies;
        }

        public ExecuteCSharpCode(string classFullName, object[] classConstructorParameters, CompilerVersions compilerVersion, string[] referencedAssemblies)
        {
            this.ClassFullName = classFullName;
            this.ClassConstructorParameters = classConstructorParameters;
            this.CompilerVersion = compilerVersion;
            this.ReferencedAssemblies = referencedAssemblies;
        }


        public CompilerResults Compile(string source, string method, object[] parameters)
        {
            Init();
            return cSharpCodeProvider.CompileAssemblyFromSource(compilerParameters, source);            
        }

        public object Execute(CompilerResults compilerResults, string method, object[] parameters)
        {
            Init();
            if (compilerResults.Errors.Count != 0)
                throw new Exception("Can not execute, there are compiler errors!" + Environment.NewLine + GetErrorsAsString(compilerResults));

            object o = compilerResults.CompiledAssembly.CreateInstance(ClassFullName);

            return InvokeMethod(o, method, parameters);
        }

        public object Execute(string source, string method, object[] parameters, bool throwExceptionIncaseOfCompilerError = true)
        {
            Init();
            CompilerResults results = cSharpCodeProvider.CompileAssemblyFromSource(compilerParameters, source);

            if (results.Errors.Count != 0)
            {
                Trace.TraceError("Error while executing dynamic c# code!");
                Trace.Indent();
                foreach (CompilerError e in results.Errors)
                {
                    Trace.TraceError(e.ToString());
                }
                Trace.Unindent();
                Trace.Flush();
                if (throwExceptionIncaseOfCompilerError)
                    throw new Exception("Can not execute, there are compiler errors!" + Environment.NewLine + GetErrorsAsString(results));
                else
                    return null;
            }

            object o = null;
            if(ClassConstructorParameters == null)
                o = results.CompiledAssembly.CreateInstance(ClassFullName);
            else 
                o = results.CompiledAssembly.CreateInstance(ClassFullName,true, BindingFlags.CreateInstance, null, ClassConstructorParameters, null, null);

            return InvokeMethod(o, method, parameters);
          
        }        

        #region Properties
        CSharpCodeProvider cSharpCodeProvider;
        CompilerParameters compilerParameters;
        public string ClassFullName { get; private set; }
        public object[] ClassConstructorParameters { get; private set; }
        public CompilerVersions CompilerVersion { get; private set; }
        public string[] ReferencedAssemblies { get; private set; }


        public string CompilerVersionString
        {
            get
            {
                switch (CompilerVersion)
                {
                    case CompilerVersions.v35:
                        return "v3.5";
                    case CompilerVersions.v40:
                        return "v4.0";
                    default:
                        return "v4.0";
                }
            }
        }

        #endregion Properties

        #region Helpers

        void Init()
        {
            Dictionary<string, string> providerOptions = new Dictionary<string, string>
                {
                    {"CompilerVersion", CompilerVersionString}
                };
            cSharpCodeProvider = new CSharpCodeProvider(providerOptions);

            compilerParameters = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false
            };

            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
            foreach (string assemblyName in ReferencedAssemblies)
            {
                string actualAssemblyName = assemblyName;
                if ((assemblyName.StartsWith("System", StringComparison.OrdinalIgnoreCase) == false)
                    && (assemblyName.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase) == false))
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    if (Eyedia.Core.Web.Utilities.IsWebHosted())
                        baseDir = Path.Combine(baseDir, "Bin");

                    if (Path.GetFileName(assemblyName) == assemblyName)
                        actualAssemblyName = Path.Combine(baseDir, assemblyName);
                }

                compilerParameters.ReferencedAssemblies.Add(actualAssemblyName);
            }
        }

        string GetErrorsAsString(CompilerResults results)
        {
            string compilerErrors = string.Empty;
            for (int i = 0; i < results.Errors.Count; i++)
            {
                compilerErrors += results.Errors[i].ToString();
            }
            return compilerErrors;
        }

        private object InvokeMethod(object o, string method, object[] parameters)
        {
            Type[] methodParameterTypes = GetTypeFromParameters(parameters);
            string strMethodParameterTypes = string.Join(",", methodParameterTypes.Select(t => t.Name).ToArray());
            if (o == null)
                throw new Exception(String.Format("Could not execute CSharp code - Could not create instance of class! Class name = '{0}', method name = '{1}', method parameter(s) = '{2}'",
                    ClassFullName, method, strMethodParameterTypes));
            
            MethodInfo mi = o.GetType().GetMethod(method, methodParameterTypes);
            if (mi == null)
                throw new Exception(String.Format("Could not execute CSharp code - Could not find the appropriate method! Class name = '{0}', method name = '{1}', method parameters(s) = '{2}'",
                    ClassFullName, method, strMethodParameterTypes));

            object resultObject = null;
            try
            {
                resultObject = mi.Invoke(o, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Could not execute CSharp code - Error while executing the dynamic code! Class name = '{0}', method name = '{1}', method parameters(s) = '{2}'{3}Stack Trace ='{4}",
                    ClassFullName, method, strMethodParameterTypes, Environment.NewLine, ex.ToString()));
            }
            return resultObject;
        }

        private Type[] GetTypeFromParameters(object[] parameters)
        {
            List<Type> types = new List<Type>();
            if (parameters != null)
            {
                foreach (Object parameter in parameters)
                {
                    types.Add(parameter.GetType());
                }
            }
            return types.ToArray();
        }
        #endregion Helpers
    }
}



