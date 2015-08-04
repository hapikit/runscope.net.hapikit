using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Runscope.Messages
{
    public class Account
    {
        public string Name { get; set; }
        public Guid Uuid { get; set; }
        public string Email { get; set; }
        public List<Team> Teams { get; set; }


        private static readonly Dictionary<string, Action<JProperty, Account>> _ParseMap
          = new Dictionary<string, Action<JProperty, Account>>
        {
            {"name",            (p,t) => t.Name = RunscopeApiDocument.ReadAsString(p)},
            {"uuid",             (p,t) => t.Uuid = RunscopeApiDocument.ReadAsGuid(p)},
            {"email",             (p,t) => t.Email = RunscopeApiDocument.ReadAsString(p)},
            {"teams",             (p,t) => t.Teams = (p.Value as JArray).Select(Team.Parse).ToList()},

        };

        public static Account Parse(JToken token)
        {
            return RunscopeApiDocument.ParseObject(token, _ParseMap);
        }

    }
}
