using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenApp
{
    public class ContactMainMasterMenuItem
    {
        public ContactMainMasterMenuItem()
        {
            TargetType = typeof(ContactMainMasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}