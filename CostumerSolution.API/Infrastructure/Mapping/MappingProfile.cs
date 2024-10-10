using AutoMapper;
using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.CreateCostumerCommand;
using CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.UpdateCostumerCommand;
using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Domain.ValueObjects;

namespace CostumerSolution.API.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCostumerCommand, Costumer>()
                .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj))
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones.Select(t => new Telefone(t.Value))))
                .ForMember(dest => dest.Emails, opt => opt.MapFrom(src => src.Emails));

            CreateMap<UpdateCostumerCommand, Costumer>()
                .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj))
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones.Select(t => new Telefone(t.Value))))
                .ForMember(dest => dest.Emails, opt => opt.MapFrom(src => src.Emails));

            CreateMap<CostumerDTO, Costumer>()
                .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => new CNPJ(src.Cnpj)))
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones.Select(t => new Telefone(t.Value))))
                .ForMember(dest => dest.Emails, opt => opt.MapFrom(src => src.Emails));

            CreateMap<Costumer, CostumerDTO>()
                .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj.Value))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones.Select(t => new Telefone(t.Value))))
                .ForMember(dest => dest.Emails, opt => opt.MapFrom(src => src.Emails))
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos));

            CreateMap<AddressDTO, Endereco>()
                .ReverseMap();
        }
    }
}
