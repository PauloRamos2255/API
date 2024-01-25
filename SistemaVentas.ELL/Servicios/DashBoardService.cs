using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SistemaVentas.ELL.Servicios.Contrato;
using SistemaVentas.Dal.Respositorios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;
using System.Globalization;

namespace SistemaVentas.ELL.Servicios
{
    public class DashBoardService : IDashBoarService
    {
        private readonly IVentaRepository _vemtaRepositorio;
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;

        public DashBoardService(IVentaRepository vemtaRepositorio, IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _vemtaRepositorio = vemtaRepositorio;
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }

        private IQueryable<Venta> retronarVenta(IQueryable<Venta> tablaVenta , int retasCantidasDias)
        {
            DateTime? ultimaFecha = tablaVenta.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();
            ultimaFecha = ultimaFecha.Value.AddDays(retasCantidasDias);

            return tablaVenta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date); 
        }

        private async Task<int> TotalVentasUltimasSemanas()
        {
            int Total = 0;

            IQueryable<Venta> _ventaQuery = await _vemtaRepositorio.Consultar();
            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = retronarVenta(_ventaQuery, -7);
                Total = tablaVenta.Count();
            }

            return Total;

        }

        private async Task<string> TotalIngresosUltimasSemanas()
        {
            decimal resultado = 0;
            IQueryable<Venta> _ventaQuery = await _vemtaRepositorio.Consultar();

            if(_ventaQuery.Count() > 0)
            {
                var tablaventa = retronarVenta(_ventaQuery ,- 7);
                resultado = tablaventa.Select(v => v.Total).Sum(v => v.Value);
            }

            return Convert.ToString(resultado, new CultureInfo("es-PE"));

        }

        private async Task<int> TotalProductos()
        {
            IQueryable<Producto> _prodcutoQuery = await _productoRepositorio.Consultar();
            int Total = _prodcutoQuery.Count();
            return Total;
        }

        private async Task<Dictionary<string , int>> VentasUltimasSemana()
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();
            IQueryable<Venta> _ventaQuery = await _vemtaRepositorio.Consultar();
            if(_ventaQuery.Count() > 0)
            {
                var tablaVenta = retronarVenta(_ventaQuery, -7);
                resultado = tablaVenta.GroupBy(v => v.FechaRegistro.Value.Date).OrderBy(g => g.Key).Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() })
                    .ToDictionary(keySelector : r => r.fecha, elementSelector : r => r.total);
            }
            return resultado;
        }

        public async Task<DashBoardDTO> Resumen()
        {
            DashBoardDTO vmDashBoard = new DashBoardDTO();
            try
            {
                vmDashBoard.TotalVnetas = await TotalVentasUltimasSemanas();
                vmDashBoard.TotalIngresos = await TotalIngresosUltimasSemanas();
                vmDashBoard.TotalProductos = await TotalProductos();


                List<VentasSemanaDTO> listaVentaSemana = new List<VentasSemanaDTO>();

                foreach(KeyValuePair <string , int> iten in await VentasUltimasSemana())
                {
                    listaVentaSemana.Add(new VentasSemanaDTO()
                    {
                        Fecha = iten.Key,
                        Total = iten.Value
                    }) ;
                }

                vmDashBoard.VentasUltimasSemanas = listaVentaSemana;

            }
            catch (Exception)
            {

                throw;
            }
            return vmDashBoard;
        }
    }
}
