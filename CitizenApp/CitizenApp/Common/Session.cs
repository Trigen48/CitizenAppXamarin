using System;
using System.Collections.Generic;
using System.Text;

namespace CitizenApp.Common
{
    public class Session
    {

        public Session()
        {

        }


        public string LoginToken { get; set; }
        public string LoginKey { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime LoginExpire { get; set; }
        public int LoginExpireInt { get; set; }
        public bool AlwaysOn { get; set; }
    }
}
