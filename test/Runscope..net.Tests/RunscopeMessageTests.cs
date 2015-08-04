using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Runscope.Links;
using Runscope.Messages;
using Xunit;

namespace RunscopeWebPackTests
{
    public class RunscopeMessageTests
    {

        [Fact]
        public void CreateMessageLink()
        {
            var link = new MessagesLink();

            Assert.NotNull(link);
        }


        [Fact]
        public void CreateEmptyRunscopeMessage()
        {
            var message = new RunscopeMessage();

            Assert.NotNull(message);
        }

        [Fact]
        public async Task CreateMinimumRunscopeRequest()
        {
            var httpRequestMessage = new HttpRequestMessage() { RequestUri = new Uri("http://example.org")};
            
            var message = await RunscopeRequest.CreateFromAsync(httpRequestMessage);
            var jrequest = message.ToJObject();
            var expected = 
                new JObject(new[]
                {
                    new JProperty("method", "GET"),
                    new JProperty("url", "http://example.org"),
                });

            Assert.Equal(expected,jrequest);
        }
        [Fact]
        public async Task CreateRunscopeRequestWithSingleValueHeaders()
        {
            var httpRequestMessage = new HttpRequestMessage() { RequestUri = new Uri("http://example.org") };
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("basic","foo");
            httpRequestMessage.Headers.CacheControl= new CacheControlHeaderValue() {MaxAge = new TimeSpan(0,0,0,30)};
            var message = await RunscopeRequest.CreateFromAsync(httpRequestMessage);
            var jrequest = message.ToJObject();
            var expected =
                new JObject(new[]
                {
                    new JProperty("method", "GET"),
                    new JProperty("url", "http://example.org"),
                    new JProperty("headers", new JObject(new []
                    {
                        new JProperty("Authorization","basic foo"),
                        new JProperty("Cache-Control","max-age=30")
                    
                    } )),
                });

            Assert.Equal(expected, jrequest);
        }

        [Fact]
        public async Task CreateRunscopeRequestWithMultiValueHeaders()
        {
            var httpRequestMessage = new HttpRequestMessage() { RequestUri = new Uri("http://example.org") };
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var message = await RunscopeRequest.CreateFromAsync(httpRequestMessage);
            var jrequest = message.ToJObject();
            var expected =
                new JObject(new[]
                {
                    new JProperty("method", "GET"),
                    new JProperty("url", "http://example.org"),
                    new JProperty("headers", new JObject(new []
                    {
                        new JProperty("Accept","application/xml, application/json"),
                      } )),
                });

            Assert.Equal(expected, jrequest);
        }
        [Fact]
        public async Task CreateRunscopeRequestWithBody()
        {
            var httpRequestMessage = new HttpRequestMessage() { RequestUri = new Uri("http://example.org") };
            httpRequestMessage.Content = new StringContent("This is some text");

            var message = await RunscopeRequest.CreateFromAsync(httpRequestMessage);
            var jrequest = message.ToJObject();
            var expected =
                new JObject(new[]
                {
                    new JProperty("method", "GET"),
                    new JProperty("url", "http://example.org"),
                    new JProperty("body", "This is some text"),
                    new JProperty("headers", new JObject(new []
                    {
                        new JProperty("Content-Type","text/plain; charset=utf-8"),
                      } ))

                });

            Assert.Equal(expected, jrequest);
        }

        [Fact]
        public async Task CreateMinimumRunscopeResponse()
        {
            var httpResponseMessage = new HttpResponseMessage() { StatusCode= HttpStatusCode.NotFound};
            var message = await RunscopeResponse.CreateFromAsync(httpResponseMessage);
            var jresponse = message.ToJObject();
            var expected =
                new JObject(new[]
                {
                    new JProperty("status", 404),
                    
                });

            Assert.Equal(expected, jresponse);
        }

        [Fact]
        public void FindEasyHexEscape()
        {
            byte b = 135;
            var s = Uri.HexEscape((char) b);
            var uri = new Uri(s, UriKind.Relative);
            var suri = uri.ToString();
            int z = 0;
            var c = Uri.HexUnescape(s,ref z);
        }
        //[Fact]
        //public void Unicodeuri()
        //{
        //    string message = "Here is a string with a Unicode char \u0298";
        //    char.IsLetterOrDigit()
        //        Uri.
        //    var bytes = String.Join("",Encoding.UTF8.GetBytes(message).Select(b => Uri.HexEscape((char)b)));
        //}

    }

}
