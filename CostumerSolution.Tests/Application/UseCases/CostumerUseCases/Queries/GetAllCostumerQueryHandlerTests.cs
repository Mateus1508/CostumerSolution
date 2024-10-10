using AutoMapper;
using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.UseCases.CostumerUseCases.Queries.GetAllCostumersQuery;
using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Domain.Enums;
using CostumerSolution.API.Domain.Interfaces;
using CostumerSolution.API.Domain.ValueObjects;
using Moq;

namespace CostumerSolution.API.Application.UseCases.CostumerUseCases.Queries.GetAllCostumersQuery
{
    public class GetAllCostumersQueryHandlerTests
    {
        private readonly Mock<ICostumerRepository> _costumerRepository;
        private readonly Mock<IMapper> _mapper;

        public GetAllCostumersQueryHandlerTests()
        {
            _costumerRepository = new Mock<ICostumerRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Fact(DisplayName = "busca de clientes realizada com sucesso")]
        public async Task HandleGetAllCostumersReturnsSuccessResponse()
        {
            var costumers = new List<Costumer>
            {
                new Costumer
                {
                    Cnpj = new CNPJ("12345678000199"),
                    Nome = "Cliente 1",
                    Status = CostumerStatus.Ativo
                },
                new Costumer
                {
                    Cnpj = new CNPJ("98765432000188"),
                    Nome = "Cliente 2",
                    Status = CostumerStatus.Ativo
                }
            };

            var costumerDTOs = new List<CostumerDTO>
            {
                new CostumerDTO
                {
                    Cnpj = "12345678000199",
                    Nome = "Cliente 1",
                    Status = CostumerStatus.Ativo
                },
                new CostumerDTO
                {
                    Cnpj = "98765432000188",
                    Nome = "Cliente 2",
                    Status = CostumerStatus.Ativo
                }
            };

            _costumerRepository.Setup(repo => repo.GetAll(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(costumers);

            _mapper.Setup(m => m.Map<IEnumerable<CostumerDTO>>(It.IsAny<IEnumerable<Costumer>>()))
                   .Returns(costumerDTOs);

            var handler = new GetAllCostumerQueryHandler(_costumerRepository.Object, _mapper.Object);
            var query = new GetAllCostumerQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Clientes recuperados com sucesso.", result.Message);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(2, result.Data.Count());
            Assert.Equal("12345678000199", result.Data.First().Cnpj);
        }
    }
}
