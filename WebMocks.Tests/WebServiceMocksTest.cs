using System;
using System.IO;
using System.Net;
using NUnit.Framework;

namespace WebMocks.Tests
{
    [TestFixture]
    public class WebServiceMockTest
    {
        private ISampleService _service;
        private WebServiceMock _webServiceMock;

        [SetUp]
        public void SetUp()
        {
            _service = new SampleService();
            _webServiceMock = new WebServiceMock(_service);
        }

        [TearDown]
        public void TearDown()
        {
            _webServiceMock.Dispose();
        }

        [Test]
        public void Post_WithProperArguments_ReturnsFullName()
        {
            var url = _webServiceMock.BaseAddress + "post";
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                var result = webClient.UploadString(url, "{\"firstName\":\"Bob\",\"lastName\":\"Jane\"}");
                Console.WriteLine("Response received: " + result);
                Assert.AreEqual("\"Bob Jane\"", result);
            }
        }

        [Test]
        public void Post_WithInvalidArguments_ThrowsWebException()
        {
            var url = _webServiceMock.BaseAddress + "post";
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                var ex = Assert.Throws<WebException>(() => webClient.UploadString(url, "{\"firstName\":\"Bob\"}"));
                Console.WriteLine(ex.Message);
                var responseStream = ex.Response.GetResponseStream();
                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        string result = reader.ReadToEnd();
                        Console.WriteLine("Response received: " + result);
                    }
                }
            }
        }

        [Test]
        public void Get_WithProperArguments_ReturnsFullName()
        {
            var url = _webServiceMock.BaseAddress + "get?first=Bob&last=Jane";
            using (var webClient = new WebClient())
            {
                var result = webClient.DownloadString(url);
                Console.WriteLine("Response received: " + result);
                Assert.AreEqual("\"Bob Jane\"", result);
            }
        }

        [Test]
        public void Get_WithInvalidArguments_ThrowsWebException()
        {
            var url = _webServiceMock.BaseAddress + "get?first=Bob";
            using (var webClient = new WebClient())
            {
                var ex = Assert.Throws<WebException>(() => webClient.DownloadData(url));
                Console.WriteLine(ex.Message);
                var responseStream = ex.Response.GetResponseStream();
                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        string result = reader.ReadToEnd();
                        Console.WriteLine("Response received: " + result);
                    }
                }
            }
        }

    }
}
