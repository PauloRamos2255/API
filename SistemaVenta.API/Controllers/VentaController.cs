using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.ELL.Servicios.Contrato;
using SistemaVentas.DTO;
using SistemaVenta.API.Utilidad;



namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {

        private IVentaService _ventasServicio;

        public VentaController(IVentaService ventasServicio)
        {
            _ventasServicio = ventasServicio;
        }

        [HttpPost]
        [Route("Registrar")]

        public async Task<IActionResult> Registrar([FromBody] VentaDTO venta)
        {
            var rsp = new Response<VentaDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _ventasServicio.Registrar(venta);
            }
            catch (Exception X)
            {
                rsp.status = false;
                rsp.msg = X.Message;
            }

            return Ok(rsp);
        }


        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Historial( string buscarPor , string? numeroVenta  , string? fechaInicio , string? fechaFin)
        {
            var rsp = new Response<List<VentaDTO>>();
            numeroVenta = numeroVenta is null ? "" : numeroVenta;
            fechaInicio = fechaInicio is null ? "" : fechaInicio;
            fechaFin = fechaFin is null ? "" : fechaFin;

            try
            {
                rsp.status = true;
                rsp.value = await _ventasServicio.Historial( buscarPor , numeroVenta , fechaInicio , fechaFin);
            }
            catch (Exception X)
            {
                rsp.status = false;
                rsp.msg = X.Message;
            }

            return Ok(rsp);

        }


        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Reporte(string? fechaInicio, string? fechaFin)
        {
            var rsp = new Response<List<ReporteDTO>>();
            

            try
            {
                rsp.status = true;
                rsp.value = await _ventasServicio.Reporte(fechaInicio, fechaFin);
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
