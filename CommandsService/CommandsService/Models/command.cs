﻿using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models
{
    public class command
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string HowTo { get; set; }
        [Required]
        public string CommandLine { get; set; }
        [Required]
        public int PlatformId { get; set; }
        public Platform platform { get; set; }
    }
}
