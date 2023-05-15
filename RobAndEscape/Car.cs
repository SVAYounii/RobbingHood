using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbingHood
{
    public class Car
    {
        public string Name { get; set; }
        public Model Model { get; set; }
        public int Price { get; set; }
        public Vehicle Vehicle { get; set; }
        public Car(Model model, int price)
        {
            Model = model;
            Vehicle = World.CreateVehicle(model, Vector3.RandomXYZ());
            Name = Vehicle.LocalizedName;
            Vehicle.Delete();
            Price = price;
        }
    }
}
