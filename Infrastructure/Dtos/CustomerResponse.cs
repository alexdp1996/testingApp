namespace Infrastructure.Dtos
{
    public class CustomerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long OrderCount { get; set; }
    }
}
