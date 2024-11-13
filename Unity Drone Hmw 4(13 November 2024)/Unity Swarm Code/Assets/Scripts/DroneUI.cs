using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class DroneUI : MonoBehaviour
{
    [SerializeField] InputField InputID;
    [SerializeField] Text FindTXT;
    [SerializeField] Text DestructTXT;
    [SerializeField] Text BatteryLevelFindTXT;
        private TextMeshProUGUI fadeawaytext;


    public float fadetime;
    public DroneBTCommunication HighBattComms;
    public DroneBTCommunication LowBattComms;

    void start()
    {
        fadeawaytext = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (fadetime > 0)
        {
            fadetime -= Time.deltaTime;
            fadeawaytext.color = new Color(fadeawaytext.color.r, fadeawaytext.color.g, fadeawaytext.color.b, fadetime);
        }
    }
    public float CalculateSimulatedTime(Drone currentDrone, Drone targetDrone, float speed = 1.0f)
    {
        if (targetDrone == null || currentDrone == null) return -1;
        float distance = Vector2.Distance(currentDrone.GPSLocation, targetDrone.GPSLocation);
        return distance / speed; // Simulated time proportional to distance
    }
    public bool LLSearchDrone(DroneCommunication droneCommunication)
    {
        if (int.TryParse(InputID.text, out int droneID))
        {
            Drone currentDrone = droneCommunication.Head.DroneData;
            Drone foundDrone = droneCommunication.FindDroneByID(droneID);
            if (foundDrone != null)
            {
                float simulatedTime = CalculateSimulatedTime(currentDrone, foundDrone);
                FindTXT.text = $"Drone ID: {foundDrone.DroneID} Position: {foundDrone.GPSLocation} Simulated Time: {simulatedTime:F2} seconds";
                return true; 
            }
            else
            {
                FindTXT.text = "Drone not found.";
                return false; 
            }
        }
        else
        {
            FindTXT.text = "Invalid ID input.";
            return false;
        }

    }

    public bool LLSelfDestruct(DroneCommunication droneCommunication)
    {
        if (int.TryParse(InputID.text, out int droneID))
        {
            Drone currentDrone = droneCommunication.Head.DroneData;
            Drone foundDrone = droneCommunication.FindDroneByID(droneID);
            if (foundDrone != null)
            {
                foundDrone.SelfDestruct();
                float simulatedTime = CalculateSimulatedTime(currentDrone, foundDrone);
                DestructTXT.text = $"Drone ID: {foundDrone.DroneID} is destroyed, Simulated Time: {simulatedTime:F2} seconds";
                return true;
            }
            else
            {
                DestructTXT.text = "Drone not found";
                return false;   
            }
        }
        else {
            DestructTXT.text = "Invalid ID";
            return false;   
        }
    }

    public bool SearchDroneBT(DroneBTCommunication droneBTComms)
    {
        if (int.TryParse(InputID.text, out int droneID))
        {
            Drone CurrentDrone = droneBTComms.Root.DroneData;
            Drone foundDrone = droneBTComms.SearchDroneID(droneID);

            if (foundDrone != null)
            {
                float simulatedTime = CalculateSimulatedTime(CurrentDrone,foundDrone);
                FindTXT.text = $"Drone ID: {foundDrone.DroneID} Position: {foundDrone.GPSLocation} Simulated Time: {simulatedTime:F2} seconds";
                return true;
            }
            else
            {
                FindTXT.text = "Drone not found.";
                return false;
            }
        }
        else
        {
            FindTXT.text = "Invalid input.";
            return false;
        }
    }

    public bool SelfDestructBT(DroneBTCommunication droneBTComms)
    {
        if (int.TryParse(InputID.text, out int droneID))
        {
            Drone CurrentDrone = droneBTComms.Root.DroneData;
            Drone foundDrone = droneBTComms.SearchDroneID(droneID);

            if (foundDrone != null)
            {
                foundDrone.SelfDestruct();
                float simulatedTime = CalculateSimulatedTime(CurrentDrone, foundDrone);
                DestructTXT.text = $"Drone ID: {foundDrone.DroneID} is destroyed, Simulated Time: {simulatedTime:F2} seconds";
                droneBTComms.remove(droneID);
                return true;
            }
            else
            {
                DestructTXT.text = "Drone not found.";
                return false;
            }
        }
        else
        {
            DestructTXT.text = "Invalid input.";
            return false;
        }
    }

    public bool SearchDroneBattery(DroneBTCommunication droneBTComms)
    {
        if (int.TryParse(InputID.text, out int Batterylevel))
        {
            Drone CurrentDrone = droneBTComms.Root.DroneData;
            Drone foundDrone = droneBTComms.SearchBatteryLevel(Batterylevel);
            if (foundDrone != null)
            {
                float simulatedTime = CalculateSimulatedTime(CurrentDrone, foundDrone);
                BatteryLevelFindTXT.text = $"Drone ID: {foundDrone.DroneID} Position: {foundDrone.GPSLocation} Simulated Time: {simulatedTime:F2} seconds Battery Level: {foundDrone.BatteryLevel} %";
                return true;
            }
            else
            {
                BatteryLevelFindTXT.text = $"No drones at {Batterylevel} %";
                return false;
            }
        }
        else
        {
            BatteryLevelFindTXT.text = "Invalid input"; 
            return false;
        }
    }
}


