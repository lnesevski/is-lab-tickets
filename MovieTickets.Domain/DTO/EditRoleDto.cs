using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieTickets.Domain.DTO
{
    public class EditRoleDto
    {
        
            public EditRoleDto()
            {
                Users = new List<string>();
            }

            public string Id { get; set; }

            [Required(ErrorMessage = "Role Name is required")]
            public string RoleName { get; set; }

            public List<string> Users { get; set; }
        }

}
