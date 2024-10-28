using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCommunication 
{
    public Node Head;
    public class Node
    {
        public Drone DroneData;
        public Node Next;
        public Node (Drone drone)
        {
            DroneData = drone;
            Next = null;
        }
    }

    public void AddDrone(Drone drone)
    {
        Node NewNode = new Node(drone);
        if (Head == null)
        {
            Head = NewNode;
        } else
        {
            Node Current = Head;
            while (Current.Next != null)
            {
                Current = Current.Next;
            }
            Current.Next = NewNode;
        }
    }

    public Drone FindDroneByID(int id)
    {
        Node current = Head;
        while (current != null)
        {
            if (current.DroneData.DroneID == id)
            {
                return current.DroneData;
            }
            current = current.Next;
        }
        return null;
    }

    public void Clear()
    {
        Head = null;
    }

    public float CalculateSimulatedTime(Drone currentDrone, Drone targetDrone, float speed = 1.0f)
    {
        if (targetDrone == null || currentDrone == null) return -1;
        float distance = Vector2.Distance(currentDrone.GPSLocation, targetDrone.GPSLocation);
        return distance / speed; // Simulated time proportional to distance
    }

}
