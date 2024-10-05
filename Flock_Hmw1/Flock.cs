using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmw1
{
    public class Flock
    {
        Drone[] agents;
        int num;

        public Flock(int maxnum)
        {
            agents = new Drone[maxnum];
        }

        // actually add the drones
        public void Init(int num)
        {
            this.num = num;
            for (int i = 0; i < num; i++)
            {
                agents[i] = new Drone(i);
            }
        }

        public float average()
        {
            float sum = 0;
            for (int i = 0; i < num; i++)
            {
                sum += agents[i].Temperature;
            }
            return sum / num;
        }

        public int max()
        {
            float maxTemp = agents[0].Temperature;
            int maxI = 0;

            for (int i = 1; i < num; i++)
            {
                if (agents[i].Temperature > maxTemp)
                {
                    maxTemp = agents[i].Temperature;
                    maxI = i;
                }
            }
            return maxI;
        }
        
        public int min()
        {
            float minTemp = agents[0].Temperature;
            int minI = 0;

            for (int i = 1; i < num; i++)
            {
                if (agents[i].Temperature < minTemp)
                {
                    minTemp = agents[i].Temperature;
                    minI = i;
                }
            }
            return minI;
        }

        public void print()
        {
            for (int i = 0; i < num; i++)
            {
                Console.WriteLine($"Drone {agents[i].ID}: Temperature = {agents[i].Temperature}, Wind = {agents[i].Wind}, Battery = {agents[i].Battery}");
            }
        }

        public void bubblesort()
        {
            for (int i = 0; i < num - 1; i++)
            {
                for (int j = 0; j < num - i - 1; j++)
                {
                    if (agents[j].Temperature > agents[j + 1].Temperature)
                    {
                        Drone temp = agents[j];
                        agents[j] = agents[j + 1];
                        agents[j + 1] = temp;
                    }
                }
            }
        }
    }
}
