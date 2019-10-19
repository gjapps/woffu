using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Woffu.AdminService.Tests.Utils
{
    public class TestMessageHandler : HttpMessageHandler
    {
        HttpResponseMessage message;
        public TestMessageHandler(HttpResponseMessage message) {
            this.message = message;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(message);
        }
    }
}
