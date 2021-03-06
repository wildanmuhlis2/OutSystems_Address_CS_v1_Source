/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using System;
using OutSystems.HubEdition.Extensibility.Data;
using OutSystems.HubEdition.Extensibility.Data.TransactionService;
using OutSystems.HubEdition.RuntimePlatform.DBCompatibilityProxies.Adapters;
using OutSystems.RuntimeCommon;

namespace OutSystems.HubEdition.RuntimePlatform {
    [Obsolete("Use OutSystems.RuntimePublic.Db.DatabaseAccess class to access a database and its services")]
    public class SqlServerConnectionManager : DBConnectionManager {
        public SqlServerConnectionManager(DBKind dbKind, ConnectionString connectionString, string schema, int retries)
            : base(AdaptersUtils.GetDatabaseServices(dbKind, connectionString, schema).TransactionService.CreateTransactionManager()) {}
    }
}
