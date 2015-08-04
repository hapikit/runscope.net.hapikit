using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Runscope.Links;

namespace Runscope.Messages
{
    public class Team
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }

        private static readonly Dictionary<string, Action<JProperty, Team>> _ParseMap
        = new Dictionary<string, Action<JProperty, Team>>
        {
            {"name", (p,t) => t.Name = RunscopeApiDocument.ReadAsString(p)},
            {"uuid", (p,t) => t.Uuid = RunscopeApiDocument.ReadAsGuid(p)},
        };

        public static Team Parse(JToken token)
        {
            return RunscopeApiDocument.ParseObject(token, _ParseMap);
        }
    }
}
