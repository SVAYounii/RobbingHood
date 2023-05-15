using GTA;
using GTA.Math;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood
{
    public class CarShop
    {
        public Vector3 MarkerPos { get; set; }
        public Vector3 VehicleRot { get; set; }
        public Vector3 VehicleShowcasePos { get; set; }
        public Vector3 VehicleShowcaseRot { get; set; }
        public Model ShopModel { get; set; }
        public bool IsSpawned { get; set; }
        public Vehicle Vehicle { get; set; }
        public Ped Mechanic { get; set; }
        public Model MechanicModel { get; set; }
        public Blip Blip { get; set; }
        public CarShop(Vector3 markerPos, Vector3 vehicleRot, Model shopModel, Vector3 vehicleShowcasePos, Vector3 vehicleShowcaseRot)
        {
            MarkerPos = markerPos;
            VehicleRot = vehicleRot;
            ShopModel = shopModel;
            VehicleShowcasePos = vehicleShowcasePos;
            VehicleShowcaseRot = vehicleShowcaseRot;
            IsSpawned = false;
            MechanicModel = new Model(1142162924);

            Blip = World.CreateBlip(MarkerPos);

            Blip.Sprite = BlipSprite.Squadee;
            Blip.IsShortRange = true;
            Blip.Color = BlipColor.Yellow;
            Blip.Name = "Car Shop";
        }

        public void SpawnCarShop()
        {
            if (IsSpawned)
                return;

            IsSpawned = true;

            Vehicle[] nearbyCars = World.GetNearbyVehicles(MarkerPos, 10);

            //make room for the shop
            for (int j = 0; j < nearbyCars.Length; j++)
            {
                nearbyCars[j].Delete();

            }
            ShopModel.Request(250);
            MechanicModel.Request(250);


            Vehicle = World.CreateVehicle(ShopModel, MarkerPos);
            Vehicle.Rotation = VehicleRot;
            //veh.LockStatus = VehicleLockStatus.LockedForPlayer;
            Vehicle.IsPositionFrozen = true;
            Vehicle.IsExplosionProof = true;


            //add person next to the shop
            Mechanic = World.CreatePed(MechanicModel, new Vector3(MarkerPos.X - 1, MarkerPos.Y - 3, MarkerPos.Z));
            Mechanic.Rotation = new Vector3(VehicleRot.X, VehicleRot.Y, VehicleRot.Z + 90);
        }
        public void DeSpawn()
        {
            if (!IsSpawned) return;
            IsSpawned= false;

            Vehicle.MarkAsNoLongerNeeded();
            Mechanic.MarkAsNoLongerNeeded();  
            MechanicModel.MarkAsNoLongerNeeded();
            ShopModel.MarkAsNoLongerNeeded();

        }
    }
}
