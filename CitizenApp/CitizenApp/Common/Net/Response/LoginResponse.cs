using System;
using System.Collections.Generic;
using System.Text;

namespace CitizenApp.Common.Net.Response
{
    public class LoginResponse : Common.Response
    {
        public Account AccountData { get; set; }
    }
}
