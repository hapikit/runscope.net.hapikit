using System;
using System.Collections.Generic;
using Hapikit.Templates;
using Newtonsoft.Json.Linq;
using Runscope.Links;


namespace Runscope.Messages
{
    public class Bucket
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string AuthToken { get; set; }
        public Team Team { get; set; }
        public bool VerifySSL { get; set; }
        public bool Default { get; set; }
        public CollectionsLink Collections { get; set; }
        public MessagesLink Messages { get; set; }
        
        public TestsLink Tests { get; set; }

        public MessagesLink Errors
        {
            get
            {
                return new MessagesLink()
                {
                    BucketKey = Key,
                    Template = new UriTemplate("/buckets/{bucket_key}/errors")
                };
            }
        }

        public MessagesLink Shared
        {
            get
            {
                return new MessagesLink()
                {
                    BucketKey = Key,
                    Template = new UriTemplate("/buckets/{bucket_key}/shared")
                };
            }
        }
        public MessagesLink Captures
        {
            get
            {
                return new MessagesLink()
                {
                    BucketKey = Key,
                    Template = new UriTemplate("/buckets/{bucket_key}/captures")
                };
            }
        }


        private static readonly Dictionary<string, Action<JProperty, Bucket>> _ParseMap
          = new Dictionary<string, Action<JProperty, Bucket>>
        {
            {"name",            (p,t) => t.Name = RunscopeApiDocument.ReadAsString(p)},
            {"key",             (p,t) => t.Key = RunscopeApiDocument.ReadAsString(p)},
            {"default",         (p,t) => t.Default = RunscopeApiDocument.ReadAsBoolean(p)},
            {"auth_token",      (p,t) => t.AuthToken = RunscopeApiDocument.ReadAsString(p)},
            {"verify_ssl",      (p,t) => t.VerifySSL = RunscopeApiDocument.ReadAsBoolean(p)},
            {"team",            (p,t) => t.Team = Team.Parse(p.Value)},
            {"collections_url", (p,t) => t.Collections = RunscopeApiDocument.ReadAsLink<CollectionsLink>(p)},
            {"messages_url",    (p,t) => t.Messages =  RunscopeApiDocument.ReadAsLink<MessagesLink>(p)},
            {"tests_url",       (p,t) => t.Tests = RunscopeApiDocument.ReadAsLink<TestsLink>(p)},

        };

        public static Bucket Parse(JToken token)
        {
            return RunscopeApiDocument.ParseObject(token, _ParseMap);
        }

    }
}