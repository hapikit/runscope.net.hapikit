using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace Runscope.Messages
{
    public class RunscopeRequest
    {
        private JObject _jRequest = new JObject();

        public JObject ToJObject()
        {
            return _jRequest;
        }

        public RunscopeRequest(JObject jRequest)
        {
            _jRequest = jRequest;
        }

        public static async Task<RunscopeRequest> CreateFromAsync(HttpRequestMessage httpRequestMessage)
        {
            var rr = new RunscopeRequest();
            await rr.LoadHttpRequest(httpRequestMessage);
            return rr;
        }

        public RunscopeRequest()
        {
            
        }

        private async Task LoadHttpRequest(HttpRequestMessage httpRequest)
        {
            if (httpRequest.RequestUri == null) throw new ArgumentException("Request Uri is required for a Runscope Request");
            _jRequest["method"] = httpRequest.Method.ToString();
            _jRequest["url"] = httpRequest.RequestUri.OriginalString;

            var jheaders = new JObject();
            RunscopeMessage.AddHeaders(httpRequest.Headers, jheaders);
            if (httpRequest.Content != null)
            {
                RunscopeMessage.AddHeaders(httpRequest.Content.Headers, jheaders);
                _jRequest["body"] = await httpRequest.Content.ReadAsStringAsync();
            }

            if (jheaders.Properties().Any())
            {
                _jRequest["headers"] = jheaders;
            }
        }
    }
}