using AutoMapper;
using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Domain.Enums;
using CostumerSolution.API.Domain.Interfaces;
using CostumerSolution.API.Domain.ValueObjects;
using Moq;

namespace CostumerSolution.API.Application.UseCases.CostumerUseCases.Queries.GetCostumerByCnpjQuery
{
    public class GetCostumerByCnpjQueryHandlerTests
    {
        private readonly Mock<ICostumerRepository> _costumerRepository;
        private readonly Mock<IMapper> _mapper;

        public GetCostumerByCnpjQueryHandlerTests()
        {
            _costumerRepository = new Mock<ICostumerRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Fact(DisplayName = "Recuperação de cliente por CNPJ realizada com sucesso")]
        public async Task HandleGetCostumerByCnpjReturnsSuccessResponse()
        {
            var costumer = new Costumer
            {
                Cnpj = new CNPJ("12345678000199"),
                Nome = "Cliente Teste",
                Status = CostumerStatus.Ativo
            };

            var costumerDTO = new CostumerDTO
            {
                Cnpj = "12345678000199",
                Nome = "Cliente Teste",
                Status = CostumerStatus.Ativo
            };

            _costumerRepository.Setup(repo => repo.GetByCnpj(new CNPJ("12345678000199"), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(costumer);

            _mapper.Setup(m => m.Map<CostumerDTO>(It.IsAny<Costumer>()))
                   .Returns(costumerDTO);

            var handler = new GetCostumerByCnpjQueryHandler(_costumerRepository.Object, _mapper.Object);
            var query = new GetCostumerByCnpjQuery(new CNPJ("12345678000199"));

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Cliente recuperado com sucesso.", result.Message);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(costumerDTO.Cnpj, result.Data.Cnpj);
        }

        [Fact(DisplayName = "Cliente não encontrado ao buscar por CNPJ")]
        public async Task HandleGetCostumerByCnpjReturnsNotFoundResponse()
        {
            _costumerRepository.Setup(repo => repo.GetByCnpj(new CNPJ("12345678000199"), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Costumer)null);

            var handler = new GetCostumerByCnpjQueryHandler(_costumerRepository.Object, _mapper.Object);
            var query = new GetCostumerByCnpjQuery(new CNPJ("12345678000199"));

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Cliente não encontrado.", result.Message);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
