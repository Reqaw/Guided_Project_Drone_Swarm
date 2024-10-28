using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class DroneUI : MonoBehaviour
{
    [SerializeField] InputField InputID;
    [SerializeField] Text SearchOutTXT;
    [SerializeField] Text DestructOutTXT;
    public bool SearchDrone(DroneCommunication droneCommunication)
    {
        if (int.TryParse(InputID.text, out int droneID))
        {
            Drone currentDrone = droneCommunication.Head.DroneData;
            Drone foundDrone = droneCommunication.FindDroneByID(droneID);
            if (foundDrone != null)
            {
                float simulatedTime = droneCommunication.CalculateSimulatedTime(currentDrone, foundDrone);
                SearchOutTXT.text = $"Drone ID: {foundDrone.DroneID} Position: {foundDrone.GPSLocation} Simulated Time: {simulatedTime:F2} seconds";
                return true; 
            }
            else
            {
                SearchOutTXT.text = "Drone not found.";
                return false; 
            }
        }
        else
        {
            SearchOutTXT.text = "Invalid ID input.";
            return false;
        }

    }

    public bool SelfDestruct(DroneCommunication droneCommunication)
    {
        if (int.TryParse(InputID.text, out int droneID))
        {
            Drone currentDrone = droneCommunication.Head.DroneData;
            Drone foundDrone = droneCommunication.FindDroneByID(droneID);
            if (foundDrone != null)
            {
                foundDrone.SelfDestruct();
                float simulatedTime = droneCommunication.CalculateSimulatedTime(currentDrone, foundDrone);
                DestructOutTXT.text = $"Drone ID: {foundDrone.DroneID} is destroyed, Simulated Time: {simulatedTime:F2} seconds";
                return true;
            }
            else
            {
                DestructOutTXT.text = "Drone not found";
                return false;   
            }
        }
        else {
            DestructOutTXT.text = "Invalid ID";
            return false;   
        }
    }

}


