using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.Response;
using CostumerSolution.API.Domain.Enums;
using CostumerSolution.API.Domain.ValueObjects;
using MediatR;

namespace CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.CreateCostumerCommand
{
    public class CreateCostumerCommand(CostumerDTO dto) : IRequest<BaseResponse<CostumerDTO>>
    {
        public CNPJ Cnpj { get; set; } = new CNPJ(dto.Cnpj);
        public string Nome { get; set; } = dto.Nome;
        public CostumerStatus Status { get; set; } = dto.Status;
        public IReadOnlyList<Endereco> Enderecos { get; set; } = dto.Enderecos;
        public IReadOnlyList<Telefone> Telefones { get; set; } = dto.Telefones;
        public IReadOnlyList<Email> Emails { get; set; } = dto.Emails;
    }
}
 