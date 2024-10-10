using CostumerSolution.API.Application.UseCases.AddressProxyUseCases.Queries.GetAddressByCepQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CostumerSolution.API.Api.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{cep}")]
        public async Task<IActionResult> GetEnderecoByCep(string cep)
        {
            var query = new GetAddressByCepQuery(cep);

            var response = await _mediator.Send(query);

            return StatusCode(response.StatusCode, response);
        }
    }
}
