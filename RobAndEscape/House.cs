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
    public class House
    {
        public Vector3 OutsideMarkerPos { get; set; }
        public Vector3 EntryPos { get; set; }
        public Vector3 EntryRot { get; set; }
        public Vector3 ExitPos { get; set; }
        public Vector3 TempCarRot { get; set; }
        public Vector3 HeistPlanningBoardPos { get; set; }
        public int Cost { get; set; }
        public bool HasBouht { get; set; }

        public Blip Blip { get; set; }
        public Blip HeistBlip { get; set; }


        public House(Vector3 outsideMarkerPos, Vector3 tempCarRot, int cost, bool isInSide, Vector3 entryPos, Vector3 entryRot, Vector3 exitPos, Vector3 heistPlanningBoardPos)
        {
            OutsideMarkerPos = outsideMarkerPos;
            EntryPos = entryPos;
            EntryRot = entryRot;
            ExitPos = exitPos;
            TempCarRot = tempCarRot;
            Cost = cost;
            HasBouht = isInSide;
            HeistPlanningBoardPos = heistPlanningBoardPos;

            Blip = World.CreateBlip(OutsideMarkerPos);

            if (HasBouht)
            {
                Blip.Sprite = BlipSprite.Safehouse;
                Blip.Name = "Secret Safehouse";

            }
            else
            {
                Blip.Sprite = BlipSprite.SafehouseForSale;
                Blip.Name = "Secret Safehouse, $" + Cost.ToString();

            }

            Blip.IsShortRange = true;
            Blip.Color = BlipColor.Yellow;

            HeistBlip = World.CreateBlip(HeistPlanningBoardPos);
            HeistBlip.Color = BlipColor.White;
            HeistBlip.Name = "Heist";
            HeistBlip.IsShortRange = true;
            HeistBlip.Sprite = BlipSprite.Heist;
            HeistBlip.Alpha = 0;
        }

        public void EnterdApartment(int i)
        {
            Ped _player = Game.Player.Character;
            _player.Position = EntryPos;
            _player.Rotation = EntryRot;
            Game.Player.WantedLevel = 0;
            _player.Health = _player.MaxHealth;

            Player.IsInApartment = true;
            _player.CanSwitchWeapons = false;
            _player.Weapons.Select(WeaponHash.Unarmed);
            Player.CurrentApartmentID = i;


            Player.SaveSettings();
        }

        public void ExitApartment()
        {
            Ped _player = Game.Player.Character;

            Player.IsInApartment = false;
            _player.CanSwitchWeapons = true;
            _player.Position = OutsideMarkerPos;
            Player.CurrentApartmentID = -1;

            Player.SaveSettings();
        }
    }
}
