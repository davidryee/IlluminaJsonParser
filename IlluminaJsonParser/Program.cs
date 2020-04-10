using Newtonsoft.Json;
using System;
using System.IO;
using Domain;

namespace IlluminaJsonParser
{
    public class IlluminaJsonParser
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the path to the file you wish to use as input. File must be in valid json format.");
            var filePath = Console.ReadLine();

            try
            {
                JsonSerializer jsonSerializer = new JsonSerializer();
                using (StreamReader streamReader = File.OpenText(filePath))
                {
                    Console.WriteLine($"Reading from file {filePath}.");
                    using (JsonReader reader = new JsonTextReader(streamReader))
                    {
                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                var url = jsonSerializer.Deserialize<UrlData>(reader);
                            }
                        }                        
                    }
                }
            }
            catch
            {
                Console.WriteLine($"File {filePath} could not be opened");
            }
        }
    }
}
