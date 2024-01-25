using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVentas.Dal.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.Dal.Respositorios.Contrato;
using SistemaVentas.Dal.Respositorios;
using SistemaVentas.Utility;
using SistemaVentas.ELL.Servicios.Contrato;
using SistemaVentas.ELL.Servicios;

namespace SistemaVentas.IOC
{
    public static class Dependencia
    {

        public static void InyectarDependencias( this IServiceCollection services  , IConfiguration configuration)
        {
            services.AddDbContext<DbventaContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("cadenaSQL"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository, VentaRepository>();

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped<IRolService , RolService>();
            services.AddScoped<IUsuarioService , UsuarioService>();
            services.AddScoped<ICategoriaService , CategoriaService>();
            services.AddScoped<IProductoService , ProductoService>();
            services.AddScoped<IVentaService , VentaService>();
            services.AddScoped<IDashBoarService , DashBoardService>();
            services.AddScoped<IMenuService , MenuService>();


        }
      

    }
}
