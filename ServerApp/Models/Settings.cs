using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApp.Models
{
    public class Settings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public required string Id { get; set; }
        public string? SelectedOperations { get; set; }
        public bool IsSpaceOnConcatenation { get; set; }
    }
}
