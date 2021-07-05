using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieTickets.Domain.DTO
{
    public class CreateRoleDto
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
