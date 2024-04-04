namespace PassIn.Infrastructure.Entities
{
    public class Attendee
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required string Email { get; set; }
        public Guid Event_Id { get; set; }
        public DateTime Created_At { get; set; }
        public CheckIn? CheckIn { get; set; }
    }
}
