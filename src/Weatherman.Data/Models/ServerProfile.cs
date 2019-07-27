using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weatherman.Data.Models
{
    public class ServerProfile
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string Prefix { get; set; }
        public string LastChangedBy { get; set; }
        public DateTime? LastChangedDate { get; set; }
    }
}
