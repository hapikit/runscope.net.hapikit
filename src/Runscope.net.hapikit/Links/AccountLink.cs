using System;
using Hapikit.Links;
using Newtonsoft.Json.Linq;
using Runscope.Messages;


namespace Runscope.Links
{
    [LinkRelationType("https://runscope.com/rels/account")]
    public class AccountLink : Link
    {
        public AccountLink()
        {
            Target = new Uri("/account",UriKind.Relative); 
        }

        public static Account InterpretMessageBody(RunscopeApiDocument document)
        {
            return RunscopeApiDocument<Account>.ParseDataObject(document.Data as JObject, Account.Parse);
        }

    }
}