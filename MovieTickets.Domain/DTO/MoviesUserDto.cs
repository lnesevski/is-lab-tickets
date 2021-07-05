using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTickets.Domain.DTO
{
    public class MoviesUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserRole { get; set; }
    }
}
