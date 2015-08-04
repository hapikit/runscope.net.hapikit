using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Runscope.Links;
using Xunit;

namespace RunscopeWebPackTests
{
    public class BucketsLinkTests
    {
        [Fact]
        public void CreateBucketRequest()
        {
            var link = new BucketsLink();
            var hrm = link.CreateRequest();
            
            Assert.Equal("/buckets",hrm.RequestUri.OriginalString);
            Assert.Equal("GET", hrm.Method.ToString());
        }
    }
}
