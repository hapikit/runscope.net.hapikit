using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hapikit.Links;


namespace Runscope.Links
{
    [LinkRelationType("https://runscope.com/rels/testtrigger")]
    public class TestTriggerLink : Link
    {
        public TestTriggerLink()
        {
            Method = HttpMethod.Post;
        }
    }
}
