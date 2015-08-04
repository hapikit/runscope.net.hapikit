using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hapikit.Links;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Runscope.Messages;


namespace Runscope.Links
{
    [LinkRelationType("https://runscope.com/rels/buckets")]
    public class BucketsLink : IRequestFactory //, IResponseHandler
    {
        public Uri Target { get; set; }

        public BucketsLink()
        {
            Target = new Uri("/buckets", UriKind.Relative);
        }


        
        public HttpRequestMessage CreateRequest()
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = Target
            };
            return requestMessage;
        }

        public string LinkRelation { get { return "urn:runscope:buckets"; } }

        public static List<Bucket> InterpretMessageBody(RunscopeApiDocument document)
        {
            return RunscopeApiDocument<Bucket>.ParseDataArray(document.Data as JArray, Bucket.Parse);
        }

    }
}
