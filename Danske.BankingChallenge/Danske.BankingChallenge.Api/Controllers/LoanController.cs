using System.Threading.Tasks;
using Danske.BankingChallenge.Api.CQRS.CalculateLoan;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Danske.BankingChallenge.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoanController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoanController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<CalculateLoanQueryResult> Calculate([FromQuery] CalculateLoanQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
