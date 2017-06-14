using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace MyLibrary.Models
{
    public class IdentityAccountService
    {
        private UserManager<IdentityUser> _userManager;

        public IdentityAccountService()
        {
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext<IdentityUser>()));
        }

        /// <summary>
        /// Aktuálisan bejelentkezett felhasználó nevének lekérdezése.
        /// </summary>
        public String CurrentUserName
        {
            get
            {
                String name = HttpContext.Current.User.Identity.Name;

                return String.IsNullOrEmpty(name) ? null : name;
            }
        }

        public Boolean Login(UserViewModel user)
        {
            if (user == null)
                return false;

            // ellenőrizzük az annotációkat
            if (!Validator.TryValidateObject(user, new ValidationContext(user, null, null), null))
                return false;

            // megkeressük a felhasználót
            IdentityUser identityUser = _userManager.Find(user.UserName, user.UserPassword);

            if (identityUser == null) // ha nem sikerült, akkor nincs bejelentkeztetés
                return false;

            // ha valaki már bejelentkezett, kijelentkeztetjük
            HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // bejelentkeztetjük az új felhasználót
            ClaimsIdentity claimsIdentity = _userManager.CreateIdentity(identityUser, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, claimsIdentity);
            // perzisztens bejelentkezést állítunk be, amennyiben megjegyzést kért

            return true;
        }

        /// <summary>
        /// Felhasználó kijelentkeztetése.
        /// </summary>
        public Boolean Logout()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return true;
        }

        /// <summary>
        /// Vendég regisztrációja.
        /// </summary>
        /// <param name="guest">A vendég nézetmodellje.</param>
        public Boolean Register(UserViewModel user)
        {
            if (user == null)
                return false;

            // ellenőrizzük az annotációkat
            if (!Validator.TryValidateObject(user, new ValidationContext(user, null, null), null))
                return false;

            if (_userManager.FindByName(user.UserName) != null)
                return false;
            // ha már megtaláltuk ezt a felhasználót, akkor nem regisztrálhatunk ugyanezzel a névvel

            IdentityResult result = _userManager.Create(new IdentityUser
            {
                UserName = user.UserName,
            }, user.UserPassword); // felhasználó létrehozása

            return result.Succeeded; // eredményben megkapjuk, sikeres volt-e a létrehozás            
        }
    }
}