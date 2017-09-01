/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using System;
using System.IO;
using System.Text;
using System.Web.UI;
using OutSystems.HubEdition.RuntimePlatform;
using OutSystems.HubEdition.RuntimePlatform.Log;
using OutSystems.RuntimeCommon;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using OutSystems.RuntimeCommon.Cryptography;

namespace OutSystems.HubEdition.WebWidgets {
    public abstract class OSPageViewStateCompressed : OSPageViewState {

        protected OSPageViewStateCompressed(bool quirksMode) : base(quirksMode) {}

        private byte[] CompressBinary(byte[] input) {
            var inputBuffer = new MemoryStream(input, 0, input.Length);
			var outputBuffer = new MemoryStream();
            var compressor = new DeflaterOutputStream(outputBuffer, new Deflater(Deflater.BEST_COMPRESSION, false));
			StreamUtils.PumpStream(inputBuffer, compressor);
			inputBuffer.Close();
			compressor.Close();
			return outputBuffer.ToArray();
		}
		
		private byte[] DecompressBinary(byte[] input) {            
			var inputBuffer = new MemoryStream(input, 0, input.Length);
            var decompressor = new InflaterInputStream(inputBuffer);
			var outputBuffer = new MemoryStream();
			StreamUtils.PumpStream(decompressor, outputBuffer);
			decompressor.Close();
			outputBuffer.Close();
			return outputBuffer.ToArray();
		}

        protected override object DeserializeViewState(string viewState, out string hash) {
            ObjectStateFormatter formatter = new ObjectStateFormatter();
            OutSystems.HubEdition.RuntimePlatform.RuntimePlatformUtils.SetViewstateSize(viewState.Length);
            byte[] bytes = new byte[0];


            try {
                if (viewState.Length == 0) {
                    if (Settings.GetBool(Settings.Configs.EncryptViewState)) {
                        throw new Exception("Missing viewstate");
                    }
                } else {
                    bytes = Convert.FromBase64String(viewState);
                    if (Settings.GetBool(Settings.Configs.EncryptViewState)) {
                        bytes = AuthenticatedEncryptionWithAssociatedData.Instance.Decrypt(bytes, GetAssociatedData());
                    }
                    bytes = DecompressBinary(bytes);
                }
                return base.DeserializeViewState(Convert.ToBase64String(bytes), out hash);

			} catch (Exception excep) {
                HeContext context = null;
                try { context = AppInfo.GetAppInfo().OsContext; } catch { }
                string pageName = "";
                try { pageName = context.CurrentScreen.ToString(); } catch { }
				try{
                    ErrorLog.LogApplicationError("Error Deserializing ViewState of page \"" + pageName + "\", visitorid \"" + AppInfo.GetAppInfo().VisitorId + "\", size " + viewState.Length + " bytes (\"" + viewState.Substring(0, Math.Min(10, viewState.Length)) + "...\")." + "\n" +
                    excep.Message, excep.StackTrace, context, "Global");
				} catch { }
			
				throw;
			}
		}

        private byte[] GetAssociatedData() {

            string visitorId = null;
            string userId = null;

            if (Settings.GetBool(Settings.Configs.EncryptViewStateWithVisitorId)) {
                visitorId = AppInfo.GetAppInfo().VisitorId;
            }

            if (Settings.GetBool(Settings.Configs.EncryptViewStateWithUserId)) {
                var osContext = AppInfo.GetAppInfo().OsContext;
                if (osContext != null && osContext.Session != null) {
                    userId = osContext.Session.UserId.ToString();
                }
            }

            if (visitorId == null && userId == null) {
                return null;
            }

            return Encoding.UTF8.GetBytes((visitorId ?? "") + (userId ?? ""));
        }

        protected override string SerializeViewState(object viewState, out string hash) {
            string serializedViewState = base.SerializeViewState(viewState, out hash);

            byte[] bytes = CompressBinary( Convert.FromBase64String( serializedViewState));

            if( Settings.GetBool( Settings.Configs.EncryptViewState)) {

                bytes = AuthenticatedEncryptionWithAssociatedData.Instance.Encrypt(bytes, GetAssociatedData());
            }

            serializedViewState = Convert.ToBase64String(bytes);
            return serializedViewState;
        }
	}
}