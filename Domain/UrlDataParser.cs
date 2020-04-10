using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class UrlDataParser
    {
        public UrlOutputData Parse(UrlData input)
        {
            UrlOutputData urlOutputData = new UrlOutputData();
            urlOutputData.Path = new Dictionary<string, UrlInfo>();
            urlOutputData.Path.Add(input.Path, new UrlInfo { Url = input.Url, SizeInBytes = input.SizeInBytes });
            return urlOutputData;
        }
    }
}
