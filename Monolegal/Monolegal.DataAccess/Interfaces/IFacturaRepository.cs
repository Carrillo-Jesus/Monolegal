using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monolegal.Domain.Entities;

namespace Monolegal.DataAccess.Interfaces
{
    public interface IFacturaRepository
    {
        Task<List<Factura>> GetInvoices();
        Task<Factura> GetInvoiceById(string id);
        Task<Factura> UpdateInvoice(Factura factura);
        Task<Factura> AddInvoice(Factura factura);
    }
}
