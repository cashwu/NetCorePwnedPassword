using System;
using System.Linq;
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
        /// https://github.com/dotnet/aspnetcore/blob/bcfbd5cc47dde7f2be50a24721f24a020dc77356/src/Identity/Extensions.Core/src/PasswordValidator.cs#L39
        /// </summary>
        /// <param name="pw"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("/pw")]
        public async Task<IActionResult> Password([FromQuery] string pw, CancellationToken cancellationToken)
        {
            var isPass = CheckPw(pw);

            var result = await _passwordsClient.HasPasswordBeenPwned(pw, cancellationToken);

            Console.WriteLine($"pw - {pw}, isPass - {isPass}, is has password been - {result}");

            return Ok();
        }

        private bool CheckPw(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                Console.WriteLine("length < 6");

                return false;
            }

            // if (password.All(IsLetterOrDigit))
            // {
            //     return false;
            // }

            if (!password.Any(IsDigit))
            {
                Console.WriteLine("not digit");

                return false;
            }

            if (!password.Any(IsLower))
            {
                Console.WriteLine("not lower");

                return false;
            }

            if (!password.Any(IsUpper))
            {
                Console.WriteLine("not upper");

                return false;
            }

            // if (options.RequiredUniqueChars >= 1 && password.Distinct().Count() < options.RequiredUniqueChars)
            // {
            //     return false;
            // }

            return true;
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character is a digit.
        /// </summary>
        /// <param name="c">The character to check if it is a digit.</param>
        /// <returns>True if the character is a digit, otherwise false.</returns>
        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character is a lower case ASCII letter.
        /// </summary>
        /// <param name="c">The character to check if it is a lower case ASCII letter.</param>
        /// <returns>True if the character is a lower case ASCII letter, otherwise false.</returns>
        private bool IsLower(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character is an upper case ASCII letter.
        /// </summary>
        /// <param name="c">The character to check if it is an upper case ASCII letter.</param>
        /// <returns>True if the character is an upper case ASCII letter, otherwise false.</returns>
        private bool IsUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character is an ASCII letter or digit.
        /// </summary>
        /// <param name="c">The character to check if it is an ASCII letter or digit.</param>
        /// <returns>True if the character is an ASCII letter or digit, otherwise false.</returns>
        private bool IsLetterOrDigit(char c)
        {
            return IsUpper(c) || IsLower(c) || IsDigit(c);
        }
    }
}