using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Runscope.Messages
{
    public class Collection
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string BucketKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPublic { get; set; }
        
        
        private static readonly Dictionary<string, Action<JProperty, Collection>> _ParseMap
        = new Dictionary<string, Action<JProperty, Collection>>
        {
            {"name",            (p,t) => t.Name = RunscopeApiDocument.ReadAsString(p)},
            {"uuid",            (p,t) => t.Uuid = RunscopeApiDocument.ReadAsGuid(p)},
            {"bucket_key",      (p,t) => t.BucketKey = RunscopeApiDocument.ReadAsString(p)},
            {"created_at",      (p,t) => t.CreatedAt = (DateTime)p.Value},
            {"is_public",       (p,t) => t.IsPublic = RunscopeApiDocument.ReadAsBoolean(p)},

        };

        public static Collection Parse(JToken token)
        {
            return RunscopeApiDocument.ParseObject(token, _ParseMap);
        }

    }
}
