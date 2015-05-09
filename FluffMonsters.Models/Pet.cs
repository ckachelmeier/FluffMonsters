using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FluffMonsters.Models
{
    public class Pet
    {
        public List<Customer> Owner { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Species { get; set; }
        public string FavoriteToy { get; set; }
        public float WeightInKilograms { get; set; }
        public float HeightInMeters { get; set; }
        public List<Visit> VisitHistory { get; set; }

    }
}
