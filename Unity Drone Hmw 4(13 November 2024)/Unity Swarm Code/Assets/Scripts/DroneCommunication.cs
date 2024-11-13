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



}
