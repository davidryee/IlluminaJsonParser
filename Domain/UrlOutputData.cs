using System.Collections.Generic;

namespace Domain
{
    public class UrlOutputData
    {
        public Dictionary<string, UrlInfo> Path { get; set; }
    }

    public class UrlInfo
    {
        public string Url { get; set; }
        public int Size { get; set; }
    }
}
