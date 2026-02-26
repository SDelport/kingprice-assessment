using System.ComponentModel.DataAnnotations;

namespace King_Price_Assessment.Models
{
    public class Group
    {
        [Key]
        public Guid GroupId{ get; set; } = Guid.Empty;
        public string Name { get; set; } = default!;
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
