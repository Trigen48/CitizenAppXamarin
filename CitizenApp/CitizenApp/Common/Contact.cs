using System;
using System.Collections.Generic;
using System.Text;

namespace CitizenApp.Common
{
    public class Contact
    {

        private String accountKey = "";
        private String name= "";
        private String lastname= "";
        private String title= "";
        private String address = "";
        private String contactNumberHome = "";
        private String contactNumberWork = "";
        private String emailAddress = "";
        private String profileImage = "";

        private string organiation = "";

        private String whatsAppNo = "";
        private String linkedInHandle = "";
        private String twitterHandle = "";
        private String instagramHandle = "";

        public string Fullname { get => this.name + ", " + this.lastname; }

        public string AccountKey { get => accountKey; set => accountKey = value; }

        public string Name { get => name; set => name = value; }

        public string Lastname { get => lastname; set => lastname = value; }

        public string Title { get => title; set => title = value; }

        public string Address { get => address; set => address = value; }

        public string ContactNumberHome { get => contactNumberHome; set => contactNumberHome = value; }

        public string ContactNumberWork { get => contactNumberWork; set => contactNumberWork = value; }

        public string EmailAddress { get => emailAddress; set => emailAddress = value; }

        public string Organization { get => organiation; set => organiation = value; }

        public string ProfileImage { get => profileImage; set => profileImage = value; }
        public string WhatsAppNo { get => whatsAppNo; set => whatsAppNo = value; }
        public string LinkedInHandle { get => linkedInHandle; set => linkedInHandle = value; }
        public string TwitterHandle { get => twitterHandle; set => twitterHandle = value; }
        public string InstagramHandle { get => instagramHandle; set => instagramHandle = value; }
    }
}
