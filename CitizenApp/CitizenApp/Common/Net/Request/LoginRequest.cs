using System;
using System.Collections.Generic;
using System.Text;

namespace CitizenApp.Common.Net.Request
{
    public class LoginRequest
    {

        public string Username { get; set; }
        public string Password { get; set; }
        public string AlwaysLoggedIn { get; set; }
    }
}
