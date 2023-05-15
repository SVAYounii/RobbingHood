using GTA;
using GTA.Math;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood.Shops
{
    public class WeaponShop
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Model Model { get; set; }
        public Vehicle Vehicle { get; set; }
        public bool IsSpawned { get; set; }
        public Blip Blip { get; set; }
        public WeaponShop(Vector3 position, Vector3 rotation, Model model)
        {
            Position = position;
            Rotation = rotation;
            Model = model;
            IsSpawned = false;


            Blip = World.CreateBlip(Position);

            Blip.Sprite = BlipSprite.WeaponWorkshop;
            Blip.IsShortRange = true;
            Blip.Color = BlipColor.Yellow;
            Blip.Name = "Black Market Weapon Shop";
        }

        public void Spawn()
        {

            if (IsSpawned)
            {
                return;
            }
            IsSpawned = true;

            Model.Request(250);

            Vehicle = World.CreateVehicle(Model, Position);
            Vehicle.Rotation = Rotation;
            Vehicle.LockStatus = VehicleLockStatus.Locked;
            Vehicle.IsExplosionProof = true;
            Vehicle.RadioStation = RadioStation.ChannelX;
            Vehicle.IsPositionFrozen = true;

        }

        public void DeSpawn()
        {
            if (!IsSpawned)
            { return; }
            IsSpawned = false;

            Vehicle.MarkAsNoLongerNeeded();
            Model.MarkAsNoLongerNeeded();
        }


    }
}
