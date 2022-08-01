using System;
using System.ComponentModel.DataAnnotations;

namespace nts_platform_server.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
