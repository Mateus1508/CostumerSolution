using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.Response;
using CostumerSolution.API.Domain.ValueObjects;
using MediatR;

namespace CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.DeleteCostumerCommand
{
    public class DeleteCostumerCommand(CNPJ Cnpj) : IRequest<BaseResponse<CostumerDTO>>
    {
        public CNPJ Cnpj { get; } = Cnpj;
    }
}
