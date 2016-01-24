﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eSIS.Core.Entities.Infrastructure
{
    [Table("UserPassword", Schema = "inf")]
    public class UserPassword : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public byte[] Password { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public virtual User User { get; set; }
    }
}