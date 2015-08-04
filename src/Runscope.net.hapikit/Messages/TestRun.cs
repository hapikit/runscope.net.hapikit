using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Runscope.Links;

namespace Runscope.Messages
{
    public class TestRun
    {
        public Guid TestRunId { get; set; }
        public TestRunLink TestRunLink { get; set; }

        public string BucketKey { get; set; }
        public string Agent { get; set; }
        public string Region { get; set; }

        public int RequestExecuted { get; set; }
        
        public int AssertionsDefined { get; set; }
        public int AssertionsPassed { get; set; }
        public int AssertionsFailed { get; set; }

        public int ScriptsDefined { get; set; }
        public int ScriptsPassed { get; set; }
        public int ScriptsFailed { get; set; }

        public int VariablesDefined { get; set; }
        public int VariablesPassed { get; set; }
        public int VariablesFailed { get; set; }

        public string Result { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }


        private static readonly Dictionary<string, Action<JProperty, TestRun>> _ParseMap
        = new Dictionary<string, Action<JProperty, TestRun>>
        {
            {"test_run_id",     (p,t) => t.TestRunId = RunscopeApiDocument.ReadAsGuid(p)},
            {"test_run_url",    (p,t) => t.TestRunLink = RunscopeApiDocument.ReadAsLink<TestRunLink>(p)},
            {"bucket_key",      (p,t) => t.BucketKey = RunscopeApiDocument.ReadAsString(p)},
            {"agent",           (p,t) => t.Agent = RunscopeApiDocument.ReadAsString(p)},
            {"region",          (p,t) => t.Region = RunscopeApiDocument.ReadAsString(p)},
            {"request_executed", (p,t) => t.RequestExecuted = RunscopeApiDocument.ReadAsInteger(p)},
            {"assertions_defined", (p,t) => t.AssertionsDefined =  RunscopeApiDocument.ReadAsInteger(p)},
            {"assertions_passed", (p,t) => t.AssertionsPassed =  RunscopeApiDocument.ReadAsInteger(p)},
            {"assertions_failed", (p,t) => t.AssertionsFailed =  RunscopeApiDocument.ReadAsInteger(p)},
            {"scripts_defined", (p,t) => t.ScriptsDefined =  RunscopeApiDocument.ReadAsInteger(p)},
            {"scripts_passed", (p,t) => t.ScriptsPassed =  RunscopeApiDocument.ReadAsInteger(p)},
            {"scripts_failed", (p,t) => t.ScriptsFailed =  RunscopeApiDocument.ReadAsInteger(p)},
            {"variables_defined", (p,t) => t.VariablesDefined =  RunscopeApiDocument.ReadAsInteger(p)},
            {"variables_passed",  (p,t) => t.VariablesPassed =  RunscopeApiDocument.ReadAsInteger(p)},
            {"variables_failed",  (p,t) => t.VariablesFailed =  RunscopeApiDocument.ReadAsInteger(p)},
            {"result",            (p,t) => t.Result = RunscopeApiDocument.ReadAsString(p)},
            {"started_at",        (p,t) => t.StartedAt = RunscopeApiDocument.ReadAsDateTime(p)},
            {"finished_at",       (p,t) => t.FinishedAt = RunscopeApiDocument.ReadAsDateTime(p)},


        };

        public static TestRun Parse(JToken token)
        {
            return RunscopeApiDocument.ParseObject(token, _ParseMap);
        }
    }
}
