using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Authors
    {
        [Key]
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
