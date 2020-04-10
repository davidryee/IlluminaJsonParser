using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class UrlOutputData
    {
        public Dictionary<string, UrlInfo> Path { get; set; }
    }

    public class UrlInfo
    {
        public string Url { get; set; }
        public int SizeInBytes { get; set; }
    }
}
