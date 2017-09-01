/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using OutSystems.HubEdition.Extensibility.Data.IntrospectionService;
using OutSystems.HubEdition.Extensibility.Data.Platform.DMLService;
using OutSystems.RuntimeCommon;

namespace OutSystems.HubEdition.DatabaseProvider.MySQL.Platform.DMLService {
    internal class MySQLPlatformDMLProgrammaticSQL: BasePlatformDMLProgrammaticSQL {

        private Dictionary<long, string> blockIdentifier = new Dictionary<long,string>();


        public MySQLPlatformDMLProgrammaticSQL(IPlatformDMLService dmlService) : base(dmlService) {
        }

        public override string IfElseStatement(string condition, string ifBody, string elseBody) {
            return elseBody.IsNullOrEmpty()? String.Format("IF {0} THEN {1} END IF;", condition, ifBody): 
                String.Format("IF {0} THEN {1} ELSE {2} END IF;", condition, ifBody, elseBody);
        }

        
        public override string GetVariableValue(string name) {
            return name;
        }

        public override string SetVariable(string varName, string value) {
            return string.Format("set {0} = {1};", varName, value);
        }

        public override IDictionary<QueryPlaceholder, string> SetVariableFromQuery(string varName) {
            IDictionary<QueryPlaceholder,string> placeholders = new Dictionary<QueryPlaceholder, string>();
            placeholders.Add(QueryPlaceholder.BeforeFromKeyword, " INTO " + varName + " ");
            placeholders.Add(QueryPlaceholder.AfterStatement, ";");
            return placeholders;
        }

        public override string SetVariableFromLastInsertedId(string varName) {
            return string.Format("set {0} = (SELECT LAST_INSERT_ID());", varName);
        }

        public override string BeginProgrammaticSQLBlock(params VariableDetails[] variables) {
            string sql = "";
            string procName = "ddl_" + DateTime.UtcNow.Ticks.ToString();
            blockIdentifier[Thread.CurrentThread.ManagedThreadId] = procName;

            sql = string.Format(@"DROP PROCEDURE IF EXISTS {0};
                     CREATE PROCEDURE {0} ()
                     BEGIN 
                     ", DMLService.Identifiers.EscapeIdentifier(procName));

            return sql + variables.Select(variable => String.Format("DECLARE {0} {1};", variable.Name, variable.Type.SqlDataType)).StrCat(Environment.NewLine);
        }

        public override string EndProgrammaticSQLBlock() {
            string procName = blockIdentifier[Thread.CurrentThread.ManagedThreadId];

            if (string.IsNullOrEmpty(procName)) throw new Exception("Trying to close a programmatic block that wasn't opened.");

            return string.Format(@"END;
                    CALL {0} ();
                    DROP PROCEDURE IF EXISTS {0};", DMLService.Identifiers.EscapeIdentifier(procName)); 
        }
  }
}