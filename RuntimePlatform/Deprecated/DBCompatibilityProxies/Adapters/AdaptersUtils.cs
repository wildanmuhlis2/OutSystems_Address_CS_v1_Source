/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using System;
using OutSystems.HubEdition.Extensibility.Data;
using OutSystems.HubEdition.Extensibility.Data.ConfigurationService;
using OutSystems.HubEdition.Extensibility.Data.ConfigurationService.MetaConfiguration;
using OutSystems.HubEdition.Extensibility.Data.Platform;
using OutSystems.Internal.Db;
using OutSystems.RuntimeCommon;
using System.IO;

namespace OutSystems.HubEdition.RuntimePlatform.DBCompatibilityProxies.Adapters {
    [Obsolete("This class is only used to implement compatibility proxies for the old database access API.")]
    internal static class AdaptersUtils {
        internal static DBSqlServerVersion GetSqlServerVersion(IDatabaseServices databaseServices) {
#if JAVA
            return DBSqlServerVersion.Unknown;
#else
            return DBCompatibilityReflectionUtils.GetSqlServerVersion(databaseServices.ExecutionService);
#endif
        }

        internal static DBAuthenticationType GetAuthenticationType(IDatabaseServices databaseServices) {
#if JAVA
            return DBAuthenticationType.DBAuth;
#else
            return DBCompatibilityReflectionUtils.GetIsIntegratedSecurityOnly(databaseServices.ExecutionService)
                ? DBAuthenticationType.IntegratedAuth
                : DBAuthenticationType.DBAuth;

#endif

        }

        internal static DBKind GetDBKind(IDatabaseServices databaseServices) {
            return (DBKind) Enum.Parse(typeof(DBKind), databaseServices.DatabaseConfiguration.ProviderKey().Serialize());
        }

        internal static IRuntimeDatabaseConfiguration CreateDatabaseConfiguration(IPlatformDatabaseProvider databaseProvider, string connectionString, 
                string onsClusterConfig, string connectionStringExtras, string schema) {

            return CreateDatabaseConfiguration(databaseProvider, connectionString, onsClusterConfig, connectionStringExtras, schema, false);
        }

        internal static IRuntimeDatabaseConfiguration CreateDatabaseConfiguration(IPlatformDatabaseProvider databaseProvider, string connectionString,
                string onsClusterConfig, string connectionStringExtras, string schema, bool forConnectionState) {

            var config = databaseProvider.CreateEmptyRuntimeDatabaseConfiguration();

            
            connectionStringExtras = connectionStringExtras ?? "";
            var fullConnectionString = RuntimePlatformUtils.connectionStringJoin(connectionString, connectionStringExtras);
            var databaseIdentifier = schema;
            var databaseUnicodeSupport = Settings.GetBool(Settings.Configs.Compiler_DatabaseUnicodeSupport);

            var wrapper = new MetaDatabaseConfiguration(config);
            wrapper.GetParameter("ConnectionString").Set(fullConnectionString);
            var sch = wrapper.GetParameter("Schema");
            if (sch != null) {
                sch.Set(databaseIdentifier);
            }
          
            var unicode = wrapper.GetParameter("DatabaseUnicodeSupport");
            if (unicode != null) {
                unicode.Set(databaseUnicodeSupport + "");
            }

            return config;
        }

        private static DirectoryInfo GetDatabasePluginsPath() {
            return new DirectoryInfo(Path.Combine(Settings.Get(Settings.Configs.PluginDirectory), "database"));
        }

        internal static IPlatformDatabaseServices GetDatabaseServices(DBKind dbKind, ConnectionString connectionString, string schema) {
            var pluginProvider = new DatabasePluginProvider<IPlatformDatabaseProvider>(GetDatabasePluginsPath());
            var provider = pluginProvider.GetImplementation(DatabaseProviderKey.Deserialize(dbKind.ToString()));

            var databaseConfiguration = CreateDatabaseConfiguration(provider, connectionString.GetConnectionString(),
                connectionString.GetOnsConfig(), String.Empty, schema);

            return provider.GetPlatformDatabaseServices(databaseConfiguration);
        }

        internal static IPlatformDatabaseServices GetDatabaseServices(DBKind dbKind, ConnectionString connectionString) {
            return GetDatabaseServices(dbKind, connectionString, null);
        }
        
        internal static ConnectionString GetConnectionString(IDatabaseServices databaseServices) {
            var databaseConfiguration = databaseServices.TransactionService.DatabaseServices.DatabaseConfiguration;
            return DBCompatibilityReflectionUtils.GetConnectionString(databaseConfiguration);
        }
    }
}
