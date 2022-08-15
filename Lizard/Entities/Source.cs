using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lizard.Entities
{
    [Table(nameof(Source), Schema = Schema.Name)]
    public class Source
    {
        [Key]
        public long SourceID { get; set; }

        [Required, MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Version { get; set; } = string.Empty;
    }
}