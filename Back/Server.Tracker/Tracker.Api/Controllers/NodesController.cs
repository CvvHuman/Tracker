using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tracker.Application.Features.Nodes.Quires.GetNodes;
namespace Tracker.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NodesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await Mediator.Send(new GetNodesQuery());
            return Ok(result);
        }
    }
}
