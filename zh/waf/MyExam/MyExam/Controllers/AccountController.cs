using MyExam.Models;
using System.Linq;
using System.Web.Mvc;

namespace MyExam.Controllers
{
    public class AccountController : Controller
    {
        private IdentityAccountService _accountService = new IdentityAccountService();
        private ExamDBEntities db = new ExamDBEntities();

        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Bejelentkezés.
        /// </summary>
        [HttpGet]
        public ActionResult Login()
        {
            return View("Login");
        }

        /// <summary>
        /// Bejelentkezés.
        /// </summary>
        /// <param name="user">A bejelentkezés adatai.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserViewModel user)
        {
            if (!ModelState.IsValid)
                return View("Login", user);

            // bejelentkeztetjük a felhasználót
            if (!_accountService.Login(user))
            {
                // nem szeretnénk, ha a felhasználó tudná, hogy a felhasználónévvel, vagy a jelszóval van-e baj, így csak általános hibát jelzünk
                ModelState.AddModelError("", "Hibás felhasználónév, vagy jelszó.");
                return View("Login", user);
            }

            return RedirectToAction("Index", "Tasks"); // átirányítjuk a főoldalra
        }

        /// <summary>
        /// Regisztráció.
        /// </summary>
        [HttpGet]
        public ActionResult Register()
        {
            return View("Register");
        }

        /// <summary>
        /// Regisztráció.
        /// </summary>
        /// <param name="guest">Regisztrációs adatok.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(TeacherRegistrationViewModel guest)
        {
            // végrehajtjuk az ellenőrzéseket
            if (!ModelState.IsValid)
                return View("Register", guest);

            if (!_accountService.Register(guest))
            {
                ModelState.AddModelError("UserName", "A megadott felhasználónév már létezik.");
                return View("Register", guest);
            }

            _accountService.Logout(); // ha már volt valaki bejelentkezve, kijelentkeztetjük

            ViewBag.Information = "A regisztráció sikeres volt. Kérjük, jelentkezzen be.";

            return RedirectToAction("Login");
        }

        /// <summary>
        /// Kijelentkezés.
        /// </summary>
        public ActionResult Logout()
        {
            _accountService.Logout();

            return RedirectToAction("Index", "Home"); // átirányítjuk a főoldalra
        }
    }
}