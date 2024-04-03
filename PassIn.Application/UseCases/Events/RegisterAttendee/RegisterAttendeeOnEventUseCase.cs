using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendee
{
    public class RegisterAttendeeOnEventUseCase
    {
        private readonly PassInDBContext _dbContext;

        public RegisterAttendeeOnEventUseCase()
        {
            _dbContext = new PassInDBContext();
        }

        public ResponseRegisteredJson Execute(Guid eventId, RequestRegisterEventJson request)
        {
            Validate(eventId, request);

            var attendee = new Attendee
            {
                Name = request.Name,
                Email = request.Email,
                Event_Id = eventId,
                Created_At = DateTime.UtcNow,
            };

            _dbContext.Attendees.Add(attendee);
            _dbContext.SaveChanges();

            return new ResponseRegisteredJson { Id = attendee.Id };
        }

        private void Validate(Guid eventId, RequestRegisterEventJson request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ErrorOnValidationException("Name is required");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ErrorOnValidationException("Email is required");
            }

            if (!IsEmailValid(request.Email))
            {
                throw new ErrorOnValidationException($"{request.Email} is not a valid email");
            }

            var eventEntity = _dbContext.Events.Find(eventId) ?? throw new NotFoundException($"Event with id {eventId} not found");

            var alreadyRegistered = _dbContext.Attendees.Any(a => a.Email.Equals(request.Email) && a.Event_Id == eventId);
            if (alreadyRegistered)
            {
                throw new ConflictException("This email is already registered");
            }

            var eventHasMaxAttendees = _dbContext.Attendees.Count(a => a.Event_Id == eventId) >= eventEntity.Maximum_Attendees;
            if (eventHasMaxAttendees)
            {
                throw new ConflictException("This event has reached the maximum number of attendees");
            }
        }

        private bool IsEmailValid(string email)
        {
            try
            {
                _ = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
