using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood.Heist_Info
{
    public class CheckPoint
    {
        public string Description { get; set; }
        public Vector3 MarkerPosition { get; set; }
        public bool Interactive { get; set; }
        public bool PlayerHitCheckPoint{ get; set; }
        public bool InstantStopCar{ get; set; }
        public bool IsCalled{ get; set; }


        public CheckPoint(string description, Vector3 markerPosition, bool interactive, bool instantStopCar = false, bool isCalled = false)
        {
            Description = description;
            MarkerPosition = markerPosition;
            Interactive = interactive;
            PlayerHitCheckPoint = false;
            InstantStopCar = instantStopCar;
            IsCalled = isCalled;
        }
    }
}
