using System.ComponentModel.DataAnnotations;

namespace Cargo4You.Models
{
    
    public class UserActivityModel
    {
        [Key]
        public int Id { get; set; }

        public string Data { get; set; }

        public string Url { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

    }
}
