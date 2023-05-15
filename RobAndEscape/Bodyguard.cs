using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood
{
    public class Bodyguard
    {
        public string Name { get; set; }
        public string Description{ get; set; }
        public Model Model { get; set; }
        public int Cost { get; set; }

        public Bodyguard(string name, Model model, int cost, string description)
        {
            Name = name;
            Model = model;
            Cost = cost;
            Description = description;
        }
    }
}
