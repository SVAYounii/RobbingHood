using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood.Heist_Info
{
    public class HeistCamera
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Blip Blip { get; set; }
        public Prop Prop { get; set; }
        public HeistCamera(string name, Vector3 position)
        {
            Name = name;
            Position = position;
            Blip = World.CreateBlip(Position);
            Blip.IsFriendly = false;
            Blip.Alpha = 0;
            Prop = null;
        }
    }
}
