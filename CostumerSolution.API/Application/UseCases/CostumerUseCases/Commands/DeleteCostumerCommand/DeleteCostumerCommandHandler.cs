using AutoMapper;
using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.Response;
using CostumerSolution.API.Domain.Interfaces;
using MediatR;

namespace CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.DeleteCostumerCommand
{
    public class DeleteCostumerCommandHandler : IRequestHandler<DeleteCostumerCommand, BaseResponse<CostumerDTO>>
    {
        private readonly ICostumerRepository _clienteRepository;
        private readonly IMapper _mapper;

        public DeleteCostumerCommandHandler(ICostumerRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        async public Task<BaseResponse<CostumerDTO>> Handle(DeleteCostumerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cliente = await _clienteRepository.GetByCnpj(request.Cnpj, cancellationToken);

                if (cliente == null)
                {
                    return new BaseResponse<CostumerDTO>(false, "Cliente não encontrado.", 404);
                }

                var response = await _clienteRepository.Delete(cliente, cancellationToken);

                string message = response ? "Cliente deletado com sucesso." : "Erro ao deletar cliente. Tente novamente";
                int statusCode = response ? 200 : 400;

                return new BaseResponse<CostumerDTO>(
                    response,
                    message,
                    statusCode
                );
            }
            catch (Exception ex)
            {
                return new BaseResponse<CostumerDTO>(false, $"Erro ao deletar cliente: {ex.Message}", 500);
            }
        }
    }
}
