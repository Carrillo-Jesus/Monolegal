using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monolegal.Domain.Entities
{
    public class Cliente
    {
        public required string Nombre { get; set; } = null!;
        public required string CorreoElectronico { get; set; } = null!;
        public string? Telefono { get; set; } = null;
    }
}
