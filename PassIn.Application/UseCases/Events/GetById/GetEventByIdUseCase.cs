using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.GetById
{
    public class GetEventByIdUseCase
    {
        public ResponseEventJson Execute(Guid id)
        {
            var dbContext = new PassInDBContext();
            var entity = dbContext.Events.Find(id);

            return entity == null
                ? throw new PassInException("An event with the specified ID was not found.")
                : new ResponseEventJson
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Details = entity.Details,
                    MaximumAttendees = entity.Maximum_Attendees
                };
        }
    }
}
