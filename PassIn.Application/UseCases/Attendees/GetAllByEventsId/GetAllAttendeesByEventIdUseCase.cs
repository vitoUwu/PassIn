using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Attendees.GetAllByEventsId
{
    public class GetAllAttendeesByEventIdUseCase
    {
        private readonly PassInDBContext _dbContext;

        public GetAllAttendeesByEventIdUseCase()
        {
            _dbContext = new PassInDBContext();
        }

        public ResponseAllAttendeesJson Execute(Guid eventId)
        {
            var eventEntity = _dbContext.Events
                .Include(e => e.Attendees)
                .ThenInclude(a => a.CheckIn)
                .FirstOrDefault(e => e.Id == eventId) ?? throw new NotFoundException($"Event with id {eventId} not found");

            return new ResponseAllAttendeesJson
            {
                Attendees = eventEntity.Attendees.Select(a => new ResponseAttendeeJson
                {
                    Id = a.Id,
                    Name = a.Name,
                    Email = a.Email,
                    CreatedAt = a.Created_At,
                    CheckedInAt = a.CheckIn?.Created_at,
                }).ToList()
            };
        }
    }
}
