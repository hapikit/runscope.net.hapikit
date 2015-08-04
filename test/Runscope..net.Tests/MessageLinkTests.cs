using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Runscope;
using Runscope.Links;
using Runscope.Messages;
using Xunit;

namespace RunscopeWebPackTests
{
    public class MessageLinkTests
    {
        [Fact]
        public async Task CreatePOSTRequest()
        {

            var runscopeMessage = new RunscopeMessage()
                {
                    Request = await RunscopeRequest.CreateFromAsync(new HttpRequestMessage() { RequestUri = new Uri("/foo", UriKind.Relative) }),
                    Response = await RunscopeResponse.CreateFromAsync(new HttpResponseMessage())
                };
            

            var messagesLink = new MessagesLink
            {
                BucketKey = "blah",
                RunscopeMessage = runscopeMessage,
                Method = HttpMethod.Post
            };
            var messageRequest = messagesLink.CreateRequest();

            Assert.NotNull(messageRequest);

        }
    }
}
