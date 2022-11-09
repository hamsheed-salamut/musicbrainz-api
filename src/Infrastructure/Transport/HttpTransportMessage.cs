using RestSharp;
using System.Collections.Generic;

namespace Infrastructure.Transport
{
    public class HttpTransportMessage
    {
        public string RelativePath { get; set; }

        public byte[] Content { get; set; }

        public string ContentType { get; set; }

        public Method Method { get; set; } = Method.Get;

        public bool KeepAlive { get; set; } = false;

        public int Timeout { get; set; } = 60000;

        public string UserAgent { get; set; }

        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public string Uri { get; set; }

        public string ClientName { get; set; }
    }
}
