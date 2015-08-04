using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Hapikit.Links;
using Hapikit.RequestBuilders;
using Hapikit.Templates;
using Newtonsoft.Json.Linq;
using Runscope.Messages;

namespace Runscope.Links
{
    [LinkRelationType("https://runscope.com/rels/messages")]
    public class MessagesLink : Link
    {
        
        [LinkParameter("bucket_key")]
        public String BucketKey { get; set; }
        
        public RunscopeMessage RunscopeMessage { get; set; }

        public MessagesLink()
        {
            Template = new UriTemplate("/buckets/{bucket_key}/messages");
            this.AddRequestBuilder(new InlineRequestBuilder(r =>
            {
                if (RunscopeMessage  != null) {
                    r.Content = RunscopeMessage.ToHttpContent();
                }
                return r;
            }));
        }


        public static List<RunscopeMessage> InterpretGetMessageBody(RunscopeApiDocument document)
        {
            return RunscopeApiDocument<RunscopeMessage>.ParseDataArray(document.Data as JArray, RunscopeMessage.Parse);
        }

        public static List<NewMessageResponse> InterpretPostMessageBody(RunscopeApiDocument document)
        {
            return RunscopeApiDocument<NewMessageResponse>.ParseDataArray(document.Data as JArray, NewMessageResponse.Parse);
        }


    }
}
