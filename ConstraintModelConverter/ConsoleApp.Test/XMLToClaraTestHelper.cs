using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp.Test
{
    public static class XMLToClaraTestHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static XElement CreateFunctionTag()
        {
            return XElement.Parse(
            @"<function type='and'>
		        <function type='notequal'>
			        <variable>t3</variable>
			        <variable>t2</variable>
    	        </function>
    	        <function type='notequal'>
      		        <variable>t3</variable>
      		        <variable>t5</variable>
    	        </function>
  	        </function>");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public static XElement CreateFunctionTag()
        //{
 
        //}
    }
}
