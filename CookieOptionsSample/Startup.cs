using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WebApplication47
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, MyCookieOptions>();
            services.AddSingleton<ITicketStore, MemoryCacheTicketStore>();

            services.AddAuthentication()
                    .AddCookie()
                    .AddCookie("AnotherCookie");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                if (context.Request.Path == "/login")
                {
                    await context.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity("Cookies")));
                    return;
                }

                // Get the cookie auth result
                var cookieResult = await context.AuthenticateAsync("Cookies");

                // Get the other cookie auth result (this is just to show that options can
                // configure multiple schemes)
                var otherCookieResult = await context.AuthenticateAsync("AnotherCookie");

                if (cookieResult.Succeeded)
                {
                    await context.Response.WriteAsync("Hello World");
                    return;
                }

                await context.ChallengeAsync("Cookies");
            });
        }
    }
}
