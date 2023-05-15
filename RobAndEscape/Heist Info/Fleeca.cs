using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using RobbingHood.Heist_Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood.Heist_Info
{
    public class Fleeca : IHeist
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 CashPosition { get; set; }
        public List<Door> Doors { get; set; }
        public List<HeistCamera> Cameras { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public List<Ped> NPCs { get; set; }
        public Camera VaultCamera { get; set; }
        public int VaultDelay { get; set; }


        public static Ped[] NearbyPeds { get; set; }
        public static List<Blip> NearbyPedsBlip { get; set; }


        Random random = new Random();
        public Fleeca(Vector3 HomePosition)
        {
            Name = "Fleeca";
            Position = new Vector3(-2973.909f, 482.9201f, 15.26166f);
            CashPosition = new Vector3(-2954.063f, 484.3052f, 16.51987f - 0.6f);
            VaultCamera = World.CreateCamera(new Vector3(-2956.911f, 486.0496f, 17.77533f), new Vector3(-10f, 0, 177.6678f), 60);
            VaultCamera.Shake(CameraShake.Hand, 1);
            NearbyPedsBlip = new List<Blip>();
            NPCs = new List<Ped>();
            Doors = new List<Door>
            {
                new Door("v_ilev_genbankdoor1", new Vector3(-2965.82f, 481.63f, 16.05f)),
                new Door("v_ilev_genbankdoor2", new Vector3(-2965.71f, 484.22f, 16.05f))
            };

            Cameras = new List<HeistCamera>
            {
                new HeistCamera( "prop_cctv_cam_06a", new Vector3(-2966.354f, 485.635f, 15.55272f)),
                new HeistCamera( "prop_cctv_cam_06a", new Vector3(-2962.288f, 486.5195f, 15.69272f)),
                new HeistCamera( "prop_cctv_cam_06a", new Vector3(-2959.277f, 477.2637f, 15.69687f)),
                new HeistCamera( "prop_cctv_cam_06a", new Vector3(-2958.855f, 478.9295f, 15.69696f)),
                new HeistCamera( "prop_cctv_cam_06a", new Vector3(-2961.104f, 477.1835f, 15.69689f))
            };

            CheckPoints = new List<CheckPoint>
            {
                new CheckPoint("Before we go to the bank, we need something first.", new Vector3(-1221.118f, -644.5023f, 25.9013f),false),
                new CheckPoint("Go to the rooftop and kill the guys standing near the car", new Vector3(-1226.282f, -669.4984f, 39.99438f),false),
                new CheckPoint("Good job! Now get in the car and go to the bank.", new Vector3(-2974.302f, 483.1777f, 15.26316f), false, true),
                new CheckPoint("First things firts. Destroy the cameras and make sure no one escape", new Vector3(0f, 0f, 0f), false),
                new CheckPoint("Alright, well done. Go to the vault, i have got a suprise for you.", new Vector3(-2957.671f, 479.3519f, 15.69696f), true),
                new CheckPoint("Good job, there isn't much time left, get the money and escape!", new Vector3(0f, 0f, 0f), false),
                new CheckPoint("Well done, let's go home and we will split the money", HomePosition, false)
            };

            HeistGroup();
            GarageGroup();

            NPCs[0].Task.UseMobilePhone(10000);
            NPCs[1].Task.ChatTo(NPCs[0]);

            //NPCs[3].Task.ChatTo(NPCs[2]);

        }

        public void OpenDoors()
        {
            for (int i = 0; i < Doors.Count; i++)
            {
                Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, Doors[i].Name), Doors[i].Position.X, Doors[i].Position.Y, Doors[i].Position.Z, false, 1f, 0);
            }
        }
        public void HandsUp()
        {

        }
        void HeistGroup()
        {
            List<Ped> peds = new List<Ped>
            {
                 //Bank NPCs
                World.CreateRandomPed(new Vector3(-2963.08f, 483.0299f, 15.70311f),-86f),
                World.CreateRandomPed(new Vector3(-2960.58f, 483.0299f, 15.70311f),86f),
                World.CreateRandomPed(new Vector3(-2964.298f, 477.163f, 15.69702f),3.5f),
                World.CreateRandomPed(new Vector3(-2964.099f, 479.233f, 15.6969f),-173.118f),
            };
            peds.ForEach(p => { p.Money = 1; });

            NPCs.AddRange(peds);

        }

        void GarageGroup()
        {
            List<Ped> peds = new List<Ped>
            {
                 World.CreateRandomPed(new Vector3(-1226.282f, -669.4984f, 39.99438f).Around(5),random.Next(0,300)),
                World.CreateRandomPed(new Vector3(-1226.282f, -669.4984f, 39.99438f).Around(5),random.Next(0, 300)),
                World.CreateRandomPed(new Vector3(-1226.282f, -669.4984f, 39.99438f).Around(5),random.Next(0, 300)),
                World.CreateRandomPed(new Vector3(-1226.282f, -669.4984f, 39.99438f).Around(5),random.Next(0, 300)),
                World.CreateRandomPed(new Vector3(-1226.282f, -669.4984f, 39.99438f).Around(5),random.Next(0, 300)),
                World.CreateRandomPed(new Vector3(-1226.282f, -669.4984f, 39.99438f).Around(5),random.Next(0, 300)),
                World.CreateRandomPed(new Vector3(-1226.282f, -669.4984f, 39.99438f).Around(5),random.Next(0, 300))
            };
            int GroupId = Function.Call<int>(Hash.CREATE_GROUP, 0);
            for (int i = 0; i < peds.Count; i++)
            {
                if (i == 0)
                {
                    Function.Call(Hash.SET_PED_AS_GROUP_LEADER, peds[i], GroupId);

                }
                else
                {
                    Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, peds[i], GroupId);

                }
                peds[i].Weapons.Give(WeaponHash.AssaultRifle, 200, false, true);

            }
            NPCs.AddRange(peds);
        }

        public void CheckPointMessage(int checkPointIndex)
        {
            if (CheckPoints[checkPointIndex].IsCalled)
                return;
            Notification.Show(NotificationIcon.Lester, "Lester", "...", CheckPoints[checkPointIndex].Description, false, true);
            CheckPoints[checkPointIndex].IsCalled = true;

        }

    }
}
