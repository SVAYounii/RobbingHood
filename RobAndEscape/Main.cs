using System;
using System.Windows.Forms;
using GTA;
using GTA.UI;
using GTA.Native;
using GTA.Math;
using NativeUI;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using RobbingHood.Shops;
using RobbingHood.Rob_Places;
using RobbingHood.Heist_Info;
using System.Reflection;
using System.Threading;
using System.Linq;

namespace RobbingHood
{
    public class Main : Script
    {
        #region Heist
        List<IHeist> ListHeist = new List<IHeist>();
        Pickup HeistBagPickup = null;


        bool HeistActive;
        bool StartCheckpoint;
        bool HeistCallOnce;

        int CurrentHeistID = -1;
        int Heistcheckpoint = 0;
        #endregion

        #region Player Property


        Camera CarCam;
        #endregion

        #region Weapon Property
        //List<WeaponsSpecial> WeaponsSpecialList = new List<WeaponsSpecial>();
        List<Weapon> ListWeapons = new List<Weapon>();
        List<SpecialWeapon> WeaponsSpecialList = new List<SpecialWeapon>();



        #endregion

        #region Camp Property

        List<Camp> CampList = new List<Camp>();

        #endregion

        #region ATM Property
        List<ATM> ListATM = new List<ATM>();

        Ped[] NearbyPeds;
        Ped[] Coppp;
        List<Ped> Cop = new List<Ped>();

        #endregion

        #region Weapon Store
        List<WeaponShop> ListWeaponStore = new List<WeaponShop>();


        int lastCountWeapShop;
        int lastCountCarShop;
        int lastCountCamp;

        bool callonceWeapShop;
        bool callonceCarShop;
        bool callonceCamp;

        #endregion

        #region Rob Property

        int robbedMoney;
        int robbedXP;

        int ATM_ID;

        int KnifeUsed;
        int KnifeBreakingAmount = 3;
        DateTime nullDate = new DateTime(1999, 11, 06);
        #endregion

        #region Car Shop Property
        List<CarShop> ListCarShop = new List<CarShop>();
        int CarshopID = -1;
        //List<Car> ListCars = new List<Car>();
        List<Car> ListCars = new List<Car>();

        Vehicle SpawnCarTemp;
        Vehicle[] NearbyCarsTemp;
        List<Blip> NearbyCarsBlips = new List<Blip>();
        #endregion

        #region Misson
        //level2
        bool Ready2;
        int ATM_ID_Waypoint;

        //level4
        bool Ready41;
        bool Ready42;
        int WStore_ID;

        //Level5
        bool Ready51;




        Blip Waypoint;
        Camera MissionCam;
        bool AbleToPlayScene;
        bool WaypointActive;

        #endregion

        #region SafeHouse
        List<House> ListHouses = new List<House>();
        List<Vector3> ListHousesHeist = new List<Vector3>();
        #endregion

        #region Store Shops
        List<Store> ListStoreShops = new List<Store>();

        #endregion

        #region Bodyguard Shops

        List<BodyguardShop> ListBodyguardShop = new List<BodyguardShop>();
        #endregion

        #region UI
        bool ShowRankUp = false;
        Scaleform UIHandler = new Scaleform("mp_big_message_freemode");
        int UIDelayTime = 5000;
        int UINextTime = 0;
        bool CallOnce;
        #endregion

        #region Level
        List<int> LevelXP = new List<int>();
        List<string> LevelName = new List<string>();

        #endregion

        int Loader = 0;


        Random random = new Random();

        Ped _player = Game.Player.Character;

        bool FirstTime = false;
        bool IsLoading;
        bool LoadingDone;


        bool IsRobbing = false;

        MenuPool MenuPool;
        MenuPool MenuPool1;
        MenuPool MenuPool2;
        MenuPool MenuPoolHeist;
        MenuPool MenuPoolBodyguard;

        UIMenu MainMenuTools;
        UIMenu MainMenuWeapons;
        UIMenu SubMenuWeapons;
        UIMenu MainMenuCars;
        UIMenu MainMenuHeist;
        UIMenu MainMenuBodyguard;

        List<UIMenuItem> MenuToolsItemList = new List<UIMenuItem>();
        List<int> MenuToolsItemListLevel = new List<int>();

        List<UIMenu> MenuWeaponsList = new List<UIMenu>();
        List<int> MenuWeaponsItemListLevel = new List<int>();

        List<UIMenuItem> MenuComponentsList = new List<UIMenuItem>();

        List<UIMenuItem> ListCarItems = new List<UIMenuItem>();
        List<int> ListCarItemsLevel = new List<int>();

        List<UIMenuItem> ListHeistItems = new List<UIMenuItem>();
        List<int> ListHeistItemsLevel = new List<int>();

        List<UIMenuItem> ListBodyguardItems = new List<UIMenuItem>();




        int GoBack = -1;


        public Main()
        {
            this.Tick += OnTick;
            this.KeyDown += OnKeyDown;
        }





        private void OnTick(object sender, EventArgs e)
        {


            Start();
            if (Game.IsLoading)
                return;

            if (!LoadingDone)
                return;

            if (_player != Game.Player.Character)
                _player = Game.Player.Character;

            //God Mode
            Game.Player.Character.Health = _player.MaxHealth;


            CheckRobberyStatus();

            CheckIfATMIsAvailable();

            CheckIfStoreIsAvailable();

            MoneyHasChanged();

            CheckForNewLevel();

            ProcessMenu();

            SpawnCampIfNearby();
            SpawnWeaponstoreIfNearby();
            SpawnCarShopIfNearby();

            CheckPlayerCloseToCamp();

            CheckIfPlayerCloseToATM();
            CheckIfTutIsCompleted();

            CheckIfPlayerCloseToWeaponStore();

            CheckIfPlayerHasBouhgt();

            CheckIfPlayerIsCloseCarShop();

            CheckIfPlayerIsCloseSafehouse();

            CheckIfPlayerIsCloseStore();
            CheckIfPlayerIsCloseBodyguardShop();

            CheckIfPlayerIscloseWayPointLevel2();
            CheckIfPlayerIscloseWayPointLevel3();
            CheckIfPlayerIscloseWayPointLevel41();
            CheckIfPlayerIscloseWayPointLevel42();
            CheckIfPlayerIscloseWayPointLevel51();
            CheckIfPlayerIsInAppartment();

            CheckMoney();

            RankUp();

            CheckMenuLowLevel();


            StartHeist();
            DrawMarker();
        }



        void Start()
        {
            if (!IsLoading)
            {
                switch (Loader)
                {
                    case 0:
                        if (!FirstTime)
                        {
                            //GTA.UI.Screen.FadeOut(1);
                            Player.ConfigureSettings();
                            Wait(200);






                            LevelXP.Add(0);
                            LevelXP.Add(0);
                            LevelXP.Add(150);       //level 2
                            LevelXP.Add(500);       //level 3
                            LevelXP.Add(1200);      //level 4
                            LevelXP.Add(2500);      //level 5
                            LevelXP.Add(3250);      //level 6
                            LevelXP.Add(5000);      //level 7f11
                            LevelXP.Add(11000);
                            LevelXP.Add(13000);
                            LevelXP.Add(15000);


                            LevelName.Add("");
                            LevelName.Add("Street Rookie");
                            LevelName.Add("Street Hustler");
                            LevelName.Add("Street Captain");
                            LevelName.Add("None");
                            LevelName.Add("Public Enemy");
                            LevelName.Add("Shot Caller");
                            LevelName.Add("King of Los Santos");

                            if (Player.PlayerLevel < 10)
                                Player.NextLevel = Player.PlayerLevel + 1;

                            InitializeCamp();
                            FirstTime = true;

                        }
                        else
                        {
                            Wait(200);
                            FirstTime = false;
                            Loader++;

                        }
                        break;
                    case 1:
                        if (!FirstTime)
                        {
                            if (Player.PlayerLevel >= 2)
                            {
                                InitializeATM();
                            }
                            else
                            {
                                FirstTime = true;

                            }
                        }
                        else
                        {
                            Wait(200);
                            FirstTime = false;
                            Loader++;

                        }
                        break;
                    case 2:
                        if (!FirstTime)
                        {

                            InitializeWeapons();


                        }
                        else
                        {
                            Wait(200);
                            FirstTime = false;
                            Loader++;

                        }
                        break;
                    case 3:
                        if (!FirstTime)
                        {
                            InitializeWeaponsSpecial();
                        }
                        else
                        {
                            Wait(200);
                            FirstTime = false;
                            Loader++;
                        }
                        break;
                    case 4:
                        if (!FirstTime)
                        {
                            if (Player.PlayerLevel >= 4)
                            {

                                InitializeWeaponStore();
                                //FirstTime = true;

                            }
                            else
                            {
                                FirstTime = true;
                            }

                        }
                        else
                        {
                            Wait(200);
                            FirstTime = false;
                            Loader++;
                        }
                        break;
                    case 5:
                        if (!FirstTime)
                        {
                            InitializeCars();
                        }
                        else
                        {
                            Wait(200);
                            FirstTime = false;
                            Loader++;
                        }
                        break;
                    case 6:
                        if (!FirstTime)
                        {
                            if (Player.PlayerLevel >= 4)
                            {
                                InitializeCarShop();
                                //FirstTime = true;

                            }
                            else
                            {
                                FirstTime = true;
                            }

                        }
                        else
                        {
                            Wait(200);
                            FirstTime = false;
                            Loader++;
                        }
                        break;
                    case 7:
                        if (!FirstTime)
                        {
                            if (Player.PlayerLevel >= 5)
                            {
                                InitializeHouses();
                            }
                            else
                            {
                                FirstTime = true;

                            }



                        }
                        else
                        {
                            Wait(200);
                            FirstTime = false;
                            Loader++;
                        }
                        break;

                    case 8:
                        if (!FirstTime)
                        {


                            InitializeStores();

                        }
                        else
                        {
                            Wait(200);
                            FirstTime = false;
                            Loader++;
                        }
                        break;

                    case 9:
                        InitializeHeist();

                        InitializeBodyguards();
                        Wait(200);

                        SetupMenu();

                        CheckIfPlayerIsInAppartment();
                        LoadingDone = true;
                        IsLoading = true;
                        //{RELEASE} remove this
                        //if (_player.Weapons.HasWeapon(WeaponHash.Crowbar))
                        //    _player.Weapons.Remove(WeaponHash.Crowbar);

                        //if (_player.Weapons.HasWeapon(WeaponHash.Knife))
                        //    _player.Weapons.Remove(WeaponHash.Knife);

                        //if (_player.Weapons.HasWeapon(WeaponHash.StickyBomb))
                        //    _player.Weapons.Remove(WeaponHash.StickyBomb);

                        //if (_player.Weapons.HasWeapon(WeaponHash.PipeBomb))
                        //    _player.Weapons.Remove(WeaponHash.PipeBomb);


                        //GTA.UI.Screen.FadeIn(1000);
                        break;



                }
            }

        }
        private void CheckIfPlayerIsCloseBodyguardShop()
        {
            for (int i = 0; i < ListBodyguardShop.Count; i++)
            {

                if (Vector3.Distance(_player.Position, ListBodyguardShop[i].MarkerPosition) <= 50)
                {
                    if (Vector3.Distance(_player.Position, ListBodyguardShop[i].MarkerPosition) <= 1f)
                    {
                        if (HeistActive)
                        {
                            GTA.UI.Screen.ShowSubtitle("Unable to interact, your currently in a heist");
                            return;
                        }

                        DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to open the menu");
                        if (Game.IsControlJustPressed(GTA.Control.Context))
                        {
                            MainMenuBodyguard.Visible = true;
                            Player.CurrentBodyguardShopID = i;
                            Camera camera = World.CreateCamera(new Vector3(-1273.557f, -810.0147f, 18.12542f), new Vector3(0, 0, -47.62475f), 60);
                            camera.Shake(CameraShake.Hand, 1);
                            World.RenderingCamera = camera;
                            ListBodyguardShop[Player.CurrentBodyguardShopID].SwitchBodyguard(0);
                        }


                    }
                }
            }
        }
        private void InitializeBodyguards()
        {
            BodyguardShop shop = new BodyguardShop(new Vector3(-1266.035f, -820.3091f, 16.4411f), new Vector3(0.03087303f, 0.0948398f, 178.253f), new Vector3(-1267.915f, -820.1229f, 17.09916f));
            shop.Spawn();
            ListBodyguardShop.Add(shop);
        }

        private void CheckIfPlayerIsInAppartment()
        {
            House closest = ListHouses.OrderBy(item => Vector3.Distance(_player.Position, item.EntryPos)).First();

            if (Vector3.Distance(_player.Position, closest.EntryPos) < 30)
            {
                int id = 0;
                for (int i = 0; i < ListHouses.Count; i++)
                {
                    if (closest.EntryPos == ListHouses[i].EntryPos)
                    {
                        id = i;
                        break;
                    }
                }
                _player.CanSwitchWeapons = false;
                _player.Weapons.Select(WeaponHash.Unarmed);

                Player.CurrentApartmentID = id;
                Player.IsInApartment = true;
                Player.SaveSettings();
            }
            else
            {
                _player.CanSwitchWeapons = true;

            }

        }

        void Reset()
        {

        }


