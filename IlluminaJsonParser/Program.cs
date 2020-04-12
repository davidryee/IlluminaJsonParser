using System;
using System.IO;
using System.Collections;
using System.Threading;
using Newtonsoft.Json;
using Domain;

namespace IlluminaJsonParser
{
    public class IlluminaJsonParser
    {
        static Queue dataToParse = new Queue();
        static UrlDataParser parser = new UrlDataParser();
        static string outputFilePath;
        static string inputFilePath;
        static bool dataLeftInFile = true;

        static void Main(string[] args)
        {

            Console.WriteLine("Please enter the path to the file you wish to use as input. File must be in valid json format.");
            inputFilePath = Console.ReadLine().Trim();

            Console.WriteLine("Please enter a file path to use for the output.");
            outputFilePath = Console.ReadLine().Trim();
            
            ThreadStart addToQueueThreadStart = new ThreadStart(ReadFromFile);
            Thread addToQueueThread = new Thread(addToQueueThreadStart);
            
            ThreadStart removeFromQueueThreadStart = new ThreadStart(WriteToFile);
            Thread removeFromQueueThread = new Thread(removeFromQueueThreadStart);
            
            addToQueueThread.Start();
            removeFromQueueThread.Start();
        }

        private static void WriteToFile()
        {
            using(StreamWriter streamWriter = new StreamWriter(outputFilePath))
            {
                streamWriter.WriteLine("{");
                
                while (dataLeftInFile || dataToParse.Count > 0)
                {
                    if (dataToParse.Count > 0)
                    {
                        UrlData urlInfo = (UrlData)(dataToParse.Dequeue());
                        UrlOutputData outputData = null;
                        try
                        {
                            outputData = parser.Parse(urlInfo);
                        }
                        catch (UriFormatException ex)
                        {
                            Console.WriteLine("Error! " + ex.Message);
                            Environment.Exit(1);
                        }

                        string jsonOutputData = JsonConvert.SerializeObject(outputData.Path);

                        //remove the first and last brackets from the json object { myobject }
                        streamWriter.Write(jsonOutputData.Substring(1, jsonOutputData.Length - 2));

                        //only write the comma if there is data left to parse, either remaining in the file or on the queue
                        //i.e. don't write the comma if this is the last element
                        if (dataLeftInFile || dataToParse.Count > 0)
                        {
                            streamWriter.WriteLine(",");
                        }
                    }
                }
                
                streamWriter.WriteLine("}");
            }

            Console.WriteLine($"Success! All data has been written to {outputFilePath}");
        }

        private static void ReadFromFile()
        {
            JsonSerializer jsonDeserializer = new JsonSerializer();
            using (StreamReader streamReader = File.OpenText(inputFilePath))
            {
                Console.WriteLine($"Reading from file {inputFilePath}.");
                using (JsonReader reader = new JsonTextReader(streamReader))
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            UrlData url = null;
                            try
                            {
                                url = jsonDeserializer.Deserialize<UrlData>(reader);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("JSON object could not be parsed.");
                            }
                            dataToParse.Enqueue(url);
                        }
                    }
                    dataLeftInFile = false;
                }
            }
        }
    }
}