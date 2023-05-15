using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood
{
    public class BodyguardShop
    {
        public Vector3 Position { get; set; }
        public Vector3 MarkerPosition { get; set; }
        public Vector3 Rotation { get; set; }
        public bool IsSpawned { get; set; }
        public Model Model { get; set; }
        public Vehicle Vehicle { get; set; }
        public Blip Blip { get; set; }
        public List<Ped> Peds { get; set; }
        public List<Bodyguard> Bodyguards { get; set; }
        public Ped BodyguardPlaceholder { get; set; }
        public Vector3 PlaceholderPosition { get; set; }
        public BodyguardShop(Vector3 position, Vector3 rotation, Vector3 markerPosition)
        {
            Position = position;
            Rotation = rotation;
            Model = new Model(1475773103);
            PlaceholderPosition = new Vector3(-1271.639f, -808.4915f, 17.12553f - 1f);
            Peds = new List<Ped>
            {
                World.CreatePed(new Model(-1275859404),new Vector3(-1267.579f, -822.1544f, 17.09917f), 95f),
                World.CreatePed(new Model(2047212121),new Vector3(-1267.708f, -817.8958f, 17.09918f), 95f),
                World.CreatePed(new Model(1349953339),new Vector3(-1268.003f, -816.87f, 17.09918f), 125f),
            };
            Peds.ForEach(p => { p.Weapons.Give(WeaponHash.AssaultRifle, 300, true, true); });

            Bodyguards = new List<Bodyguard>
            {
                new Bodyguard("Jimmy", new Model(1459905209),50000, "Jimmy isn't experienced in shooting, but he will definitely help!"),
                new Bodyguard("Ron", new Model(-1124046095),75000, "Ron knows what to do, but is mostly scared"),
                new Bodyguard("Lamar", new Model(1706635382),100000, "He leads the streets, a well experienced shooter! "),
            };
            BodyguardPlaceholder = World.CreateRandomPed(PlaceholderPosition);
            BodyguardPlaceholder.IsVisible = false;
            BodyguardPlaceholder.IsPositionFrozen= true;

            Blip = World.CreateBlip(Position);

            Blip.Sprite = BlipSprite.Adversary4;
            Blip.IsShortRange = true;
            Blip.Color = BlipColor.Yellow;
            Blip.Name = "Bodyguard Shop";
            MarkerPosition = markerPosition;
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
            Vehicle.LockStatus = VehicleLockStatus.PlayerCannotEnter;
            Vehicle.IsExplosionProof = true;
            Vehicle.Doors[GTA.VehicleDoorIndex.BackRightDoor].Open();
        }

        public void SwitchBodyguard(int i)
        {
            if (BodyguardPlaceholder != null)
            {
                BodyguardPlaceholder.Delete();
                BodyguardPlaceholder = null;
            }

            BodyguardPlaceholder = World.CreatePed(Bodyguards[i].Model, PlaceholderPosition, 128f);
            BodyguardPlaceholder.Weapons.Give(WeaponHash.AssaultRifle, 8000, true, true);
        }
        public void BuyBodyguard(int i)
        {
            if (Bodyguards[i].Cost > Player.Money)
            {
                Notification.Show("You don't have enough money to buy this bodyguard");

                return;
            }

            Player.Money -= Bodyguards[i].Cost;
            Player.Bodyguards.Add(BodyguardPlaceholder);
            Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, Player.Bodyguards[Player.Bodyguards.Count - 1], Player.GroupId);
            bool isingroup = Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, Player.Bodyguards[0], Player.GroupId);
            Notification.Show(isingroup.ToString());

            BodyguardPlaceholder = null;
        }
    }
}
