using System.Collections.Generic;

namespace Domain
{
    public class UrlDataParser
    {
        public UrlOutputData Parse(UrlData input)
        {
            UrlOutputData urlOutputData = new UrlOutputData();
            urlOutputData.Path = new Dictionary<string, UrlInfo>();
            urlOutputData.Path.Add(input.Path, new UrlInfo { Url = input.Url, SizeInBytes = input.Size });
            return urlOutputData;
        }
    }
}
