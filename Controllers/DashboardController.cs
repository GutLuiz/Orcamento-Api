using Microsoft.AspNetCore.Mvc;
using Orcamento.Services;
using System.Security.Claims;

namespace Orcamento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var dados = await _dashboardService.BuscarValores(userId);

            return Ok(dados);
        }
    }
}