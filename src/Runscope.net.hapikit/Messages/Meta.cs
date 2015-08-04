using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Runscope.Messages
{
    public class Meta
    {
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }


        private static readonly Dictionary<string, Action<JProperty, Meta>> _ParseMap
       = new Dictionary<string, Action<JProperty, Meta>>
        {
            {"success_count",  (p,t) => t.SuccessCount = RunscopeApiDocument.ReadAsInteger(p)},
            {"error_count",    (p,t) => t.ErrorCount = RunscopeApiDocument.ReadAsInteger(p)},
            {"warning_count",  (p,t) => t.WarningCount = RunscopeApiDocument.ReadAsInteger(p)},

        };

        public static Meta Parse(JToken token)
        {
            return RunscopeApiDocument.ParseObject(token, _ParseMap);
        }

    }
}