using Converter.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    //read xml data from file
                    Console.WriteLine("Select the following XML Files to Convert, or type Exit to close application:");
                    Console.WriteLine("1: Party Puzzle");
                    Console.WriteLine("2: Pizza Delivery");
                    Console.WriteLine("0: Exit");
                    //Console.WriteLine("Enter file path to translate XML-Model to Clara-Model, type exit to close application: ");
                    string selection = Console.ReadLine();

                    string filePath = string.Empty;

                    switch (selection.ToLower())
                    {
                        case "1":
                            filePath = "PartyPuzzle.xml";
                            break;
                        case "2":
                            filePath = "PizzaDelivery.xml";
                            break;
                        case "exit":
                        case "0":
                            return;
                        default:
                            break;
                    }
                    
                    if (string.IsNullOrWhiteSpace(filePath) == false)
                    {
                        if (filePath.ToLower().Equals("exit"))
                        {
                            break;
                        }
                        else
                        {
                            IXMLClaraProcessor _processor = new XMLClaraProcessor();
                            XDocument xDocument = XDocument.Load(filePath);
                            //invoke processor for translation
                            StringBuilder claraOutput = _processor.ConvertXMlModelToClaraModel(xDocument);
                            
                            //write clara output to file
                            File.WriteAllText(@"C:\Sumit\Clara\ClaraFormat.clara", claraOutput.ToString());

                            File.WriteAllText(@"C:\Sumit\Clara\filename_write", "load batch_file");

                            //write search to Clara file
                            //File.WriteAllText(@"C:\Sumit\Clara\filename_write", "search Delivery");

                            Thread.Sleep(4000);

                            File.WriteAllText(@"C:\Sumit\Clara\filename_write", "solution");

                            //display translated Clara specification in console    
                            Console.WriteLine(claraOutput);
                            Console.WriteLine();
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occured:");
                Console.WriteLine(String.Format("Error Message: {0}", ex.Message));
                Console.ReadLine();
            }
        }
    }
}
