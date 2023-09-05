using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public abstract class Base
    {
        public Guid Id { get; set; }
        [Column(TypeName = "datetime2(7)")]
        public DateTime LastUpdated { get; set; }
    }
}
