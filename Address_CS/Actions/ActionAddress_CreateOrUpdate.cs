﻿/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using OutSystems.HubEdition.RuntimePlatform;
using OutSystems.HubEdition.RuntimePlatform.Sms;
using OutSystems.HubEdition.RuntimePlatform.Log;
using OutSystems.HubEdition.RuntimePlatform.Db;
using OutSystems.Internal.Db;
using OutSystems.HubEdition.RuntimePlatform.Email;
using OutSystems.HubEdition.RuntimePlatform.Web;
using OutSystems.HubEdition.RuntimePlatform.NewRuntime;
using OutSystems.RuntimeCommon;
using OutSystems.ObjectKeys;
using System.Resources;

namespace ssAddress_CS {

	public partial class Actions {
		public class lcvAddress_CreateOrUpdate: VarsBag {
			public ENAddressEntityRecord inParamAddressR;
			public long resCreateOrUpdateAddress_outParamId = 0L;

			public lcvAddress_CreateOrUpdate(ENAddressEntityRecord inParamAddressR) {
				this.inParamAddressR = inParamAddressR;
			}
		}
		public class lcoAddress_CreateOrUpdate: VarsBag {
			public long outParamAddressId = 0L;

			public lcoAddress_CreateOrUpdate() {
			}
		}
		/// <summary>
		/// Action <code>ActionAddress_CreateOrUpdate</code> that represents the Service Studio user action
		///  <code>Address_CreateOrUpdate</code> <p> Description: This action creates or updates the give
		/// n address.</p>
		/// </summary>
		public static void ActionAddress_CreateOrUpdate(HeContext heContext, ENAddressEntityRecord inParamAddressR, out long outParamAddressId) {
			lcoAddress_CreateOrUpdate result = new lcoAddress_CreateOrUpdate();
			lcvAddress_CreateOrUpdate localVars = new lcvAddress_CreateOrUpdate(inParamAddressR);
			try {
				// new address?
				if (((localVars.inParamAddressR.ssId==Convert.ToInt64(BuiltInFunction.NullIdentifier())))) {
					// creation info
					// AddressR.Active = True
					localVars.inParamAddressR.ssActive = true;
					// AddressR.CreatedDate = CurrDateTime
					localVars.inParamAddressR.ssCreatedDate = BuiltInFunction.CurrDateTime();
					// AddressR.CreatedBy = GetUserId
					localVars.inParamAddressR.ssCreatedBy = BuiltInFunction.GetUserId();
				}

				// update info
				// AddressR.LastUpdatedDate = CurrDateTime
				localVars.inParamAddressR.ssLastUpdatedDate = BuiltInFunction.CurrDateTime();
				// AddressR.LastUpdatedBy = GetUserId
				localVars.inParamAddressR.ssLastUpdatedBy = BuiltInFunction.GetUserId();
				// CreateOrUpdateAddress
				ExtendedActions.CreateOrUpdateAddress(heContext, localVars.inParamAddressR.ChangedAttributes, (((RCAddressRecord) localVars.inParamAddressR)), out localVars.resCreateOrUpdateAddress_outParamId);

				// AddressId = CreateOrUpdateAddress.Id
				result.outParamAddressId = localVars.resCreateOrUpdateAddress_outParamId;
			} // try

			finally {
				outParamAddressId = result.outParamAddressId;
			}
		}

		public static class FuncActionAddress_CreateOrUpdate {



		}


	}


}