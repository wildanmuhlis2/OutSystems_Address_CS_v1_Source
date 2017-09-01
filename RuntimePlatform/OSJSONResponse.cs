/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using OutSystems.HubEdition.RuntimePlatform.JSONSharp;
using OutSystems.HubEdition.RuntimePlatform.JSONSharp.Collections;
using OutSystems.HubEdition.RuntimePlatform.JSONSharp.Values;
using OutSystems.RuntimeCommon;

namespace OutSystems.HubEdition.RuntimePlatform {    
    /// <summary>
    /// This class holds and serialize, in JSON format, the AJAX response with the snippets to replace in the page
    /// </summary>
    public class OSJSONResponse
    {
        /// <summary>
        /// Returns the "outers" snippets to replace in the page
        /// </summary>
        public Dictionary<JSONStringValue, JSONValue> Outer {
            get {
                return outer;
            }
        }
        public void AddToOuter(String name, string value) {
            string tagName;
            string innerHTML;
            JSONObjectCollection attributes;
            SplitHTMLChunk(value.Trim(), out tagName, out innerHTML, out attributes);

            JSONObjectCollection splitOuter = new JSONObjectCollection();
            splitOuter.Add(innerValue, new JSONStringValue(innerHTML, true));
            splitOuter.Add(attributesValue, attributes);

            add(outer, new JSONStringValue(name), splitOuter);
        }

