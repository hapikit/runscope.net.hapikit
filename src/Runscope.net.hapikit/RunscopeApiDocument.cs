using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hapikit.Links;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Runscope.Messages;

namespace Runscope
{

    public class RunscopeApiDocument
    {
        protected JObject _rootObject;
        public object Error { get; set; }
        public Meta Meta { get; set; }
        public object Status { get; set; }  // Not sure what to do with this


        public static async Task<RunscopeApiDocument> CreateFrom(HttpContent content)
        {
            var stream = await content.ReadAsStreamAsync();
            return new RunscopeApiDocument(stream);
        }
        public RunscopeApiDocument(Stream stream)
        {
            _rootObject = JObject.Load(new JsonTextReader(new StreamReader(stream)));

            var meta = _rootObject["meta"] as JObject;
            if (meta != null)
            {
                Meta = Meta.Parse(meta);
            }

            var status = _rootObject["status"] as JObject;
            if (status != null)
            {
                // ParseStatus
            }

            var error = _rootObject["error"] as JObject;
            if (error != null)
            {
                // ParseError
            }


            
        }

        public JToken Data
        {
            get
            {
                return _rootObject["data"];
            }
        }

        public static string ReadAsString(JProperty prop)
        {
            return (string)prop.Value;
        }

        internal static bool ReadAsBoolean(JProperty prop)
        {
            return (bool)prop.Value;
        }

        internal static DateTime ReadAsDateTime(JProperty prop)
        {
            if (prop.Value == null) return DateTime.MinValue;

            return UnixTimeStampToDateTime((double)prop.Value);
        }

        internal static Guid ReadAsGuid(JProperty prop)
        {
            return (Guid)prop.Value;
        }

        internal static int ReadAsInteger(JProperty prop)
        {
            return (int)prop.Value;
        }

        internal static T ReadAsLink<T>(JProperty prop) where T:Link,new()
        {
            return new T()
                           {
                               Target = new Uri((string)prop.Value, UriKind.RelativeOrAbsolute),
                               Template = null
                           };
        }

        public static T ParseObject<T>(JToken token, Dictionary<string, Action<JProperty, T>> parseMap) where T:class, new()
        {
            var obj = token as JObject;
            if (obj == null) return null;


            var test = new T();
            foreach (var prop in obj.Properties())
            {
                if (parseMap.ContainsKey(prop.Name))
                {
                    parseMap[prop.Name](prop, test);
                }
            }

            return test;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }
    public class RunscopeApiDocument<T> : RunscopeApiDocument
    {
        private readonly Func<JToken, T> _parser;
        public List<T> DataList { get; set; }  
        public T DataObject { get; set; }

        public RunscopeApiDocument(Stream stream, Func<JToken, T> parser) : base(stream)
        {
            _parser = parser;
        
            var data = _rootObject["data"];
            if (data != null)
            {
                if (data is JArray)
                {
                    DataList = ParseDataArray(data as JArray, _parser);                    
                }

                if (data is JObject)
                {
                    DataObject = ParseDataObject(data as JObject, _parser);
                }
            }
        }

        
   

        public static List<T> ParseDataArray(JArray data, Func<JToken, T> parser)
        {
            var list = new List<T>();
            if (data != null)
            {
                list.AddRange(data.Select(parser).Where(bucket => bucket != null));
            }
            return list;
        }

        public static T ParseDataObject(JObject data, Func<JToken, T> parser)
        {
            return parser(data);
        }
    }
}
