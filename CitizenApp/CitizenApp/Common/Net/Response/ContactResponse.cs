using System;
using System.Collections.Generic;
using System.Text;

namespace CitizenApp.Common.Net.Response
{
    public class ContactResponse : Common.Response
    {
        public int ContactCount { get; set; }
        public Contact[] Contacts { get; set; }
    }
}
