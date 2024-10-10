using AutoMapper;
using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.CreateCostumerCommand;
using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Domain.Interfaces;
using FluentValidation;
using Moq;
using CostumerSolution.API.Domain.Enums;
using CostumerSolution.API.Domain.ValueObjects;
using FluentValidation.Results;

public class CreateCostumerCommandHandlerTests
{
    private readonly Costumer _costumer;
    private readonly Mock<IValidator<Costumer>> _validator;
    private readonly Mock<ICostumerRepository> _costumerRepository;
    private readonly Mock<IMapper> _mapper;

    public CreateCostumerCommandHandlerTests()
    {
        _costumer = new Costumer();
        _validator = new Mock<IValidator<Costumer>>();
        _costumerRepository = new Mock<ICostumerRepository>();
        _mapper = new Mock<IMapper>();
    }

    [Fact(DisplayName = "Criação de cliente realizada sucesso")]
    public async Task HandleCreateCostumerReturnsSuccessResponse()
    {
        var costumerDto = new CostumerDTO
        {
            Cnpj = "12345678000199",
            Nome = "Cliente Teste",
            Status = CostumerStatus.Ativo,
            Enderecos = new List<Endereco> { new Endereco("12345678", "Rua A", "Bairro A", "BA", "Salvador") },
            Telefones = new List<Telefone> { new Telefone("(11)1234-5678") },
            Emails = new List<Email> { new Email("cliente@gmail.com") }
        };

        _validator.Setup(v => v.ValidateAsync(It.IsAny<Costumer>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ValidationResult());

        _costumerRepository.Setup(repo => repo.Add(It.IsAny<Costumer>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(true);

        _mapper.Setup(m => m.Map<Costumer>(It.IsAny<CostumerDTO>())).Returns(new Costumer());

        var command = new CreateCostumerCommand(costumerDto);
        var handler = new CreateCostumerCommandHandler(_costumerRepository.Object, _mapper.Object, _validator.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal("Cliente criado com sucesso.", result.Message);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact(DisplayName = "Erro ao criar cliente com CNPJ inválido")]
    public async Task HandleInvalidCostumerReturnsValidationError()
    {
        var costumerDto = new CostumerDTO
        {
            Cnpj = "123",
            Nome = "Cliente Inválido",
        };

        var validationResult = new ValidationResult(new List<ValidationFailure>
    {
        new ValidationFailure("Cnpj", "CNPJ inválido.")
    });

        _validator.Setup(v => v.ValidateAsync(It.IsAny<Costumer>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(validationResult);

        var command = new CreateCostumerCommand(costumerDto);
        var handler = new CreateCostumerCommandHandler(_costumerRepository.Object, _mapper.Object, _validator.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal("CNPJ inválido.", result.Message);
        Assert.Equal(400, result.StatusCode);
    }
}
