using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("Book")]
    public partial class Book
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        [StringLength(150)]
        public string Title { get; set; } = null!;

        [Column("author")]
        [StringLength(100)]
        public string Author { get; set; } = null!;

        [Column("pages")]
        public int Pages { get; set; }

        [Column("published_date")]
        public int PublishedDate { get; set; }

        [Column("publisher_id")]
        public int PublisherId { get; set; }

        [ForeignKey("PublisherId")]
        [InverseProperty("Books")]
        public virtual Publisher Publisher { get; set; } = null!;
    }
}
