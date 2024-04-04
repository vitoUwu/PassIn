using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.CheckIns.DoCheckIn
{
    public class DoAttendeeCheckInUseCase
    {
        private readonly PassInDBContext _dbContext;

        public DoAttendeeCheckInUseCase()
        {
            _dbContext = new PassInDBContext();
        }

        public ResponseRegisteredJson Execute(Guid attendeeId)
        {
            Validate(attendeeId);

            var checkInEntity = new CheckIn
            {
                Attendee_Id = attendeeId,
                Created_at = DateTime.Now,
            };

            _dbContext.CheckIns.Add(checkInEntity);
            _dbContext.SaveChanges();

            return new ResponseRegisteredJson
            {
                Id = checkInEntity.Id,
            };
        }

        private void Validate(Guid attendeeId)
        {
            var attendeeExists = _dbContext.Attendees.Any(a => a.Id == attendeeId);
            if (!attendeeExists)
            {
                throw new NotFoundException($"Attendee with id {attendeeId} not found");
            }

            var attendeeAlreadyCheckedIn = _dbContext.CheckIns.Any(c => c.Attendee_Id == attendeeId);
            if (attendeeAlreadyCheckedIn)
            {
                throw new ConflictException($"Attendee with id {attendeeId} already checked in");
            }
        }
    }
}
