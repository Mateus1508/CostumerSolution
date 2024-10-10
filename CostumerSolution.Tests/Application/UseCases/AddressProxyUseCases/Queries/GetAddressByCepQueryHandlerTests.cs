using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using CostumerSolution.API.Application.Response;
using Moq.Protected;
using CostumerSolution.API.Application.UseCases.AddressProxyUseCases.Queries.GetAddressByCepQuery;

namespace CostumerSolution.Tests.Application.UseCases.AddressProxyUseCases.Queries
{
    public class GetAddressByCepQueryHandlerTests
    {
        private readonly GetAddressByCepQueryHandler _handler;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

        public GetAddressByCepQueryHandlerTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _handler = new GetAddressByCepQueryHandler(httpClient);
        }

        [Fact(DisplayName = "Recuperação de endereço por CEP realizada com sucesso")]
        public async Task HandleGetAddressByCepReturnsSuccessResponse()
        {
            var cep = "12345678";
            var jsonResponse = JsonConvert.SerializeObject(new ViaCepResponse
            {
                Cep = "12345678",
                Logradouro = "Rua A",
                Bairro = "Bairro A",
                Uf = "SP",
                Localidade = "São Paulo",
                Complemento = "Apto 101"
            });

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json"),
                });

            var query = new GetAddressByCepQuery(cep);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Endereço recuperado com sucesso.", result.Message);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal("12345678", result.Data.Cep);
        }

        [Fact(DisplayName = "CEP não encontrado ao buscar por CEP")]
        public async Task HandleGetAddressByCepReturnsNotFoundResponse()
        {
            var cep = "87654321";

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                });

            var query = new GetAddressByCepQuery(cep);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("CEP não encontrado.", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact(DisplayName = "Erro ao desserializar a resposta do ViaCep")]
        public async Task HandleGetAddressByCepReturnsDeserializationErrorResponse()
        {
            var cep = "12345678";
            var jsonResponse = "{ invalid json }";

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json"),
                });

            var query = new GetAddressByCepQuery(cep);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Erro ao desserializar a resposta do ViaCep.", result.Message);
            Assert.Equal(500, result.StatusCode);
        }
    }
}
