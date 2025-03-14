using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monolegal.Shared.Enums;
namespace Monolegal.Domain.Entities
{
    public class Factura
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        public required string CodigoFactura { get; set; }

        public required Cliente Cliente { get; set; }

        public required string Ciudad { get; set; }

        public required string Nit { get; set; }

        public decimal TotalFactura { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Iva { get; set; }

        public decimal Retencion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public EstadoFactura Estado { get; set; }

        public bool Pagada { get; set; }

        public DateTime? FechaPago { get; set; } = null;
    }
}
