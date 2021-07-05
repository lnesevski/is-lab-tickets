using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTickets.Domain.DTO
{
    public class UserRoleDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}
