using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using VehicleAPI.Interfaces;
using VehicleAPI.Models;

namespace VehicleAPI.Authentication
{
    public class BasicAuthentication : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        IRepository<User> _userRepo;
        VehicleTrackingContext context;

        public BasicAuthentication(IOptionsMonitor<AuthenticationSchemeOptions> options, 
                                    ILoggerFactory logger, 
                                    UrlEncoder encoder, 
                                    ISystemClock clock) : base(options, logger, encoder, clock)
        {
            context = new VehicleTrackingContext();
            _userRepo = new BaseRepository<User>(context);
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            User user = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1];
                user = await this.Authenticate(username, password);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
            if (user == null)
                return AuthenticateResult.Fail("Invalid Username or Password");
            var claims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        public async Task<User> Authenticate(string username,string password)
        {
            User user = await _userRepo.GetAll().Where(a => a.UserName.Equals(username)
                                && a.Password.Equals(password)
                                && a.UserRole == "Administrator").FirstOrDefaultAsync();
            return user;
        }


    }
}
