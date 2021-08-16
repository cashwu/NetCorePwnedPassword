using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PwnedPasswords.Client;

namespace testPwnedPasswords.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPwnedPasswordsClient _passwordsClient;

        public HomeController(IPwnedPasswordsClient passwordsClient)
        {
            _passwordsClient = passwordsClient;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return Ok();
        }

        /// <summary>
        /// https://localhost:44314/pw?pw=123456
        /// https://localhost:44314/pw?pw=P@ssw0rd
        /// </summary>
        /// <param name="pw"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("/pw")]
        public async Task<IActionResult> Password([FromQuery] string pw, CancellationToken cancellationToken)
        {
            var result = await _passwordsClient.HasPasswordBeenPwned(pw, cancellationToken);

            Console.WriteLine($"pw - {pw}, result - {result}");

            return Ok();
        }
    }
}