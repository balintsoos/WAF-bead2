using Service.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace Service.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private AdministratorDbContext _context;
        private UserManager<IdentityGuest> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AccountController()
        {
            _context = new AdministratorDbContext();
            _userManager = new UserManager<IdentityGuest>(new UserStore<IdentityGuest>(_context));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
        }

        ~AccountController()
        {
            Dispose(false);
        }

        [Route("login/{userName}/{userPassword}")]
        [HttpGet]
        public IHttpActionResult Login(String userName, String userPassword)
        {
            try
            {
                // megkeressük a felhasználót
                IdentityGuest user = _userManager.Find(userName, userPassword);

                if (user == null) // ha nem sikerült, akkor nincs bejelentkeztetés
                    return NotFound();

                // ha valaki már bejelentkezett, kijelentkeztetjük
                HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                // bejelentkeztetjük az új felhasználót
                ClaimsIdentity claimsIdentity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, claimsIdentity);

                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("logout")]
        [HttpGet]
        [Authorize] // csak bejelentklezett felhasználóknak
        public IHttpActionResult Logout()
        {
            try
            {
                // kijelentkeztetjük az aktuális felhasználót
                HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                _userManager.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}