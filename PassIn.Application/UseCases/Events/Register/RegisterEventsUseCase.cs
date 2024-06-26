﻿using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Events.Register
{
    public class RegisterEventsUseCase
    {
        public ResponseRegisteredJson Execute(RequestEventJson request)
        {
            Validate(request);

            var dbContext = new PassInDBContext();
            var slug = GenerateSlug(request.Title);
            var existingEvent = dbContext.Events.FirstOrDefault(e => e.Slug == slug);

            if (existingEvent != null)
            {
                throw new ConflictException("Event already exists");
            }

            var entity = new Event
            {
                Title = request.Title,
                Details = request.Details,
                Slug = slug,
                Maximum_Attendees = request.MaximumAttendees
            };

            dbContext.Events.Add(entity);
            dbContext.SaveChanges();

            return new ResponseRegisteredJson
            {
                Id = entity.Id,
            };
        }

        private static string GenerateSlug(string title)
        {
            return title.ToLower().Replace(" ", "-");
        }

        private static void Validate(RequestEventJson request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ErrorOnValidationException("Title is required");
            }

            if (string.IsNullOrWhiteSpace(request.Details))
            {
                throw new ErrorOnValidationException("Details is required");
            }

            if (request.MaximumAttendees <= 0)
            {
                throw new ErrorOnValidationException("MaximumAttendees must be greater than 0");
            }
        }
    }
}
