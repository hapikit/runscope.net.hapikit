using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Runscope.Links;

namespace Runscope.Messages
{
    public class Home
    {
        public BucketsLink BucketsLink { get; set; }
        public AccountLink AccountLink { get; set; }

        private static readonly Dictionary<string, Action<JProperty, Home>> _ParseMap
        = new Dictionary<string, Action<JProperty, Home>>
        {
            {"bucket_list_url",   (p,t) => t.BucketsLink = new BucketsLink()},
            {"current_account_url",   (p,t) => t.AccountLink = RunscopeApiDocument.ReadAsLink<AccountLink>(p)},
        };

        public static Home Parse(JToken token)
        {
            return RunscopeApiDocument.ParseObject(token, _ParseMap);
        }
    }
}
