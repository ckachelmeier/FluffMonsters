using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffMonsters.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address HomeAddress { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
        public string ContactMethod { get; set; }
        public string Notes { get; set; }
        public List<Pet> Pets { get; set; }
    }
}
