/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using System;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;
using OutSystems.HubEdition.RuntimePlatform.Log;
using OutSystems.HubEdition.RuntimePlatform.Callbacks.Invoke;
using System.Web;
using System.Data;
using OutSystems.RuntimeCommon;
using System.Reflection;
using OutSystems.ObjectKeys;

namespace OutSystems.HubEdition.RuntimePlatform.Callbacks.Invoke {
    [Serializable]
    internal sealed class LibraryCallbackInvoke : AbstractStringCallbackInvoke {
        [NonSerialized]
        private string _url;

        private string _provider;
        private string _service;
        private string _method;
        private string _data;

        public LibraryCallbackInvoke(string url, string serviceName, string providerName, string methodName)
            : this(url, serviceName, providerName, methodName, string.Empty) {
        }

        public LibraryCallbackInvoke(string url, string serviceName, string providerName, string methodName, string data) {
            _url = url;
            _service = serviceName;
            _provider = providerName;
            _method = methodName;
            _data = data?? string.Empty;
        }

        public override string ProviderName {
            get { return _provider; }
        }

        protected override bool Equals(AbstractStringCallbackInvoke obj) {
            return obj is LibraryCallbackInvoke && Equals((LibraryCallbackInvoke)obj);
        }

        private bool Equals(LibraryCallbackInvoke obj) {
            return _provider.Equals(obj._provider) &&
                _service.Equals(obj._service) &&
                _method.Equals(obj._method) &&
                _data.Equals(obj._data);
        }

        public override int GetHashCode() {
            return _provider.GetHashCode() ^
                _service.GetHashCode() ^
                _method.GetHashCode() ^
                _data.GetHashCode();
        }

        public override string InvokeInner(string locale, AppInfo app, SessionInfo session, AbstractCallback.EventListener listener) {
            string[] urlParts = _url.Split('|');

            
            Assembly lib;
            string handlerClassName;

            if (urlParts.Length == 2) {
                if (urlParts[0].IndexOf('/') != -1) {
                    
                    lib = Assembly.LoadFrom(urlParts[0]);
                } else {
                    
                    lib = Assembly.Load(urlParts[0]);
                }
                handlerClassName = urlParts[1];
            } else {
                
                lib = Assembly.GetExecutingAssembly();
                handlerClassName = urlParts[0];
            }

            
            ILibraryCallbackHandler handler = (ILibraryCallbackHandler)lib.CreateInstance(handlerClassName);

            return handler.GetDynamicHtmlInjection(app, session, locale, _data);
        }
    }
}