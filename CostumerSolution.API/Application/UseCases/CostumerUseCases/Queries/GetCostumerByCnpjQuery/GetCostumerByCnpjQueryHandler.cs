using AutoMapper;
using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.Response;
using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Domain.Interfaces;
using MediatR;

namespace CostumerSolution.API.Application.UseCases.CostumerUseCases.Queries.GetCostumerByCnpjQuery
{
    public class GetCostumerByCnpjQueryHandler : IRequestHandler<GetCostumerByCnpjQuery, BaseResponse<CostumerDTO>>
    {
        private readonly ICostumerRepository _clienteRepository;
        private readonly IMapper _mapper;

        public GetCostumerByCnpjQueryHandler(ICostumerRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<CostumerDTO>> Handle(GetCostumerByCnpjQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cliente = await _clienteRepository.GetByCnpj(request.Cnpj, cancellationToken);

                if (cliente == null)
                {
                    return new BaseResponse<CostumerDTO>(false, "Cliente não encontrado.", 404);
                }

                var clienteDTO = _mapper.Map<CostumerDTO>(cliente);

                return new BaseResponse<CostumerDTO>(true, "Cliente recuperado com sucesso.", 200, clienteDTO);
            }
            catch (Exception ex)
            {
                return new BaseResponse<CostumerDTO>(false, $"Erro ao recuperar cliente: {ex.Message}", 500);
            }
        }
    }
}
