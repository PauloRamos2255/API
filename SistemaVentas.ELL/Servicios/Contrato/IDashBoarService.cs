using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DTO;

namespace SistemaVentas.ELL.Servicios.Contrato
{
    public interface IDashBoarService
    {

        Task<DashBoardDTO> Resumen();

    }
}
