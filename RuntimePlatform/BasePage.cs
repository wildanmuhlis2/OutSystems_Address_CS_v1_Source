/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using System;
using System.Web.UI;
using System.Collections.Generic;
using OutSystems.HubEdition.RuntimePlatform.Email;
using OutSystems.RuntimeCommon;

namespace OutSystems.HubEdition.RuntimePlatform {

    public abstract class BasePage : Page, IVarsBag {
#if JAVA
        public abstract System.Web.HttpRequest Request { get; }                
#endif
        public abstract void EvaluateFields(VarValue variable, object parent, string baseName, string fields);

        public abstract object GetVariableValue(string varName);
        public abstract bool HasVariable(string varName);

        public abstract void ToXml(object parent, System.Xml.XmlElement baseElem, string fieldName, int detailLevel);
        public abstract string[] VarNames { get; }
        public abstract void SetNewOrigin(object origin);
        public abstract void InitVars(string[] varNames, string[] varRtNames);

        public abstract string GetBookmarkableURL();
        public abstract string GetFormAction();

        public virtual bool IsUsingMobileTheme { get { return false; } }

        protected string AddLocalInlineCss(string path, string currentInlineCss, HashSet<String> doneList) {
            return AddLocalInlineCss(path, currentInlineCss, doneList, null);
        }

        protected string AddLocalInlineCss(string path, string currentInlineCss, HashSet<String> doneList, string hostName) {
            if (!String.IsNullOrEmpty(path)) {
                string inlineCss = null;
                BaseAppUtils.getResourceFileContent(out inlineCss, path);
                currentInlineCss += EmailHelper.FlattenCSSFile(inlineCss, path, doneList, hostName);
            }
            return currentInlineCss;
        }
    }
}