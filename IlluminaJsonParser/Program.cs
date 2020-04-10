using System;
using System.IO;

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
                using (StreamReader file = File.OpenText(filePath))
                {
                    Console.WriteLine($"Reading from file {filePath}.");
                }
            }
            catch
            {
                Console.WriteLine($"File {filePath} could not be opened");
            }
        }
    }
}
