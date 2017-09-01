/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using System;
using System.Data;
using System.Text.RegularExpressions;
using OutSystems.HubEdition.DatabaseProvider.SqlServer.ConfigurationService;
using OutSystems.HubEdition.DatabaseProvider.SqlServer.DMLService;
using OutSystems.HubEdition.Extensibility.Data;
using OutSystems.HubEdition.Extensibility.Data.DatabaseObjects;
using OutSystems.RuntimeCommon;

namespace OutSystems.HubEdition.DatabaseProvider.SqlServer.DatabaseObjects {
    public class DatabaseObjectFactory : IDatabaseObjectFactory {

        // This regex captures valid identifiers (id) from any of these places: "id".[id].id
        private const string captureIdentifiers = @"(?:""([^""]*)"")|(?:\[([^\]]*)\])|([^\.\[""\s/-]+)";
        private static readonly Regex regexCaptureIdentifiers = new Regex(captureIdentifiers, RegexOptions.Compiled);

        private const string validateCompoundIdentifiers = "(" + captureIdentifiers + @")(\.(" + captureIdentifiers + "))";
        private static readonly Regex regexValidateCompoundIdentifiers = new Regex("^(" + validateCompoundIdentifiers + "{0,3})$", RegexOptions.Compiled);

        protected static readonly char[] trimChars = "[]\"".ToCharArray();

        protected readonly IDatabaseServices databaseServices;

        public DatabaseObjectFactory(IDatabaseServices databaseServices) {
            this.databaseServices = databaseServices;
        }

        public IDatabaseInfo CreateDatabaseInfo(string databaseIdentifier) {
            string linkedServer;
            string catalog;
            if (ParseDatabaseIdentifier(databaseIdentifier, out linkedServer, out catalog)) {
                return new DatabaseInfo(databaseServices, catalog, linkedServer);                
            }
            throw new InvalidDatabaseObjectIdentifierException("'" + databaseIdentifier + "' is not a valid database identifier.");
        }

        public ITableSourceInfo CreateTableSourceInfo(IDatabaseInfo database, string qualifiedName) {
            return CreateTableSourceInfo(qualifiedName);
        }

        public virtual ITableSourceInfo CreateTableSourceInfo(string qualifiedName) {
            string linkedServer;
            string catalog;
            string schema;
            string table;
            bool isDatabaseReachable;
            if (ParseQualifiedTableName(qualifiedName, out linkedServer, out catalog, out schema, out table, out isDatabaseReachable)) {
                var tableSourceInfo = new TableSourceInfo(databaseServices, new DatabaseInfo(databaseServices, catalog, linkedServer), table, schema, qualifiedName);
                if (!isDatabaseReachable) {
                    tableSourceInfo.ValidationWarning = "It was not possible to connect to the database to validate the qualified table name '{0}'. Runtime errors may occur.".F(qualifiedName);
                }
                return tableSourceInfo;
            }
            throw new InvalidDatabaseObjectIdentifierException("'" + qualifiedName + "' is not a valid qualified table name.");
        }

        private bool ParseDatabaseIdentifier(string databaseIdentifier, out string linkedServer, out string catalog) {
            linkedServer = "";
            catalog = "";
            MatchCollection databaseIdentiferParts = regexCaptureIdentifiers.Matches(databaseIdentifier);
            if (databaseIdentiferParts.Count == 0 || databaseIdentiferParts.Count > 2) {
                return false;
            }

            for (int i = databaseIdentiferParts.Count - 1; i >= 0; i--) {
                string part = databaseIdentiferParts[i].Value.Trim(trimChars);
                if (string.IsNullOrEmpty(catalog)) {
                    catalog = part;
                } else if (string.IsNullOrEmpty(linkedServer)) {
                    linkedServer = part;
                }
            }
            return true;
        }

        private bool ParseQualifiedTableName(string qualifiedName, out string linkedServer, out string catalog, out string schema, out string table, out bool isDatabaseReachable) {
            linkedServer = "";
            catalog = "";
            schema = "";
            table = "";
            isDatabaseReachable = true;

            if (!regexValidateCompoundIdentifiers.Match(qualifiedName).Success) {
                return false;
            }

            MatchCollection tableNameParts = regexCaptureIdentifiers.Matches(qualifiedName);
            if (tableNameParts.Count == 0 || tableNameParts.Count > 4) {
                return false;
            }
            string serverToTest = tableNameParts.Count == 4 ? tableNameParts[0].Value : "";
            
            // #840789 - When publishing a reference to an extension, it might not be possible to connect to the database
            string testConnectionMessage;
            isDatabaseReachable = databaseServices.TransactionService.TestConnection(out testConnectionMessage);

            // We may be introspecting a view that has a definition based on a link
            if (serverToTest.Length == 0 && isDatabaseReachable) {
                string currentCatalog = ((RuntimeDatabaseConfiguration)databaseServices.DatabaseConfiguration).Catalog;
                string matchTableName = tableNameParts[tableNameParts.Count - 1].Value.Trim(trimChars);
                string sql = @"SELECT VIEW_DEFINITION
                           FROM {0}.INFORMATION_SCHEMA.VIEWS
                           WHERE TABLE_NAME = @tablename".F(DMLIdentifiers.EscapeIdentifierInner(currentCatalog));
 
                using (IDbConnection conn = databaseServices.TransactionService.CreateConnection()) {
                    using (IDbCommand cmd = databaseServices.ExecutionService.CreateCommand(conn, sql)) {
                        databaseServices.ExecutionService.CreateParameter(cmd, "@tablename", DbType.String, matchTableName);
                        using (IDataReader reader = cmd.ExecuteReader()) {
                            while (reader.Read()) {
                                object oviewDefinition = reader["VIEW_DEFINITION"];
                                if (oviewDefinition == DBNull.Value)
                                    continue;
                                string viewDefinition = (string)oviewDefinition;
                                // This regex captures four part identifiers
                                string regex_part1 = @"([\[""][^""\]\[]+?[\]""]|(?<![""\[][^\s]*)[^\.\[""\s/-]+)(?![""\]])";
                                string regex_part234 = @"([\[""][^""\]\[]+?[\]""]|(?<![""\[])[^\.\[""\s/-]+)(?![""\]])";
                                Regex regexCaptureFourPartIdentifiers = new Regex(regex_part1 + @"(\." + regex_part234 + "){3}", RegexOptions.Compiled);
                                Match viewParts = regexCaptureFourPartIdentifiers.Match(viewDefinition);
                                if (viewParts.Success) {
                                    tableNameParts = regexCaptureIdentifiers.Matches(viewParts.Value);
                                    break;
                                }
                            }
                        }
                    }
                }
                
            }
            
            for (int i = tableNameParts.Count - 1; i >= 0; i--) {
                string part = tableNameParts[i].Value.Trim(trimChars);
                if (string.IsNullOrEmpty(table)) {
                    table = part;
                } else if (string.IsNullOrEmpty(schema)) {
                    schema = part;
                } else if (string.IsNullOrEmpty(catalog)) {
                    catalog = part;
                } else if (string.IsNullOrEmpty(linkedServer)) {
                    linkedServer = part;
                } 
            }

            return true;
        }

        public IDatabaseInfo CreateLocalDatabaseInfo() {
            return new DatabaseInfo(databaseServices, ((RuntimeDatabaseConfiguration) databaseServices.DatabaseConfiguration).Catalog);
        }
    }
}