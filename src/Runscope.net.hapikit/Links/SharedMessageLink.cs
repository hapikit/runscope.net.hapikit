using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hapikit.Links;
using Hapikit.RequestBuilders;
using Hapikit.Templates;
using Newtonsoft.Json.Linq;


namespace Runscope.Links
{
    [LinkRelationType("https://runscope.com/rels/sharedmessage")]
    public class SharedMessageLink : Link
    {

        [LinkParameter("bucket_key")]
        public string BucketKey { get; set; }

        [LinkParameter("message_id")]
        public Guid MessageId { get; set; }

        public SharedMessageLink()
        {
            Method = HttpMethod.Put;
            Template = new UriTemplate("/buckets/{bucket_key}/shared/{message_id}");
            
        }

        public static async Task<Uri> ParsePublicUri(HttpResponseMessage response)
        {

            if (response.IsSuccessStatusCode)
            {
                var jObject = JObject.Parse(await response.Content.ReadAsStringAsync());
                return new Uri((string)(jObject["data"]["public_url"]));
            }
            else
            {
                return null;
            }
        }
    }
}
