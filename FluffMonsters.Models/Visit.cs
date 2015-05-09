using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluffMonsters.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Pet Pet { get; set;}
        public string Notes { get; set; }
    }
}
