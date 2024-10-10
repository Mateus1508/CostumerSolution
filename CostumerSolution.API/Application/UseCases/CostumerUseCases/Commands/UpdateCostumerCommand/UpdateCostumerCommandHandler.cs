using AutoMapper;
using CostumerSolution.API.Application.Response;
using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CostumerSolution.API.Domain.Entities;

namespace CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.UpdateCostumerCommand
{
    public class UpdateCostumerCommandHandler : IRequestHandler<UpdateCostumerCommand, BaseResponse<CostumerDTO>>
    {
        private readonly ICostumerRepository _costumerRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<Costumer> _costumerValidator;

        public UpdateCostumerCommandHandler(ICostumerRepository costumerRepository, IMapper mapper, IValidator<Costumer> costumerValidator)
        {
            _costumerRepository = costumerRepository;
            _mapper = mapper;
            _costumerValidator = costumerValidator;
        }

        async public Task<BaseResponse<CostumerDTO>> Handle(UpdateCostumerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingCliente = await _costumerRepository.GetByCnpj(request.Cnpj, cancellationToken);

                if (existingCliente == null)
                {
                    return new BaseResponse<CostumerDTO>(false, "Cliente não encontrado.", 404);
                }

                var costumer = _mapper.Map<Costumer>(request);

                var validationResult = await _costumerValidator.ValidateAsync(costumer, cancellationToken);

                if (!validationResult.IsValid)
                {
                    string validationMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return new BaseResponse<CostumerDTO>(false, validationMessage, 400);
                }

                var response = await _costumerRepository.Update(costumer, cancellationToken);
                
                if (!response)
                {
                    throw new Exception("Erro ao atualizar cliente.");
                }

                string message = response ? "Dados do cliente atualizados com sucesso." : "Erro ao atualizar cliente.";
                int statusCode = response ? 200 : 400;
                return new BaseResponse<CostumerDTO>(
                    response,
                    message,
                    statusCode
                );
            }
            catch (DbUpdateException dbEx)
            {
                return new BaseResponse<CostumerDTO>(false, $"Erro ao atualizar o cliente: {dbEx.Message}", 500);
            }
            catch (Exception ex)
            {
                return new BaseResponse<CostumerDTO>(false, $"Erro inesperado ao atualizar o cliente:{ex.Message}", 500);
            }
        }
    }
}
