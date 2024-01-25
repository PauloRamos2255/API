using AutoMapper;
using SistemaVentas.Dal.Respositorios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.ELL.Servicios.Contrato;
using SistemaVentas.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.ELL.Servicios
{
    public class CategoriaService: ICategoriaService
    {

        private readonly IGenericRepository<Categoria> _categoriaRepositorio;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Categoria> categoriaRepositorio, IMapper mapper)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDTO>> Lista()
        {
            try
            {

                var listaCategoria = await _categoriaRepositorio.Consultar();
                return _mapper.Map<List<CategoriaDTO>>(listaCategoria.ToList());

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