        private void CheckIfPlayerIsCloseStore()
        {

            for (int i = 0; i < ListStoreShops.Count; i++)
            {
                if (ListStoreShops[i].IsClosed)
                {
                    ListStoreShops[i].Blip.Color = BlipColor.Grey;
                }
                else
                {
                    ListStoreShops[i].Blip.Color = BlipColor.GreenDark;

                }

                if (Vector3.Distance(_player.Position, ListStoreShops[i].Position) <= 50)
                {
                    if (Vector3.Distance(_player.Position, ListStoreShops[i].Position) <= 1f)
                    {
                        if (HeistActive)
                        {
                            GTA.UI.Screen.ShowSubtitle("Unable to interact, your currently in a heist");
                            return;
                        }

                        if (_player.Weapons.HasWeapon(WeaponHash.Crowbar) || _player.Weapons.HasWeapon(WeaponHash.Knife))
                        {
                            DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to rob the safe");
                            if (Game.IsControlJustPressed(GTA.Control.Context))
                            {
                                GTA.UI.Screen.FadeOut(1000);
                                Wait(1500);
                                PlaySound("Safe");
                                Wait(1200);

                                SafeRobbed(i);
                                GTA.UI.Screen.FadeIn(1000);

                            }
                        }
                        else
                        {
                            GTA.UI.Screen.ShowSubtitle("You don't have tools to rob this Safe, go to the Black Market shops and buy the right tools", 500);

                        }


                    }
                }
            }

        }




        private void CheckIfPlayerIsCloseSafehouse()
        {
            for (int i = 0; i < ListHouses.Count; i++)
            {
                //Heist
                if (Vector3.Distance(_player.Position, ListHouses[i].HeistPlanningBoardPos) <= 5)
                {
                    if (Vector3.Distance(_player.Position, ListHouses[i].HeistPlanningBoardPos) <= 1)
                    {
                        DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to open the heist menu");
                        if (Game.IsControlJustPressed(GTA.Control.Context))
                        {
                            MainMenuHeist.Visible = true;
                            break;

                        }
                    }
                }

                // Exit
                if (Player.IsInApartment)
                {
                    if (Vector3.Distance(_player.Position, ListHouses[Player.CurrentApartmentID].ExitPos) <= 1)
                    {
                        DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to exit the Safehouse");
                        if (Game.IsControlJustPressed(GTA.Control.Context))
                        {
                            GTA.UI.Screen.FadeOut(1000);
                            Wait(1000);
                            ListHouses[Player.CurrentApartmentID].ExitApartment();
                            GTA.UI.Screen.FadeIn(1000);
                            break;
                        }
                    }

                }
                //Enter
                if (Vector3.Distance(_player.Position, ListHouses[i].OutsideMarkerPos) <= 50)
                {
                    if (Vector3.Distance(_player.Position, ListHouses[i].OutsideMarkerPos) <= 1)
                    {
                        if (ListHouses[i].HasBouht)
                        {
                            if (Game.Player.WantedLevel > 0)
                            {
                                bool _playerNotfound = Function.Call<bool>(Hash.ARE_PLAYER_STARS_GREYED_OUT, Game.Player);
                                if (!_playerNotfound)
                                {
                                    GTA.UI.Screen.ShowSubtitle("Make sure the cops haven't spotted you before going inside your Secret Safehouse", 20);
                                    break;
                                }
                            }


                            DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to enter Safehouse");
                            if (Game.IsControlJustPressed(GTA.Control.Context))
                            {
                                GTA.UI.Screen.FadeOut(1000);
                                Wait(1000);
                                ListHouses[i].EnterdApartment(i);
                                GTA.UI.Screen.FadeIn(1000);
                            }
                        }
                        else
                        {
                            DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to buy this SafeHouse for $" + ListHouses[i].Cost.ToString());
                            if (Game.IsControlJustPressed(GTA.Control.Context))
                            {
                                if (Player.Money >= ListHouses[i].Cost)
                                {
                                    if (MissionCam == null)
                                    {
                                        Model lol = new Model(1871995513);
                                        lol.Request(250);

                                        Vehicle temp = World.CreateVehicle(lol, ListHouses[i].OutsideMarkerPos);
                                        temp.Rotation = ListHouses[i].TempCarRot;
                                        temp.IsPositionFrozen = true;
                                        temp.IsInvincible = true;
                                        temp.IsCollisionEnabled = false;
                                        temp.SetNoCollision(temp, true);
                                        temp.IsVisible = false;

                                        MissionCam = World.CreateCamera(temp.GetOffsetPosition(new Vector3(0f, -100, 150)), _player.Rotation, 80);
                                        MissionCam.PointAt(ListHouses[i].EntryPos);
                                        World.RenderingCamera = MissionCam;
                                        _player.IsPositionFrozen = true;
                                        GTA.UI.Screen.ShowHelpText("You have bought this Secret Safehouse", 4000, true);
                                        Wait(4000);
                                        _player.IsPositionFrozen = false;

                                        ListHouses[i].HeistBlip.Alpha = ListHouses[i].Blip.Alpha;

                                        World.RenderingCamera = null;
                                        MissionCam.Delete();
                                        MissionCam = null;
                                        temp.MarkAsNoLongerNeeded();
                                        temp.Delete();
                                    }


                                    Player.Money -= ListHouses[i].Cost;

                                    ListHouses[i].HasBouht = true;
                                    string id = "ID" + (i + 1).ToString();
                                    Player.Config.SetValue("SAFEHOUSE", id, true);

                                    Player.Config.Save();


                                    ListHouses[i].Blip.Sprite = BlipSprite.Safehouse;
                                    ListHouses[i].Blip.Color = BlipColor.Yellow;
                                    ListHouses[i].Blip.Name = "Secret Safehouse";


                                }
                                else
                                {
                                    GTA.UI.Screen.ShowSubtitle("You don't have enough money to buy this SafeHouse", 5000);
                                }
                            }
                        }

                    }
                }
            }


        }






        void DrawMarker()
        {
            if (ListHouses.Count > 0)
            {
                for (int i = 0; i < ListHouses.Count; i++)
                {
                    if (Vector3.Distance(_player.Position, ListHouses[i].OutsideMarkerPos) <= 50)
                    {
                        Function.Call(Hash.DRAW_MARKER, 1, ListHouses[i].OutsideMarkerPos.X, ListHouses[i].OutsideMarkerPos.Y, ListHouses[i].OutsideMarkerPos.Z - 1f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 1f, 0.5f, 255, 128, 0, 50, false, true, 2, false, false, false, false);
                    }
                }
            }
            //Apartment Exit
            if (Player.IsInApartment && Player.CurrentApartmentID >= 0)
            {

                Function.Call(Hash.DRAW_MARKER, 1, ListHouses[Player.CurrentApartmentID].ExitPos.X, ListHouses[Player.CurrentApartmentID].ExitPos.Y, ListHouses[Player.CurrentApartmentID].ExitPos.Z - 1f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 1f, 0.5f, 255, 128, 0, 50, false, true, 2, false, false, false, false);
                Function.Call(Hash.DRAW_MARKER, 1, ListHouses[Player.CurrentApartmentID].HeistPlanningBoardPos.X, ListHouses[Player.CurrentApartmentID].HeistPlanningBoardPos.Y, ListHouses[Player.CurrentApartmentID].HeistPlanningBoardPos.Z - 1f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 1f, 0.5f, 255, 128, 0, 50, false, true, 2, false, false, false, false);

            }
            if (HeistActive)
            {
                return;
            }

            if (CampList.Count > 0)
            {
                for (int i = 0; i < CampList.Count; i++)
                {
                    if (Vector3.Distance(_player.Position, CampList[i].Position) <= 50)
                    {
                        Function.Call(Hash.DRAW_MARKER, 1, CampList[i].TablePosition.X - 3f, CampList[i].TablePosition.Y - 3f, CampList[i].TablePosition.Z, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 1f, 0.5f, 255, 128, 0, 50, false, true, 2, false, false, false, false);
                    }
                }
            }

            if (ListBodyguardShop.Count > 0)
            {
                for (int i = 0; i < ListBodyguardShop.Count; i++)
                {
                    if (Vector3.Distance(_player.Position, ListBodyguardShop[i].Position) <= 50)
                    {
                        Function.Call(Hash.DRAW_MARKER, 1, ListBodyguardShop[i].MarkerPosition.X, ListBodyguardShop[i].MarkerPosition.Y, ListBodyguardShop[i].MarkerPosition.Z - 1f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 1f, 0.5f, 255, 128, 0, 50, false, true, 2, false, false, false, false);
                    }
                }
            }
            if (ListATM.Count > 0)
            {
                for (int i = 0; i < ListATM.Count; i++)
                {
                    if (Vector3.Distance(_player.Position, ListATM[i].Position) <= 50)
                    {
                        if (ListATM[i].IsClosed)
                            return;

                        Function.Call(Hash.DRAW_MARKER, 1, ListATM[i].Position.X, ListATM[i].Position.Y, ListATM[i].Position.Z - 1f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 1f, 0.5f, 255, 128, 0, 50, false, true, 2, false, false, false, false);
                    }
                }
            }

            if (ListStoreShops.Count > 0)
            {
                for (int i = 0; i < ListStoreShops.Count; i++)
                {
                    if (Vector3.Distance(_player.Position, ListStoreShops[i].Position) <= 25)
                    {
                        if (ListStoreShops[i].IsClosed)
                            return;
                        Function.Call(Hash.DRAW_MARKER, 1, ListStoreShops[i].Position.X, ListStoreShops[i].Position.Y, ListStoreShops[i].Position.Z - 1f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 1f, 0.5f, 255, 128, 0, 50, false, true, 2, false, false, false, false);
                    }
                }
            }

            if (ListCarShop.Count > 0)
            {

                if (Vector3.Distance(_player.Position, ListCarShop[0].MarkerPos) <= 50)
                {
                    Function.Call(Hash.DRAW_MARKER, 1, ListCarShop[0].MarkerPos.X, ListCarShop[0].MarkerPos.Y, ListCarShop[0].MarkerPos.Z - 1f, 0f, 0f, 0f, 0f, 0f, 0f, 10f, 10f, 1.5f, 255, 128, 0, 50, false, true, 2, false, false, false, false);

                }
            }




        }
        private void CheckIfPlayerIscloseWayPointLevel3()
        {
            if (!AbleToPlayScene)
                return;

            if (Vector3.Distance(_player.Position, Waypoint.Position) <= 25)
            {
                AbleToPlayScene = false;
                Waypoint.Delete();
                Waypoint = null;
                VehicleLockStatus vehicleLockStatus = VehicleLockStatus.None;
                if (_player.IsInVehicle())
                {
                    _player.CurrentVehicle.IsPositionFrozen = true;
                    _player.CurrentVehicle.LockStatus = VehicleLockStatus.CannotEnter;
                    vehicleLockStatus = _player.CurrentVehicle.LockStatus;
                }
                else
                {
                    _player.IsPositionFrozen = true;
                }

                Game.Player.WantedLevel = 0;

                if (MissionCam == null)
                {
                    MissionCam = World.CreateCamera(CampList[0].Soldier.GetOffsetPosition(new Vector3(-10, -15, 5)), _player.Rotation, 60);
                    MissionCam.PointAt(CampList[0].Soldier.Position);
                    MissionCam.Shake(CameraShake.Hand, 1);
                    World.RenderingCamera = MissionCam;

                    GTA.UI.Screen.ShowHelpText("You have unlocked the " + ListWeapons[2].Name + " and the " + ListWeapons[3].Name, 5000, true, false);
                    Wait(5000);
                    MissionCam.Delete();

                    GTA.UI.Screen.ShowHelpText("You have also unlocked 3 more store locations, open the map to find out the locations", 5000, true);
                    MissionCam = World.CreateCamera(CampList[0].Soldier.GetOffsetPosition(new Vector3(-10, -10, 5)), _player.Rotation, 60);
                    MissionCam.PointAt(CampList[0].Soldier.Position);
                    MissionCam.Shake(CameraShake.Hand, 1);

                    Wait(5000);

                    World.RenderingCamera = null;
                    MissionCam.Delete();
                    MissionCam = null;

                    Player.Config.SetValue("ScriptSettings", "id3", true);

                    Player.Config.Save();
                    WaypointActive = false;

                }

                if (_player.IsInVehicle())
                {
                    _player.CurrentVehicle.IsPositionFrozen = false;
                    _player.CurrentVehicle.LockStatus = VehicleLockStatus.Unlocked;

                }
                else
                {
                    _player.IsPositionFrozen = false;
                }
            }
            else
            {
                GTA.UI.Screen.ShowSubtitle("Go to the ~y~waypoint~w~ to find out what you have unlocked.", 1000);

            }
        }

