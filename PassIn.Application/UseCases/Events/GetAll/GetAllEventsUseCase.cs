using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.GetAll
{
    public class GetAllEventsUseCase
    {
        private readonly PassInDBContext _dbContext;

        public GetAllEventsUseCase()
        {
            _dbContext = new PassInDBContext();
        }

        public ResponseAllEventsJson Execute()
        {
            var events = _dbContext.Events.Include(e => e.Attendees).ToList();

            return new ResponseAllEventsJson
            {
                Events = events.Select(e => new ResponseEventJson
                {
                    Id = e.Id,
                    Title = e.Title,
                    Details = e.Details,
                    MaximumAttendees = e.Maximum_Attendees,
                    AttendeesAmount = e.Attendees.Count,
                }).ToList()
            };
        }
    }
}
