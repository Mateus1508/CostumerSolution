using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.Response;
using CostumerSolution.API.Domain.Enums;
using CostumerSolution.API.Domain.ValueObjects;
using MediatR;

namespace CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.UpdateCostumerCommand
{
    public class UpdateCostumerCommand(CostumerDTO dto) : IRequest<BaseResponse<CostumerDTO>>
    {
        public CNPJ Cnpj { get; } = new CNPJ(dto.Cnpj);
        public string Nome { get; } = dto.Nome;
        public CostumerStatus Status { get; } = dto.Status;
        public IReadOnlyList<Endereco> Enderecos { get; } = dto.Enderecos;
        public IReadOnlyList<Telefone> Telefones { get; } = dto.Telefones;
        public IReadOnlyList<Email> Emails { get; } = dto.Emails;
    }
}
