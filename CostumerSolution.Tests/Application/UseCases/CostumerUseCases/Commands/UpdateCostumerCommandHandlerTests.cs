using AutoMapper;
using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.UpdateCostumerCommand;
using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Domain.Interfaces;
using FluentValidation;
using Moq;
using FluentValidation.Results;
using CostumerSolution.API.Domain.Enums;
using CostumerSolution.API.Domain.ValueObjects;

namespace CostumerSolution.Tests.Application.UseCases.CostumerUseCases.Commands;

public class UpdateCostumerCommandHandlerTests
{
    private readonly Mock<ICostumerRepository> _costumerRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IValidator<Costumer>> _validator;

    public UpdateCostumerCommandHandlerTests()
    {
        _costumerRepository = new Mock<ICostumerRepository>();
        _mapper = new Mock<IMapper>();
        _validator = new Mock<IValidator<Costumer>>();
    }

    [Fact(DisplayName = "Atualização de cliente realizada com sucesso")]
    public async Task HandleUpdateCostumerReturnsSuccessResponse()
    {
        var costumerDto = new CostumerDTO
        {
            Cnpj = "12345678000199",
            Nome = "Cliente Atualizado",
            Status = CostumerStatus.Ativo,
            Enderecos = new List<Endereco>
            {
                new Endereco("12345678", "Rua A", "Bairro A", "BA", "Salvador"),
            },
            Telefones = new List<Telefone>
            {
                new Telefone("(11)1234-5678")
            },
            Emails = new List<Email>
            {
                new Email("cliente.atualizado@gmail.com")
            }
        };

        var command = new UpdateCostumerCommand(costumerDto);

        var existingCostumer = new Costumer
        {
            Cnpj = new CNPJ(costumerDto.Cnpj),
            Nome = costumerDto.Nome,
            Status = costumerDto.Status,
            Enderecos = costumerDto.Enderecos,
            Telefones = costumerDto.Telefones,
            Emails = costumerDto.Emails
        };

        _costumerRepository.Setup(repo => repo.GetByCnpj(new CNPJ(costumerDto.Cnpj), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingCostumer);

        _mapper.Setup(m => m.Map<Costumer>(It.IsAny<UpdateCostumerCommand>()))
               .Returns(existingCostumer);

        _validator.Setup(v => v.ValidateAsync(It.IsAny<Costumer>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ValidationResult());

        _costumerRepository.Setup(repo => repo.Update(existingCostumer, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(true);

        var handler = new UpdateCostumerCommandHandler(_costumerRepository.Object, _mapper.Object, _validator.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal("Dados do cliente atualizados com sucesso.", result.Message);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact(DisplayName = "Erro ao atualizar cliente que não existe")]
    public async Task HandleUpdateCostumerReturnsNotFoundResponse()
    {
        var costumerDto = new CostumerDTO
        {
            Cnpj = "12345678000199",
            Nome = "Cliente Teste",
            Status = CostumerStatus.Ativo,
            Enderecos = new List<Endereco>(),
            Telefones = new List<Telefone>(),
            Emails = new List<Email>()
        };

        var command = new UpdateCostumerCommand(costumerDto);

        _costumerRepository.Setup(repo => repo.GetByCnpj(new CNPJ(costumerDto.Cnpj), It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Costumer)null);

        var handler = new UpdateCostumerCommandHandler(_costumerRepository.Object, _mapper.Object, _validator.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal("Cliente não encontrado.", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact(DisplayName = "Erro ao atualizar cliente com dados inválidos")]
    public async Task HandleUpdateCostumerReturnsValidationErrorResponse()
    {
        var costumerDto = new CostumerDTO
        {
            Cnpj = "123",
            Nome = "Cliente Inválido",
            Status = CostumerStatus.Inativo,
            Enderecos = new List<Endereco>(),
            Telefones = new List<Telefone>(),
            Emails = new List<Email>()
        };

        var command = new UpdateCostumerCommand(costumerDto);
        var existingCostumer = new Costumer
        {
            Cnpj = new CNPJ(costumerDto.Cnpj),
            Nome = "Cliente Original",
            Status = CostumerStatus.Inativo,
            Enderecos = new List<Endereco>(),
            Telefones = new List<Telefone>(),
            Emails = new List<Email>()
        };

        _costumerRepository.Setup(repo => repo.GetByCnpj(new CNPJ(costumerDto.Cnpj), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingCostumer);

        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("Cnpj", "CNPJ inválido.")
        });

        _validator.Setup(v => v.ValidateAsync(It.IsAny<Costumer>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(validationResult);

        var handler = new UpdateCostumerCommandHandler(_costumerRepository.Object, _mapper.Object, _validator.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal("CNPJ inválido.", result.Message);
        Assert.Equal(400, result.StatusCode);
    }
}
