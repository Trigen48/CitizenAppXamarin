using System;
using System.Collections.Generic;
using System.Text;

namespace CitizenApp.Common.Net.Request
{
    public class PasswordChangeRequest
    {

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
