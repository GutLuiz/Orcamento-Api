using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamento.Models;
using System.Security.Claims;

namespace Orcamento.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetMe()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new
            {
                email = email
            });
        }
    }
}
