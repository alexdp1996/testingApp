namespace Infrastructure.Dtos
{
    public class OrderRequest
    {
        public string Name { get; set; }
        public Guid CustomerId { get; set; }
    }
}
