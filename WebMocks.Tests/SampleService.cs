using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WebMocks.Tests
{
    [ServiceContract]
    public interface ISampleService : IMockableWebService
    {
        [OperationContract]
        [WebInvoke(
            UriTemplate = "post",
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string Post(string firstName, string lastName);

        [OperationContract]
        [WebGet(
            UriTemplate = "get?first={firstName}&last={lastName}",
            ResponseFormat = WebMessageFormat.Json)]
        string Get(string firstName, string lastName);
    }

    public class SampleService : MockableWebService, ISampleService
    {
        public string Post(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                SetStatusCode(HttpStatusCode.BadRequest);
            }
            return firstName + " " + lastName;
        }

        public string Get(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                SetStatusCode(HttpStatusCode.BadRequest);
            }
            return firstName + " " + lastName;
        }
    }
}

