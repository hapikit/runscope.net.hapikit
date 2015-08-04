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
    [LinkRelationType("https://runscope.com/rels/testrun")]
    public class TestRunLink : Link
    {

        public static TestRun InterpretMessageBody(RunscopeApiDocument document)
        {
            return RunscopeApiDocument<TestRun>.ParseDataObject(document.Data as JObject, TestRun.Parse);
        }
    }
}
