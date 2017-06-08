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
    public class IdentityAccountService
    {
        private UserManager<IdentityTeacher> _teacherManager;
        private ExamDBEntities _entities;

        public IdentityAccountService()
        {
            _teacherManager = new UserManager<IdentityTeacher>(new UserStore<IdentityTeacher>(new IdentityDbContext<IdentityTeacher>()));
            _entities = new ExamDBEntities();
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
        /*
        public Guest GetGuest(String userName)
        {
            if (userName == null)
                return null;

            // keresés a regisztrált felhasználók között
            IdentityGuest guest = _guestManager.FindByName(userName);
            if (guest != null)
            {
                return new Guest
                {
                    Name = guest.Name,
                    Email = guest.Email,
                    Address = guest.Address,
                    PhoneNumber = guest.PhoneNumber
                };
            }

            // keresés a nem regisztrált felhasználók között
            return _entities.Guest.FirstOrDefault(c => c.UserName == userName);
        }
        */

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
            IdentityTeacher identityTeacher = _teacherManager.Find(user.UserId, "password");

            if (identityTeacher == null) // ha nem sikerült, akkor nincs bejelentkeztetés
            {
                Teacher teacher = _entities.Teacher.Find(user.UserId);

                if (teacher == null)
                {
                    return false;
                }

                IdentityResult result = _teacherManager.Create(new IdentityTeacher
                {
                    UserName = teacher.UserId,
                    UserId = teacher.UserId,
                }, "password"); // felhasználó létrehozása

                identityTeacher = _teacherManager.Find(teacher.UserId, "password");
            }

            // ha valaki már bejelentkezett, kijelentkeztetjük
            HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // bejelentkeztetjük az új felhasználót
            ClaimsIdentity claimsIdentity = _teacherManager.CreateIdentity(identityTeacher, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, claimsIdentity);
            // perzisztens bejelentkezést állítunk be, amennyiben megjegyzést kért

            // módosítjuk a felhasználók számát
            // UserCount++;

            return true;
        }

        /// <summary>
        /// Felhasználó kijelentkeztetése.
        /// </summary>
        public Boolean Logout()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // módosítjuk a felhasználók számát
            // UserCount--;

            return true;
        }

        /// <summary>
        /// Vendég regisztrációja.
        /// </summary>
        /// <param name="guest">A vendég nézetmodellje.</param>
        /*
        public Boolean Register(GuestRegistrationViewModel guest)
        {
            if (guest == null)
                return false;

            // ellenőrizzük az annotációkat
            if (!Validator.TryValidateObject(guest, new ValidationContext(guest, null, null), null))
                return false;

            if (_guestManager.FindByName(guest.UserName) != null)
                return false;
            // ha már megtaláltuk ezt a felhasználót, akkor nem regisztrálhatunk ugyanezzel a névvel

            IdentityResult result = _guestManager.Create(new IdentityGuest
            {
                UserName = guest.UserName,
                Name = guest.GuestName,
                Email = guest.GuestEmail,
                PhoneNumber = guest.GuestPhoneNumber,
                Address = guest.GuestAddress
            }, guest.UserPassword); // felhasználó létrehozása

            return result.Succeeded; // eredményben megkapjuk, sikeres volt-e a létrehozás            
        }

        /// <summary>
        /// Vendég adatainak módosítása.
        /// </summary>
        /// <param name="guest">A venség adatai.</param>
        public Boolean Modify(GuestRegistrationViewModel guest)
        {
            IdentityGuest identityCustomer = _guestManager.FindByName(guest.UserName); // név alapján keresünk
            if (identityCustomer == null)
                return false;

            identityCustomer.Name = guest.GuestName;
            identityCustomer.Address = guest.GuestAddress;
            identityCustomer.Email = guest.GuestEmail;
            identityCustomer.PhoneNumber = guest.GuestPhoneNumber;

            IdentityResult result = _guestManager.Update(identityCustomer); // frissítjük az adatait

            return result.Succeeded;
        }

        /// <summary>
        /// Jelszó megváltoztatása.
        /// </summary>
        /// <param name="user">A jelszóváltoztatás adatai.</param>
        public Boolean ChangePassword(UserPasswordChasngeViewModel user)
        {
            IdentityGuest guest = _guestManager.FindByName(user.UserName); // név alapján keresünk
            if (guest == null)
                return false;

            IdentityResult result = _guestManager.ChangePassword(guest.Id, user.UserPassword, user.NewPassword);

            return result.Succeeded; // visszaadjuk, hogy sikeres volt-e a jelszóváltoztatás
        }

        /// <summary>
        /// Vendég létrehozása (regisztráció nélkül).
        /// </summary>
        /// <param name="guest">A vendég nézetmodellje.</param>
        /// <param name="userName">A felhasznalónév.</param>
        public Boolean Create(GuestViewModel guest, out String userName)
        {
            userName = "user" + Guid.NewGuid(); // a felhasználónevet generáljuk

            if (guest == null)
                return false;

            // ellenőrizzük az annotációkat
            if (!Validator.TryValidateObject(guest, new ValidationContext(guest, null, null), null))
                return false;

            // elmentjük a felhasználó adatait
            _entities.Guest.Add(new Guest
            {
                Name = guest.GuestName,
                Address = guest.GuestAddress,
                Email = guest.GuestEmail,
                PhoneNumber = guest.GuestPhoneNumber,
                UserName = userName
            });

            try
            {
                _entities.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }
        */
    }
}