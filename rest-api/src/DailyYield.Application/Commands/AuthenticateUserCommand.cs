using System;
using System.Collections.Generic;
using System.Text;

namespace DailyYield.Application.Commands
{
    public class AuthenticateUserCommand
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
