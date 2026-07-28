using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tracker.Application.Features.Tasks.Commands.CreateTask;
using Tracker.Application.Features.Tasks.Commands.DeleteTask;
using Tracker.Application.Features.Tasks.Commands.UpdateTask;

namespace Tracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ApiControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTaskCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteTaskCommand(id));
            return NoContent(); // Возвращаем 204 статус успешного удаления без тела
        }
    }
}
