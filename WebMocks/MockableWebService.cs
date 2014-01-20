using System.Net;
using System.ServiceModel.Web;

namespace WebMocks
{
    public interface IMockableWebService
    {
        void SetStatusCode(HttpStatusCode code);
    }

    public class MockableWebService : IMockableWebService
    {
        public void SetStatusCode(HttpStatusCode statusCode)
        {
            // ReSharper disable PossibleNullReferenceException
            WebOperationContext.Current.OutgoingResponse.StatusCode = statusCode;
            // ReSharper restore PossibleNullReferenceException
        }
    }
}