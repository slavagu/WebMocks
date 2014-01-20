using System;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WebMocks
{
    public class WebServiceMock : IDisposable
    {
        private readonly WebServiceHost _serviceHost;

        public WebServiceMock(IMockableWebService serviceInstance)
            : this(serviceInstance, new Uri("http://127.0.0.1/"))
        {
        }

        public WebServiceMock(IMockableWebService serviceInstance, Uri baseAddress)
        {
            BaseAddress = baseAddress;

            // assign free port if not specified
            if (BaseAddress.IsDefaultPort)
            {
                var uriBuilder = new UriBuilder(baseAddress);
                var port = FindUnusedPort();
                uriBuilder.Port = port;
                BaseAddress = uriBuilder.Uri;
            }

            _serviceHost = new WebServiceHost(serviceInstance, BaseAddress);

            // mark service instance as singleton to make it work
            var behaviour = _serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            behaviour.InstanceContextMode = InstanceContextMode.Single;

            _serviceHost.Open();
        }

        public Uri BaseAddress { get; private set; }

        private int FindUnusedPort()
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                // passing 0 here finds unused port automatically
                socket.Bind(new IPEndPoint(ipAddress, 0));
                return ((IPEndPoint)socket.LocalEndPoint).Port;
            }
        }

        private void CloseHost()
        {
            _serviceHost.Close();
        }


        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    CloseHost();
                }

                _disposed = true;
            }
        }

        ~WebServiceMock()
        {
            Dispose(false);
        }

        #endregion
    }
}
