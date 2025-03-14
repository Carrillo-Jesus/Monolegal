using Monolegal.Core.Interfaces;
using Monolegal.Shared;
using Monolegal.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using Monolegal.DataAccess.Interfaces;
using AutoMapper;
using Monolegal.Domain.Entities;
using Monolegal.Shared.Enums;
using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Monolegal.Core.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaRepository _facturaRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        public FacturaService(IFacturaRepository facturaRepository, IMapper mapper, IEmailService emailService)
        {
            _facturaRepository = facturaRepository;
            _mapper = mapper;
            _emailService = emailService;
        }
        public async Task<Response<FacturaDTO>> GetInvoiceById(string id)
        {
            try
            {
                var factura = await _facturaRepository.GetInvoiceById(id);

                if (factura == null)
                {
                    return Response<FacturaDTO>.Fail("Factura no encontrada");
                }

                var facturaDto = _mapper.Map<FacturaDTO>(factura);

                return Response<FacturaDTO>.Ok(facturaDto);
            }
            catch (Exception ex)
            {
                return Response<FacturaDTO>.Fail("Ocurrió un error: " + ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<List<FacturaDTO>>> GetInvoices()
        {
            try
            {
                var facturas = await _facturaRepository.GetInvoices();

                var facturasDTO = _mapper.Map<List<FacturaDTO>>(facturas);

                return Response<List<FacturaDTO>>.Ok(facturasDTO);
            }
            catch (Exception ex)
            {
                return Response<List<FacturaDTO>>.Fail("Ocurrió un error: " + ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }

        }

        public async Task<Response<FacturaDTO>> UpdateInvoice(string id)
        {
            try
            {
                var factura = await _facturaRepository.GetInvoiceById(id);

                if (factura == null)
                {
                    return Response<FacturaDTO>.Fail("Factura no encontrada");
                }

                switch (factura.Estado)
                {
                    case EstadoFactura.primerRecordatorio:
                        return await HandlePrimerRecordatorio(factura);

                    case EstadoFactura.SegundoRecordatorio:
                        return await HandleSegundoRecordatorio(factura);

                    case EstadoFactura.Desactivado:
                        return Response<FacturaDTO>.Fail("Factura procesada, no requiere más acciones", System.Net.HttpStatusCode.Gone);

                    default:
                        return Response<FacturaDTO>.Fail("Error", System.Net.HttpStatusCode.InternalServerError);
                }
            } catch (Exception ex) {

                return Response<FacturaDTO>.Fail("Ocurrió un error: " + ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<FacturaDTO>> AddInvoice(FacturaDTO factura)
        {
            try
            {
                var newFactura = _mapper.Map<Factura>(factura);

                if (newFactura == null)
                    return Response<FacturaDTO>.Fail("Ocurrió un error al procesar la factura", System.Net.HttpStatusCode.InternalServerError);

                await _facturaRepository.AddInvoice(newFactura);

                return Response<FacturaDTO>.Ok(factura);
            }
            catch (Exception ex)
            {
                return Response<FacturaDTO>.Fail("Ocurrió un error: " + ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        private async Task<Response<FacturaDTO>> HandlePrimerRecordatorio(Factura factura)
        {
            factura.Estado = EstadoFactura.SegundoRecordatorio;
            return await SendEmailAndUpdateInvoice(factura, "Segundo recordatorio");
        }

        private async Task<Response<FacturaDTO>> HandleSegundoRecordatorio(Factura factura)
        {
            factura.Estado = EstadoFactura.Desactivado;
            return await SendEmailAndUpdateInvoice(factura, "Desactivado");
        }

        private async Task<Response<FacturaDTO>> SendEmailAndUpdateInvoice(Factura factura, string nuevoEstado)
        {
            await _emailService.SendEmailToClient(
                factura.Cliente.CorreoElectronico,
                "Cambio de estado",
                factura.Cliente.Nombre,
                factura.CodigoFactura,
                nuevoEstado
            );

            await _facturaRepository.UpdateInvoice(factura);

            var facturaDto = _mapper.Map<FacturaDTO>(factura);

            return Response<FacturaDTO>.Ok(facturaDto);
        }


        // funcion solo para facilitar pruebas
        public async Task<Response<List<FacturaDTO>>> InicializarDatosAsync()
        {

            try
            {
                // Verificar si ya existen facturas
                var existenFacturas = await _facturaRepository.GetInvoices();
                if (existenFacturas.Count > 0)
                    return Response<List<FacturaDTO>>.Fail("Ya hay datos ingresados");

                // Crear clientes inicialesS
                var clientesPrueba = new List<Cliente>
                {
                    new Cliente
                    {
                        Nombre = "Empresa ABC S.A.",
                        CorreoElectronico = "jesusdavid4521@gmail.com",
                        Telefono = "3211234567",
                    },
                    new Cliente
                    {
                        Nombre = "Comercial XYZ Ltda",
                        CorreoElectronico = "jesusdavid4521@gmail.com",
                        Telefono = "3009876543",
                    },
                    new Cliente
                    {
                        Nombre = "Distribuidora 123",
                        CorreoElectronico = "jesusdavid4521@gmail.com",
                        Telefono = "3151112233"
                    }
                };

                    // Crear 3 facturas de prueba
                var facturasPrueba = new List<Factura>
                {
                    new Factura
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        CodigoFactura = "F-2023-001",
                        Cliente = clientesPrueba[0],
                        Ciudad = "Bogotá",
                        Nit = "900123456-7",
                        SubTotal = 1000000,
                        Iva = 190000,
                        Retencion = 30000,
                        TotalFactura = 1160000,
                        FechaCreacion = DateTime.Now.AddDays(-30),
                        Estado = EstadoFactura.primerRecordatorio,
                        Pagada = true,
                        FechaPago = DateTime.Now.AddDays(-20)
                    },
                    new Factura
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        CodigoFactura = "F-2023-002",
                        Cliente = clientesPrueba[1],
                        Ciudad = "Medellín",
                        Nit = "800987654-3",
                        SubTotal = 2500000,
                        Iva = 475000,
                        Retencion = 75000,
                        TotalFactura = 2900000,
                        FechaCreacion = DateTime.Now.AddDays(-15),
                        Estado = EstadoFactura.primerRecordatorio,
                        Pagada = false
                    },
                    new Factura
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        CodigoFactura = "F-2023-003",
                        Cliente = clientesPrueba[2],
                        Ciudad = "Cali",
                        Nit = "901234567-8",
                        SubTotal = 800000,
                        Iva = 152000,
                        Retencion = 24000,
                        TotalFactura = 928000,
                        FechaCreacion = DateTime.Now.AddDays(-5),
                        Estado = EstadoFactura.primerRecordatorio,
                        Pagada = false
                    }
                };

                foreach (var factura in facturasPrueba)
                {
                    await _facturaRepository.AddInvoice(factura);
                }

               var facturasDto = _mapper.Map<List<FacturaDTO>>(facturasPrueba);

                return Response<List<FacturaDTO>>.Ok(facturasDto);
            }
            catch (Exception ex)
            {
                return Response<List<FacturaDTO>>.Fail("Ocurrió un error: " + ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
         
        }
    }
}
