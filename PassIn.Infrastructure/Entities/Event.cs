using System.ComponentModel.DataAnnotations.Schema;

namespace PassIn.Infrastructure.Entities
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Title { get; set; }
        public required string Details { get; set; }
        public required string Slug { get; set; }
        public required int Maximum_Attendees { get; set; }

        [ForeignKey("Event_Id")]
        public List<Attendee> Attendees { get; set; } = [];
    }
}
