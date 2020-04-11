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
            inputFilePath = Console.ReadLine();

            Console.WriteLine("Please enter a file path to use for the output.");
            outputFilePath = Console.ReadLine();
            
            ThreadStart addToQueueThreadStart = new ThreadStart(AddToQueue);
            Thread addToQueueThread = new Thread(addToQueueThreadStart);
            
            ThreadStart removeFromQueueThreadStart = new ThreadStart(RemoveFromQueue);
            Thread removeFromQueueThread = new Thread(removeFromQueueThreadStart);
            
            addToQueueThread.Start();
            removeFromQueueThread.Start();

            //while(dataLeftInFile || dataToParse.Count > 0)
            //{
            //    UrlData dataFromQueue = (UrlData)(dataToParse.Dequeue());
            //    UrlOutputData parsedData = parser.Parse(dataFromQueue);
            //    File.WriteAllText(outputFilePath, parsedData.ToString());
            //}

            //try
            //{
            //    JsonSerializer jsonDeserializer = new JsonSerializer();
            //    using (StreamReader streamReader = File.OpenText(inputFilePath))
            //    {
            //        Console.WriteLine($"Reading from file {inputFilePath}.");
            //        using (JsonReader reader = new JsonTextReader(streamReader))
            //        {
            //            while (reader.Read())
            //            {
            //                if (reader.TokenType == JsonToken.StartObject)
            //                {
            //                    UrlData url = null;
            //                    try
            //                    {
            //                        url = jsonDeserializer.Deserialize<UrlData>(reader);
            //                    }
            //                    catch(Exception ex)
            //                    {
            //                        Console.WriteLine("JSON object could not be parsed.");
            //                    }
            //                    dataToParse.Enqueue(url);

            //                    //validate input - Have unit tests around this
            //                    //form output data - Have unit tests around this
            //                    //*****make a test file with 500k objects
            //                }
            //            }
            //            dataLeftInFile = false;
            //        }
            //    }
            //}
            //catch
            //{
            //    Console.WriteLine($"File {inputFilePath} could not be opened");
            //}
        }

        private static void RemoveFromQueue()
        {
            while (dataLeftInFile || dataToParse.Count > 0)
            {
                if(dataToParse.Count > 0)
                {
                    UrlData urlInfo = (UrlData)(dataToParse.Dequeue());
                    UrlOutputData outputData = parser.Parse(urlInfo);
                    File.WriteAllText(outputFilePath, JsonConvert.SerializeObject(outputData));
                }
            }
        }

        private static void AddToQueue()
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
