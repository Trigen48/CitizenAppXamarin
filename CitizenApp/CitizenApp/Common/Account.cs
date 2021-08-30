using System;
using System.Collections.Generic;
using System.Text;

namespace CitizenApp.Common
{
    public class Account : Contact
    {
        private Session session;

        public Account() : base()
        {
            session = new Session();
        }

        public Session AccountSession { get => session; set => session = value; }
    }
}
