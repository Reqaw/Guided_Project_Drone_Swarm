using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Drone : MonoBehaviour
{
    public int DroneID;
    public int Temperature { set; get; } = 0;
    public bool ToggleThermal { set; get; } = false;
    public bool VictimDetected { set; get; } = false;
    public float BatteryLevel { set; get; } = 0;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
    public Vector2 GPSLocation { private set; get; }
    public float SearchRadius { get; set; } = 100.0f;
    private Renderer droneRenderer; 


    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        //Randomize initial Batterylevel and GPSLocation
        GPSLocation = new Vector2(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
        BatteryLevel = (int)(Random.value * 10);
        // RandomizeBatteryLevel(); //Function to randomize battery level
        // InvokeRepeating("RandomizeBatteryLevel", 2f, 2f); //Randomizes battery level every 2 seconds

        agentCollider = GetComponent<Collider2D>();
        droneRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        Temperature = (int)(Random.value * 100);
        GPSLocation += new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * Time.deltaTime; //Changes the value of GPS location after each frame
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    public void SetColor(Color color)
    {
        droneRenderer.material.color = color;
    }

    //Function to change if Thermal vision is on or off
    public void ToggleThermalVision()
    {
        ToggleThermal = !ToggleThermal; //Sets True to false or vice versa
    }

    //Function to check if drone detects a victim
    public bool DetectVictim(Vector2 victimLocation)
    {
        float DistanceToVictim = Vector2.Distance(GPSLocation, victimLocation); //Calculates the Distance between the drone and the victim
        if (DistanceToVictim <= SearchRadius) // Checks if distance to victim less than search radius
        {
            VictimDetected = true;
            return true;
        }
        else
        {
            VictimDetected = false;
            return false;
        }
    }


    //Function to randomize the battery level of the drones
    public void RandomizeBatteryLevel()
    {
        BatteryLevel = BatteryLevel = (int)(Random.value * 100);
    }

    public void SelfDestruct()
    {
        gameObject.SetActive(false);
    }


}
