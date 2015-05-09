using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffMonsters.Models
{
    public class Address
    {
        public string AddressLine1 {get; set;}
        public string AddressLine2 { get; set; }
        public string City { get; set; }

        /// <summary>
        /// Used for state in US, Province in Canada,  County in U.K, etc...
        /// </summary>
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
