using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace RacketSpeed.Controllers
{
    /// <summary>
    /// Provides functionality to the /Users/ route.
    /// </summary>
    [Authorize]
    public class UsersController : Controller
    {
        public IActionResult Deposit()
        {
            return View();
        }
    }
}

