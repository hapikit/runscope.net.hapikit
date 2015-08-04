using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Runscope.Messages
{
    public class RunscopeMessage
    {
        public string BucketKey { get; set; }
        public RunscopeRequest Request { get; set; }
        public RunscopeResponse Response { get; set; }
        public Guid UniqueIdentifier { get; set; }

        public HttpContent ToHttpContent()
        {
            var body = new JObject();
            if (Request != null)
            {
                body["request"] = Request.ToJObject();
            }
            if (Response != null)
            {
                body["response"] = Response.ToJObject();
            }
            if (UniqueIdentifier != Guid.Empty)
            {
                body["unique_identifier"] = UniqueIdentifier;
            }
            var content = new StringContent(body.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }


        private static readonly Dictionary<string, Action<JProperty, RunscopeMessage>> _ParseMap
       = new Dictionary<string, Action<JProperty, RunscopeMessage>>
        {
            {"bucket_key",         (p,t) => t.BucketKey = RunscopeApiDocument.ReadAsString(p) },
            {"request",            (p,t) => t.Request = new RunscopeRequest(p.Value as JObject)},
            {"response",           (p,t) => t.Response =  new RunscopeResponse(p.Value as JObject)},
            {"unique_identifier",  (p,t) => t.UniqueIdentifier = RunscopeApiDocument.ReadAsGuid(p)},
            
        };

        public static RunscopeMessage Parse(JToken token)
        {
            return RunscopeApiDocument.ParseObject(token, _ParseMap);
        }



        public static void AddHeaders(HttpHeaders httpHeaders, JObject jheaders)
        {
            foreach (var header in httpHeaders)
            {
                if (header.Value.Count() > 1)
                {
                    string delimiter = _SpaceDelimitedHeaders.Contains(header.Key) ? " " : ", ";
                    jheaders.Add(new JProperty(header.Key, string.Join(delimiter, header.Value)));
                }
                else
                {
                    jheaders.Add(new JProperty(header.Key, header.Value.First()));
                }
            }
        }
        private static readonly HashSet<string> _SpaceDelimitedHeaders =
          new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "User-Agent",
                "Server"
            };
    }
}
