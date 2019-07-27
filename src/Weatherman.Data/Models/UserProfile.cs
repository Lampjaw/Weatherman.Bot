using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weatherman.Data.Models
{
    public class UserProfile
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string HomeLocation { get; set; }
        public string LastLocation { get; set; }
        public DateTime? HomeLocationChangedDate { get; set; }
        public DateTime? LastLocationChangedDate { get; set; }
    }
}
