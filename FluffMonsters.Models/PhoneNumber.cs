using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffMonsters.Models
{
    public class PhoneNumber
    {
        /// <summary>
        /// Examples are mobile, home, work, Emergency contact, etc.  
        /// </summary>
        public string PhoneNumberType { get; set; }
        public string Number { get; set; }
    }
}
