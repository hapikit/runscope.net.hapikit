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
    [LinkRelationType("https://runscope.com/rels/collections")]
    public class CollectionsLink : Link
    {
        public CollectionsLink()
        {
            
        }

        public static List<Collection> InterpretMessageBody(RunscopeApiDocument document)
        {
            return RunscopeApiDocument<Collection>.ParseDataArray(document.Data as JArray, Collection.Parse);
        }
    }
}
