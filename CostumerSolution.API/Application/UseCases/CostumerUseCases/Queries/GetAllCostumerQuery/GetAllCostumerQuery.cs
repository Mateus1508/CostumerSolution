using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.Response;
using MediatR;

namespace CostumerSolution.API.Application.UseCases.CostumerUseCases.Queries.GetAllCostumersQuery
{
    public class GetAllCostumerQuery : IRequest<BaseResponse<IEnumerable<CostumerDTO>>>
    {
    }
}
