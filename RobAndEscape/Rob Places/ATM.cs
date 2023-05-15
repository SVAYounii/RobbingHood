using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood.Rob_Places
{
    public class ATM
    {
        public Vector3 Position { get; set; }
        public Vector3 TempCarRotation { get; set; }

        public DateTime AvailbleTime { get; set; }
        public DateTime NullDate = new DateTime(1999, 11, 06);
        public bool IsClosed { get; set; }
        public Blip Blip { get; set; }

        public ATM(Vector3 position, Vector3 tempCarRotation)
        {
            Position = position;
            TempCarRotation = tempCarRotation;
            AvailbleTime = NullDate;

            IsClosed = false;

            Blip = World.CreateBlip(Position);
            Blip.Sprite = BlipSprite.DollarSignSquared;
            Blip.Color = BlipColor.Green;
            Blip.IsShortRange = true;
            Blip.Name = "ATM";
        }

        public void CloseATM()
        {
            IsClosed= true;
            AvailbleTime= World.CurrentDate.AddHours(1);
        }

        public void OpenATM()
        {
            IsClosed = false;
            AvailbleTime = NullDate;

            int RELATIONSHIP_COP = Function.Call<int>(Hash.GET_HASH_KEY, "COP");
            Ped Police = World.CreatePed(new Model(0x5E3DA4A4), Position.Around(2));
            Police.Weapons.Give(WeaponHash.Pistol50, 300, true, true);
            Police.RelationshipGroup = RELATIONSHIP_COP;
        }
    }
}
