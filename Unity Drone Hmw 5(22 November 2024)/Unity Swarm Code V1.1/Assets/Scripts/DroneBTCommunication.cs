using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneBTCommunication 
{
    public Node Root;

    public class Node
    {
        public Drone DroneData;
        public Node Left, Right;
        
        public Node(Drone drone)
        {
            DroneData = drone;
            Left = Right = null;
        }
    }

    public void Insert(Drone drone)
    {
        Root = InsertHelper(Root, drone); 
    }

    private Node InsertHelper(Node root, Drone drone)
    {
        if (root == null)
        {
            root = new Node(drone);
            return root;
        }

        if (drone.DroneID < root.DroneData.DroneID)
        {
            root.Left = InsertHelper(root.Left, drone);
        } 
        else if (drone.DroneID > root.DroneData.DroneID)
        {
            root.Right = InsertHelper(root.Right, drone);   
        }
        return root;
    }

    public Drone SearchDroneID(int ID)
    {
        return SearchHelper(Root, ID);
    }

    private Drone SearchHelper(Node root, int id)
    {
        if (root == null) { return null; }
        
        if (id == root.DroneData.DroneID)
        {
            return root.DroneData;
        }
        else if (id < root.DroneData.DroneID)
        {
            return SearchHelper(root.Left,id);
        } else
        {
            return SearchHelper(root.Right, id);
        }
    }
    
    public Drone SearchBatteryLevel(float batteryLevel)
    {
        return SearchBatteryLevelHelper(Root, batteryLevel);
    }

    private Drone SearchBatteryLevelHelper(Node root, float batteryLevel)
    {
        if (root == null) { return null; }

        if (batteryLevel == root.DroneData.BatteryLevel)
        {
            return root.DroneData;
        }
        else if (batteryLevel < root.DroneData.BatteryLevel)
        {
            return SearchBatteryLevelHelper(root.Left, batteryLevel);
        }
        else
        {
            return SearchBatteryLevelHelper(root.Right, batteryLevel);
        }
    }

    public bool SelfDestructDrone(int id)
    {
        Drone DestroyDrone = SearchDroneID(id);
        if (DestroyDrone != null)
        {
            DestroyDrone.SelfDestruct();
            return true;
        }
        return false;
    }
    public void Clear()
    {
        Root = null;
    }

    public void remove(int droneID)
    {
        Root = removehelper(Root, droneID); 
    }

    private Node removehelper(Node Root, int droneID)
    {
        if (Root == null) { return Root; }

        if (droneID < Root.DroneData.DroneID)
        {
            Root.Left = removehelper(Root.Left, droneID);
        } else if (droneID > Root.DroneData.DroneID)
        {
            Root.Right = removehelper(Root.Right, droneID);
        } else
        {
            if (Root.Left == null)
            {
                return Root.Right;
            } else if (Root.Right == null)
            {
                return Root.Left;
            }

            Root.DroneData = FindMinimum(Root.Right).DroneData;
            Root.Right = removehelper(Root.Right, Root.DroneData.DroneID);
        }
        return Root;
    }

    private Node FindMinimum(Node root)
    {
        while (root.Left != null)
        {
            root= root.Left;
        }
        return root;
    }

}
