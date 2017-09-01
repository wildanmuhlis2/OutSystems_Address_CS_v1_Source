/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using OutSystems.RuntimeCommon.Cryptography;
using System;

namespace OutSystems.HubEdition.RuntimePlatform.NewRuntime.Authentication {

    public class Keys {

        // Note: Check Service Center's mobile security configuration screen
        // it also generates keys. Make sure the the algorithm and size match.

        // 20 is HMACSHA1's hash function output lenght
        // https://tools.ietf.org/html/rfc2104#section-3
        static int HMACKeySize { get { return 20 /*bytes*/; } }

        public static string GenerateHMACKey() {
            return PasswordHelper.GenerateStrongPassword(HMACKeySize);
        }

        public static bool TooShortForHMACKey(string candidate) {
            return string.IsNullOrEmpty(candidate) || Convert.FromBase64String(candidate).Length < HMACKeySize;
        }

        // 16 bytes is AES 128 key size
        static int EncryptKeySize { get { return 16 /*bytes*/; } }

        public static string GenerateEncryptKey() {
            return PasswordHelper.GenerateStrongPassword(EncryptKeySize);
        }

    }
}