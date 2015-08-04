using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Runscope.Messages
{
    public class Warning
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string MoreInfo { get; set; }


        private static readonly Dictionary<string, Action<JProperty, Warning>> _ParseMap
         = new Dictionary<string, Action<JProperty, Warning>>
        {
            {"code",            (p,t) => t.Code = RunscopeApiDocument.ReadAsString(p)},
            {"message",         (p,t) => t.Message = RunscopeApiDocument.ReadAsString(p)},
            {"more_info",       (p,t) => t.MoreInfo = RunscopeApiDocument.ReadAsString(p)},
        };

        public static Warning Parse(JToken token)
        {
            return RunscopeApiDocument.ParseObject(token, _ParseMap);
        }

    }
}
