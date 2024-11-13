using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmw1
{
    public class Drone
    {
        public int ID { set; get; }
        public float Temperature { set; get; } = 0;
        public float Wind { set; get; } = 0;
        public float Battery { set; get; } = 0;

        private Random rnd = new Random();

        public Drone(int ID)
        {
            this.ID = ID;
            Update();
        }

        public void Update()
        {
            Temperature = rnd.Next() * 100;
            Wind = rnd.Next() * 100;
            Battery = rnd.Next() * 100;
        }
    }
}
