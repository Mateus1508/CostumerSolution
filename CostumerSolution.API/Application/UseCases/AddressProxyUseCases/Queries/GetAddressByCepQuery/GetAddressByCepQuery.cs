using MediatR;
using CostumerSolution.API.Application.Response;
using CostumerSolution.API.Application.DTOs;

namespace CostumerSolution.API.Application.UseCases.AddressProxyUseCases.Queries.GetAddressByCepQuery
{
    public class GetAddressByCepQuery(string cep) : IRequest<BaseResponse<AddressDTO>>
    {
        public string Cep { get; } = cep;
    }
}
