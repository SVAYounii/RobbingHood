using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood
{
    public class SpecialWeapon
    {

        public string Name { get; set; }
        public int Price { get; set; }
        public WeaponHash WeaponHash { get; set; }
        public string Description { get; set; }
        public bool HasBought { get; set; }
        public List<SpecialWeaponComponents> Component { get; set; }

        public SpecialWeapon(string name, int price, WeaponHash weaponHash, string description, bool hasBought, List<SpecialWeaponComponents> component)
        {
            Name = name;
            Price = price;
            WeaponHash = weaponHash;
            Description = description;
            HasBought = hasBought;
            Component = component;
        }

      
    }
}
