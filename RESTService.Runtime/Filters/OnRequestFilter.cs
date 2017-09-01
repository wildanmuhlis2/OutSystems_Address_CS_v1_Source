/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using OutSystems.HubEdition.RuntimePlatform;
using OutSystems.RESTService.Controllers;

namespace OutSystems.RESTService.Filters {

    /// <summary>
    /// Filter that runs BEFORE model binding
    /// <remarks>
    /// Note about extensibility: AuthorizationFilterAttribute instead of an ActionFilterAttribute
    /// We cannot have an ActionFilterAttribute because it happens after ModelBinding. So, altering the request in an ActionFilter wouldn't affect anything.
    /// Also, reading the body in an ActionFilterAttribute would throw an exception since the WebAPI already consumed it.
    /// 
    /// Refer to the ASP.NET Web API HTTP Message Lifecycle: https://www.microsoft.com/en-us/download/confirmation.aspx?id=36476
    /// </remarks>
    /// </summary>
    public class OnRequestFilter : AuthorizationFilterAttribute {

        /// <summary>
        /// The type that has the OnRequest method
        /// </summary>
        private AbstractFilter filterImpl;
        private string ssKey;
        private string serviceName;

        /// <summary>
        /// The type that has the OnRequest method
        /// </summary>
        public OnRequestFilter(Type typeOfControllerFilter, string ssKey, string serviceName) {
            if(typeOfControllerFilter != null) {
                filterImpl = (AbstractFilter)Activator.CreateInstance(typeOfControllerFilter);
            }
            this.ssKey = ssKey;
            this.serviceName = serviceName;
        }        

        public override void OnAuthorization(HttpActionContext actionContext) {
            base.OnAuthorization(actionContext);

            RestServiceApiController controller = actionContext.ControllerContext.Controller as RestServiceApiController;
            if (controller == null) {
                return;
            }
            
            // Setup the context here, so that if we fail in the ValidateRequestSecurity, we can still log that request in Integration Logs
            LoggingHelper.SetupLoggingContext(actionContext, serviceName, ssKey);
            controller.ValidateRequestSecurity();
            HttpRequestMessage httpRequestMessage = actionContext.Request;
            if (controller.IsSwaggerRequest(httpRequestMessage.GetRouteData().Route.RouteTemplate)) {
				return;
			}

            LoggingHelper.LogRequest(actionContext);
			if(filterImpl != null) {
            	filterImpl.OnRequest(actionContext, httpRequestMessage, controller);
			}
        }
    }
}