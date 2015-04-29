using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter.Core
{
    public static class Constants
    {
        //String Constants
        public const string Tab = "\t";
        public const string Comma = ",";
        public const string SemiColon = ";";
        public const string Colon = ":";
        public const string Space = " ";
        //Clara Keywords

        //Data-Types
        public const string Integer = "integer";
        public const string String = "string";
        public const string Real = "string";
        public const string Boolean = "bool";

        //Character Constants
        public const char CharComma = ',';
    }

    public struct XMLTags
    {
        public const string Model = "model";
        public const string Variable = "variable";
        public const string Set = "set";
        public const string Seq = "seq";
        public const string Function = "function";
        public const string Literal = "literal";
        public const string Lower = "lower";
        public const string Upper = "upper";
    }

    public struct XMLAttributes
    {
        public const string Name = "name";
        public const string Mode = "mode";
        public const string DataType = "datatype";
        public const string Type = "type";
        public const string Value = "value";

    }

    public struct XMLAttributeValues
    {
        public const string StringDataType = "string";
        public const string IntegerDataType = "integer";
        public const string UnaryMode = "unary";
        public const string MapType = "map";
        //iterators
        public const string ForAllIterator = "forall";
    }
}
