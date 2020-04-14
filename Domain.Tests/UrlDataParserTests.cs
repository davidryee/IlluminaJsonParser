using System;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Domain.Tests
{
    [TestClass]
    public class UrlDataParserTests
    {        
        private readonly Mock<IHttpClientWrapper> _mockHttpClientWrapper;
        public UrlDataParserTests()
        {
            _mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
        }

        [TestMethod]
        public void ValidData_SuccessfullyParsesAndMaps()
        {
            UrlData inputData = new UrlData();
            inputData.Url = "https://www.google.com/";
            inputData.Path = "PathValue1";
            inputData.Size = 10;
            UrlOutputData expectedOutputData = new UrlOutputData();
            expectedOutputData.Path = new Dictionary<string, UrlInfo>();
            expectedOutputData.Path.Add(inputData.Path, new UrlInfo()
            {
                Url = inputData.Url,
                SizeInBytes = inputData.Size
            });

            HttpResponseMessage responseMessage = new HttpResponseMessage();
            responseMessage.Content = new StringContent("response");            
            responseMessage.Content.Headers.ContentLength = inputData.Size;

            _mockHttpClientWrapper.Setup(h => h.GetAsync(It.IsAny<string>())).ReturnsAsync(responseMessage); 
            UrlDataParser parser = new UrlDataParser(_mockHttpClientWrapper.Object);
            UrlOutputData result = parser.Parse(inputData);
            Assert.AreEqual(expectedOutputData.Path.Keys.Count, result.Path.Keys.Count);
            Assert.IsTrue(result.Path.ContainsKey(inputData.Path));

            expectedOutputData.Path.TryGetValue(inputData.Path, out UrlInfo expectedUrlData);
            result.Path.TryGetValue(inputData.Path, out UrlInfo resultUrlData);

            Assert.AreEqual(expectedUrlData.Url, resultUrlData.Url);
            Assert.AreEqual(expectedUrlData.SizeInBytes, resultUrlData.SizeInBytes);
        }

        [TestMethod]
        public void ParseAsync_InvalidUrl_ThrowsException()
        {
            UrlData inputData = new UrlData();
            inputData.Url = "6https://www.google.com/";

            UrlDataParser urlDataParser = new UrlDataParser(_mockHttpClientWrapper.Object);
            Assert.ThrowsException<UriFormatException>(() => urlDataParser.Parse(inputData));
        }

        [TestMethod]
        public void ParseAsync_SizesDoNotMatch_ThrowsException()
        {
            UrlData inputData = new UrlData();
            inputData.Url = "https://www.google.com/";
            inputData.Path = "PathValue1";
            inputData.Size = 6;

            HttpResponseMessage responseMessage = new HttpResponseMessage();
            responseMessage.Content = new StringContent("response");
            responseMessage.Content.Headers.ContentLength = inputData.Size + 1;

            _mockHttpClientWrapper.Setup(h => h.GetAsync(It.IsAny<string>())).ReturnsAsync(responseMessage);
            UrlDataParser parser = new UrlDataParser(_mockHttpClientWrapper.Object);
            Assert.ThrowsException<InvalidDataException>(() => parser.Parse(inputData));
        }
    }
}