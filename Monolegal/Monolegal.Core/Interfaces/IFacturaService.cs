using Monolegal.Shared.DTOs;
using Monolegal.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monolegal.Domain.Entities;

namespace Monolegal.Core.Interfaces
{
    public interface IFacturaService
    {
        Task<Response<List<FacturaDTO>>> GetInvoices();
        Task<Response<FacturaDTO>> GetInvoiceById(string id);
        Task <Response<FacturaDTO>> UpdateInvoice(string id);
        Task<Response<FacturaDTO>> AddInvoice(FacturaDTO factura);
        Task<Response<List<FacturaDTO>>> InicializarDatosAsync();
    }
}
