using System.ComponentModel.DataAnnotations;

namespace King_Price_Assessment.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.Empty;
        public string Name { get; set; } = default!;
        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