        private static Regex tagContentsRE = new Regex("^<[^>]+>", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex tagRE = new Regex("^<([a-zA-Z0-9:_-]+)", RegexOptions.Compiled);
        private static Regex attributeRE = new Regex(" ([a-zA-Z0-9:_-]+) *= *('[^']*'|\"[^\"]*\")", RegexOptions.Compiled);
        private void SplitHTMLChunk(string value, out string tagName, out string innerHTML, out JSONObjectCollection attributes) {
            Debug.Assert(value.StartsWith("<"), "HTML chunk doesn't start with an HTML tag");
            Debug.Assert(value.EndsWith(">"), "HTML chunk doesn't end with an HTML tag");

            string tagContents = tagContentsRE.Match(value).Value;
            tagName = tagRE.Match(tagContents).Groups[1].Value;

            int innerStart = tagContents.Length;
            int lastClosingTag = value.LastIndexOf('<');
            int innerLength = lastClosingTag - innerStart;

            
            // Custom widgets can have initialization scripts appended in the end
            if (tagName.ToLower() != "script" && value.Substring(lastClosingTag).ToLower() == "</script>") {
                var lastScriptStart = value.LastIndexOf("<script", StringComparison.OrdinalIgnoreCase);
                var initScript = value.Substring(lastScriptStart);
                innerHTML = (innerLength > 0 ? value.Substring(innerStart, lastScriptStart - innerStart) : "");
                innerHTML += initScript;
            } else {
                innerHTML = (innerLength > 0 ? value.Substring(innerStart, innerLength) : "");
            }

            MatchCollection matches = attributeRE.Matches(tagContents);
            attributes = new JSONObjectCollection();
            foreach (Match m in matches) {
                string attrName = m.Groups[1].Value;
                string attrValue = m.Groups[2].Value;
                attrValue = attrValue.Substring(1, attrValue.Length - 2); // Remove the single or double quotes
                attributes.Add(new JSONStringValue(attrName), new JSONStringValue(attrValue));
            }
        }

		/// <summary>
		/// Returns the "list" rows to add insert rows in tables
		/// </summary>
        public Dictionary<JSONStringValue, JSONValue> List {
			get {
				return list;
			}		
		}


		public void AddToList(String name, string id, string operation, int rowIndex, bool isTableRecord, string oddLineStyle, string evenLineStyle, bool useBullets) {
			JSONArrayCollection listItem = new JSONArrayCollection();
			listItem.Add(new JSONStringValue(id, true));
			int operationNumber = 0;
			switch (operation) {
				case "Append":
					operationNumber = 0;
					break;
				case "Insert":
					operationNumber = 1;
					break;
				case "Remove":
					operationNumber = 2;
					break;					
				case "Refresh":
					operationNumber = 3;
					break;
				default:
					Debug.Assert(false,"unknown list operation: " + operation);
					break;
			}
			listItem.Add(new JSONNumberValue(operationNumber));
			listItem.Add(new JSONNumberValue(rowIndex));
			listItem.Add(new JSONNumberValue(isTableRecord ? 1 : 0));
			if (isTableRecord) {
				listItem.Add(new JSONStringValue(oddLineStyle));
				listItem.Add(new JSONStringValue(evenLineStyle));
			} else {
				listItem.Add(new JSONNumberValue(useBullets ? 1 : 0));
			}

			add(list, new JSONStringValue(name), listItem);
		}


        /// <summary>
        /// Returns the "javascript" snippets to add to the page
        /// </summary>
		public JSONArrayCollection Js
        {
            get {
                return js;                
            }
        }

		/// <summary>
		/// Adds a javascript snippet to the response
		/// </summary>
		/// <param name="value"></param>
        public void AddToJs(string value) {
			js.Add(new JSONStringValue(value));
        }


		/// <summary>
		/// Inserts a javascript snippet to the response at a given index
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void InsertToJs(int index, string value) {
			js.Insert(index, new JSONStringValue(value));
		}


		/// <summary>
		/// Clears all javscript snippets in this response
		/// </summary>
		public void ClearJs() {
		    
			js.Clear();
		}


		/// <summary>
		/// Adds a filename to the block js filenames to include in the response
		/// </summary>
		/// <param name="jsList">js filename list</param>
        public void AddToBlockJs(IEnumerable<String> jsList) {
			foreach (string js in jsList) {
				blockJs.Add(new JSONStringValue(js));
			}
		}


        /// <summary>
        /// Returns the "hidden" snippets to replace in the page
        /// </summary>
        public Dictionary<JSONStringValue, JSONValue> Hidden
        {
            get {
                return hidden;
            }
        }
        public void AddToHidden(String name, string value) {
            add(hidden, new JSONStringValue(name), new JSONStringValue(value));
        }
      

        /// <summary>
        /// /// Returns a Dictionary of  snippets to replace in the page
        /// This JSON snippets are at the same level of the outers, inners and hidden
        /// </summary>
        public Dictionary<JSONStringValue, JSONValue> OtherValues
        {
            get {
                return otherValues;
            }
        }

        public void AddOtherValues(String name, string value) {
            add(otherValues, new JSONStringValue(name), new JSONStringValue(value));
        }
        public void AddOtherValues(String name, bool value) {
            add(otherValues, new JSONStringValue(name), new JSONBoolValue(value));
        }
        public void AddOtherValues(String name, int value) {
            add(otherValues, new JSONStringValue(name), new JSONNumberValue(value));
        }
        public void AddOtherValues(String name, double value) {
            add(otherValues, new JSONStringValue(name), new JSONNumberValue(value));
        }
        public void AddOtherValues(String name, decimal value) {
            add(otherValues, new JSONStringValue(name), new JSONNumberValue(value));
        }
        public void AddOtherValues(String name, Single value) {
            add(otherValues, new JSONStringValue(name), new JSONNumberValue(value));
        }
        public void AddOtherValues(String name, byte value) {
            add(otherValues, new JSONStringValue(name), new JSONNumberValue(value));
        }


        /// <summary>
        /// Serialize to a string in JSON format the snippets to replace in the page
        /// </summary>
        public override string ToString() {
            JSONObjectCollection ajaxResponse = new JSONObjectCollection();
            if (outer.Count > 0) {
                ajaxResponse.Add(outerValue, CreateCollection(outer));
            }
            if (list.Count > 0) {
                ajaxResponse.Add(listValue, CreateCollection(list));   
            }
            if (hidden.Count > 0) {
                ajaxResponse.Add(hiddenValue, CreateCollection(hidden));   
            }
            if (js.Count > 0) {
                ajaxResponse.Add(jsValue, Js);   
            }
            if (blockJs.Count > 0) {
                ajaxResponse.Add(blockJsValue, blockJs);   
            }
			
            foreach (KeyValuePair<JSONStringValue, JSONValue> kvp in this.otherValues) {
                ajaxResponse.Add(kvp.Key, kvp.Value);
            }

            return ajaxResponse.ToString();
        }

        private void add(Dictionary<JSONStringValue, JSONValue> addTo, JSONStringValue key, JSONValue value) {
            addTo[key] = value;
        }

        private static JSONObjectCollection CreateCollection(Dictionary<JSONStringValue, JSONValue> keyValues) {
            // Why this instead of passing the dictionary directly to the JSONObjectCollection constructor.
            // Because of Java that's why.
            // We translate JSONObjectCollection to json.org java JSONObject.
            // When we upgraded the json.org library generics where added to the constructor that accepted a map.
            // The generics used are incompatible with our maps and the constructor that accepts any object doesn't
            // do what we want. So we ended up adding one entry at a time.
            var c = new JSONObjectCollection();
            foreach (var keyValue in keyValues) {
                c.Add(keyValue.Key, keyValue.Value);
            }
            return c;
        }

        Dictionary<JSONStringValue, JSONValue> outer = new Dictionary<JSONStringValue, JSONValue>();
		Dictionary<JSONStringValue, JSONValue> list = new Dictionary<JSONStringValue, JSONValue>();
        Dictionary<JSONStringValue, JSONValue> hidden = new Dictionary<JSONStringValue, JSONValue>();
        Dictionary<JSONStringValue, JSONValue> otherValues = new Dictionary<JSONStringValue, JSONValue>();
		JSONArrayCollection js = new JSONArrayCollection();
		JSONArrayCollection blockJs = new JSONArrayCollection();
		

        static JSONStringValue outerValue = new JSONStringValue("outers");
        static JSONStringValue innerValue = new JSONStringValue("inner");
        static JSONStringValue attributesValue = new JSONStringValue("attributes");
		static JSONStringValue listValue = new JSONStringValue("list");
        static JSONStringValue hiddenValue = new JSONStringValue("hidden");
		static JSONStringValue jsValue = new JSONStringValue("js");
		static JSONStringValue blockJsValue = new JSONStringValue("blockJs");

    }
}