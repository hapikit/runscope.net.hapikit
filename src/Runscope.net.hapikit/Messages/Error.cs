using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Runscope.Links;

namespace Runscope.Messages
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string MoreInfo { get; set; }


        private static readonly Dictionary<string, Action<JProperty, Error>> _ParseMap
          = new Dictionary<string, Action<JProperty, Error>>
        {
            {"code",            (p,t) => t.Code = RunscopeApiDocument.ReadAsString(p)},
            {"message",         (p,t) => t.Message = RunscopeApiDocument.ReadAsString(p)},
            {"more_info",       (p,t) => t.MoreInfo = RunscopeApiDocument.ReadAsString(p)},
        };

        public static Error Parse(JToken token)
        {
            return RunscopeApiDocument.ParseObject(token, _ParseMap);
        }

    }

   
}
