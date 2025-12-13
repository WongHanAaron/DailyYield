using System;
using System.Collections.Generic;
using System.Text;

namespace DailyYield.Application.Commands
{
    public class AuthenticateUserCommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
