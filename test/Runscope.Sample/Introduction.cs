using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Hapikit;
using Runscope;
using Runscope.Links;
using Runscope.Messages;

using Xunit;

namespace Examples
{
    public class Introduction
    {

        [Fact]
        public async Task GetHome()
        {

            var homeLink = new HomeLink();
            var httpClient = CreateHttpClient();

            var homeResp = await httpClient.FollowLinkAsync(homeLink);

            var home = await homeResp.Content.ReadAsRunscopeApiDocumentAsync(Home.Parse);

            var accountResp = await httpClient.FollowLinkAsync(home.DataObject.AccountLink);
            var accountDocument = await accountResp.Content.ReadAsRunscopeApiDocumentAsync();
            var account = AccountLink.InterpretMessageBody(accountDocument);
            
            
            var bucketsResp = await httpClient.FollowLinkAsync(home.DataObject.BucketsLink);
            var bucketsDocument = await bucketsResp.Content.ReadAsRunscopeApiDocumentAsync();
            var buckets = BucketsLink.InterpretMessageBody(bucketsDocument);
 
            var bucket = buckets.First();

            var collectionsResp = await httpClient.FollowLinkAsync(bucket.Collections);
            var collectionsDoc = await collectionsResp.Content.ReadAsRunscopeApiDocumentAsync();
            var collections = CollectionsLink.InterpretMessageBody(collectionsDoc);

            var messagesResp = await httpClient.FollowLinkAsync(bucket.Messages);
            var messagesDoc = await messagesResp.Content.ReadAsRunscopeApiDocumentAsync();
            var messages = MessagesLink.InterpretGetMessageBody(messagesDoc);

            var testResp = await httpClient.FollowLinkAsync(bucket.Tests);
            var testsDoc = await testResp.Content.ReadAsRunscopeApiDocumentAsync();
            var tests = TestsLink.InterpretMessageBody(testsDoc);

            var test = tests.First();

            var lastTestResultResp = await httpClient.FollowLinkAsync(test.LatestTestResult);
            var testResultDoc = await lastTestResultResp.Content.ReadAsRunscopeApiDocumentAsync();
            var testResult = TestRunLink.InterpretMessageBody(testResultDoc);

        }


        [Fact]
        public async Task GetAccountInfo()
        {
            var httpClient = CreateHttpClient();

            var accountLink = new AccountLink();

            var r = await httpClient.FollowLinkAsync(accountLink);

            var accountDoc = new RunscopeApiDocument<Account>(await r.Content.ReadAsStreamAsync(), Account.Parse);
            var account = accountDoc.DataObject;

            Assert.NotNull(account);
        }

        [Fact]
        public async Task GetBucketsList()
        {
            var httpClient = CreateHttpClient();
            var bucketLinks = new BucketsLink();

            var resp = await httpClient.FollowLinkAsync(bucketLinks);

            var buckets = await resp.Content.ReadAsRunscopeApiDocumentAsync(Bucket.Parse);

            Assert.NotNull(buckets.DataList);
        }

        [Fact]
        public async Task GetMessagesFromDefaultBucket()
        {
            var httpClient = CreateHttpClient();
            
            var bucket = await GetDefaultBucket(httpClient);

            var resp = await httpClient.FollowLinkAsync(bucket.Messages);
            var messagesDoc = await resp.Content.ReadAsRunscopeApiDocumentAsync(RunscopeMessage.Parse);

            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            Assert.True(messagesDoc.DataList.Count > 1);
        }

        private static async Task<Bucket> GetDefaultBucket(HttpClient httpClient)
        {
            var bucketsLink = new BucketsLink();

            var bresp = await httpClient.FollowLinkAsync(bucketsLink);
            var buckets = await bresp.Content.ReadAsRunscopeApiDocumentAsync(Bucket.Parse);

            var bucket = buckets.DataList.Where(b => b.Default).FirstOrDefault();
            return bucket;
        }


        [Fact]
        public async Task GetMessagesFromFirstBucketUsingTemplate()
        {
            var httpClient = CreateHttpClient();

            var bucket = await GetDefaultBucket(httpClient);

            var messagesLink = new MessagesLink()
            {
                BucketKey = bucket.Key
            };

            var resp = await httpClient.FollowLinkAsync(messagesLink);
            var messagesDoc = await resp.Content.ReadAsRunscopeApiDocumentAsync(RunscopeMessage.Parse);

            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            Assert.True(messagesDoc.DataList.Count > 1);
        }


        [Fact]
        public async Task GetSharedMessages()
        {
            var httpClient = CreateHttpClient();

            var bucket = await GetDefaultBucket(httpClient);
            
            var resp = await httpClient.FollowLinkAsync(bucket.Shared);
            var messagesDoc = await resp.Content.ReadAsRunscopeApiDocumentAsync(RunscopeMessage.Parse);

            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            Assert.True(messagesDoc.DataList.Count > 1);
        }

        [Fact]
        public async Task GetTestsFromFirstBucket()
        {
            var httpClient = CreateHttpClient();
            var buckets = new BucketsLink();

            var response = await httpClient.FollowLinkAsync(buckets);
            var bucketsDoc = await response.Content.ReadAsRunscopeApiDocumentAsync(Bucket.Parse);
            var bucket = bucketsDoc.DataList.First();

            var resp = await httpClient.FollowLinkAsync(bucket.Tests);

            var testsDoc = new RunscopeApiDocument<Test>(await resp.Content.ReadAsStreamAsync(), Test.Parse);
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        }

        [Fact]
        public async Task GetCollectionsFromFirst5Buckets()
        {
            var httpClient = CreateHttpClient();
            var buckets = new BucketsLink();

            var response = await httpClient.FollowLinkAsync(buckets);
            var bucketsDoc = await response.Content.ReadAsRunscopeApiDocumentAsync(Bucket.Parse);

            foreach (var bucket in bucketsDoc.DataList.Take(5))
            {
                var resp = await httpClient.FollowLinkAsync(bucket.Collections);
             
                var collectionDocument = new RunscopeApiDocument<Collection>(await resp.Content.ReadAsStreamAsync(), Collection.Parse);
                if (collectionDocument.DataList != null && collectionDocument.DataList.Count > 0)
                {
                    Assert.Equal(1, collectionDocument.DataList.Count);
                } 
                Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
            }
            
        }

        [Fact]
        public async Task FindLatestTestThatFailed()
        {
            var httpClient = CreateHttpClient();
            var buckets = new BucketsLink();

            var failedTests = new List<Test>();
            var response = await httpClient.FollowLinkAsync(buckets);
            var bucketsDoc = await response.Content.ReadAsRunscopeApiDocumentAsync(Bucket.Parse);

            foreach (var bucket in bucketsDoc.DataList.Take(20))
            {
                var resp = await httpClient.FollowLinkAsync(bucket.Tests);
                var testsDoc = new RunscopeApiDocument<Test>(await resp.Content.ReadAsStreamAsync(), Test.Parse);
                foreach (var test in testsDoc.DataList)
                {
                    var resResp = await httpClient.FollowLinkAsync(test.LatestTestResult);
                    var resultDoc = new RunscopeApiDocument<TestRun>(await resResp.Content.ReadAsStreamAsync(),TestRun.Parse);
                    if (resultDoc.DataObject.Result != "pass")
                    {
                        failedTests.Add(test);
                    }
                }
            }
        }



        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://api.runscope.com/")
            };
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("bearer", ApiKeys.RunscopeAPIKey);
            return httpClient;
        }

        
    }
}
