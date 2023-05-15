using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood
{
    public static class Player
    {

        public static ScriptSettings Config { get; set; }
        public static ScriptSettings Temp { get; set; }


        public static int Money { get; set; }
        public static int previousMoney { get; set; }
        public static int XP { get; set; }
        public static int PreviousXP { get; set; }
        public static int PlayerLevel { get; set; }
        public static int PreviousPlayerLevel { get; set; }

        public static int DistanceForMenu = 5;
        public static int DistanceForATM = 2;
        public static int GroupId { get; set; }

        public static int NextLevel { get; set; }
        public static int CurrentApartmentID = -1;
        public static int CurrentBodyguardShopID = -1;

        public static bool RealMoney { get; set; }
        public static bool IsInApartment { get; set; }
        public static bool HasBag { get; set; }
        public static List<Ped> Bodyguards = new List<Ped>();
        public static void ConfigureSettings()
        {
            Config = ScriptSettings.Load("scripts\\RobbingHood.ini");
            RealMoney = Config.GetValue<bool>("PlayerSettings", "RealMoney ", false);
            if (!RealMoney)
            {
                Money = Config.GetValue<int>("PlayerSettings", "AmountOfMoney", 1000);
            }
            else
            {
                Money = Game.Player.Money;
            }

            XP = Config.GetValue<int>("PlayerSettings", "XP", 0);
            PreviousXP = Config.GetValue<int>("PlayerSettings", "PrevXP", 0);
            PreviousPlayerLevel = Config.GetValue<int>("PlayerSettings", "PrevPlayerLevel", 1);

            PlayerLevel = Config.GetValue<int>("PlayerSettings", "PlayerLevel", 1);
            IsInApartment = Config.GetValue<bool>("PlayerSettings", "ISINAPARTMENT", false);
            CurrentApartmentID = Config.GetValue<int>("PlayerSettings", "CURRENTAPARTMENTID", -1);
            GroupId = Function.Call<int>(Hash.CREATE_GROUP, 1);
            Function.Call(Hash.SET_PED_AS_GROUP_LEADER, Game.Player.Character, GroupId);


        }

        public static void SaveSettings()
        {
            if (!RealMoney)
            {
                Config.SetValue<int>("PlayerSettings", "AmountOfMoney", Money);
            }

            Config.SetValue<int>("PlayerSettings", "XP", XP);
            Config.SetValue<int>("PlayerSettings", "PrevXP", PreviousXP);

            Config.SetValue<int>("PlayerSettings", "PlayerLevel", PlayerLevel);
            Config.SetValue<int>("PlayerSettings", "PrevPlayerLevel", PreviousPlayerLevel);
            Config.SetValue<bool>("PlayerSettings", "ISINAPARTMENT", IsInApartment);
            Config.SetValue<int>("PlayerSettings", "CURRENTAPARTMENTID", CurrentApartmentID);
            Config.Save();
        }

        public static void GiveBag()
        {
            if (!HasBag)
            {
                HasBag = true;
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 1, 2, 2);
            }
        }
        public static void RemoveBag()
        {
            if (HasBag)
            {
                HasBag = false;
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 2);
            }

        }


    }
}
