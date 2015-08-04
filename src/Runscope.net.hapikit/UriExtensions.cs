using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Runscope
{
    public static class HttpContentExtensions
    {
        public static async Task<RunscopeApiDocument<T>> ReadAsRunscopeApiDocumentAsync<T>(this HttpContent content, Func<JToken,T> parser)
        {
            return new RunscopeApiDocument<T>(await content.ReadAsStreamAsync(), parser);
        }
        public static async Task<RunscopeApiDocument> ReadAsRunscopeApiDocumentAsync(this HttpContent content)
        {
            return new RunscopeApiDocument(await content.ReadAsStreamAsync());
        }     
    }

    public static class UriExtensions
    {
        public static Uri ToRunscopeUrl(this Uri requestUri, string bucketKey, string gatewayHost = "runscope.net")
        {
            var cleanHost = requestUri.Host.Replace("-", "~").Replace(".", "-");
            var newHost = String.Format("{0}-{1}.{2}", cleanHost, bucketKey, gatewayHost).Replace("~", "--");

            string userName = null;
            string password = null;
            if (!String.IsNullOrEmpty(requestUri.UserInfo))
            {
                var info = requestUri.UserInfo.Split(':');
                if (info.Length == 2)
                {
                    password = info[1];
                }
                userName = info[0];
            }

            var uriBuilder = new UriBuilder()
            {
                Scheme = requestUri.Scheme,
                Host = newHost,
                UserName = userName,
                Password = password,
                Port = -1,
                Path = requestUri.AbsolutePath,
                Query = requestUri.Query.StartsWith("?") ? requestUri.Query.Substring(1) : requestUri.Query,  // Remove leading ?
                Fragment = requestUri.Fragment.StartsWith("#") ? requestUri.Fragment.Substring(1) : requestUri.Fragment
            };
            return uriBuilder.Uri;

        }
    }
}
