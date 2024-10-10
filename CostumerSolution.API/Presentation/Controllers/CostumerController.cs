using CostumerSolution.API.Application.DTOs;
using CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.CreateCostumerCommand;
using CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.DeleteCostumerCommand;
using CostumerSolution.API.Application.UseCases.CostumerUseCases.Commands.UpdateCostumerCommand;
using CostumerSolution.API.Application.UseCases.CostumerUseCases.Queries.GetAllCostumersQuery;
using CostumerSolution.API.Application.UseCases.CostumerUseCases.Queries.GetCostumerByCnpjQuery;
using CostumerSolution.API.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CostumerSolution.API.API.Controllers
{
    [ApiController]
    [Route("api/costumer")]
    public class CostumerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CostumerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCostumers()
        {
            var query = new GetAllCostumerQuery();
            var response = await _mediator.Send(query);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{cnpj}")]
        public async Task<IActionResult> GetCostumerByCnpj(string cnpj)
        {
            var cnpjValue = new CNPJ(cnpj);
            var query = new GetCostumerByCnpjQuery(cnpjValue);
            var response = await _mediator.Send(query);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCostumer([FromBody] CostumerDTO costumerDTO)
        {
            var command = new CreateCostumerCommand(costumerDTO);
            var response = await _mediator.Send(command);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCostumer([FromBody] CostumerDTO costumerDTO)
        {
            var command = new UpdateCostumerCommand(costumerDTO);
            var response = await _mediator.Send(command);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{cnpj}")]
        public async Task<IActionResult> DeleteCostumer(string cnpj)
        {
            var cnpjValue = new CNPJ(cnpj);
            var command = new DeleteCostumerCommand(cnpjValue);
            var response = await _mediator.Send(command);

            return StatusCode(response.StatusCode, response);
        }
    }
}
