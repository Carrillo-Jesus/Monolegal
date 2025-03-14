using Microsoft.AspNetCore.Mvc;
using Monolegal.Core.Interfaces;
using Monolegal.Domain.Entities;
using Monolegal.Shared.DTOs;

namespace Monolegal.Services.Controllers
{
    /// <summary>
    /// Controlador de Facturas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FacturasController : Controller
    {
       private readonly IFacturaService _facturaService;
       public FacturasController(IFacturaService facturaService) 
       {
            _facturaService = facturaService;
       }

        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var response = await _facturaService.GetInvoices();
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Obtiene una factura por su ID.
        /// </summary>
        /// <param name="idfactura">El ID de la factura.</param>
        /// <returns>La factura correspondiente al ID.</returns>
        [HttpGet("{idfactura}")]
        public async Task<IActionResult> GetInvoiceById(string idfactura)
        {
            var response = await _facturaService.GetInvoiceById(idfactura);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Envía un correo informando el estado de la factura
        /// </summary>
        /// <param name="idFactura">El ID de la factura para la cual se enviará el correo.</param>
        /// <returns>El estado de la operación de envío de correo.</returns>
        [HttpPost("enivar-email/{idFactura}")]
        public async Task<IActionResult> SendMail(string idFactura)
        {
            var response = await _facturaService.UpdateInvoice(idFactura);
            return StatusCode((int)response.StatusCode, response);

        }

        /// <summary>
        /// Solo ejecutar una vez iniciado el proyecto. para pruebas
        /// </summary>
        /// <returns>El estado de la operación de inicialización de datos.</returns>
        [HttpPost("agregar-datos")]
        public async Task<IActionResult> addData()
        {
            var response = await _facturaService.InicializarDatosAsync();
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
