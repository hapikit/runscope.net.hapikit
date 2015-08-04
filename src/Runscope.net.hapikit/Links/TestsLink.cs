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
    [LinkRelationType("https://runscope.com/rels/tests")]
    public class TestsLink : Link
    {

        public static List<Test> InterpretMessageBody(RunscopeApiDocument document)
        {
            return RunscopeApiDocument<Test>.ParseDataArray(document.Data as JArray, Test.Parse);
        }
    }
}
