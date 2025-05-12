using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Model
{
    [Table("Publisher")]
    public partial class Publisher
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [StringLength(500)]
        public string Name { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("Publisher")] // Links to Book.Publisher navigation property
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}