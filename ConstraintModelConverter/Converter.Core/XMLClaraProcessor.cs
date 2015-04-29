using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Converter.Core
{
    public class XMLClaraProcessor: IXMLClaraProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        public StringBuilder ConvertXMlModelToClaraModel(XDocument xmlDocument)
        {
            StringBuilder output = null;
            try
            {
                if (xmlDocument != null)
                {
                    output = new StringBuilder();
                    
                    //create Model Spec with Name: model [name]
                    StringBuilder modelBuilder = new StringBuilder();
                    IEnumerable<XElement> modelElements = xmlDocument.Root.Elements(XMLTags.Model);
                    if (modelElements != null && modelElements.Count() > 0)
                    {
                        modelElements.ForEach(elem => this.ExpandModelElement(elem, modelBuilder));
                    }

                    //output.AppendLine("model " + rootElement.GetAttributeValue("name"));                    
                    //process Variables Tag
                    //IEnumerable<XElement> variablesElements = xmlDocument.Elements("variables");

                    //Process set elements:
                    StringBuilder setSeqBuilder = new StringBuilder();

                    IEnumerable<XElement> setElements = xmlDocument.Root.Elements("set");
                    if (setElements != null && setElements.Count() > 0)
                        setElements.ForEach(set => this.ExpandSubSets(set, setSeqBuilder));

                    //Process seq elements
                    
                    IEnumerable<XElement> seqElements = xmlDocument.Root.Elements("seq");
                    if (seqElements != null && seqElements.Count() > 0)
                        seqElements.ForEach(seq => this.ExpandSubSets(seq, setSeqBuilder));

                    //Process Series

                    //process Constraints ChildElement:                    
                    StringBuilder constraintBuilder = new StringBuilder();
                    IEnumerable<XElement> xmlConstraints = xmlDocument.Root.Elements("constraints"); //get XElement with constraints
                    if (xmlConstraints != null && xmlConstraints.Count() > 0)
                    {                        
                        foreach (var constraints in xmlConstraints)
                        {
                            constraintBuilder.AppendLine("constraints" + Constants.Space + constraints.GetAttributeValue("name"));
                            ExpandConstraints(constraints, constraintBuilder);
                        }
                    }

                    output.AppendLine(modelBuilder.ToString());
                    output.AppendLine(setSeqBuilder.ToString());
                    output.AppendLine(constraintBuilder.ToString());
                }
            }
            catch (XmlException xmlEx)
            {
                //Logging for XML Exception  
              
            }
            catch (Exception ex)
            {
                //Logging for Exception
                throw ex;
            }

            return output;
        }

        public XDocument ConvertClaraSolutionToXMLModel(string claraSolutionModel)
        {
            throw new NotImplementedException();
        }

        private void ExpandConstraints(XElement xElement, StringBuilder constraintBuilder)
        {
            //This method assumes that the first line of "constraints [name]" has already been appended to the StringBuilder

            //Constraints can be of various forms:
            //forall
            //exists
            //purely relational functions
            //purely logical functions
            if (xElement != null && xElement.HasElements)
            {
                IEnumerable<XElement> childConstraints = xElement.Elements();
                foreach (XElement constraint in childConstraints)
                {
                    string elementType = constraint.Name.LocalName;

                    switch (elementType)
                    {
                        case "iterator":    //case for iterators
                            //append iterator type

                            //append iterator arguments

                            //append iterator expression
                            break;
                        case "exists":      //case for exists
                            break;
                        case "":
                            break;
                        default:    //case for else
                            constraintBuilder.AppendLine(Constants.Tab + ExpandFunctionRecursive(constraint));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xElement"></param>
        /// <param name="setBuilder"></param>
        /// <returns></returns>
        private void ExpandSubSets(XElement xElement, StringBuilder setBuilder)
        {
            string subSetExpression 
                = string.Concat(Clara.Subset ,Constants.Space ,xElement.GetAttributeValue(XMLAttributes.Name) ,Constants.Comma, Constants.Space );
            string tagName = xElement.Name.LocalName;

            switch (tagName)
            {
                case XMLTags.Set:
                    List<string> internalList = new List<string>();
                    IEnumerable<XElement> literalElems = xElement.Elements(XMLTags.Literal);
                    var elementList = literalElems.Select(s => s.Value);
                    if (xElement.IsAttributeValue(XMLAttributes.DataType,XMLAttributeValues.StringDataType))
                    {
                        elementList.ForEach(value =>
                        {
                            internalList.Add("\"" + value + "\"");
                        });
                    }
                    else if (xElement.IsAttributeValue(XMLAttributes.DataType, XMLAttributeValues.IntegerDataType))
                    {
                        internalList.AddRange(elementList);
                    }

                    subSetExpression
                        = subSetExpression + "[" + string.Join(",", internalList) + "]";
                    break;
                case XMLTags.Seq:
                    //<!--subset Order,  1..5-->
                    //<seq name="Order" datatype="int">
                    //  <lower>1</lower>
                    //  <upper>5</upper>
                    //</seq>
                    if (xElement.HasElements)
                    {                        
                        //get lowerValue
                        string lower = xElement.Element(XMLTags.Lower).Value;
                        //get upperValue
                        string upper = xElement.Element(XMLTags.Upper).Value;
                        //get delta Value
                        subSetExpression = subSetExpression + lower + ".." + upper;
                    }
                    else
                    {
                        subSetExpression = subSetExpression + xElement.GetAttributeValue(XMLAttributes.Value);
                    }
                    break;
                default:
                    break;
            }

            setBuilder.AppendLine(subSetExpression);
        }

        //private string ExpandSequence(XElement xElement, StringBuilder seqBuilder)
        //{
 
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xElement"></param>
        /// <returns></returns>
        private string ExpandFunctionRecursive(XElement xElement)
        {
            if (xElement.HasElements == false)
            {
                //No Child Elements(element is a Variable)
                if (xElement.Name.ToString().IsEqual(XMLTags.Variable))   //element is a variable
                {
                    if (xElement.Attribute("name") != null)
                    {
                        return xElement.Attribute("name").Value;
                    }
                    else
                    {
                        return xElement.DescendantNodes().FirstOrDefault().ToString();
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                IEnumerable<XElement> childElements = xElement.Elements();
                int currentIndex = 0;
                int childCount = childElements.Count();

                string expansion = "(";
                foreach (var element in childElements)
                {
                    //check if child element is a unary function
                    if (xElement.GetAttributeValue("mode") == "unary")
                    {
                        expansion = expansion + Constants.Space + this.GetClaraFunctionType(xElement.GetAttributeValue("type")) + "(" + ExpandFunctionRecursive(element) + ")";
                    }
                    else
                    {
                        if (currentIndex == (childCount - 1))
                            expansion = expansion + " " + ExpandFunctionRecursive(element);
                        else
                            expansion = expansion + " " + ExpandFunctionRecursive(element) + " " + this.GetClaraFunctionType(xElement.GetAttributeValue("type"));
                    }                    
                    currentIndex++;
                }
                return expansion + ")";
            }
        }

        private void ParseSetTag(XElement xElement)
        {

        }

        /// <summary>
        /// This method will parse a single variable-xml to variable in the form: [type]: [var-name]
        /// </summary>
        /// <param name="variableElement"></param>
        /// <returns></returns>
        private string ParseVariableTag(XElement variableElement)
        {
            string claraVariable = string.Empty;
            //variable tag can be of the form: <variable name="" type=""/> (SCALAR)
            if (variableElement.HasElements == false)
            {
                //<variable name="someValue" type="someType" />
                claraVariable 
                    = string.Concat(Constants.Tab, this.GetClaraDataType(variableElement.GetAttributeValue("type")), Constants.Colon, Constants.Space, variableElement.GetAttributeValue("name"));
            }
            else
            {

            }
            return claraVariable;
        }

        private string ParseFunctionVariableTag(XElement funcElement)
        {
            string funcVariable = string.Empty;
            if (funcElement.GetAttributeValue(XMLAttributes.Type) == XMLAttributeValues.MapType)
            {
                //Map(Person,Apartment): apt
                //<function name="apt" type="map" mode="unary">
                //  <variable name="Person"/>
                //  <variable name="Apartment"/>
                //</function>
                funcVariable = Constants.Tab + Clara.Map + "(";
                IEnumerable<XElement> innerVariables = funcElement.Elements(XMLTags.Variable);

                var nameList = innerVariables.Select(s => s.GetAttributeValue(XMLAttributes.Name)).ToList<string>();
                funcVariable = funcVariable + string.Join(Constants.Comma, nameList);
                funcVariable = string.Concat(funcVariable, ")", Constants.Colon, Constants.Space, funcElement.GetAttributeValue("name"));
            }
            return funcVariable;
        }

        private void ExpandModelElement(XElement xElement, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("model" + Constants.Space + xElement.GetAttributeValue("name"));
            
            //parse scalar variable Tags
            IEnumerable<XElement> variables = xElement.Element("variables").Elements(XMLTags.Variable);
            if (variables != null && variables.Count() > 0)
                variables.ForEach(x => stringBuilder.AppendLine(this.ParseVariableTag(x)));
            
            //parse variables of type function
            IEnumerable<XElement> funcVariables = xElement.Element("variables").Elements(XMLTags.Function);
            if (funcVariables != null && funcVariables.Count() > 0)
                funcVariables.ForEach(x => stringBuilder.AppendLine(this.ParseFunctionVariableTag(x)));
            //IEnumerable<XElement> childModelElements = xElement.Elements("model");
            //if (childModelElements != null && childModelElements.Count() > 0)
            //{
            //    foreach (XElement model in childModelElements)
            //    {
            //        stringBuilder.AppendLine("model" + Constants.Space + model.GetAttributeValue("name"));

            //        //parse variable Tags
            //        IEnumerable<XElement> variables = model.Descendants("variable");
            //        variables.ForEach(x => stringBuilder.AppendLine(this.ParseVariableTag(x)));
            //        //variables.ForEach(stringBuilder.AppendLine(x => this.ParseVariableTag(x)));
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetClaraDataType(string key)
        {
            string value = string.Empty;
            if (XMLClaraKeywordMapper.Instance.DataTypeMap.TryGetValue(key,out value))
            {
                return value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetClaraFunctionType(string key)
        {
            string value = string.Empty;
            if (XMLClaraKeywordMapper.Instance.Map.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return key;
            }
        }
    }
}
