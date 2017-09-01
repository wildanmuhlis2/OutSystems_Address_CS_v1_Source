/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/


namespace OutSystems.HubEdition.RuntimePlatform {

    public static class RegularExpressionUtils {

        public static int CountUnescapedParentheses(string regex) {

            if (regex == null) {
                return 0;
            }

            int i = 0;
            int maxPos = regex.Length;
            int count = 0;
            while (i < maxPos) {
                switch(regex[i]) {
                    case '\\':                
                        i++;
                        break;
                    case '(':
                        count++;
                        break;
                    default:
                        break;
                }
                i++;
            }
            return count;
        }
    }
}