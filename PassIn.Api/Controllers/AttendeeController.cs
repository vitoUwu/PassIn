using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Attendees.GetAllByEventsId;
using PassIn.Application.UseCases.Events.RegisterAttendee;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;

namespace PassIn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeeController : ControllerBase
    {
        [HttpPost]
        [Route("{eventId}/register")]
        [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
        public IActionResult RegisterAttendee([FromBody] RequestRegisterEventJson request, [FromRoute] Guid eventId)
        {
            var useCase = new RegisterAttendeeOnEventUseCase();
            var response = useCase.Execute(eventId, request);

            return Created("", new ResponseRegisteredJson { Id = response.Id });
        }

        [HttpGet]
        [Route("{eventId}")]
        [ProducesResponseType(typeof(ResponseAllAttendeesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public IActionResult GetAll([FromRoute] Guid eventId)
        {
            var useCase = new GetAllAttendeesByEventIdUseCase();
            var response = useCase.Execute(eventId);

            return Ok(response);
        }
    }
}
