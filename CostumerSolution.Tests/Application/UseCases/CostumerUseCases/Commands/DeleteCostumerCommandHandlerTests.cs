using AutoMapper;
using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.DeleteCostumerCommand;
using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Domain.Interfaces;
using CostumerSolution.API.Domain.ValueObjects;
using Moq;


namespace CostumerSolution.Tests.Application.UseCases.CostumerUseCases.Commands
{
    public class DeleteCostumerCommandHandlerTests
    {
        private readonly Mock<ICostumerRepository> _costumerRepositoryMock;
        private readonly IMapper _mapper;
        private readonly DeleteCostumerCommandHandler _handler;

        public DeleteCostumerCommandHandlerTests()
        {
            _costumerRepositoryMock = new Mock<ICostumerRepository>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CostumerDTO, Costumer>());
            _mapper = config.CreateMapper();
            _handler = new DeleteCostumerCommandHandler(_costumerRepositoryMock.Object, _mapper);
        }

        [Fact(DisplayName = "Deleção de cliente realizada com sucesso")]
        public async Task HandleDeleteCostumerReturnsSuccessResponse()
        {
            var cnpj = new CNPJ("12345678000195");
            var command = new DeleteCostumerCommand(cnpj);
            var costumer = new Costumer { Cnpj = command.Cnpj };
            _costumerRepositoryMock.Setup(repo => repo.GetByCnpj(command.Cnpj, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(costumer);
            _costumerRepositoryMock.Setup(repo => repo.Delete(costumer, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Cliente deletado com sucesso.", result.Message);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact(DisplayName = "Deleção de Cliente retorna cliente não encontrado")]
        public async Task HandleCostumerReturnsNotFoundResponse()
        {
            var cnpj = new CNPJ("12345678000195");
            var command = new DeleteCostumerCommand(cnpj);
            _costumerRepositoryMock.Setup(repo => repo.GetByCnpj(command.Cnpj, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync((Costumer?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Cliente não encontrado.", result.Message);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
