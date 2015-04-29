using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Converter.Core
{
    public class XMLClaraKeywordMapper
    {
        private static volatile XMLClaraKeywordMapper _instance;
        private static object _syncRoot = new Object();

        public Dictionary<string, string> Map { private set; get; }
        public Dictionary<string, string> DataTypeMap { private set; get; }

        private XMLClaraKeywordMapper() 
        {
            //Initialization Code for creating one on one Mapping between XML Keyword and Clara Keyword
            Map = new Dictionary<string, string>();
            this.DataTypeMap = new Dictionary<string, string>();
            //Create function Map:
            InitializeFunctionMap();
        }

        private void InitializeFunctionMap()
        {
            //Logical functions
            this.Map.Add("equal", "=");
            this.Map.Add("equality", "==");
            this.Map.Add("not", "!");
            this.Map.Add("notequal", "!=");
            this.Map.Add("and", "&");
            this.Map.Add("or", "|");
            this.Map.Add("exclusiveor", "^");
            this.Map.Add("conditional", "->");
            this.Map.Add("equivalence", "<->");

            //Relational Function
            this.Map.Add("lessthan", "<");
            this.Map.Add("greaterthan", ">");
            this.Map.Add("lessthanequal", "<=");
            this.Map.Add("greaterthanequal", ">=");

            //Mathematical Functions
            this.Map.Add("sum", "+");
            this.Map.Add("difference", "-");
            this.Map.Add("product", "*");
            this.Map.Add("modulo", "%");
            this.Map.Add("division", "/");

            //set and seq
            this.Map.Add("set", "subset");
            this.Map.Add("seq", "subset");

            //DataType Mappings
            this.DataTypeMap.Add("bool", "Boolean");
            //this.DataTypeMap.Add("bool", "Boolean");
            //this.DataTypeMap.Add("bool", "Boolean");
        }

        /// <summary>
        /// 
        /// </summary>
        public static XMLClaraKeywordMapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new XMLClaraKeywordMapper();
                    }
                }
                return _instance;
            }
        }
    }
}
