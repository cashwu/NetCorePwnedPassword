using Microsoft.AspNetCore.Mvc;

namespace testPwnedPasswords.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}