using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood
{
    public class Weapon
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public WeaponHash WeaponHash { get; set; }

        public Weapon(string name, int price, WeaponHash weaponHash, string description)
        {
            Name = name;
            Description = description;
            Price = price;
            WeaponHash = weaponHash;
        }
    }
}
