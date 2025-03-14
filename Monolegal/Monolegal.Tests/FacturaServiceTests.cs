using AutoMapper;
using Moq;
using Monolegal.Core.Services;
using Monolegal.Core.Interfaces;
using Monolegal.DataAccess.Interfaces;
using Monolegal.Shared.DTOs;
using Monolegal.Domain.Entities;
using FluentAssertions;
using MongoDB.Bson;
using Monolegal.Shared.Enums;
namespace Monolegal.Tests.Services
{
    public class FacturaServiceTests
    {
        private readonly Mock<IFacturaRepository> _facturaRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly FacturaService _facturaService;

        public FacturaServiceTests()
        {
            _facturaRepositoryMock = new Mock<IFacturaRepository>();
            _mapperMock = new Mock<IMapper>();
            _emailServiceMock = new Mock<IEmailService>();

            _facturaService = new FacturaService(_facturaRepositoryMock.Object, _mapperMock.Object, _emailServiceMock.Object);
        }

        [Fact]
        public async Task GetInvoiceById_FacturaNoExiste_DeberiaRetornarError()
        {
            // Arrange
            string facturaId = "67d32f41172bdf2319edf10a";

            _facturaRepositoryMock.Setup(repo => repo.GetInvoiceById(facturaId))
                      .ReturnsAsync(default(Factura));
            // Act
            var result = await _facturaService.GetInvoiceById(facturaId);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Factura no encontrada");
        }

        [Fact]
        public async Task GetInvoiceById_FacturaExiste_DeberiaRetornarFactura()
        {
            // Arrange
            string facturaId = "67d32f41172bdf2319edf10a";
            var factura = CrearFacturaEjemplo();
            var facturaDTO = CrearFacturaDTOEjemplo();

            _facturaRepositoryMock.Setup(repo => repo.GetInvoiceById(facturaId))
                                  .ReturnsAsync(factura);

            _mapperMock.Setup(mapper => mapper.Map<FacturaDTO>(factura))
                       .Returns(facturaDTO);

            // Act
            var result = await _facturaService.GetInvoiceById(facturaId);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(facturaDTO);
        }

        [Fact]
        public async Task GetInvoices_ExistenFacturas_DeberiaRetornarLista()
        {
            // Arrange
            var facturas = new List<Factura>
            {
                CrearFacturaEjemplo("F-2025-001"),
                CrearFacturaEjemplo("F-2025-002")
            };

            var facturasDTO = new List<FacturaDTO>
            {
                CrearFacturaDTOEjemplo("F-2025-001"),
                CrearFacturaDTOEjemplo("F-2025-002")
            };

            _facturaRepositoryMock.Setup(repo => repo.GetInvoices())
                                  .ReturnsAsync(facturas);

            _mapperMock.Setup(mapper => mapper.Map<List<FacturaDTO>>(facturas))
                       .Returns(facturasDTO);

            // Act
            var result = await _facturaService.GetInvoices();

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(facturasDTO);
        }

        [Fact]
        public async Task UpdateInvoice_FacturaNoExiste_DeberiaRetornarError()
        {
            // Arrange
            string facturaId = "67d32f41172bdf2319edf10a";
            _facturaRepositoryMock.Setup(repo => repo.GetInvoiceById(facturaId))
                                  .ReturnsAsync((Factura?)null);

            // Act
            var result = await _facturaService.UpdateInvoice(facturaId);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Factura no encontrada");
        }

        [Fact]
        public async Task AddInvoice_FacturaValida_DeberiaRetornarFactura()
        {
            // Arrange
            var facturaDTO = CrearFacturaDTOEjemplo();
            var factura = CrearFacturaEjemplo();

            _mapperMock.Setup(mapper => mapper.Map<Factura>(facturaDTO))
                       .Returns(factura);

            _facturaRepositoryMock.Setup(repo => repo.AddInvoice(It.IsAny<Factura>()))
                      .ReturnsAsync((Factura f) => f);

            // Act
            var result = await _facturaService.AddInvoice(facturaDTO);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(facturaDTO);
        }

        private Factura CrearFacturaEjemplo(string codigo = "F-2023-001", bool pagada = true)
        {
            return new Factura
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CodigoFactura = codigo,
                Cliente = new Cliente
                {
                    Nombre = "Empresa ABC S.A.",
                    CorreoElectronico = "cliente@email.com",
                    Telefono = "3211234567"
                },
                Ciudad = "Bogotá",
                Nit = "900123456-7",
                SubTotal = 1000000,
                Iva = 190000,
                Retencion = 30000,
                TotalFactura = 1160000,
                FechaCreacion = DateTime.Now.AddDays(-30),
                Estado = EstadoFactura.primerRecordatorio,
                Pagada = pagada,
                FechaPago = pagada ? DateTime.Now.AddDays(-20) : null
            };
        }

        private FacturaDTO CrearFacturaDTOEjemplo(string codigo = "F-2023-001", bool pagada = true)
        {
            return new FacturaDTO
            {
                CodigoFactura = codigo,
                Cliente = new ClienteDTO
                {
                    Nombre = "Empresa ABC S.A.",
                    CorreoElectronico = "cliente@email.com",
                    Telefono = "3211234567"
                },
                Ciudad = "Bogotá",
                Nit = "900123456-7",
                SubTotal = 1000000,
                Iva = 190000,
                Retencion = 30000,
                TotalFactura = 1160000,
                FechaCreacion = DateTime.Now.AddDays(-30),
                Estado = EstadoFactura.primerRecordatorio,
                Pagada = pagada,
                FechaPago = pagada ? DateTime.Now.AddDays(-20) : null
            };
        }

    }
}