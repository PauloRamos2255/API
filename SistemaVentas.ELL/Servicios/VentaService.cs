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
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.ELL.Servicios
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _vemtaRepositorio;
        private readonly IGenericRepository<DetalleVenta> _detalleVemtaRepositorio;
        private readonly IMapper _mapper;

        public VentaService(IVentaRepository vemtaRepositorio, IGenericRepository<DetalleVenta> detalleVemtaRepositorio, IMapper mapper)
        {
            _vemtaRepositorio = vemtaRepositorio;
            _detalleVemtaRepositorio = detalleVemtaRepositorio;
            _mapper = mapper;
        }

        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {
                var ventasGeneradas = await _vemtaRepositorio.Registrar(_mapper.Map<Venta>(modelo));
                if(ventasGeneradas.IdVenta == 0)
                {
                    throw new TaskCanceledException("No se pudo crear");
                }

                return _mapper.Map<VentaDTO>(ventasGeneradas);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<VentaDTO>> Historial(string buscarPOR, string numeroVenta, string fechainicio, string fechafin)
        {
            IQueryable<Venta> query = await _vemtaRepositorio.Consultar();
            var ListaResultado = new List<Venta>();

            try
            {
                if(buscarPOR == "fecha")
                {
                    DateTime fech_Inicio = DateTime.ParseExact(fechainicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                    DateTime fech_fin = DateTime.ParseExact(fechafin, "dd/MM/yyyy", new CultureInfo("es-PE"));

                    ListaResultado = await query.Where(v => v.FechaRegistro.Value.Date >= fech_Inicio.Date && v.FechaRegistro.Value.Date <= fech_fin.Date)
                        .Include(d => d.DetalleVenta).ThenInclude(p => p.IdProductoNavigation).ToListAsync();

                }
                else
                {
                    ListaResultado = await query.Where(v => v.NumeroDocumento == numeroVenta)
                        .Include(d => d.DetalleVenta).ThenInclude(p => p.IdProductoNavigation).ToListAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return _mapper.Map <List<VentaDTO>> (ListaResultado);
        }

       

        public async Task<List<ReporteDTO>> Reporte(string fechainicio, string fechafin)
        {
            IQueryable<DetalleVenta> query = await _detalleVemtaRepositorio.Consultar();
            var ListaResultado = new List<DetalleVenta>();

            try
            {
                DateTime fech_Inicio = DateTime.ParseExact(fechainicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                DateTime fech_fin = DateTime.ParseExact(fechafin, "dd/MM/yyyy", new CultureInfo("es-PE"));

                ListaResultado = await query.Include(p => p.IdProductoNavigation).Include(v => v.IdVentaNavigation).
                    Where(dv => dv.IdVentaNavigation.FechaRegistro.Value.Date >= fech_Inicio.Date && 
                    dv.IdVentaNavigation.FechaRegistro.Value.Date <= fech_fin.Date).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
            return _mapper.Map<List<ReporteDTO>>(ListaResultado);
        }
    }
}
