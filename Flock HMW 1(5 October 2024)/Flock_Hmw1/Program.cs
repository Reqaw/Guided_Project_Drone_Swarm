/*
    Complete the code in Flock.cs. (1 function per group member; 
    bubblesort is a must - someone must choose this).
    Time the run for each function with 
    number of drones varying from 100 to 1000000.
    (run each multiple times - unless each run takes too long).
    Save result to CSV (google up).
    Plot the runtime for each function (using Excel/Sheet)
*/

using Hmw1;
using System;
class HelloWorld
{
    static void Main()
    {

        int numRepeat = 100;
        int max = 12000; //1000000;
        int min = 100;
        int stepsize = 100;
        int numsteps = (max - min) / stepsize;

        // repeat this declaration for all other functions
        float[] timeAverage = new float[numsteps];
        for (int i = 0; i < numsteps; i++)
        {
            int numdrones = i * stepsize + min;
            Console.WriteLine("Current num drones = " + numdrones);

            Flock flock = new Flock(numdrones);
            flock.Init((int)(0.9 * numdrones)); // fill up 90%

            // calculate time for average
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            //Run Algorithm/Functions
            flock.average();
            flock.min();
            flock.max();
            flock.bubblesort();
            flock.print();

            //Capture Timing
            watch.Stop();
            long time = watch.ElapsedMilliseconds;
            // store value
            timeAverage[i] = time / numRepeat;
            // ... repeat for all other functions ...
        }
        // write results to csv files
        // see https://www.csharptutorial.net/csharp-file/csharp-write-csv-files/
        WriteResultsToCsv("results.csv", timeAverage, min, stepsize, numsteps);
    }
    static void WriteResultsToCsv(string filePath, float[] results, int min, int stepSize, int numSteps)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // create header inside csv file
            writer.WriteLine("Number of Drones,Average Time (ms)");

            // print results to file
            for (int i = 0; i < numSteps; i++)
            {
                int numDrones = i * stepSize + min;
                writer.WriteLine($"{numDrones},{results[i]}");
            }
        }
        Console.WriteLine("Results saved to " + filePath);
    }
}