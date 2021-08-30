using System;
using System.Collections.Generic;
using System.Text;

namespace CitizenApp.Common
{
    public class Response
    {
        public int code { get; set; } // Refer to code specs above
        public bool status { get; set; } // True(1) / False(0)
        public string message { get; set; } // Display Message
        public object data { get; set; } // Output: redirect url, json object , text, etc
        public string url { get; set; }

    }
}
