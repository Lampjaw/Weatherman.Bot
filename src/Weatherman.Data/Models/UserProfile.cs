using System;
using System.ComponentModel.DataAnnotations;

namespace Weatherman.Data.Models
{
    public class UserProfile
    {
        [Required]
        public string Id { get; set; }
        public string HomeLocation { get; set; }
        public string LastLocation { get; set; }
        public DateTime? HomeLocationChangedDate { get; set; }
        public DateTime? LastLocationChangedDate { get; set; }
    }
}
