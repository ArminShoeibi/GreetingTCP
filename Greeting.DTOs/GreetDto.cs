namespace Greeting.DTOs
{
    public record GreetDto
    {
        public string FullName { get; init; }
        public string FirstLine { get; init; }
        public string SecondLine { get; init; }
    }
}
