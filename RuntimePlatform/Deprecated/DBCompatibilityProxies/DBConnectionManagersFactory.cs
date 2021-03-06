/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using System;
using OutSystems.HubEdition.RuntimePlatform.Db;
using OutSystems.Internal.Db;
using OutSystems.RuntimeCommon;

namespace OutSystems.HubEdition.RuntimePlatform {
    [Obsolete("No longer used by new APIs")]
    public class DBConnectionNotFoundException : Exception {
        public DBConnectionNotFoundException(string message) : base(message) { }
    }

    [Obsolete("No longer used by new APIs")]
    public class DBUnknownDatabaseException : Exception {
        public DBUnknownDatabaseException(string message) : base(message) { }
    }

    [Obsolete("Use OutSystems.RuntimePublic.Db.DatabaseAccess class to access a database and its services. The ForRunningApplication method will give you access to the running application database")]
    public static class DBConnectionManagersFactory {
        public static string GetCatalogName(int eSpaceId) {
            try {
                var catalogName = AppCache.GetESpaceCachedValue<string>("DBCatalogCache" + eSpaceId,
                                                                                     eSpaceId,
                                                                                     LoadCatalogName);

                return catalogName.IsNullOrEmpty()
                    ? DatabaseAccess.ForSystemDatabase.DatabaseServices.DatabaseConfiguration.DatabaseIdentifier
                    : catalogName;
            } catch (InvalidDatabaseAccessException e) {
                throw new InvalidDBTransactionManagerAccess(e.Message);
            }
        }

        private static string LoadCatalogName(int eSpaceId) {
            using (var trans = DatabaseAccess.ForSystemDatabase.GetCommitableTransaction()) {
                return DBRuntimePlatform.Instance.GetCatalogName(trans, eSpaceId);
            }
        }

    }
}
