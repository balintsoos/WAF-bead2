using System;

namespace MyExam.Models
{
    /// <summary>
    /// Felhasználókezelési szolgáltatás felülete.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Felhasználószám lekérdezése.
        /// </summary>
        Int32 UserCount { get; }

        /// <summary>
        /// Aktuálisan bejelentkezett felhasználó nevének lekérdezése.
        /// </summary>
        String CurrentUserName { get; }

        /// <summary>
        /// Vendégadatok lekérdezése.
        /// </summary>
        /// <param name="userName">A felhasználónév.</param>
        // Guest GetGuest(String userName);

        /// <summary>
        /// Felhasználó bejelentkeztetése.
        /// </summary>
        /// <param name="user">A felhasználó nézetmodellje.</param>
        Boolean Login(UserViewModel user);

        /// <summary>
        /// Felhasználó kijelentkeztetése.
        /// </summary>
        Boolean Logout();

        /// <summary>
        /// Vendég regisztrációja.
        /// </summary>
        /// <param name="guest">A vendég nézetmodellje.</param>
        //Boolean Register(GuestRegistrationViewModel guest);

        /// <summary>
        /// Vendég létrehozása (regisztráció nélkül).
        /// </summary>
        /// <param name="guest">A vendég nézetmodellje.</param>
        /// <param name="userName">A felhasznalónév.</param>
        //Boolean Create(GuestViewModel guest, out String userName);
    }
}
