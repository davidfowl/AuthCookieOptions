using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace WebApplication47
{
    internal class MyCookieOptions : IConfigureNamedOptions<CookieAuthenticationOptions>
    {
        private readonly ITicketStore _store;

        public MyCookieOptions(ITicketStore store)
        {
            _store = store;
        }

        public void Configure(string name, CookieAuthenticationOptions options)
        {
            // Only configure the "Cookies" scheme
            if (name == CookieAuthenticationDefaults.AuthenticationScheme)
            {
                options.LoginPath = "/login";
                options.SessionStore = _store;
            }
        }

        public void Configure(CookieAuthenticationOptions options)
        {
        }
    }
}