using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Domain.Tests
{
    [TestClass]
    public class UrlDataParserTests
    {
        [TestMethod]
        public void ValidData_SuccessfullyParsesAndMaps()
        {
            UrlData inputData = new UrlData();
            inputData.Url = "https://www.google.com/";
            inputData.Path = "PathValue1";
            inputData.SizeInBytes = 10;
            UrlOutputData expectedOutputData = new UrlOutputData();
            expectedOutputData.Path = new Dictionary<string, UrlInfo>();
            expectedOutputData.Path.Add(inputData.Path, new UrlInfo()
            {
                Url = inputData.Url,
                SizeInBytes = inputData.SizeInBytes
            });

            UrlDataParser parser = new UrlDataParser();
            UrlOutputData result = parser.Parse(inputData);
            Assert.AreEqual(expectedOutputData.Path.Keys.Count, result.Path.Keys.Count);
            Assert.IsTrue(result.Path.ContainsKey(inputData.Path));

            expectedOutputData.Path.TryGetValue(inputData.Path, out UrlInfo expectedUrlData);
            result.Path.TryGetValue(inputData.Path, out UrlInfo resultUrlData);

            Assert.AreEqual(expectedUrlData.Url, resultUrlData.Url);
            Assert.AreEqual(expectedUrlData.SizeInBytes, resultUrlData.SizeInBytes);
        }
    }
}
