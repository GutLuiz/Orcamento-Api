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

        [HttpGet("cards")]
        public async Task<IActionResult> GetCardsDashboard()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var dados = await _dashboardService.BuscarValoresCards(userId);

            return Ok(dados);
        }
        [HttpGet("graficos")]
        public async Task<IActionResult> GetGraficoDashboard()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var dados = await _dashboardService.BuscarValoresGrafico(userId);

            return Ok(dados);
        }
        [HttpGet("listas")]
        public async Task<IActionResult> GetListaDashboard()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var dados = await _dashboardService.BuscarValoresLista(userId);

            return Ok(dados);
        }
    }
}