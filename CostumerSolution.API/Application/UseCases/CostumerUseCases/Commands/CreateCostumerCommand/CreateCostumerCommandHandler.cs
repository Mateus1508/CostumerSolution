using AutoMapper;
using CostumerSolution.API.Application.Response;
using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.CreateCostumerCommand
{
    public class CreateCostumerCommandHandler : IRequestHandler<CreateCostumerCommand, BaseResponse<CostumerDTO>>
    {
        private readonly ICostumerRepository _costumerRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<Costumer> _costumerValidator;

        public CreateCostumerCommandHandler(ICostumerRepository costumerRepository, IMapper mapper, IValidator<Costumer> costumerValidator)
        {
            _costumerRepository = costumerRepository;
            _mapper = mapper;
            _costumerValidator = costumerValidator;

        }

        async public Task<BaseResponse<CostumerDTO>> Handle(CreateCostumerCommand request, CancellationToken cancellationToken)
        {
            var costumer = _mapper.Map<Costumer>(request);

            var validationResult = await _costumerValidator.ValidateAsync(costumer, cancellationToken);

            if (!validationResult.IsValid)
            {
                string message = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<CostumerDTO>(false, message, 400);
            }

            try
            {
                var response = await _costumerRepository.Add(costumer, cancellationToken);

                string message = response ? "Cliente criado com sucesso." : "Erro ao criar cliente.";
                int statusCode = response ? 200 : 400;

                return new BaseResponse<CostumerDTO>
                (
                    response,
                    message,
                    statusCode
                );
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
                {
                    return new BaseResponse<CostumerDTO>(false, "Já existe um cliente com este CNPJ.", 404);
                }

                return new BaseResponse<CostumerDTO>(false, $"Erro ao criar cliente: {ex.Message}", 500);
            }
            catch (Exception ex)
            {
                return new BaseResponse<CostumerDTO>(false, $"Erro ao criar cliente: {ex.Message}", 500);
            }
        }
    }
}