        private void CheckIfPlayerIscloseWayPointLevel2()
        {
            if (!Ready2)
                return;

            if (Vector3.Distance(_player.Position, Waypoint.Position) <= 25)
            {
                Ready2 = false;
                Waypoint.Delete();
                Waypoint = null;
                if (_player.IsInVehicle())
                {
                    _player.CurrentVehicle.IsPositionFrozen = true;
                }
                else
                {
                    _player.IsPositionFrozen = true;
                }
                Game.Player.WantedLevel = 0;


                if (MissionCam == null)
                {

                    Vehicle temp = World.CreateVehicle(new Model(1871995513), ListATM[ATM_ID_Waypoint].Position);
                    temp.Rotation = ListATM[ATM_ID_Waypoint].TempCarRotation;
                    temp.IsPositionFrozen = true;
                    temp.IsInvincible = true;
                    temp.IsCollisionEnabled = false;
                    temp.SetNoCollision(temp, true);
                    temp.IsVisible = false;

                    MissionCam = World.CreateCamera(temp.GetOffsetPosition(new Vector3(-5, -10, 3)), _player.Rotation, 60);
                    MissionCam.PointAt(temp.Position);
                    World.RenderingCamera = MissionCam;

                    GTA.UI.Screen.ShowHelpText("You can rob ATM's now, make sure you have the right tools for this job, because it will pay-out more!");
                    Wait(5000);

                    World.RenderingCamera = null;
                    MissionCam.Delete();
                    Player.Config.SetValue("ScriptSettings", "ID2", true);
                    Player.Config.Save();

                    temp.MarkAsNoLongerNeeded();
                    temp.Delete();
                    WaypointActive = false;
                }

                if (_player.IsInVehicle())
                {
                    _player.CurrentVehicle.IsPositionFrozen = false;
                }
                else
                {
                    _player.IsPositionFrozen = false;

                }

            }
            else
            {
                GTA.UI.Screen.ShowSubtitle("Go to the ~y~waypoint~w~ to find out what you have unlocked.", 1000);
            }
        }

        private void CheckIfPlayerIscloseWayPointLevel41()
        {
            if (!Ready41)
                return;

            if (Vector3.Distance(_player.Position, Waypoint.Position) <= 25)
            {
                Ready41 = false;
                Waypoint.Delete();
                Waypoint = null;
                if (_player.IsInVehicle())
                {
                    _player.CurrentVehicle.IsPositionFrozen = true;
                    Game.Player.WantedLevel = 0;
                }
                else
                {
                    _player.IsPositionFrozen = true;
                    Game.Player.WantedLevel = 0;

                }


                if (MissionCam == null)
                {
                    Vehicle temp = World.CreateVehicle(new Model(1871995513), ListWeaponStore[WStore_ID].Position);
                    temp.Rotation = ListWeaponStore[WStore_ID].Rotation;
                    temp.IsPositionFrozen = true;
                    temp.IsInvincible = true;
                    temp.IsCollisionEnabled = false;
                    temp.SetNoCollision(temp, true);
                    temp.IsVisible = false;

                    MissionCam = World.CreateCamera(temp.GetOffsetPosition(new Vector3(-5, -5, 3)), _player.Rotation, 60);
                    MissionCam.PointAt(temp.Position);
                    World.RenderingCamera = MissionCam;

                    GTA.UI.Screen.ShowHelpText("You can buy now special weapons that will help you with your heist", 5000, true);
                    Wait(5000);

                    World.RenderingCamera = null;
                    MissionCam.Delete();
                    MissionCam = null;
                    Player.Config.SetValue("ScriptSettings", "ID41", true);
                    Player.Config.Save();
                    WaypointLevel42();
                    temp.MarkAsNoLongerNeeded();
                    temp.Delete();
                    WaypointActive = false;

                }

                if (_player.IsInVehicle())
                {
                    _player.CurrentVehicle.IsPositionFrozen = false;
                }
                else
                {
                    _player.IsPositionFrozen = false;
                }

            }
            else
            {
                GTA.UI.Screen.ShowSubtitle("Go to the ~y~waypoint~w~ to find out what you have unlocked.", 1000);
            }
        }

        private void CheckIfPlayerIscloseWayPointLevel42()
        {
            if (!Ready42)
                return;

            if (Vector3.Distance(_player.Position, Waypoint.Position) <= 25)
            {
                Ready42 = false;
                Waypoint.Delete();
                Waypoint = null;
                if (_player.IsInVehicle())
                {
                    _player.CurrentVehicle.IsPositionFrozen = true;
                    Game.Player.WantedLevel = 0;
                }
                else
                {
                    _player.IsPositionFrozen = true;
                    Game.Player.WantedLevel = 0;

                }


                if (MissionCam == null)
                {
                    MissionCam = World.CreateCamera(ListCarShop[0].Vehicle.GetOffsetPosition(new Vector3(-12, -10, 5)), _player.Rotation, 60);
                    MissionCam.PointAt(ListCarShop[0].Vehicle.Position);
                    World.RenderingCamera = MissionCam;

                    GTA.UI.Screen.ShowHelpText("You can buy now special vehicles, trust me, you will need a good ride for a heist.", 5000, true);
                    Wait(5000);

                    World.RenderingCamera = null;
                    MissionCam.Delete();
                    MissionCam = null;
                    Player.Config.SetValue("ScriptSettings", "ID42", true);
                    Player.Config.Save();
                    WaypointActive = false;
                }

                if (_player.IsInVehicle())
                {
                    _player.CurrentVehicle.IsPositionFrozen = false;
                }
                else
                {
                    _player.IsPositionFrozen = false;
                }

            }
            else
            {
                GTA.UI.Screen.ShowSubtitle("Go to the ~y~waypoint~w~ to find out what you have unlocked.", 1000);
            }
        }

        private void CheckIfPlayerIscloseWayPointLevel51()
        {
            if (!Ready51)
                return;

            if (Vector3.Distance(_player.Position, Waypoint.Position) <= 5)
            {
                Ready51 = false;
                Waypoint.Delete();
                Waypoint = null;
                if (_player.IsInVehicle())
                {
                    _player.CurrentVehicle.IsPositionFrozen = true;
                    Game.Player.WantedLevel = 0;
                }
                else
                {
                    _player.IsPositionFrozen = true;
                    Game.Player.WantedLevel = 0;

                }


                if (MissionCam == null)
                {
                    Model lol = new Model(1871995513);
                    lol.Request(250);

                    Vehicle temp = World.CreateVehicle(lol, ListHouses[0].OutsideMarkerPos);
                    temp.Rotation = ListHouses[0].TempCarRot;
                    temp.IsPositionFrozen = true;
                    temp.IsInvincible = true;
                    temp.IsCollisionEnabled = false;
                    temp.SetNoCollision(temp, true);
                    temp.IsVisible = false;

                    MissionCam = World.CreateCamera(temp.GetOffsetPosition(new Vector3(0f, -100, 150)), _player.Rotation, 80);
                    MissionCam.PointAt(ListHouses[0].EntryPos);
                    MissionCam.Shake(CameraShake.Hand, 1);
                    World.RenderingCamera = MissionCam;
                    _player.IsPositionFrozen = true;



                    GTA.UI.Screen.ShowHelpText("If you are trying to hide from the cops, then this is the place to be!", 4000, true);
                    Wait(4000);

                    GTA.UI.Screen.ShowHelpText("Make sure the cops haven't spotted you, and enter your Secret Safehouse to lose the wanted level", 4000, true);
                    Wait(4000);


                    _player.IsPositionFrozen = false;

                    World.RenderingCamera = null;
                    MissionCam.Delete();
                    MissionCam = null;
                    Player.Config.SetValue("ScriptSettings", "ID51", true);
                    Player.Config.Save();
                    WaypointActive = false;

                    temp.MarkAsNoLongerNeeded();
                    temp.Delete();
                    Notification.Show(NotificationIcon.Lester, "Lester", "...", "Looks like you have made a name for yourself here, buy one of the Secret Safehouses and i will show you what money is.", false, true);

                }

                if (_player.IsInVehicle())
                {
                    _player.CurrentVehicle.IsPositionFrozen = false;
                }
                else
                {
                    _player.IsPositionFrozen = false;
                }

            }
            else
            {
                GTA.UI.Screen.ShowSubtitle("Go to the ~y~waypoint~w~ to find out what you have unlocked.", 1000);
            }
        }



        void WaypointLevel2()
        {
            if (ListATM.Count == 0)
            {

                InitializeATM();
            }

            Wait(100);
            int index = 0;
            float closestLength = 0;

            for (int i = 0; i < ListATM.Count; i++)
            {
                if (i == 0)
                {
                    closestLength = Vector3.Distance(_player.Position, ListATM[i].Position);
                    index = i;
                }
                else
                {

                    if (Vector3.Distance(_player.Position, ListATM[i].Position) < closestLength)
                    {
                        closestLength = Vector3.Distance(_player.Position, ListATM[i].Position);
                        index = i;
                    }
                }

            }

            ATM_ID_Waypoint = index;
            Ready2 = true;

            GoToWaypoint(ListATM[ATM_ID_Waypoint].Position);
        }

        void WaypointLevel41()
        {

            InitializeWeaponStore();
            Wait(200);

            int index = 0;
            float closestLength = 0;

            for (int i = 0; i < ListWeaponStore.Count; i++)
            {
                if (i == 0)
                {
                    closestLength = Vector3.Distance(_player.Position, ListWeaponStore[i].Position);
                    index = i;
                }
                else
                {

                    if (Vector3.Distance(_player.Position, ListWeaponStore[i].Position) < closestLength)
                    {
                        closestLength = Vector3.Distance(_player.Position, ListWeaponStore[i].Position);
                        index = i;
                    }
                }

            }
            WStore_ID = index;
            Ready41 = true;
            GoToWaypoint(ListWeaponStore[index].Position);

        }
        void WaypointLevel42()
        {

            if (ListWeaponStore.Count == 0)
            {

                InitializeWeaponStore();
                Wait(200);
            }

            if (ListCarShop.Count == 0)
            {
                InitializeCarShop();
                Wait(200);
            }

            GoToWaypoint(ListCarShop[0].MarkerPos);
            Ready42 = true;
        }
        void WaypointLevel51()
        {
            if (ListHouses.Count == 0)
            {
                InitializeHouses();
                Wait(200);
            }

            GoToWaypoint(ListHouses[0].OutsideMarkerPos);
            Ready51 = true;


        }


        private void MainMenuCars_OnMenuClose(UIMenu sender)
        {
            if (SpawnCarTemp != null)
                SpawnCarTemp.Delete();
        }

        private void MainMenuCars_OnIndexChange(UIMenu sender, int newIndex)
        {
            ChangeCarInMenu(newIndex);
        }

        private void MenuCars_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            for (int i = 0; i < ListCarItems.Count; i++)
            {
                if (selectedItem == ListCarItems[i])
                {
                    if (ListCarItemsLevel[i] <= Player.PlayerLevel)
                    {
                        BuyCar(ListCars[index].Price, ListCars[index].Name);
                        MainMenuCars.Visible = false;
                        break;
                    }
                    else
                    {
                        Notification.Show("You need to be Level ~r~" + ListCarItemsLevel[i] + "~w~ to unlock this car");
                    }
                }
            }
        }

        void CheckMoney()
        {
            if (Game.IsControlJustPressed(GTA.Control.CharacterWheel))
            {
                Notification.Show("Your current balance: ~g~$" + Player.Money + "~w~" + Environment.NewLine + "Your Level: ~g~" + Player.PlayerLevel + " (" + Player.XP.ToString() + " / " + LevelXP[Player.NextLevel].ToString() + ")");

            }
        }

        void CheckIfPlayerIsCloseCarShop()
        {

            for (int i = 0; i < ListCarShop.Count; i++)
            {
                if (Vector3.Distance(_player.Position, ListCarShop[i].MarkerPos) <= Player.DistanceForMenu + 5)
                {
                    if (ListCarShop[i].Mechanic != null)
                    {
                        ListCarShop[i].Mechanic.Task.TurnTo(_player);

                    }
                    if (Vector3.Distance(_player.Position, ListCarShop[i].MarkerPos) <= Player.DistanceForMenu)
                    {
                        if (HeistActive)
                        {
                            GTA.UI.Screen.ShowSubtitle("Unable to interact, your currently in a heist");
                            return;
                        }

                        DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to open the menu");
                        if (Game.IsControlJustPressed(GTA.Control.Context))
                        {
                            CarshopID = i;
                            //Open menu
                            if (CheckNearbyCars(i))
                            {
                                break;
                            }
                            else
                            {
                                MainMenuCars.Visible = true;
                                if (MainMenuCars.Visible)
                                {
                                    ShowCar();

                                    ChangeCarInMenu(0);

                                    _player.IsPositionFrozen = true;
                                }

                            }

                        }
                    }
                    else
                    {
                        //Close menu
                        MainMenuCars.Visible = false;
                    }
                }
            }

            if (!MainMenuCars.Visible)
            {
                //World.RenderingCamera = null;
                _player.IsPositionFrozen = false;
                if (CarCam != null)
                {
                    CarCam.Delete();
                    CarCam = null;
                }
            }
        }

