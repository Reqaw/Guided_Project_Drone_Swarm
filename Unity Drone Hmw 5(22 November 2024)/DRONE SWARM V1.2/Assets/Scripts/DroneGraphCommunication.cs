using System;
using System.Collections.Generic;
using UnityEngine;

public class DroneGraphCommunication
{
    private Dictionary<int, Drone> droneMap = new Dictionary<int, Drone>();
    private Dictionary<int, List<int>> adjacencyList = new Dictionary<int, List<int>>();

    // Add a drone to the network
    public void AddDrone(Drone drone)
    {
        if (!adjacencyList.ContainsKey(drone.DroneID))
        {
            adjacencyList[drone.DroneID] = new List<int>();
            droneMap[drone.DroneID] = drone;

            foreach (int otherID in adjacencyList.Keys)
            {
                if (otherID != drone.DroneID)
                {
                    adjacencyList[drone.DroneID].Add(otherID);
                    adjacencyList[otherID].Add(drone.DroneID);
                }
            }
        }
    }

    // Search for a drone in the network using BFS
    public Drone SearchDroneByID(int targetID)
    {
        if (droneMap.ContainsKey(targetID))
        {
            return droneMap[targetID];
        }
        else
        {
            return null;
        }
    }

    // Find shortest path between two drones using Dijkstra's algorithm
    public List<int> FindShortestPath(int startID, int endID)
    {
        Dictionary<int, float> distances = new Dictionary<int, float>();
        Dictionary<int, int> previous = new Dictionary<int, int>();
        PriorityQueue<int> priorityQueue = new PriorityQueue<int>();

        foreach (int id in adjacencyList.Keys)
        {
            distances[id] = float.MaxValue;
            previous[id] = -1; 
        }

        distances[startID] = 0f;
        priorityQueue.Enqueue(startID, 0f);

        while (priorityQueue.Count > 0)
        {
            int currentID = priorityQueue.Dequeue();

            if (currentID == endID)
            {
                break;
            }

            foreach (int neighbor in adjacencyList[currentID])
            {
                float newDist = distances[currentID] + Vector2.Distance(droneMap[currentID].GPSLocation, droneMap[neighbor].GPSLocation);

                if (newDist < distances[neighbor])
                {
                    distances[neighbor] = newDist;
                    previous[neighbor] = currentID;
                    priorityQueue.Enqueue(neighbor, newDist);
                }
            }
        }

        // Reconstruct the shortest path
        List<int> path = new List<int>();
        for (int at = endID; at != -1; at = previous[at])
        {
            path.Add(at);
        }
        path.Reverse();

        return path; // Return the shortest path
    }
    public void Clear()
    {
        adjacencyList.Clear();
        droneMap.Clear();
    }
}

