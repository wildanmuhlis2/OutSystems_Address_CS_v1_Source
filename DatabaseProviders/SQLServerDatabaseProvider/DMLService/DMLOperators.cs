/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using OutSystems.HubEdition.Extensibility.Data.DMLService;

namespace OutSystems.HubEdition.DatabaseProvider.SqlServer.DMLService {
    internal class DMLOperators: BaseDMLOperators {
        
        internal DMLOperators(IDMLService dmlService): base(dmlService) {}

        public override string Concatenate(string p1, string p2) {
            return p1 + " + " + p2;
        }
    }
}