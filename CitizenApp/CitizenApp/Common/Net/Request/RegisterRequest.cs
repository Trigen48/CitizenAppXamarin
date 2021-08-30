using System;
using System.Collections.Generic;
using System.Text;

namespace CitizenApp.Common.Net.Request
{
    public class RegisterRequest
    {

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string ContactNumberHome { get; set; }

        public string Organization { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
