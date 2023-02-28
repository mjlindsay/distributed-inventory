using DistributedInventory.Application.Commands;
using DistributedInventory.Application.Commands.Responses;
using DistributedInventory.Application.Queries;
using DistributedInventory.Application.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistributedInventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryCountController : ControllerBase
    {

        private readonly ILogger<InventoryCountController> _logger;

        private readonly IMediator _mediator;

        public InventoryCountController(ILogger<InventoryCountController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        //[HttpGet("{locationId}")]
        //public async Task<ActionResult<ReadAllInventoryCountsQueryResponse>> Get(
        //    string locationId,
        //    CancellationToken cancellationToken)
        //{
        //    var query = new ReadAllInventoryCountsQuery();
        //}

        [HttpGet]
        public async Task<ActionResult<ReadMultipleInventoryCountsQueryResponse>> GetAll(
            CancellationToken cancellationToken,
            [FromHeader(Name = "x-continuation-token")] string? continuationToken = null,
            [FromQuery(Name = "pageSize")] int pageSize = 10)
        {
            var query = new ReadAllInventoryCountsQuery()
            {
                ContinuationToken = continuationToken,
                PageSize = pageSize
            };

            var response = await _mediator.Send(query, cancellationToken);

            Response.Headers.Add("X-Continuation-Token", response.ContinuationToken);
            return Ok(response);
        }

        [HttpGet("{locationId}")]
        public async Task<ActionResult<ReadMultipleInventoryCountsQueryResponse>> GetAllFromLocation(
            string locationId,
            CancellationToken cancellationToken,
            [FromHeader(Name = "x-continuation-token")] string? continuationToken = null,
            [FromQuery(Name = "pageSize")] int pageSize = 10)
        {
            var query = new ReadAllInventoryCountsFromLocationQuery()
            {
                LocationId = locationId,
                ContinuationToken = continuationToken,
                PageSize = pageSize
            };

            var response = await _mediator.Send(query, cancellationToken);

            Response.Headers.Add("X-Continuation-Token", response.ContinuationToken);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<CreateInventoryCountCommandResponse>> CreateInventoryCount(
            [FromBody] CreateInventoryCountCommand command,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(CreateInventoryCount), response);
        }
    }
}
