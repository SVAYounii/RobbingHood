using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood
{
    public class SpecialWeaponComponents
    {
        public uint Id { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public Hash Hash { get; set; }
        public SpecialWeaponComponents(uint id, int price, string name, Hash hash)
        {
            Id = id;
            Price = price;
            Name = name;
            Hash = hash;
        }
    }
}
