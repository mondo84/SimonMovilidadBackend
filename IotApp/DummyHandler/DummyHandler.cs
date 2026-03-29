using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace IotApp.DummyHandler
{
    
    public class DummyHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public DummyHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        ) : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // No hace nada, porque tu middleware ya pone HttpContext.User
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
