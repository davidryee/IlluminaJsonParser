using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Domain
{
    public class UrlDataParser
    {
        private IHttpClientWrapper _httpClientWrapper;

        public UrlDataParser(IHttpClientWrapper httpClientWrapper)
        {
            _httpClientWrapper = httpClientWrapper;
        }

        public UrlOutputData Parse(UrlData input)
        {
            if(!Uri.IsWellFormedUriString(input.Url, UriKind.Absolute))
            {
                throw new UriFormatException($"The URL {input.Url} is malformed");
            }

            HttpResponseMessage response = _httpClientWrapper.GetAsync(input.Url).Result;
            response.EnsureSuccessStatusCode();
            
            if(response.Content.Headers.ContentLength.Value != input.Size)
            {
                throw new InvalidDataException("Specified size does not match actual size of data");
            }

            UrlOutputData urlOutputData = new UrlOutputData();
            urlOutputData.Path = new Dictionary<string, UrlInfo>();
            urlOutputData.Path.Add(input.Path, new UrlInfo { Url = input.Url, SizeInBytes = input.Size });
            return urlOutputData;
        }
    }

    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }

    public class HttpClientWrapper : IHttpClientWrapper
    {
        HttpClient httpClient;
        public HttpClientWrapper()
        {
            httpClient = new HttpClient();
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return httpClient.GetAsync(url);
        }
    }
}
