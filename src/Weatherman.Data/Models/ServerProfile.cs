using System;
using System.ComponentModel.DataAnnotations;

namespace Weatherman.Data.Models
{
    public class ServerProfile
    {
        [Required]
        public string Id { get; set; }
        public string Prefix { get; set; }
        public string LastChangedBy { get; set; }
        public DateTime? LastChangedDate { get; set; }
    }
}
