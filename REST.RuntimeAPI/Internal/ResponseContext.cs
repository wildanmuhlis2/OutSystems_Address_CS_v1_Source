/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using System;
using System.IO;
using System.Net;
using System.Text;

namespace OutSystems.Internal.REST {

    /// <summary>
    /// Internal class used to store the context of a Rest Response.
    /// This class is for internal use only. Use the OutSystems.RuntimePublic.REST.RestResponse class instead.
    /// </summary>
    public class ResponseContext : IDisposable {

        [ThreadStatic]
        private static ResponseContext currentContext;

        private ResponseContext previousContext = null;

        private readonly string actionName;
        private readonly HttpWebResponse httpResponse;
        private string responseBodyAsText = null;
        private byte[] responseBodyAsBinary = null;

        /// <summary>
        /// Constructor for ResponseContext.
        /// This class is for internal use only. Use the OutSystems.RuntimePublic.REST.RestResponse class instead.
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="httpResponse"></param>
        public ResponseContext(string actionName, HttpWebResponse httpResponse) {
            previousContext = currentContext;
            ResponseContext.currentContext = this;

            this.actionName = actionName;
            this.httpResponse = httpResponse;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            ResponseContext.currentContext = previousContext;
        }

        internal static ResponseContext GetCurrent() {
            return ResponseContext.currentContext;
        }

        /// <summary>
        /// Returns the name of the REST API Method that invoked the extension.
        /// </summary>
        public string GetActionName() {
            return actionName;
        }

        /// <summary>
        /// Returns the native HttpWebResponse object that resulted from the web request.
        /// </summary>
        public System.Net.HttpWebResponse GetHttpWebResponse() {
            return httpResponse;
        }

        /// <summary>
        /// Returns the message body of the web response as a string.
        /// </summary>
        public string GetText() {
            EnsureTextIsInitialized();
            return responseBodyAsText;
        }

        /// <summary>
        /// Sets the message body of the web response.
        /// If the REST API Method has its 'Request Format' property set to 'Binary', no changes are made
        /// to the message body.
        /// </summary>
        public void SetText(string text) {
            this.responseBodyAsText = text;
            this.responseBodyAsBinary = null;
        }

        /// <summary>
        /// Returns the message body of the web response as binary content.
        /// </summary>
        public byte[] GetBinary() {
            EnsureBinaryIsInitialized();
            return responseBodyAsBinary;
        }

        /// <summary>
        /// Sets the message body of the web response.
        /// </summary>
        public void SetBinary(byte[] bytes) {
            responseBodyAsBinary = bytes;
            responseBodyAsText = null;
        }

        private static readonly Encoding DefaultResponseEncoding = Encoding.GetEncoding("ISO-8859-1");

        private Encoding GetResponseEncoding() {
            String charsetFromContentType = httpResponse.CharacterSet;
            if (!String.IsNullOrEmpty(charsetFromContentType)) {
                try {
                    return Encoding.GetEncoding(httpResponse.CharacterSet);
                } catch (ArgumentException) {
                    // return the default encoding
                }
            }
            return DefaultResponseEncoding;
        }

        private void EnsureTextIsInitialized() {
            if (responseBodyAsText != null) {
                return;
            }

            EnsureBinaryIsInitialized();
            if (responseBodyAsBinary != null) {
                responseBodyAsText = GetResponseEncoding().GetString(responseBodyAsBinary);
            } else {
                // Fallback to empty string
                responseBodyAsText = string.Empty;
            }
        }

        private bool httpResponseAlreadyRead = false;

        private void EnsureBinaryIsInitialized() {
            if (responseBodyAsBinary != null) {
                return;
            }

            if (responseBodyAsText != null) {
                // Text is already set, so build the binary from it
                responseBodyAsBinary = GetResponseEncoding().GetBytes(responseBodyAsText);
                return;
            }

            if (!httpResponseAlreadyRead) {
                try {
                    // Ignoring httpResponse.ContentLength because the GetResponseStream() could already have been read outside of this scope
                    byte[] responseBuffer = new byte[4096];
                    MemoryStream responseMemoryStream = new MemoryStream();
                    var responseStream = httpResponse.GetResponseStream();
                    int readBytes;
                    while ((readBytes = responseStream.Read(responseBuffer, 0, responseBuffer.Length)) > 0) {
                        responseMemoryStream.Write(responseBuffer, 0, readBytes);
                    }
                    responseBodyAsBinary = responseMemoryStream.ToArray();
                } catch (IOException) {
                    responseBodyAsBinary = new byte[0];
                }

                httpResponseAlreadyRead = true;
            } else {
                responseBodyAsBinary = new byte[0];
            }
        }
    }
}