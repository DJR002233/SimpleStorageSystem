using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleStorageSystem.AvaloniaDesktop.Handler.HttpHandler;

public class HttpSocketExceptionHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex) when (ex.InnerException is SocketException socketEx)
        {
            if (socketEx.SocketErrorCode == SocketError.ConnectionRefused)
            {
                throw new HttpRequestException("Server unavailable!", null, HttpStatusCode.ServiceUnavailable);
            }

            if (socketEx.SocketErrorCode == SocketError.TimedOut)
            {
                throw new HttpRequestException("Request timed out!\nPlease try again..", null, HttpStatusCode.RequestTimeout);
            }

            throw; // Re-throw if not a handled case
        }
        catch (TaskCanceledException)
        {
            throw new HttpRequestException("Request took too long!\nPlease try again...", null, HttpStatusCode.RequestTimeout);
        }
    }
}
