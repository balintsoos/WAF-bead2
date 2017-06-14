using MyLibrary.Models;
using System.Web.Mvc;
using System.Linq;

namespace MyLibrary.Controllers
{
    /// <summary>
    /// Vezrlő ősosztálya.
    /// </summary>
    public class BaseController : Controller
    {
        protected IdentityAccountService _accountService;

        public BaseController()
        {
            _accountService = new IdentityAccountService();

            if (_accountService.CurrentUserName != null)
                ViewBag.CurrentGuestName = _accountService.CurrentUserName;
        }
    }
}