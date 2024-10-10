using AutoMapper;
using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.Response;
using CostumerSolution.API.Domain.Interfaces;
using MediatR;

namespace CostumerSolution.API.Application.UseCases.CostumerUseCases.Queries.GetAllCostumersQuery
{
    public class GetAllCostumerQueryHandler : IRequestHandler<GetAllCostumerQuery, BaseResponse<IEnumerable<CostumerDTO>>>
    {
        private readonly ICostumerRepository _clienteRepository;
        private readonly IMapper _mapper;

        public GetAllCostumerQueryHandler(ICostumerRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<CostumerDTO>>> Handle(GetAllCostumerQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var clientes = await _clienteRepository.GetAll(cancellationToken);

                var clienteDTOs = _mapper.Map<IEnumerable<CostumerDTO>>(clientes);

                foreach (var clienteDTO in clienteDTOs)
                {
                    clienteDTO.Cnpj = clienteDTO.Cnpj.ToString();
                }

                return new BaseResponse<IEnumerable<CostumerDTO>>(true, "Clientes recuperados com sucesso.", 200, clienteDTOs);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<CostumerDTO>>(false, $"Erro ao recuperar clientes: {ex.Message}", 500);
            }
        }
    }
}
