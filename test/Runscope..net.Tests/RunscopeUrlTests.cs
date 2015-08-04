using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runscope;
using Xunit;
using Xunit.Extensions;

namespace RunscopeWebPackTests
{
    public class RunscopeUrlTests
    {
        [Theory,
         InlineData("foo", "http://example.org/", "http://example-org-foo.runscope.net/"),
         InlineData("foo", "http://example.org/with/a/path", "http://example-org-foo.runscope.net/with/a/path"),
         InlineData("foo", "http://example.org/with/a/path?and=query&string=true",
             "http://example-org-foo.runscope.net/with/a/path?and=query&string=true"),
         InlineData("foo", "http://example.org/with/a/path#andFragment",
             "http://example-org-foo.runscope.net/with/a/path#andFragment"),
         InlineData("bar", "http://example.org:99/", "http://example-org-bar.runscope.net/"),
         InlineData("bar", "https://example.org/", "https://example-org-bar.runscope.net/"),
         InlineData("bar", "https://example.org:871/", "https://example-org-bar.runscope.net/"),
         InlineData("pass", "https://jane:doe@example.org:871/", "https://jane:doe@example-org-pass.runscope.net/"),
         InlineData("path", "https://foo-bar.example.org/~home", "https://foo--bar-example-org-path.runscope.net/~home"),
        ]
        public void CreateRequest(string bucket, string input, string expected)
        {

            var inputUri = new Uri(input);

            var requestUri = inputUri.ToRunscopeUrl(bucket);

            Assert.Equal(expected, requestUri.OriginalString);
        }
    }
}
