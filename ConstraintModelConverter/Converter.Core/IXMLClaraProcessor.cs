using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Converter.Core
{
    public interface IXMLClaraProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        StringBuilder ConvertXMlModelToClaraModel(XDocument xmlDocument);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        XDocument ConvertClaraSolutionToXMLModel(string claraSolutionModel);
    }
}
