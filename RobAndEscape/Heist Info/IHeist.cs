using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood.Heist_Info
{
    public interface IHeist
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

        public void CheckPointMessage(int checkPointIndex);

        public void OpenDoors();
    }
}
