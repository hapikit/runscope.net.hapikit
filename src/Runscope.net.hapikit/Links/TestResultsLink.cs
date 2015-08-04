using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hapikit.Links;
using Newtonsoft.Json.Linq;
using Runscope.Messages;

namespace Runscope.Links
{
    [LinkRelationType("https://runscope.com/rels/testresults")]
    public class TestResultsLink : Link
    {


        public static List<TestRun> InterpretMessageBody(RunscopeApiDocument document)
        {
            return RunscopeApiDocument<TestRun>.ParseDataArray(document.Data as JArray, TestRun.Parse);
        }
    }
}
