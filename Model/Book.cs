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
        [StringLength(255)]
        public string Title { get; set; } = null!;

        [Column("author")]
        [StringLength(555)]
        public string Author { get; set; } = null!;

        [Column("description")]
        [MaxLength(5000)]
        public string? Description { get; set; } = null!;


        [Column("pages")]
        public int Pages { get; set; }

        [Column("rating")]
        public decimal? Rating { get; set; }

        [Column("publisher_id")]
        public int PublisherId { get; set; }

        [ForeignKey("PublisherId")]
        [InverseProperty("Books")]
        public virtual Publisher? Publisher { get; set; } = null!;
    }
}
