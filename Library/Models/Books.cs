using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Books
    {
        public enum BookCategory
        {
            Adventure,
            Comedy,
            Drama,
            Horror,
            Romantic
        }
        [Key]
        public int BookId { get; set; }
        public string BookName { get; set; }
        [Required]
        [DisplayName("Author")]
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        [Display(Name = "Author")]
        public Authors Authors { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public string Material { get; set; }
        public BookCategory Category { get; set; }
    }
}
