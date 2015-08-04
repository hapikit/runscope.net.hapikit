using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Runscope.Messages
{
    public class NewMessageResponse
    {
        public string Status { get; set; }
        public string UniqueIdentifier { get; set; }
        public Guid Uuid { get; set; }
        public Error Error { get; set; }
        public Warning Warning { get; set; }

        private static readonly Dictionary<string, Action<JProperty, NewMessageResponse>> _ParseMap
       = new Dictionary<string, Action<JProperty, NewMessageResponse>>
        {
            {"status", (p,t) => t.Status = RunscopeApiDocument.ReadAsString(p)},
            {"unique_identifier", (p,t) => t.UniqueIdentifier = RunscopeApiDocument.ReadAsString(p)},
            {"uuid", (p,t) => t.Uuid = RunscopeApiDocument.ReadAsGuid(p)},
            {"error", (p,t) => t.Error = Messages.Error.Parse(p.Value)},
            {"warning", (p,t) => t.Warning = Messages.Warning.Parse(p.Value)},
        };

        public static NewMessageResponse Parse(JToken token)
        {
            return RunscopeApiDocument.ParseObject(token, _ParseMap);
        }
    }
}
