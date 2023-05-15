using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood
{
    public class Store
    {
        public Vector3 Position { get; set; }
        public DateTime AvailbleTime { get; set; }
        public DateTime NullDate = new DateTime(1999, 11, 06);
        public bool IsClosed { get; set; }
        public Blip Blip { get; set; }
        public Store(Vector3 pos)
        {
            Position = pos;
            AvailbleTime = NullDate;

            IsClosed = false;

            Blip = World.CreateBlip(Position);
            Blip.Sprite = BlipSprite.DollarSignCircled;
            Blip.Color = BlipColor.Green;
            Blip.IsShortRange = true;
            Blip.Name = "Store Safe";
        }

        public void OpenStore()
        {
            IsClosed = false;
            AvailbleTime = NullDate;
        }

        public void CloseStore()
        {
            IsClosed = true;
            AvailbleTime = World.CurrentDate.AddHours(1);
        }
    }
}
