using AutoMapper;
using Monolegal.Domain.Entities;
using Monolegal.Shared.DTOs;
using Monolegal.Shared.Enums;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<FacturaDTO, Factura>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CodigoFactura, opt => opt.MapFrom(src => src.CodigoFactura))
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => new Cliente
            {
                Nombre = src.Cliente.Nombre,
                CorreoElectronico = src.Cliente.CorreoElectronico,
                Telefono = src.Cliente.Telefono
            }))
            .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad))
            .ForMember(dest => dest.Nit, opt => opt.MapFrom(src => src.Nit))
            .ForMember(dest => dest.TotalFactura, opt => opt.MapFrom(src => src.TotalFactura))
            .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal))
            .ForMember(dest => dest.Iva, opt => opt.MapFrom(src => src.Iva))
            .ForMember(dest => dest.Retencion, opt => opt.MapFrom(src => src.Retencion))
            .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => (EstadoFactura)src.Estado))
            .ForMember(dest => dest.Pagada, opt => opt.MapFrom(src => src.Pagada))
            .ForMember(dest => dest.FechaPago, opt => opt.MapFrom(src => src.FechaPago));

        CreateMap<Factura, FacturaDTO>()
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => (int)src.Estado));

        CreateMap<Cliente, ClienteDTO>();
        CreateMap<ClienteDTO, Cliente>();
    }
}
