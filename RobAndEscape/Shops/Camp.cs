using GTA;
using GTA.Math;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood
{
    public class Camp
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 TablePosition { get; set; }
        public Vector3 TableRotation { get; set; }
        public Model CampModel { get; set; }
        public Model TableModel { get; set; }
        public bool IsSpawned { get; set; }
        public Vehicle Vehicle { get; set; }
        public Prop Table { get; set; }
        public Ped Soldier { get; set; }
        public Model SoldierModel { get; set; }
        public Blip Blip { get; set; }
        public Camp(Vector3 position, Vector3 rotation, Vector3 tablePosition, Model campModel, Model tableModel)
        {
            Position = position;
            Rotation = rotation;
            TablePosition = tablePosition;
            TableRotation = new Vector3(Rotation.X, Rotation.Y, Rotation.Z - 90);
            TableModel = tableModel;
            CampModel = campModel;
            IsSpawned = false;
            SoldierModel = new Model(0x613E626C);


            Blip = World.CreateBlip(Position);

            Blip.Sprite = BlipSprite.BunkerVehicleWorkshop;
            Blip.IsShortRange = true;
            Blip.Color = BlipColor.Yellow;
            Blip.Name = "Black Market Shop";

        }

        public void SpawnCamp()
        {
            if (IsSpawned)
            {
                return;

            }
            IsSpawned = true;

            CampModel.Request(250);
            TableModel.Request(250);
            SoldierModel.Request(250);

            Vehicle = World.CreateVehicle(CampModel, Position);
            Vehicle.Rotation = Rotation;
            Vehicle.LockStatus = VehicleLockStatus.PlayerCannotEnter;
            Vehicle.IsExplosionProof = true;

            Table = World.CreateProp(TableModel, TablePosition, TableRotation, false, false);
            Table.Position = Table.GetOffsetPosition(new Vector3(0, -3, 0));
            Table.IsPositionFrozen = true;

           

            Soldier = World.CreatePed(SoldierModel, Vehicle.GetOffsetPosition(new Vector3(-3, 2, 0)));
            Soldier.Rotation = new Vector3(Rotation.X, Rotation.Y, Rotation.Z - 90);
            Soldier.Weapons.Give(WeaponHash.AdvancedRifle, 30, true, true);


        }

        public void DeSpawn()
        {
            if (!IsSpawned)
                return;
            IsSpawned = false;

            Vehicle.MarkAsNoLongerNeeded();
            Table.MarkAsNoLongerNeeded();
            Soldier.MarkAsNoLongerNeeded();
            SoldierModel.MarkAsNoLongerNeeded();
        }
    }
}
