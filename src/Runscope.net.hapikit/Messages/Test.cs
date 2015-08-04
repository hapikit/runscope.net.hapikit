using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Runscope.Links;

namespace Runscope.Messages
{
    public class Test
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }

        public string InitialScript { get; set; }
        // Initial Variables
        // Schedule Minutes

        public int AssertionsDefined { get; set; }
        public int VariablesDefined { get; set; }
        public int RequestsDefined { get; set; }
        public int ScriptsDefined { get; set; }
        public TestRunLink LatestTestResult { get; set; }
        public TestResultsLink TestResults { get; set; }
        public TestTriggerLink TestTrigger { get; set; }
        // Test Url  - Not sure why there is a self-url here

        public static Dictionary<string,Action<JProperty,Test>> _ParseMap
            = new Dictionary<string, Action<JProperty, Test>>
        {
            {"uuid",(prop, test) => test.Uuid = RunscopeApiDocument.ReadAsGuid(prop)},
            {"name",(prop, test) => test.Name = RunscopeApiDocument.ReadAsString(prop)},
            {"description",(p,t) => t.Description = RunscopeApiDocument.ReadAsString(p)},

            {"created_at",(p,t) => RunscopeApiDocument.ReadAsDateTime(p)},
            {"edited_at",(p,t) => RunscopeApiDocument.ReadAsDateTime(p)},
            {"initial_script",(p,t) => t.InitialScript = RunscopeApiDocument.ReadAsString(p)},
            
            {"assertions_defined",(p,t) => t.AssertionsDefined = RunscopeApiDocument.ReadAsInteger(p)},
            {"variables_defined",(p,t) => t.VariablesDefined = RunscopeApiDocument.ReadAsInteger(p)},
            {"requests_defined",(p,t) => t.RequestsDefined = RunscopeApiDocument.ReadAsInteger(p)},
            {"scripts_defined",(p,t) => t.ScriptsDefined = RunscopeApiDocument.ReadAsInteger(p)},

            {"latest_test_result_url",(p,t) => t.LatestTestResult = RunscopeApiDocument.ReadAsLink<TestRunLink>(p)},
            {"test_results_url",(p,t) => t.TestResults = RunscopeApiDocument.ReadAsLink<TestResultsLink>(p)},
            {"trigger_url",(p,t) => t.TestTrigger = RunscopeApiDocument.ReadAsLink<TestTriggerLink>(p)},

        };

        public static Test Parse(JToken token)
        {
            return RunscopeApiDocument.ParseObject(token, _ParseMap);
        }
        
    }
}
