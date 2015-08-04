using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Runscope.Messages
{
    public class RunscopeResponse
    {
        private JObject _jResponse = new JObject();
        
        public JToken ToJObject()
        {
            return _jResponse;
        }

        public RunscopeResponse(JObject jResponse)
        {
            _jResponse = jResponse;
        }

        private RunscopeResponse()
        {
            
        }

        private async Task LoadResponse(HttpResponseMessage httpResponse)
        {
            _jResponse["status"] = (int) httpResponse.StatusCode;

            var jheaders = new JObject();
            RunscopeMessage.AddHeaders(httpResponse.Headers, jheaders);

            if (httpResponse.Content != null)
            {
                RunscopeMessage.AddHeaders(httpResponse.Content.Headers, jheaders);
                _jResponse["body"] = await httpResponse.Content.ReadAsStringAsync();
            }
            if (jheaders.Properties().Any())
            {
                _jResponse["headers"] = jheaders;
            }
        }

        public static async Task<RunscopeResponse> CreateFromAsync(HttpResponseMessage httpResponseMessage)
        {
            var rr = new RunscopeResponse();
            await rr.LoadResponse(httpResponseMessage);
            return rr;
        }
    }
}