        private void CheckIfPlayerHasBouhgt()
        {

            for (int i = 0; i < WeaponsSpecialList.Count; i++)
            {
                if (_player.Weapons.HasWeapon(WeaponsSpecialList[i].WeaponHash))
                {
                    WeaponsSpecialList[i].HasBought = true;
                }

            }

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {

                Game.Player.WantedLevel = 0;

                //Notification.Show("Distance: " + Vector3.Distance(_player.Position, ListHouses[CurrentApartmentID].Item6).ToString());

            }

            if (e.KeyCode == Keys.F9)
            {
                Notification.Show("Before: " + Waypoint.ShowRoute.ToString());
                Waypoint.ShowRoute = true;
                Notification.Show("After: " + Waypoint.ShowRoute.ToString());

            }
            if (e.KeyCode == Keys.F3)
            {
                if (Game.IsWaypointActive)
                {
                    Vector3 lol = Function.Call<Vector3>(Hash.GET_BLIP_COORDS, World.WaypointBlip);
                    _player.Position = lol;
                }


            }

            if (e.KeyCode == Keys.F10)
            {
                Player.Temp = ScriptSettings.Load("scripts\\lol.ini");
                Player.Temp.SetValue("PLAYERSETTINGS", "Pos", "new Vector3(" + _player.Position.X + "f, " + _player.Position.Y + "f, " + _player.Position.Z + "f)," + Environment.NewLine + "new Vector3(" + _player.Rotation.X + "f, " + _player.Rotation.Y + "f, " + _player.Rotation.Z + "f),");
                Player.Temp.Save();
            }

            if (e.KeyCode == Keys.F1)
            {
                Function.Call(Hash.REQUEST_MISSION_AUDIO_BANK, "creatures@deer@amb@world_deer_grazing@base", 1, -1);
            }
            if (e.KeyCode == Keys.F2)
            {
                Function.Call(Hash.REQUEST_MISSION_AUDIO_BANK, "creatures@deer@amb@world_deer_grazing@enter", 1, -1);
            }
            if (e.KeyCode == Keys.F3)
            {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "WIN", "0");

            }



        }

        #region Menu's
        void SetupMenu()
        {
            SetupMenuTools();
            Wait(100);
            SetupMenuWeapons();
            Wait(100);
            SetupMenuCars();
            Wait(100);
            SetupMenuHeist();
            Wait(100);
            SetupMenuBodyguard();
            Wait(100);

            MainMenuTools.OnItemSelect += MainMenuTools_OnItemSelect;
            MainMenuWeapons.OnItemSelect += MainMenuWeapons_OnItemSelect;
            MainMenuCars.OnItemSelect += MenuCars_OnItemSelect;
            MainMenuHeist.OnItemSelect += MainMenuHeist_OnItemSelect;
            MainMenuBodyguard.OnItemSelect += MainMenuBodyGuard_OnItemSelect;
            MainMenuBodyguard.OnIndexChange += MainMenuBodyGuard_OnIndexChange;
            MainMenuBodyguard.OnMenuClose += MainMenuBodyGuard_OnMenuClose;
            MainMenuCars.OnIndexChange += MainMenuCars_OnIndexChange;
            MainMenuCars.OnMenuClose += MainMenuCars_OnMenuClose;

            for (int i = 0; i < MenuWeaponsList.Count; i++)
            {
                MenuWeaponsList[i].OnItemSelect += SubMenuWeapons_OnItemSelect;
            }


        }

    

        private void MainMenuBodyGuard_OnMenuClose(UIMenu sender)
        {

            for (int i = 0; i < ListBodyguardShop.Count; i++)
            {
                if (ListBodyguardShop[i].BodyguardPlaceholder != null)
                {
                    ListBodyguardShop[i].BodyguardPlaceholder.Delete();
                }
            }

            Player.CurrentBodyguardShopID = -1;
            World.RenderingCamera = null;

        }

        private void MainMenuBodyGuard_OnIndexChange(UIMenu sender, int newIndex)
        {
            ListBodyguardShop[Player.CurrentBodyguardShopID].SwitchBodyguard(newIndex);
        }

        private void MainMenuBodyGuard_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            ListBodyguardShop[Player.CurrentBodyguardShopID].BuyBodyguard(index);
            MainMenuBodyguard.Visible = false;

        }

        void SetupMenuTools()
        {

            MainMenuTools = new UIMenu("Black Market Tools", "Tools that helps with ATM robberies");
            MenuToolsItemListLevel.Add(1);
            MenuToolsItemListLevel.Add(1);
            MenuToolsItemListLevel.Add(3);
            MenuToolsItemListLevel.Add(3);
            for (int i = 0; i < ListWeapons.Count; i++)
            {
                UIMenuItem item = new UIMenuItem("$" + ListWeapons[i].Price + " - " + ListWeapons[i].Name, ListWeapons[i].Description);
                MenuToolsItemList.Add(item);


            }

            MenuPool = new MenuPool();
            MenuPool.Add(MainMenuTools);

            for (int i = 0; i < MenuToolsItemList.Count; i++)
            {
                MainMenuTools.AddItem(MenuToolsItemList[i]);

            }

        }




        void SetupMenuWeapons()
        {


            MainMenuWeapons = new UIMenu("Weapon Shop", "Weapons that helps with robberies");

            MenuWeaponsItemListLevel.Add(4);
            MenuWeaponsItemListLevel.Add(4);
            MenuWeaponsItemListLevel.Add(5);
            MenuWeaponsItemListLevel.Add(5);
            MenuWeaponsItemListLevel.Add(5);

            MenuPool1 = new MenuPool();

            Wait(5);

            for (int i = 0; i < WeaponsSpecialList.Count; i++)
            {

                //item = new UIMenu("$" + WeaponsSpecialList[i].Item2 + " - " + WeaponsSpecialList[i].Item1, WeaponsSpecialList[i].Item4);
                SubMenuWeapons = MenuPool1.AddSubMenu(MainMenuWeapons, "$" + WeaponsSpecialList[i].Price + " - " + WeaponsSpecialList[i].Name, WeaponsSpecialList[i].Description);
                MenuWeaponsList.Add(SubMenuWeapons);

            }
            Wait(5);

            MenuPool1.Add(MainMenuWeapons);

            for (int i = 0; i < WeaponsSpecialList.Count; i++)
            {
                for (int j = 0; j < WeaponsSpecialList[i].Component.Count; j++)
                {
                    UIMenuItem item = new UIMenuItem("$ " + WeaponsSpecialList[i].Component[j].Price + " - " + WeaponsSpecialList[i].Component[j].Name.ToString());
                    MenuComponentsList.Add(item);
                }

            }
            Wait(5);


            int temp = 0;
            for (int j = 0; j < WeaponsSpecialList.Count; j++)
            {
                Wait(5);
                for (int i = temp; i < MenuComponentsList.Count; i++)
                {

                    if (WeaponsSpecialList[j].Component.Count - 1 == i - temp)
                    {

                        temp = i + 1;
                        MenuWeaponsList[j].AddItem(MenuComponentsList[i]);
                        break;
                    }

                    MenuWeaponsList[j].AddItem(MenuComponentsList[i]);


                }
            }
        }
        void SetupMenuCars()
        {

            MainMenuCars = new UIMenu("Car Shop", "Need a escape vehicle?");
            ListCarItemsLevel.Add(4);
            ListCarItemsLevel.Add(4);
            ListCarItemsLevel.Add(5);
            ListCarItemsLevel.Add(5);
            ListCarItemsLevel.Add(5);

            for (int i = 0; i < ListCars.Count; i++)
            {
                UIMenuItem item = new UIMenuItem("$" + ListCars[i].Price + " - " + ListCars[i].Name);
                ListCarItems.Add(item);


            }

            MenuPool2 = new MenuPool();
            MenuPool2.Add(MainMenuCars);

            for (int i = 0; i < ListCarItems.Count; i++)
            {
                MainMenuCars.AddItem(ListCarItems[i]);

            }

        }
        void SetupMenuBodyguard()
        {
            MainMenuBodyguard = new UIMenu("Bodyguard", "Need more backup? Then you are at the right place!");


            for (int i = 0; i < ListBodyguardShop[0].Bodyguards.Count; i++)
            {
                ListBodyguardItems.Add(new UIMenuItem("$" + ListBodyguardShop[0].Bodyguards[i].Cost + " " + ListBodyguardShop[0].Bodyguards[i].Name, ListBodyguardShop[0].Bodyguards[i].Description));

            }


            MenuPoolBodyguard = new MenuPool();
            MenuPoolBodyguard.Add(MainMenuBodyguard);

            for (int i = 0; i < ListBodyguardItems.Count; i++)
            {
                MainMenuBodyguard.AddItem(ListBodyguardItems[i]);

            }

        }

        void SetupMenuHeist()
        {
            MainMenuHeist = new UIMenu("Heist", "This is the big money, choose a bank and lets get the money");
            ListHeistItemsLevel.Add(5);
            ListHeistItemsLevel.Add(5);
            ListHeistItemsLevel.Add(6);
            ListHeistItemsLevel.Add(6);
            ListHeistItemsLevel.Add(7);


            ListHeistItems.AddRange(new List<UIMenuItem>
            {
                new UIMenuItem("Fleeca Job")
            });



            MenuPoolHeist = new MenuPool();
            MenuPoolHeist.Add(MainMenuHeist);

            for (int i = 0; i < ListHeistItems.Count; i++)
            {
                MainMenuHeist.AddItem(ListHeistItems[i]);

            }

        }

        private void MainMenuHeist_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            HeistActive = true;
            CurrentHeistID = index;
            if (MainMenuHeist.Visible)
                MainMenuHeist.Visible = false;

            switch (index)
            {
                case 0:

                    IHeist heist = new Fleeca(ListHouses[Player.CurrentApartmentID].EntryPos);
                    ListHeist.Add(heist);
                    break;
            }


            GTA.UI.Screen.FadeOut(1000);
            Wait(1000);
            ListHouses[Player.CurrentApartmentID].ExitApartment();
            GTA.UI.Screen.FadeIn(1000);

            GoToWaypoint(ListHeist[index].Position);

        }

        private void MainMenuTools_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            for (int i = 0; i < ListWeapons.Count; i++)
            {
                if (selectedItem == MenuToolsItemList[i])
                {
                    if (MenuToolsItemListLevel[i] <= Player.PlayerLevel)
                    {
                        GivePlayerWeapon(ListWeapons[i].WeaponHash, i, false);
                        break;
                    }
                    else
                    {
                        Notification.Show("You need to be Level " + MenuToolsItemListLevel[i] + " to unlock this item");
                    }
                }
            }
        }

        private void MainMenuWeapons_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            for (int i = 0; i < WeaponsSpecialList.Count; i++)
            {
                if (selectedItem == MenuWeaponsList[i].ParentItem)
                {
                    if (MenuWeaponsItemListLevel[i] <= Player.PlayerLevel)
                    {
                        if (!WeaponsSpecialList[i].HasBought)
                        {
                            GivePlayerWeapon(WeaponsSpecialList[i].WeaponHash, i, true);
                        }
                        break;
                    }
                    else
                    {
                        Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, 0, "SET_DATA_SLOT");

                        GoBack = i;
                        Notification.Show("You need to be Level " + MenuWeaponsItemListLevel[i] + " to unlock this item");
                        break;

                    }
                }
            }
        }
        void CheckMenuLowLevel()
        {
            if (GoBack >= 0)
            {
                if (!MainMenuWeapons.Visible)
                {
                    MenuWeaponsList[GoBack].GoBack();
                    GoBack = -1;
                }
            }
        }

        private void SubMenuWeapons_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            bool breaking = false;
            int temp = 0;
            for (int j = 0; j < WeaponsSpecialList.Count; j++)
            {
                if (breaking)
                    return;
                for (int i = temp; i < MenuComponentsList.Count; i++)
                {

                    if (selectedItem == MenuComponentsList[i])
                    {
                        GiveWeaponComponents(WeaponsSpecialList[j].WeaponHash, WeaponsSpecialList[j].Component[index].Id, j, index);

                        breaking = true;
                        break;
                    }


                    if (WeaponsSpecialList[j].Component.Count - 1 == i - temp)
                    {
                        temp = i + 1;
                        break;
                    }



                }
            }
        }
        #endregion

        #region SimpleFunction

        void ChangeCarInMenu(int newIndex)
        {
            if (SpawnCarTemp != null)
            {
                SpawnCarTemp.Delete();
            }

            SpawnCarTemp = World.CreateVehicle(ListCars[newIndex].Model, ListCarShop[CarshopID].VehicleShowcasePos);
            SpawnCarTemp.Rotation = ListCarShop[CarshopID].VehicleShowcaseRot;
        }

        bool CheckNearbyCars(int i)
        {


            if (NearbyCarsTemp != null)
            {
                for (int j = 0; j < NearbyCarsBlips.Count; j++)
                {
                    NearbyCarsBlips[j].Delete();
                }
                Array.Clear(NearbyCarsTemp, 0, NearbyCarsTemp.Length);
                NearbyCarsBlips.Clear();
            }
            NearbyCarsTemp = World.GetNearbyVehicles(ListCarShop[i].VehicleShowcasePos, 10);


            for (int k = 0; k < NearbyCarsTemp.Length; k++)
            {
                Blip blip = World.CreateBlip(NearbyCarsTemp[k].Position);
                blip.IsFlashing = true;
                blip.IsShortRange = true;
                blip.IsFriendly = false;
                NearbyCarsBlips.Add(blip);
            }


            if (NearbyCarsTemp.Length > 0)
            {
                GTA.UI.Screen.ShowSubtitle("Please move the vehicle shown on the mini map, because it is blocking the spawn point", 5000);
                return true;
            }
            else
            {
                return false;
            }

        }
        void BuyCar(int carCost, string carName)
        {
            if (Player.Money >= carCost)
            {
                Player.Money -= carCost;
                SpawnCarTemp.IsPersistent = true;
                SpawnCarTemp = null;

                Notification.Show("You bought: " + carName);

            }
            else
            {
                Notification.Show("You don't have enough money to buy this car");
            }
        }

        void ShowCar()
        {
            if (CarCam == null)
            {
                CarCam = World.CreateCamera(new Vector3(-1457.819f, -652.6882f, 29.50239f), new Vector3(0, 0, -13.01742f), 60);
                CarCam.Shake(CameraShake.Hand, 1);
            }
            World.RenderingCamera = CarCam;


        }



        void GiveWeaponComponents(WeaponHash weaponHash, uint component, int weaponIndex, int ComponentsIndex)
        {
            if (Player.Money >= WeaponsSpecialList[weaponIndex].Component[ComponentsIndex].Price)
            {

                _player.Weapons.Select(weaponHash);
                if (_player.Weapons.Current.Ammo >= _player.Weapons.Current.MaxAmmo)
                {
                    Notification.Show("You have the maximum amount of this item", false);
                    return;
                }
                else
                {
                    Notification.Show("Comp: " + component.ToString(), true);
                    Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_PED, _player, weaponHash, WeaponsSpecialList[weaponIndex].Component[ComponentsIndex].Hash);

                    Function.Call(Hash._ADD_AMMO_TO_PED_BY_TYPE, _player, Function.Call<uint>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, _player, weaponHash), _player.Weapons.Current.DefaultClipSize);


                    Player.Money -= WeaponsSpecialList[weaponIndex].Component[ComponentsIndex].Price;
                    Notification.Show("You have bought " + WeaponsSpecialList[weaponIndex].Component[ComponentsIndex].Name, false);
                }
            }
            else
            {
                Notification.Show("You don't have enough money to buy this item", false);
            }
        }

        void CheckIfPlayerCloseToWeaponStore()
        {


            for (int i = 0; i < ListWeaponStore.Count; i++)
            {
                if (Vector3.Distance(_player.Position, ListWeaponStore[i].Position) <= Player.DistanceForMenu + 5)
                {
                    if (Vector3.Distance(_player.Position, ListWeaponStore[i].Position) <= Player.DistanceForMenu)
                    {
                        if (HeistActive)
                        {
                            GTA.UI.Screen.ShowSubtitle("Unable to interact, your currently in a heist");
                            return;
                        }

                        DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to open the menu");
                        if (Game.IsControlJustPressed(GTA.Control.Context))
                        {
                            Notification.Show("Pressed!!!", false);
                            MainMenuWeapons.Visible = true;
                            Notification.Show(MainMenuWeapons.Visible.ToString() + " menu", false);

                        }
                    }
                    else
                    {
                        MainMenuWeapons.Visible = false;

                    }
                }

            }
        }

        void CheckPlayerCloseToCamp()
        {


            int i = 0;
            while (i < CampList.Count)
            {
                if (Vector3.Distance(_player.Position, CampList[i].Position) <= Player.DistanceForMenu + 10)
                {
                    if (CampList[i].Soldier != null)
                        CampList[i].Soldier.Task.LookAt(_player);

                    //Function.Call(Hash.DRAW_MARKER, 1, CampList[i].Table.Position.X, CampList[i].Table.Position.Y, CampList[i].Table.Position.Z, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 1f, 0.5f, 255, 128, 0, 50, false, true, 2, false, false, false, false);

                    if (Vector3.Distance(_player.Position, CampList[i].Position) <= Player.DistanceForMenu)
                    {
                        if (HeistActive)
                        {
                            GTA.UI.Screen.ShowSubtitle("Unable to interact, your currently in a heist");
                            return;
                        }
                        DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to open the menu");
                        if (Game.IsControlJustPressed(GTA.Control.Context))
                        {
                            MainMenuTools.Visible = !MainMenuTools.Visible;
                        }
                        break;
                    }
                    else
                    {
                        if (MainMenuTools.Visible)
                            MainMenuTools.Visible = false;

                    }
                }



                i++;
            }
        }

        void CheckRobberyStatus()
        {
            if (IsRobbing)
            {
                if (Game.Player.IsDead)
                {
                    robbedMoney = 0;
                    robbedXP = 0;
                    IsRobbing = false;
                    Notification.Show("You died while escaping, make sure next time you don't get caught", false);
                    Player.RemoveBag();

                    if (HeistActive)
                    {
                        HeistActive = false;
                    }

                }
                else if (Game.Player.WantedLevel == 0)
                {
                    if (HeistActive)
                    {
                        if (ListHeist[CurrentHeistID].CheckPoints.Count - 1 != Heistcheckpoint)
                        {
                            return;
                        }
                        if (Vector3.Distance(_player.Position, ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition) <= 2)
                        {
                            if (Player.IsInApartment)
                            {
                                //player is at home and the heist is done
                                HeistRobbedSuccesfully();
                            }
                        }
                    }
                    else
                    {
                        SuccesfullyRobbed();
                    }

                }
            }
        }

        private void HeistRobbedSuccesfully()
        {
            int Lestercut = (int)(robbedMoney * 0.1);
            HeistActive = false;
            Player.XP += robbedXP;
            Player.Money += (robbedMoney - Lestercut);
            Notification.Show("Total money stolen: ~g~$" + robbedMoney + Environment.NewLine + "~w~Your cut: ~g~$" + (robbedMoney - Lestercut) + Environment.NewLine + "~w~Lester cut: ~g~$" + Lestercut + " (10%)");
            robbedMoney = 0;
            robbedXP = 0;
            IsRobbing = false;
            Player.RemoveBag();
            OutOfHeist();
            if (Waypoint != null)
            {
                Waypoint.Delete();
                Waypoint = null;
                WaypointActive = false;
            }
        }


        void ProcessMenu()
        {
            if (MenuPool != null)
            {
                MenuPool.ProcessMenus();
            }

            if (MenuPool1 != null)
            {
                MenuPool1.ProcessMenus();
            }

            if (MenuPool2 != null)
            {
                MenuPool2.ProcessMenus();
            }

            if (MenuPoolHeist != null)
            {
                MenuPoolHeist.ProcessMenus();
            }

            if (MenuPoolBodyguard != null)
            {
                MenuPoolBodyguard.ProcessMenus();
            }

            for (int i = 0; i < WeaponsSpecialList.Count; i++)
            {
                if (WeaponsSpecialList[i].HasBought)
                {
                    MenuWeaponsList[i].ParentItem.Text = WeaponsSpecialList[i].Name;
                }
            }
        }


        void PlaySound(string soundFileName)
        {

            SoundPlayer soundPlayer = new SoundPlayer(".\\scripts\\RobbingHood_sounds\\" + soundFileName + ".wav");
            soundPlayer.Load();
            soundPlayer.Play();
        }


        void CheckIfATMIsAvailable()
        {
            for (int i = 0; i < ListATM.Count; i++)
            {
                if (ListATM[i].AvailbleTime != ListATM[i].NullDate)
                {
                    if (World.CurrentDate > ListATM[i].AvailbleTime)
                    {
                        ListATM[i].OpenATM();
                    }
                }
            }
        }

        void CheckIfStoreIsAvailable()
        {
            for (int i = 0; i < ListStoreShops.Count; i++)
            {
                if (ListStoreShops[i].AvailbleTime != ListStoreShops[i].NullDate)
                {

                    if (World.CurrentDate > ListStoreShops[i].AvailbleTime)
                    {
                        ListStoreShops[i].OpenStore();
                    }
                }
            }
        }

        void CheckForNewLevel()
        {
            if (Player.XP > Player.PreviousXP)
            {
                Player.PreviousXP = Player.XP;

                for (int i = Player.PlayerLevel; i < LevelXP.Count; i++)
                {
                    if (Player.XP > LevelXP[i])
                    {
                        Player.PlayerLevel = i;

                        if (Player.PlayerLevel > Player.PreviousPlayerLevel)
                        {
                            if (Player.PlayerLevel < 10)
                                Player.NextLevel = Player.PlayerLevel + 1;

                            PlaySound("LevelUp");
                            Player.SaveSettings();

                            Player.PreviousPlayerLevel = Player.PlayerLevel;
                            ShowRankUp = true;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        void CheckIfTutIsCompleted()
        {
            if (WaypointActive)
                return;

            if (Player.PlayerLevel == 2 && !Player.Config.GetValue("ScriptSettings", "id2", false))
            {
                WaypointLevel2();
                WaypointActive = true;
                return;

            }

            if (Player.PlayerLevel == 3 && !Player.Config.GetValue("ScriptSettings", "id3", false))
            {
                GoToWaypoint(CampList[0].Position);
                AbleToPlayScene = true;
                WaypointActive = true;
                return;


            }
            if (Player.PlayerLevel == 4 && !Player.Config.GetValue("ScriptSettings", "id41", false))
            {
                WaypointLevel41();
                WaypointActive = true;
                return;

            }
            else if (Player.PlayerLevel == 4 && !Player.Config.GetValue("ScriptSettings", "id42", false))
            {
                WaypointLevel42();
                WaypointActive = true;
                return;
            }

            if (Player.PlayerLevel == 5 && !Player.Config.GetValue("ScriptSettings", "ID51", false))
            {
                WaypointLevel51();
                WaypointActive = true;
                return;

            }
        }

        void RankUp()
        {
            if (ShowRankUp)
            {
                if (!CallOnce)
                {
                    CallOnce = true;
                    UINextTime = Game.GameTime + UIDelayTime;
                }
                if (Game.GameTime < UINextTime)
                {
                    //Test.CallFunction("INITIALISE", 5000);
                    //Test.CallFunction("SHOW_SHARD_WASTED_MP_MESSAGE", "~y~Nice", "Very Nice", 1);
                    UIHandler.CallFunction("SHOW_SHARD_RANKUP_MP_MESSAGE", "Rank Up", "Rank: " + Player.PlayerLevel + ", " + LevelName[Player.PlayerLevel], 9);
                    //Test.Render2DScreenSpace(new PointF(130f, -185f), new PointF(1000f, 700f));

                    UIHandler.Render2D();
                }
                else
                {
                    ShowRankUp = false;
                    CallOnce = false;
                }
            }
        }

        void GoToWaypoint(Vector3 destination)
        {
            if (Waypoint != null)
            {
                Waypoint.Delete();
            }
            WaypointActive = true;
            Waypoint = World.CreateBlip(destination);
            Waypoint.Color = BlipColor.Yellow;
            Waypoint.ShowRoute = true;



        }

        void SuccesfullyRobbed()
        {
            Player.XP += robbedXP;
            Player.Money += robbedMoney;
            robbedMoney = 0;
            robbedXP = 0;
            IsRobbing = false;
            Player.RemoveBag();

        }

        void SafeRobbed(int index)
        {
            IsRobbing = true;
            ListStoreShops[index].CloseStore();

            int robbedMoneytemp = random.Next(250, 500);
            int robbedXPtemp = random.Next(50, 80);

            if (Game.Player.WantedLevel < 2)
            {
                Game.Player.WantedLevel = 2;

            }


            if (_player.Weapons.HasWeapon(WeaponHash.PipeBomb))
            {
                _player.Weapons.Select(WeaponHash.PipeBomb);
                if (_player.Weapons.Current.Ammo == 1)
                {
                    _player.Weapons.Remove(WeaponHash.PipeBomb);

                }
                else
                {
                    _player.Weapons.Current.Ammo--;

                }

                robbedMoney += (int)(robbedMoneytemp * 1.8);
                robbedXP += (int)(robbedXPtemp * 1.4);

            }
            else if (_player.Weapons.HasWeapon(WeaponHash.StickyBomb))
            {
                _player.Weapons.Select(WeaponHash.StickyBomb);
                _player.Weapons.Current.Ammo--;
                if (_player.Weapons.Current.Ammo == 1)
                {
                    _player.Weapons.Remove(WeaponHash.StickyBomb);

                }
                else
                {
                    _player.Weapons.Current.Ammo--;

                }
                robbedMoney += (int)(robbedMoneytemp * 1.4);
                robbedXP += robbedXPtemp;

            }
            else if (_player.Weapons.HasWeapon(WeaponHash.Crowbar))
            {
                robbedMoney += robbedMoneytemp;
                robbedXP += robbedXPtemp;

            }
            else if (_player.Weapons.HasWeapon(WeaponHash.Knife))
            {
                robbedMoney += robbedMoneytemp;
                robbedXP += robbedXPtemp;

                KnifeUsed++;

                if (KnifeUsed == KnifeBreakingAmount)
                {
                    Notification.Show("Your Knife has been broken, a knife breaks after using 3 times. To Rob again buy a new Knife, or a new Crowbar (Crowbar never breaks)", true);
                    KnifeUsed = 0;
                    _player.Weapons.Remove(WeaponHash.Knife);
                }
            }

            Notification.Show("You have robbed: ~g~$" + robbedMoney.ToString(), false);

        }

        void ATMRobbed(int index)
        {
            IsRobbing = true;

            ListATM[index].CloseATM();

            int robbedMoneytemp = random.Next(800, 1300);
            int robbedXPtemp = random.Next(90, 120);
            if (Game.Player.WantedLevel <= 2)
            {
                Game.Player.WantedLevel = 2;

            }



            if (_player.Weapons.HasWeapon(WeaponHash.PipeBomb))
            {
                _player.Weapons.Select(WeaponHash.PipeBomb);
                if (_player.Weapons.Current.Ammo == 1)
                {
                    _player.Weapons.Remove(WeaponHash.PipeBomb);

                }
                else
                {
                    _player.Weapons.Current.Ammo--;

                }
                robbedMoney += (int)(robbedMoneytemp * 1.8);
                robbedXP += (int)(robbedXPtemp * 1.4);

            }
            else if (_player.Weapons.HasWeapon(WeaponHash.StickyBomb))
            {
                _player.Weapons.Select(WeaponHash.StickyBomb);
                _player.Weapons.Current.Ammo--;
                if (_player.Weapons.Current.Ammo == 1)
                {
                    _player.Weapons.Remove(WeaponHash.StickyBomb);

                }
                else
                {
                    _player.Weapons.Current.Ammo--;

                }
                robbedMoney += (int)(robbedMoneytemp * 1.4);
                robbedXP += robbedXPtemp;

            }
            else if (_player.Weapons.HasWeapon(WeaponHash.Crowbar))
            {
                robbedMoney += robbedMoneytemp;
                robbedXP += robbedXPtemp;
            }
            else if (_player.Weapons.HasWeapon(WeaponHash.Knife))
            {
                robbedMoney += robbedMoneytemp;
                robbedXP += robbedXPtemp;

                KnifeUsed++;

                if (KnifeUsed == KnifeBreakingAmount)
                {
                    Notification.Show("Your Knife has been broken, a knife breaks after using 3 times. To Rob again buy a new Knife, or a new Crowbar (Crowbar never breaks)", true);
                    KnifeUsed = 0;
                    _player.Weapons.Remove(WeaponHash.Knife);
                }
            }



            Notification.Show("You have robbed: ~g~$" + robbedMoney.ToString(), false);


        }

        void ScareNearbyPeds()
        {
            if (NearbyPeds != null)
            {
                for (int i = 0; i < NearbyPeds.Length; i++)
                {
                    NearbyPeds[i].Delete();
                }
                Array.Clear(NearbyPeds, 0, NearbyPeds.Length);
            }



            NearbyPeds = World.GetAllPeds();
            for (int i = 0; i < NearbyPeds.Length; i++)
            {
                if (NearbyPeds[i] != _player)
                {
                    if (NearbyPeds[i].IsInCombatAgainst(_player))
                    {
                        continue;
                    }
                    if (NearbyPeds[i].IsInVehicle())
                    {
                        //Speed up riding people
                        if (NearbyPeds[i].CurrentVehicle.IsStopped)
                        {
                            NearbyPeds[i].CurrentVehicle.Speed = 30;
                        }
                        else
                        {
                            NearbyPeds[i].CurrentVehicle.Speed += 20;

                        }

                    }
                    else
                    {
                        NearbyPeds[i].Task.ReactAndFlee(_player);

                    }

                }
            }
            Array.Clear(NearbyPeds, 0, NearbyPeds.Length);


            NearbyPeds = World.GetNearbyPeds(_player.Position, 5);
            for (int i = 0; i < NearbyPeds.Length; i++)
            {
                if (NearbyPeds[i] != _player && !NearbyPeds[i].IsInVehicle())
                {
                    NearbyPeds[i].Task.ClearAllImmediately();
                    NearbyPeds[i].Task.HandsUp(10000);
                }
            }
        }



        void GivePlayerWeapon(WeaponHash weaponHash, int weaponListIndex, bool specialWeapon)
        {
            if (specialWeapon)
            {
                if (Player.Money < WeaponsSpecialList[weaponListIndex].Price)
                {
                    Notification.Show("You don't have enough money to buy this item, your total money: ~r~$" + Player.Money, false);
                    GoBack = weaponListIndex;
                    return;
                }

                if (!_player.Weapons.HasWeapon(weaponHash))
                {
                    _player.Weapons.Give(weaponHash, 300, true, true);
                    WeaponsSpecialList[weaponListIndex].HasBought = true;
                    Notification.Show("You have bought a: " + WeaponsSpecialList[weaponListIndex].Name, false);

                }
                else
                {
                    if (_player.Weapons.Current.Ammo == _player.Weapons.Current.MaxAmmo)
                    {
                        Notification.Show("You have the maximum amount of this item", false);
                        return;
                    }

                    _player.Weapons.Select(weaponHash, true);
                    _player.Weapons.Current.Ammo += 30;

                }

                Player.Money -= WeaponsSpecialList[weaponListIndex].Price;
            }
            else
            {
                if (Player.Money < ListWeapons[weaponListIndex].Price)
                {
                    Notification.Show("You don't have enough money to buy this item, your total money: ~r~$" + Player.Money, false);
                    return;
                }
                if (weaponHash == WeaponHash.Knife || weaponHash == WeaponHash.Crowbar)
                {
                    if (!_player.Weapons.HasWeapon(weaponHash))
                    {
                        _player.Weapons.Give(weaponHash, 1, true, true);
                        Player.Money -= ListWeapons[weaponListIndex].Price;
                        Notification.Show("You have bought a: " + ListWeapons[weaponListIndex].Name, false);

                    }
                    else
                    {
                        Notification.Show("You already have this item in your inventory");

                    }
                }
                else
                {
                    if (!_player.Weapons.HasWeapon(weaponHash))
                    {
                        _player.Weapons.Give(weaponHash, 1, true, true);

                        Notification.Show("You have bought a: " + ListWeapons[weaponListIndex].Name);

                    }
                    else
                    {
                        if (_player.Weapons.Current.Ammo == _player.Weapons.Current.MaxAmmo)
                        {
                            Notification.Show("You have the maximum amount of this item");
                            return;
                        }

                        _player.Weapons.Select(weaponHash, true);
                        _player.Weapons.Current.Ammo += 1;

                    }

                    Player.Money -= ListWeapons[weaponListIndex].Price;
                }
            }

        }


        void MoneyHasChanged()
        {

            if (Player.Money > Player.previousMoney || Player.Money < Player.previousMoney)
            {
                if (Player.RealMoney)
                {
                    if (Player.Money > Player.previousMoney)
                    {
                        int earned = Player.Money - Player.previousMoney;
                        _player.Money -= earned;

                    }
                    else
                    {
                        int lossed = Player.previousMoney - Player.Money;
                        _player.Money -= lossed;

                    }
                }
                Player.previousMoney = Player.Money;
                NotifyBank();
                Player.SaveSettings();

            }
        }

        void NotifyBank()
        {
            Notification.Show("Your current balance: ~g~$" + Player.Money + "~w~" + Environment.NewLine + "Your Level: ~g~" + Player.PlayerLevel + " (" + Player.XP.ToString() + " / " + LevelXP[Player.NextLevel].ToString() + ")");

        }


        void DisplayHelpTextThisFrame(string text)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "STRING");
            //Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, text);
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, -1);
        }

        #endregion

        #region ATM

        void InitializeATM()
        {
            ATM atm = new ATM
                (
                new Vector3(-537.7364f, -853.2301f, 29.28673f),
                new Vector3(0f, 0f, -173.3764f)
                );
            ATM atm1 = new ATM
               (
                  new Vector3(-1108.748f, -1689.996f, 4.374625f),
               new Vector3(-0f, -0f, 0f)
               );
            ATM atm2 = new ATM
               (
            new Vector3(-619.1254f, -706.8843f, 30.05277f),
                new Vector3(-3.027737E-07f, 1.590277E-15f, -85.64626f)
               );
            ATM atm3 = new ATM
               (
               new Vector3(-254.0114f, -691.3589f, 33.58937f),
                new Vector3(-8.11653E-07f, 0f, 164.2604f)
               );

            ListATM.Add(atm);
            ListATM.Add(atm1);
            ListATM.Add(atm2);
            ListATM.Add(atm3);

            FirstTime = true;

        }

        void CheckIfPlayerCloseToATM()
        {

            for (int k = 0; k < ListATM.Count; k++)
            {
                if (ListATM[k].IsClosed)
                {
                    ListATM[k].Blip.Color = BlipColor.Grey;
                }
                else
                {
                    ListATM[k].Blip.Color = BlipColor.Green;

                }

                if (Vector3.Distance(_player.Position, ListATM[k].Position) <= 4)
                {
                    if (Vector3.Distance(_player.Position, ListATM[k].Position) <= Player.DistanceForATM)
                    {
                        if (HeistActive)
                        {
                            GTA.UI.Screen.ShowSubtitle("Unable to interact, your currently in a heist");
                            return;
                        }
                        if (Coppp != null)
                        {
                            Array.Clear(Coppp, 0, Coppp.Length);
                            Cop.Clear();
                        }

                        Coppp = World.GetNearbyPeds(_player.Position, 4, new Model(0x5E3DA4A4));
                        Cop.AddRange(Coppp);
                        for (int i = 0; i < Cop.Count; i++)
                        {
                            if (Cop[i].IsDead)
                            {
                                Cop[i].Delete();
                                Cop.RemoveAt(i);
                            }
                        }

                        if (ListATM[k].IsClosed)
                        {
                            ATM_ID = -1;
                            GTA.UI.Screen.ShowSubtitle("You have recently robbed this ATM, come back later", 1000);

                        }
                        else if (Cop.Count > 0)
                        {

                            GTA.UI.Screen.ShowSubtitle("You need to take out the cop before robbing the ATM", 1000);
                        }
                        else
                        {
                            if (_player.Weapons.HasWeapon(WeaponHash.Crowbar) || _player.Weapons.HasWeapon(WeaponHash.Knife))
                            {
                                if (_player.IsInVehicle())
                                {
                                    return;
                                }

                                ATM_ID = k;
                                DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to rob the ATM");
                                if (Game.IsControlJustPressed(GTA.Control.Context))
                                {
                                    Function.Call(Hash.DO_SCREEN_FADE_OUT, 1000);
                                    Wait(1000);
                                    Wait(200);

                                    if (_player.Weapons.HasWeapon(WeaponHash.PipeBomb))
                                    {
                                        PlaySound("ATMRobPipe");
                                        Wait(12000);

                                    }
                                    else if (_player.Weapons.HasWeapon(WeaponHash.StickyBomb))
                                    {
                                        PlaySound("ATMRobSticky");
                                        Wait(6500);

                                    }
                                    else
                                    {
                                        PlaySound("ATMRobKnife");
                                        Wait(5000);

                                    }

                                    ATMRobbed(ATM_ID);

                                    Function.Call(Hash.DO_SCREEN_FADE_IN, 1000);
                                    ScareNearbyPeds();
                                }

                            }
                            else
                            {
                                GTA.UI.Screen.ShowSubtitle("You don't have tools to rob this ATM, go to the Black Market shops and buy the right tools", 1000);
                                ATM_ID = -1;

                            }

                        }
                    }
                    else
                    {
                        ATM_ID = -1;


                    }
                }
            }
        }

        void SpawnCamp(bool start)
        {
            for (int i = 0; i < CampList.Count; i++)
            {
                if (i == 0 && !start)
                {
                    i = 1;
                }



                if (start)
                {
                    if (!Player.Config.GetValue<bool>("SCRIPTSETTINGS", "ID3", false))
                    {
                        break;
                    }
                }

            }

        }

        void SpawnCampIfNearby()
        {
            if (CampList.Count == 0)
            {
                lastCountCamp = 0;
                return;

            }

            if (CampList.Count > lastCountCamp)
            {
                int total = 0;
                if (!callonceCamp)
                {
                    total = Game.GameTime + 1500;
                    callonceCamp = true;
                }
                if (Game.GameTime < total)
                {
                    return;
                }
                else
                {
                    lastCountCamp = CampList.Count;
                }
            }

            for (int index = 0; index < CampList.Count; index++)
            {

                if (Vector3.Distance(_player.Position, CampList[index].Position) <= 250)
                {
                    if (Vector3.Distance(_player.Position, CampList[index].Position) <= 100)
                    {
                        CampList[index].SpawnCamp();


                    }
                    else if (Vector3.Distance(_player.Position, CampList[index].Position) > 150)
                    {
                        CampList[index].DeSpawn();

                    }
                }

            }



        }

        void SpawnWeaponstoreIfNearby()
        {
            if (ListWeaponStore.Count == 0)
            {
                lastCountWeapShop = 0;
                return;

            }

            if (lastCountWeapShop < ListWeaponStore.Count)
            {
                int total = 0;
                if (!callonceWeapShop)
                {
                    total = Game.GameTime + 1500;
                    callonceWeapShop = true;
                }
                if (Game.GameTime < total)
                {
                    return;
                }
                else
                {
                    lastCountWeapShop = ListWeaponStore.Count;
                }
            }

            for (int i = 0; i < ListWeaponStore.Count; i++)
            {
                if (Vector3.Distance(_player.Position, ListWeaponStore[i].Position) <= 250)
                {
                    if (Vector3.Distance(_player.Position, ListWeaponStore[i].Position) <= 100)
                    {
                        ListWeaponStore[i].Spawn();
                    }
                    else if (Vector3.Distance(_player.Position, ListWeaponStore[i].Position) > 120)
                    {
                        ListWeaponStore[i].DeSpawn();

                    }
                }
            }
        }

        void GoingInHeist()
        {
            ListATM.ForEach(a => a.Blip.Alpha = 0);
            ListStoreShops.ForEach(a => a.Blip.Alpha = 0);
            ListCarShop.ForEach(a => a.Blip.Alpha = 0);
            ListWeaponStore.ForEach(a => a.Blip.Alpha = 0);
            CampList.ForEach(a => a.Blip.Alpha = 0);
        }

        void OutOfHeist()
        {
            ListATM.ForEach(a => a.Blip.Alpha = 255);
            ListStoreShops.ForEach(a => a.Blip.Alpha = 255);
            ListCarShop.ForEach(a => a.Blip.Alpha = 255);
            ListWeaponStore.ForEach(a => a.Blip.Alpha = 255);
            CampList.ForEach(a => a.Blip.Alpha = 255);
        }

        void StartHeist()
        {
            if (!HeistActive)
                return;



            //Renders Marker
            if (ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition != new Vector3(0, 0, 0))
            {
                if (Vector3.Distance(_player.Position, ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition) <= 100)
                {
                    Function.Call(Hash.DRAW_MARKER, 1, ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition.X, ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition.Y, ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition.Z - 1f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 1f, 0.5f, 255, 128, 0, 50, false, true, 2, false, false, false, false);

                }
            }

            //if player is at checkpoint remove waypoint
            if (Vector3.Distance(_player.Position, ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition) <= 2)
            {
                if (!ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].PlayerHitCheckPoint)
                    ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].PlayerHitCheckPoint = true;

                if (WaypointActive)
                {
                    Waypoint.Delete();
                    Waypoint = null;
                    WaypointActive = false;

                }

                if (_player.IsInVehicle() && ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].InstantStopCar)
                {
                    _player.CurrentVehicle.IsPositionFrozen = true;
                    _player.CurrentVehicle.IsPositionFrozen = false;
                }

            }
            else
            {
                if (ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].PlayerHitCheckPoint)
                    ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].PlayerHitCheckPoint = false;

                if (WaypointActive)
                {
                    GTA.UI.Screen.ShowSubtitle("Go to the ~y~waypoint~w~ ", 2);
                }
            }



            switch (Heistcheckpoint)
            {
                case 0:
                    if (!HeistCallOnce)
                    {
                        GoingInHeist();
                        HeistCallOnce = true;
                        ListHeist[CurrentHeistID].CheckPointMessage(Heistcheckpoint);
                        GoToWaypoint(ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition);
                    }

                    if (ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].PlayerHitCheckPoint)
                    {
                        Heistcheckpoint++;
                        HeistCallOnce = false;
                    }
                    break;

                case 1:
                    if (!HeistCallOnce)
                    {
                        HeistCallOnce = true;
                        World.CreateVehicle(new Model(410882957), ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition, 40.15068f);
                        Fleeca.NearbyPeds = World.GetNearbyPeds(ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition, 8f);
                        ListHeist[CurrentHeistID].CheckPointMessage(Heistcheckpoint);

                    }
                    else
                    {
                        if (Fleeca.NearbyPedsBlip.Count == 0)
                        {
                            for (int i = 0; i < Fleeca.NearbyPeds.Length; i++)
                            {
                                Blip blip = World.CreateBlip(Fleeca.NearbyPeds[i].Position);
                                blip.IsFriendly = false;
                                Fleeca.NearbyPedsBlip.Add(blip);
                            }
                        }
                        else
                        {
                            int _aliveEnemies = Fleeca.NearbyPeds.Length;

                            for (int i = 0; i < Fleeca.NearbyPeds.Length; i++)
                            {
                                //if dead remove blib
                                if (Fleeca.NearbyPeds[i].IsDead)
                                {
                                    _aliveEnemies--;
                                    Fleeca.NearbyPedsBlip[i].Alpha = 0;
                                    continue;
                                }

                                // updates the red dot to the current enemy position
                                Fleeca.NearbyPedsBlip[i].Position = Fleeca.NearbyPeds[i].Position;

                            }

                            //everyone is dead
                            if (_aliveEnemies == 0)
                            {
                                HeistCallOnce = false;
                                Heistcheckpoint++;
                            }

                        }

                    }
                    break;
                case 2:
                    if (!HeistCallOnce)
                    {
                        HeistCallOnce = true;
                        ListHeist[CurrentHeistID].CheckPointMessage(Heistcheckpoint);
                        for (int i = 0; i < Fleeca.NearbyPeds.Length; i++)
                        {
                            Fleeca.NearbyPeds[i].MarkAsNoLongerNeeded();
                            Fleeca.NearbyPedsBlip[i].Delete();
                        }
                        Game.Player.WantedLevel = 0;
                        GoToWaypoint(ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition);
                    }
                    else
                    {
                        if (ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].PlayerHitCheckPoint)
                        {
                            HeistCallOnce = false;
                            Heistcheckpoint++;
                            Notification.Show("Called!!");
                            ListHeist[CurrentHeistID].OpenDoors();
                        }
                    }
                    break;

                case 3:
                    if (!HeistCallOnce)
                    {
                        HeistCallOnce = true;
                        ListHeist[CurrentHeistID].CheckPointMessage(Heistcheckpoint);

                        //door open

                        for (int i = 0; i < ListHeist[CurrentHeistID].Cameras.Count; i++)
                        {
                            HeistCamera _currCam = ListHeist[CurrentHeistID].Cameras[i];
                            _currCam.Prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, _currCam.Position.X, _currCam.Position.Y, _currCam.Position.Z, 5f, Function.Call<int>(Hash.GET_HASH_KEY, _currCam.Name), false, false, false);
                            _currCam.Blip.Alpha = 255;
                        }
                    }

                    for (int i = 0; i < ListHeist[CurrentHeistID].NPCs.Count; i++)
                    {
                        if (ListHeist[CurrentHeistID].NPCs[i].IsFleeing && ListHeist[CurrentHeistID].NPCs[i].Money == 1)
                        {
                            //ListHeist[CurrentHeistID].NPCs[i].Task.ClearAllImmediately();
                            ListHeist[CurrentHeistID].NPCs[i].Task.HandsUp(10000);

                        }
                    }

                    int AliveCam = 0;
                    for (int i = 0; i < ListHeist[CurrentHeistID].Cameras.Count; i++)
                    {
                        HeistCamera _currCam = ListHeist[CurrentHeistID].Cameras[i];

                        if (_currCam.Prop.HasBeenDamagedBy(_player))
                        {
                            _currCam.Blip.Alpha = 0;
                        }

                        if (_currCam.Blip.Alpha > 0)
                        {
                            AliveCam++;
                        }
                    }

                    if (AliveCam == 0)
                    {
                        Heistcheckpoint++;
                        HeistCallOnce = false;
                    }
                    break;

                case 4:
                    if (!HeistCallOnce)
                    {
                        HeistCallOnce = true;
                        ListHeist[CurrentHeistID].CheckPointMessage(Heistcheckpoint);
                        Game.Player.WantedLevel = 2;
                    }
                    Function.Call(Hash.DRAW_MARKER, 1, ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition.X, ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition.Y, ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition.Z - 1f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 1f, 0.5f, 255, 128, 0, 50, false, true, 2, false, false, false, false);

                    if (Vector3.Distance(_player.Position, ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition) <= 1f)
                    {
                        DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to open the vault");
                        if (Game.IsControlJustPressed(GTA.Control.Context))
                        {

                            //GTA.UI.Screen.FadeOut(1000);
                            //Wait(1000);
                            if (HeistBagPickup != null)
                            {
                                HeistBagPickup.Delete();
                                HeistBagPickup = null;
                            }
                            HeistBagPickup = World.CreatePickup(PickupType.MoneyVariable, ListHeist[CurrentHeistID].CashPosition, new Vector3(0f, 0f, 0f), new Model(-711724000), 0);
                            Heistcheckpoint++;
                            HeistCallOnce = false;
                        }


                    }
                    break;
                case 5:
                    if (!HeistCallOnce)
                    {
                        ListHeist[CurrentHeistID].VaultDelay = Game.GameTime + 3000;
                        World.RenderingCamera = ListHeist[CurrentHeistID].VaultCamera;
                        PlaySound("Safe");
                        ListHeist[CurrentHeistID].CheckPointMessage(Heistcheckpoint);
                        HeistCallOnce = true;
                        _player.IsPositionFrozen = true;

                    }
                    else
                    {
                        for (int i = 0; i < ListHeist[CurrentHeistID].NPCs.Count; i++)
                        {
                            if (ListHeist[CurrentHeistID].NPCs[i].IsFleeing && ListHeist[CurrentHeistID].NPCs[i].Money == 1)
                            {
                                ListHeist[CurrentHeistID].NPCs[i].Task.HandsUp(10000);

                            }
                        }

                        Prop _vaultDoor = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -2957.847f, 481.7378f, 15.69704f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_gb_vauldr"), false, false, false);
                        ListHeist[CurrentHeistID].VaultCamera.PointAt(_vaultDoor.Position);
                        if (_vaultDoor.Rotation != new Vector3(_vaultDoor.Rotation.X, _vaultDoor.Rotation.Y, -100f))
                        {
                            _vaultDoor.Rotation = Vector3.Lerp(_vaultDoor.Rotation, new Vector3(_vaultDoor.Rotation.X, _vaultDoor.Rotation.Y, -100f), 0.005f);

                        }
                        if (Game.GameTime > ListHeist[CurrentHeistID].VaultDelay)
                        {
                            World.RenderingCamera = null;
                            _player.IsPositionFrozen = false;

                        }
                        if (!HeistBagPickup.ObjectExists())
                        {
                            //Heistcheckpoint++;
                            ListHeist[CurrentHeistID].CheckPointMessage(Heistcheckpoint);
                            Player.GiveBag();
                            Game.Player.WantedLevel = 3;
                            IsRobbing = true;

                            robbedMoney = random.Next(200000, 300001);
                            robbedXP = random.Next(250, 301);
                            HeistCallOnce = false;
                            Notification.Show("You have robbed: ~g~$" + robbedMoney.ToString(), false);
                            Heistcheckpoint++;

                        }

                    }

                    break;
                case 6:
                    if (!HeistCallOnce)
                    {
                        HeistCallOnce = true;

                    }
                    else
                    {
                        if (Game.Player.WantedLevel == 0)
                        {
                            ListHeist[CurrentHeistID].CheckPointMessage(Heistcheckpoint);
                            GoToWaypoint(ListHeist[CurrentHeistID].CheckPoints[Heistcheckpoint].MarkerPosition);



                        }
                        else
                        {
                            if (WaypointActive)
                            {
                                Waypoint.Delete();
                                Waypoint = null;
                                WaypointActive = false;

                            }
                            GTA.UI.Screen.ShowSubtitle("Lose the Cops", 2);
                        }
                    }
                    break;


            }



        }
        void SpawnCarShopIfNearby()
        {


            if (ListCarShop.Count == 0)
            {
                lastCountCarShop = 0;
                return;

            }

            if (lastCountCarShop < ListCarShop.Count)
            {
                int total = 0;
                if (!callonceCarShop)
                {
                    total = Game.GameTime + 1500;
                    callonceCarShop = true;
                }
                if (Game.GameTime < total)
                {
                    return;
                }
                else
                {
                    lastCountCarShop = ListCarShop.Count;
                }
            }

            for (int i = 0; i < ListCarShop.Count; i++)
            {
                if (Vector3.Distance(_player.Position, ListCarShop[i].MarkerPos) <= 250)
                {
                    if (Vector3.Distance(_player.Position, ListCarShop[i].MarkerPos) <= 100)
                    {
                        ListCarShop[i].SpawnCarShop();
                    }
                    else if (Vector3.Distance(_player.Position, ListCarShop[i].MarkerPos) > 120)
                    {
                        ListCarShop[i].DeSpawn();

                    }
                }
            }
        }


        #endregion

        #region Weapons
        void InitializeWeapons()
        {
            Weapon weapon = new Weapon
                (
                "Knife",
                350,
                WeaponHash.Knife,
                "The knife lets you rob ATM's"
                );

            Weapon weapon1 = new Weapon
              (
               "Sticky Bomb",
               250,
               WeaponHash.StickyBomb,
               "The sticky bomb gives you a 40% more pay-out while robbing an ATM"
              );

            Weapon weapon2 = new Weapon
              (
              "Crowbar",
               2500,
               WeaponHash.Crowbar,
              "The crowbar lets you rob ATM's without breaking the tool"
              );

            Weapon weapon3 = new Weapon
              (
               "Pipe Bomb",
               350,
               WeaponHash.PipeBomb,
               "The pipe bomb gives you a 80% more pay-out, and gives you 40% more XP while robbing an ATM "
              );


            ListWeapons.Add(weapon);
            ListWeapons.Add(weapon1);
            ListWeapons.Add(weapon2);
            ListWeapons.Add(weapon3);


            FirstTime = true;
        }


        void InitializeWeaponsSpecial()
        {
            SpecialWeaponComponents special = new SpecialWeaponComponents
                (
                 0xD05319F,
            100,
            "Default Clip",
           (Hash)0x18929DA
                );

            SpecialWeaponComponents special1 = new SpecialWeaponComponents
                (
                  0xB0198D5F,
           500,
           "Tracer Rounds",
           (Hash)0x822060A9
                );

            SpecialWeaponComponents special2 = new SpecialWeaponComponents
                (
                  0x92F129CD,
           750,
           "Incendiary Rounds",
           (Hash)0xA99CF95A
                );

            SpecialWeaponComponents special3 = new SpecialWeaponComponents
                (
                      0x1941D244,
          1000,
          "Armor Piercing Rounds",
           (Hash)0xFAA7F5ED
                );

            SpecialWeaponComponents special4 = new SpecialWeaponComponents
                (
                    0x5E962DDC,
          1250,
          "Full Metal Jacket Rounds",
           (Hash)0x43621710
                );
            List<SpecialWeaponComponents> specials = new List<SpecialWeaponComponents>();
            specials.Add(special);
            specials.Add(special1);
            specials.Add(special2);
            specials.Add(special3);
            specials.Add(special4);
            SpecialWeapon specialWeapon = new SpecialWeapon
              (
              "Bullpup Rifle Mk II",
             25000,
             WeaponHash.BullpupRifleMk2,
             "The Bullpup Rifle Mk II",
             false,
             specials
              );

            //222222222222222222222222222222222222222
            SpecialWeaponComponents special5 = new SpecialWeaponComponents
                (
                 0xD05319F,
            100,
            "Default Clip",
           (Hash)0x8610343F
                );

            SpecialWeaponComponents special6 = new SpecialWeaponComponents
                (
                  0xB0198D5F,
            500,
            "Tracer Rounds",
           (Hash)0xEF2C78C1
                );

            SpecialWeaponComponents special7 = new SpecialWeaponComponents
                (
                   0x92F129CD,
            750,
            "Incendiary Rounds",
           (Hash)0xFB70D853
                );

            SpecialWeaponComponents special8 = new SpecialWeaponComponents
                (
                0x1941D244,
            1000,
            "Armor Piercing Rounds",
           (Hash)0xA7DD1E58
                );

            SpecialWeaponComponents special9 = new SpecialWeaponComponents
                (
                    0x1941D244,
            1000,
            "Armor Piercing Rounds",
           (Hash)0xA7DD1E58
                );
            SpecialWeaponComponents special10 = new SpecialWeaponComponents
                (
                     0x5E962DDC,
            1250,
            "Full Metal Jacket Rounds",
           (Hash)0x63E0A098
                );
            specials.Clear();
            specials.Add(special5);
            specials.Add(special6);
            specials.Add(special7);
            specials.Add(special8);
            specials.Add(special9);
            specials.Add(special10);

            SpecialWeapon specialWeapon1 = new SpecialWeapon
                (
                 "Assault Rifle Mk II",
              33000,
              WeaponHash.AssaultrifleMk2,
              "The Assault Rifle Mk II",
               false,
               specials
                );

            //333333333333333333333333333333333333
            SpecialWeaponComponents special11 = new SpecialWeaponComponents
                (
                  0x90083D3B,
           100,
           "Default Shells",
           (Hash)0xCD940141
                );

            SpecialWeaponComponents special12 = new SpecialWeaponComponents
                (
                   0xDBACD794,
            500,
            "Incendiary Shells",
           (Hash)0x9F8A1BF5
                );

            SpecialWeaponComponents special13 = new SpecialWeaponComponents
                (
                 0x7C867272,
            750,
            "Flechette Shells",
           (Hash)0xE9582927
                );

            SpecialWeaponComponents special14 = new SpecialWeaponComponents
                (
               0x72A3A760,
            1000,
            "Steel Buckshot Shells",
           (Hash)0x4E65B425
                );

            SpecialWeaponComponents special15 = new SpecialWeaponComponents
                (
             0xED906955,
            1250,
            "Explosive Shells",
           (Hash)0x3BE4465D
                );

            specials.Clear();
            specials.Add(special11);
            specials.Add(special12);
            specials.Add(special13);
            specials.Add(special14);
            specials.Add(special15);

            SpecialWeapon specialWeapon2 = new SpecialWeapon
               (
                           "Pump Shotgun Mk II",
             42000,
             WeaponHash.PumpShotgunMk2,
             "The Pump Shotgun Mk II",
              false,
              specials
               );

            //444444444444444444444444444444444
            SpecialWeaponComponents special16 = new SpecialWeaponComponents
               (
            0xD05319F,
            100,
            "Default Clip",
           (Hash)0x4C7A391E
               );

            SpecialWeaponComponents special17 = new SpecialWeaponComponents
                (
           0xB0198D5F,
            500,
            "Tracer Rounds",
           (Hash)0x1757F566
                );

            SpecialWeaponComponents special18 = new SpecialWeaponComponents
                (
             0x92F129CD,
            750,
            "Incendiary Rounds",
           (Hash)0x3D25C2A7
                );

            SpecialWeaponComponents special19 = new SpecialWeaponComponents
                (
              0x1941D244,
            1000,
            "Armor Piercing Rounds",
           (Hash)0x255D5D57
                );

            SpecialWeaponComponents special20 = new SpecialWeaponComponents
                (
            0x5E962DDC,
            1250,
            "Full Metal Jacket Rounds",
           (Hash)0x44032F11
                );

            specials.Clear();
            specials.Add(special15);
            specials.Add(special16);
            specials.Add(special17);
            specials.Add(special18);
            specials.Add(special19);
            specials.Add(special20);


            SpecialWeapon specialWeapon3 = new SpecialWeapon
              (
                "Carbine Rifle Mk II",
            50000,
            WeaponHash.CarbineRifleMk2,
            "The Carbine Rifle Mk II",
             false,
             specials
              );
            //55555555555555555555555555555555555555
            SpecialWeaponComponents special21 = new SpecialWeaponComponents
               (
           0xD05319F,
           100,
           "Default Clip",
           (Hash)0x492B257C
               );

            SpecialWeaponComponents special22 = new SpecialWeaponComponents
                (
              0x4919B4EB,
            500,
            "Tracer Rounds",
           (Hash)0xF6649745
                );

            SpecialWeaponComponents special23 = new SpecialWeaponComponents
                (
            0x57237470,
            750,
            "Incendiary Rounds",
           (Hash)0xC326BDBA
                );

            SpecialWeaponComponents special24 = new SpecialWeaponComponents
                (
              0x2EC80A10,
            1000,
            "Armor Piercing Rounds",
           (Hash)0x29882423
                );

            SpecialWeaponComponents special25 = new SpecialWeaponComponents
                (
             0xDFD80B5,
            1250,
            "Full Metal Jacket Rounds",
           (Hash)0x57EF1CC8
                );

            specials.Clear();
            specials.Add(special21);
            specials.Add(special22);
            specials.Add(special23);
            specials.Add(special24);
            specials.Add(special25);
            SpecialWeapon specialWeapon4 = new SpecialWeapon
               (
                 "Combat MG Mk II",
             55000,
             WeaponHash.CombatMGMk2,
             "The Combat MG Mk II",
              false,
              specials
               );


            WeaponsSpecialList.Add(specialWeapon);
            WeaponsSpecialList.Add(specialWeapon1);
            WeaponsSpecialList.Add(specialWeapon2);
            WeaponsSpecialList.Add(specialWeapon3);
            WeaponsSpecialList.Add(specialWeapon4);

            FirstTime = true;


        }
        #endregion

        #region Camp

        void InitializeCamp()
        {
            Camp camp = new Camp(
                 new Vector3(-494.2796f, -950.888f, 23.91703f),
                new Vector3(0, 0, 47),
                new Vector3(-494.2796f, -950.888f, 23.91703f - 1),
                new Model(1876516712),
                new Model(1982532724)
                );

            Camp camp1 = new Camp(
               new Vector3(1363.06f, -2077.167f, 51.99856f),
                new Vector3(0, 0, -93),
                new Vector3(1363.06f, -2077.167f, 51.99856f - 1),
                new Model(1876516712),
                new Model(1982532724)
               );

            Camp camp2 = new Camp(
                new Vector3(901.2462f, 3609.981f, 32.1364f),
                new Vector3(0, 0, -177),
                new Vector3(901.2462f, 3609.981f, 32.1364f - 0.4f),
                new Model(1876516712),
                new Model(1982532724)
               );

            Camp camp3 = new Camp(
                new Vector3(48.11019f, 6298.078f, 30.55762f),
                new Vector3(0, 0, -177),
                new Vector3(48.11019f, 6298.078f, 30.55762f - 0.4f),
                new Model(1876516712),
                new Model(1982532724)
               );


            CampList.Add(camp);
            CampList.Add(camp1);
            CampList.Add(camp2);
            CampList.Add(camp3);

            //SpawnCamp(true);


            Notification.Show("1");

            FirstTime = true;
        }



        #endregion

        #region Weapon Store

        void InitializeWeaponStore()
        {

            WeaponShop weaponShop = new WeaponShop
                (
                new Vector3(1374.719f, -2222.083f, 60.2688f),
               new Vector3(-0.78f, -0.4f, 2.43f),
               new Model(-326143852)
               );

            WeaponShop weaponShop1 = new WeaponShop
               (
              new Vector3(-1566.789f, -439.5986f, 36.5003f),
              new Vector3(4.691f, -4.4402f, 16.89307f),
              new Model(-326143852)
              );
            ListWeaponStore.Add(weaponShop);
            ListWeaponStore.Add(weaponShop1);



            Notification.Show("5");

            FirstTime = true;


        }

        #endregion

        #region CarShop
        void InitializeCarShop()
        {
            CarShop carShop = new CarShop
                (
                  new Vector3(-1485.793f, -661.5005f, 28.8959f),
             new Vector3(0.081f, -0.00024f, 125.7826f),
             new Model(2091594960),
                             new Vector3(-1455.55f, -646.3531f, 29.07183f),
                             new Vector3(0, 0, 128f)

                );
            ListCarShop.Add(carShop);

            Notification.Show("7");

            FirstTime = true;

        }

        void InitializeCars()
        {
            Car car = new Car
                (
                 new Model(-312295511),
                 50000
                );
            Car car1 = new Car
               (
                new Model(1922255844),
                75000
               );

            Car car2 = new Car
                 (
               new Model(2071877360),
                  125000
                 );

            Car car3 = new Car
                (
              new Model(-326143852),
                 175000
                );

            Car car4 = new Car
               (
             new Model(410882957),
                200000
               );
            ListCars.Add(car);
            ListCars.Add(car1);
            ListCars.Add(car2);
            ListCars.Add(car3);
            ListCars.Add(car4);

            FirstTime = true;

            Notification.Show("6");

        }

        #endregion


        void InitializeHouses()
        {
            House house = new House(
                new Vector3(-775.0875f, 312.08f, 85.6981f),
                new Vector3(-0.09804531f, 0f, 8.855692f),
                400000,                                                //cost
                Player.Config.GetValue<bool>("SAFEHOUSE", "ID1", false),
                new Vector3(-777.0534f, 317.6533f, 176.8036f),          //Entry pos
                new Vector3(0f, 0f, -92f),                              //Entry rot
                new Vector3(-778.5043f, 317.2141f, 176.8036f),
                new Vector3(-775.4376f, 329.4012f, 176.8058f)
                );

            House house1 = new House(
                new Vector3(-261.543f, -970.9977f, 31.22f),
                new Vector3(-0.07816608f, 3.335056E-09f, 38.53732f),
                350000,
                Player.Config.GetValue<bool>("SAFEHOUSE", "ID2", false),
                new Vector3(-263.9105f, -965.9164f, 77.23129f),
                new Vector3(0f, 0f, 0f),
                new Vector3(-263.7408f, -967.2877f, 77.23129f),
                new Vector3(-274.263f, -960.2347f, 77.23311f)
               );

            House house2 = new House(
                new Vector3(-1442.809f, -545.215f, 34.74186f),
                new Vector3(-0.0001588918f, -2.60551E-11f, 44.90155f),
                325000,
                Player.Config.GetValue<bool>("SAFEHOUSE", "ID3", false),
                new Vector3(-1458.677f, -521.9625f, 56.92899f),
                new Vector3(0f, 0f, 178f),
                new Vector3(-1457.803f, -520.3065f, 56.92899f),
                new Vector3(-1453.297f, -532.0329f, 56.93106f)
             );


            ListHouses.Add(house);
            ListHouses.Add(house1);
            ListHouses.Add(house2);


            for (int i = 0; i < ListHouses.Count; i++)
            {
                string id = "ID" + (i + 1).ToString();
                if (Player.Config.GetValue("SAFEHOUSE", id, false))
                    ListHouses[i].HeistBlip.Alpha = ListHouses[i].Blip.Alpha;
                else
                    ListHouses[i].HeistBlip.Alpha = 0;
            }

            FirstTime = true;

        }

        void InitializeStores()
        {
            Store store = new Store(new Vector3(-709.3881f, -904.1693f, 19.21561f));
            Store store1 = new Store(new Vector3(-43.18745f, -1748.579f, 29.42102f));
            Store store2 = new Store(new Vector3(28.23676f, -1339.579f, 29.49702f));

            ListStoreShops.Add(store);
            ListStoreShops.Add(store1);
            ListStoreShops.Add(store2);

            FirstTime = true;
        }

        void InitializeHeist()
        {
            //IHeist heist = new Fleeca();

            //ListHeist.Add(heist);

        }

    }
}
