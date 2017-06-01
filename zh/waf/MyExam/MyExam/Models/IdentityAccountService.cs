using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace MyExam.Models
{
    /// <summary>
    /// Felhasználókezelési szolgáltatás típusa.
    /// </summary>
    public class IdentityAccountService : IAccountService
    {
        private UserManager<IdentityTeacher> _teacherManager;
        private ExamDBEntities db;

        public IdentityAccountService()
        {
            _teacherManager = new UserManager<IdentityTeacher>(new UserStore<IdentityTeacher>(new IdentityDbContext<IdentityTeacher>()));
            db = new ExamDBEntities();
        }

        /// <summary>
        /// Felhasználószám lekérdezése.
        /// </summary>
        public Int32 UserCount
        {
            get
            {
                // a felhasználószámot globális állapotként tároljuk
                return HttpContext.Current.Application["userCount"] == null ? 0 : (Int32)HttpContext.Current.Application["userCount"];
            }
            set
            {
                HttpContext.Current.Application["userCount"] = value;
            }
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

        /// <summary>
        /// Vendégadatok lekérdezése.
        /// </summary>
        /// <param name="userName">A felhasználónév.</param>
        

        /// <summary>
        /// Felhasználó bejelentkeztetése.
        /// </summary>
        /// <param name="user">A felhasználó nézetmodellje.</param>
        public Boolean Login(UserViewModel user)
        {
            if (user == null)
                return false;

            // ellenőrizzük az annotációkat
            if (!Validator.TryValidateObject(user, new ValidationContext(user, null, null), null))
                return false;

            // megkeressük a felhasználót
            IdentityTeacher identity = _teacherManager.Find(user.UserId, user.UserPassword);
            Teacher teacher = db.Teacher.Find(user.UserId);

            if (teacher != null)
                return true;

            // ha valaki már bejelentkezett, kijelentkeztetjük
            HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // bejelentkeztetjük az új felhasználót
            ClaimsIdentity claimsIdentity = _teacherManager.CreateIdentity(identity, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = user.RememberLogin }, claimsIdentity);
            // perzisztens bejelentkezést állítunk be, amennyiben megjegyzést 

            // módosítjuk a felhasználók számát
            UserCount++;

            return true;
        }

        /// <summary>
        /// Felhasználó kijelentkeztetése.
        /// </summary>
        public Boolean Logout()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // módosítjuk a felhasználók számát
            UserCount--;

            return true;
        }

        /// <summary>
        /// Vendég regisztrációja.
        /// </summary>
        /// <param name="guest">A vendég nézetmodellje.</param>
        public Boolean Register(TeacherRegistrationViewModel guest)
        {
            if (guest == null)
                return false;

            // ellenőrizzük az annotációkat
            if (!Validator.TryValidateObject(guest, new ValidationContext(guest, null, null), null))
                return false;

            if (_teacherManager.FindByName(guest.UserId) != null)
                return false;
            // ha már megtaláltuk ezt a felhasználót, akkor nem regisztrálhatunk ugyanezzel a névvel

            IdentityResult result = _teacherManager.Create(new IdentityTeacher
            {
                UserId = guest.UserId
            }, guest.UserPassword); // felhasználó létrehozása

            return result.Succeeded; // eredményben megkapjuk, sikeres volt-e a létrehozás            
        }

        /// <summary>
        /// Vendég adatainak módosítása.
        /// </summary>
        /// <param name="guest">A venség adatai.</param

        /// <summary>
        /// Jelszó megváltoztatása.
        /// </summary>
        /// <param name="user">A jelszóváltoztatás adatai.</param>
        

        /// <summary>
        /// Vendég létrehozása (regisztráció nélkül).
        /// </summary>
        /// <param name="guest">A vendég nézetmodellje.</param>
        /// <param name="userName">A felhasznalónév.</param>
        
    }
}