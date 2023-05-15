using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood.Heist_Info
{
    public class Door
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }

        public Door(string name, Vector3 position)
        {
            Name = name;
            Position = position;
        }
    }
}
