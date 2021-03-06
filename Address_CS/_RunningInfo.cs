﻿/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using System;
using System.Data;
using System.Configuration;
using OutSystems.HubEdition.RuntimePlatform;
using OutSystems.HubEdition.RuntimePlatform.Db;
using OutSystems.Internal.Db;

namespace ssAddress_CS {
	public class _RunningInfo: RunningInfo
	{
		private static bool alreadyRegistered = false;
		public static void Register() {
			if (alreadyRegistered)
			return;

			InitializeRunningInfo();
			if (!RunningInfo.IsInitialized) {
				// Retry
				InitializeRunningInfo();
				if (!RunningInfo.IsInitialized) {
					throw new Exception("Failed initializing RunningInfo: " + RunningInfo.InitializingErrorMessage);
				}
			}

			RunningInfo.EspaceUID = "070ed75c-5567-4cfd-abec-7c525b0af273";
			using(Transaction tran = DatabaseAccess.ForSystemDatabase.GetCommitableTransaction()) {
				if (espaceId == 0) {
					throw new Exception("Failed initializing RunningInfo: null eSpace identifier.");
				}

				areaId = DBRuntimePlatform.Instance.GetOrCreateArea(tran, espaceId, userId);
				RegisterInfo(tran);
				tran.Commit();
			}
		}

		public static void RegisterInfo(Transaction tran) {
			if (alreadyRegistered)
			return;
			alreadyRegistered = true;
			if (RunningInfo.EspaceName == "Address_CS") {
				Reset(tran);
			}
			RegisterAssembly(tran, "Address_CS", ConfigurationManager.AppSettings["OutSystems.HubEdition.EspaceCompilationHash"], false, "1", "WK8dCdPZhUSoqVMa8mDZjQ:cqbwXmvrVneU2hm+6cAoGQ\n4Mq+Y8Ogz0ijgylaJ7wVkQ:iPqk6it65xL+lq9bd+apaQ\nptGCa4RnlESpXSVw3+_Gtg:ABzQdaaKGfAKrvADOtEBrQ\npk8tmXeXqkqQi+OSq3X6xQ:o1bNagfsn9yKXjz_jdSAfA\nhIWpySV1SUqfbfcJA4FVQg:+3YEtzq4Csr8cLK4OOIdlA\nliSz1aQ6akWLFwxTvi_KDg:p9zJseTp4y2mHVL1iY+K6A");
			RunningInfo.InvokeRegister(tran, "ssBusiness_CS._RunningInfo, Business_CS");
			RunningInfo.RegisterAssemblyDependency(tran, "Address_CS", "Business_CS", "_LZkxnqGQUibtaYHV6m08w:JrEA8PENBZITKbtkSQKDfA");

		}
	}
}
