using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.Dal.DBContext;
using SistemaVentas.Dal.Respositorios.Contrato;
using SistemaVentas.Model;


namespace SistemaVentas.Dal.Respositorios
{
    public class VentaRepository:GenericRepository<Venta> ,IVentaRepository
    {
        private readonly DbventaContext dbcontext;

        public VentaRepository(DbventaContext dbcontext) :base(dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
            Venta ventaGenerada = new Venta();

            using (var transaction = dbcontext.Database.BeginTransaction())
            {
                try
                {
                    foreach (DetalleVenta dv in modelo.DetalleVenta)
                    {
                        Producto producto_encontrado = dbcontext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        if (producto_encontrado != null)
                        {
                            producto_encontrado.Stock -= dv.Cantidad;
                            dbcontext.Productos.Update(producto_encontrado);
                        }
                    }
                    await dbcontext.SaveChangesAsync();

                    NumeroDocumento correlativo = dbcontext.NumeroDocumentos.First();
                    correlativo.UltimoNumero = correlativo.UltimoNumero+1;
                    correlativo.FechaRegistro = DateTime.Now;

                    dbcontext.NumeroDocumentos.Update(correlativo);
                    await dbcontext.SaveChangesAsync();

                    int CantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", CantidadDigitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();
                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - CantidadDigitos, CantidadDigitos);
                    modelo.NumeroDocumento = numeroVenta;

                    await dbcontext.Venta.AddAsync(modelo);
                    await dbcontext.SaveChangesAsync();

                    ventaGenerada = modelo;
                    transaction.Commit();
                }
                catch 
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return ventaGenerada;
        }

    
    }
}
