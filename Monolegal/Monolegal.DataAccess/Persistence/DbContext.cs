using MongoDB.Driver;
using Monolegal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Monolegal.DataAccess.Persistence
{
    public class MongoDbSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
    }
    public class DbContext
    {
        private readonly IMongoDatabase _database;
        public DbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Factura> Facturas => _database.GetCollection<Factura>("Facturas");
    }
}
