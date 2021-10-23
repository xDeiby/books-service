﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2.DTOs
{
    public class AuthorCreationDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
