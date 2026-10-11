// Browser-facing OIDC URLs remain public; container HTTP requests use Docker DNS.
public sealed class DockerBackchannelHandler(Uri publicAuthority, Uri internalAuthority) : DelegatingHandler(new HttpClientHandler())
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri is { } uri && uri.GetLeftPart(UriPartial.Authority) == publicAuthority.GetLeftPart(UriPartial.Authority))
        {
            request.Headers.Host = uri.Authority;
            request.RequestUri = new Uri(internalAuthority, uri.PathAndQuery);
        }
        return base.SendAsync(request, cancellationToken);
    }
}
