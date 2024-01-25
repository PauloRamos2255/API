using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.ELL.Servicios.Contrato;
using SistemaVentas.DTO;
using SistemaVenta.API.Utilidad;



namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoarService _dashBoarServicio;
        public DashBoardController(IDashBoarService dashBoarServicio)
        {
            _dashBoarServicio = dashBoarServicio;
        }

        [HttpGet]
        [Route("Resumen")]
        public async Task<IActionResult> Resumen()
        {
            var rsp = new Response<DashBoardDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _dashBoarServicio.Resumen();
            }
            catch (Exception X)
            {
                rsp.status = false;
                rsp.msg = X.Message;
            }

            return Ok(rsp);

        }

    }
}
