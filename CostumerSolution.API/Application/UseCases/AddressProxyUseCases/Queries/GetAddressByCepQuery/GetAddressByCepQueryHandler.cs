using MediatR;
using CostumerSolution.API.Application.Response;
using CostumerSolution.API.Application.DTOs;
using Newtonsoft.Json;
using FluentValidation;

namespace CostumerSolution.API.Application.UseCases.AddressProxyUseCases.Queries.GetAddressByCepQuery
{
    public class GetAddressByCepQueryHandler : IRequestHandler<GetAddressByCepQuery, BaseResponse<AddressDTO>>
    {
        private readonly HttpClient _httpClient;
        private readonly IValidator<string> _addressValidator;

        public GetAddressByCepQueryHandler(IHttpClientFactory httpClientFactory, IValidator<string> addressValidator)
        {
            _httpClient = httpClientFactory.CreateClient("ViacepClient");
            _addressValidator = addressValidator;
        }

        public async Task<BaseResponse<AddressDTO>> Handle(GetAddressByCepQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _addressValidator.ValidateAsync(request.Cep);

                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return new BaseResponse<AddressDTO>(false, errors, 400);
                }

                var response = await _httpClient.GetAsync($"/ws/{request.Cep}/json/", cancellationToken);

                var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                var viaCepResponse = JsonConvert.DeserializeObject<ViaCepResponse>(jsonResponse);

                if (viaCepResponse == null)
                {
                    return new BaseResponse<AddressDTO>(false, "Erro ao desserializar a resposta do ViaCep.", 500);
                }

                if (viaCepResponse.Cep == null)
                {
                    return new BaseResponse<AddressDTO>(false, "Cep não encontrado!", 404);
                }

                var enderecoDTO = new AddressDTO(
                    viaCepResponse.Cep,
                    viaCepResponse.Logradouro,
                    viaCepResponse.Bairro,
                    viaCepResponse.Uf,
                    viaCepResponse.Localidade,
                    viaCepResponse.Complemento
                );

                return new BaseResponse<AddressDTO>(true, "Endereço recuperado com sucesso.", 200, enderecoDTO);
            }
            catch (HttpRequestException httpEx)
            {
                return new BaseResponse<AddressDTO>(false, $"Erro ao realizar a requisição ao ViaCep: {httpEx.Message}", 500);
            }
            catch (Exception ex)
            {
                return new BaseResponse<AddressDTO>(false, $"Erro inesperado ao recuperar endereço: {ex.Message}", 500);
            }
        }
    }
}
