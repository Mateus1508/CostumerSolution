using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.Response;
using CostumerSolution.API.Domain.ValueObjects;
using MediatR;

namespace CostumerSolution.API.Application.UseCases.CostumerUseCases.Queries.GetCostumerByCnpjQuery
{
    public class GetCostumerByCnpjQuery(CNPJ cnpj) : IRequest<BaseResponse<CostumerDTO>>
    {
        public CNPJ Cnpj { get; set; } = cnpj;
    }
}
