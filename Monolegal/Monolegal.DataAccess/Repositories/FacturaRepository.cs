using MongoDB.Driver;
using Monolegal.DataAccess.Interfaces;
using Monolegal.DataAccess.Persistence;
using Monolegal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monolegal.DataAccess.Repositories
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly IMongoCollection<Factura> _facturas;

        public FacturaRepository(DbContext dbContext)
        {
            _facturas = dbContext.Facturas;
        }
        public async Task<Factura> GetInvoiceById(string id)
        {
            return await _facturas.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Factura>> GetInvoices()
        {
            return await _facturas.Find(f => true).ToListAsync();
        }

        public async Task<Factura> UpdateInvoice(Factura factura)
        {
            await _facturas.ReplaceOneAsync(f => f.Id == factura.Id, factura);
            return factura;
        }

        public async Task<Factura> AddInvoice(Factura factura)
        {
            await _facturas.InsertOneAsync(factura);
            return factura;
        }
    }
}
