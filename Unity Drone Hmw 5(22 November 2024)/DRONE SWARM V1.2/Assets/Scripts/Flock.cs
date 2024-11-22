using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Flock : MonoBehaviour
{
    public DroneUI DroneU;
    public Drone agentPrefab;
    public List<Drone> agents = new List<Drone>();
    public FlockBehavior behavior;

    [Range(10, 5000)]
    public int startingCount = 250;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }
    float deltaTime = 0.0f;
    bool firstWrite = true;

    public DroneCommunication HighBattLLComms { get; private set; } = new DroneCommunication();
    public DroneCommunication LowBattLLComms { get; private set; } = new DroneCommunication();
    public DroneBTCommunication HighBattBTComms { get; private set; } = new DroneBTCommunication();
    public DroneBTCommunication LowBattBTComms { get; private set; } = new DroneBTCommunication();
    public DroneGraphCommunication HighBattNetwork = new DroneGraphCommunication();
    public DroneGraphCommunication LowBattNetwork = new DroneGraphCommunication();

    public Sprite highBattery, lowBattery;
    public Text FindTXT;

    // New variables for cursor follow behavior
    private bool isFollowingCursor = false;
    private bool isFollowingLeader = false;
    public Button followCursorButton;
    public Button stopButton;
    public Button followLeaderButton;
    private Drone leaderDrone ;
    public float leaderSpeed = 2f; // Speed for the leader drone
    public float followerSpeed = 2f; // Speed for other drones

    public GameObject victim;
    public Button spawnVictimButton;
    public Text messageText;   
    public VictimMovement victimMovementScript;   

    private bool isVictimActive = false;

    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            Drone newAgent = Instantiate(
                agentPrefab,
                UnityEngine.Random.insideUnitCircle * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * UnityEngine.Random.Range(0f, 360f)),
                transform
            );
            newAgent.name = "Agent " + i;
            newAgent.DroneID = i + 1;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }

        SetLeaderByDroneID(1);
        followCursorButton.onClick.AddListener(OnFollowCursorButtonPressed);
        stopButton.onClick.AddListener(OnStopButtonPressed);
        followLeaderButton.onClick.AddListener(OnFollowLeaderButtonPressed);

        
    }

    void Update()
    {
        // Create a copy of the agents list to avoid modifying it during enumeration
        Drone[] drones = agents.ToArray();

        // Process movement logic
        for (int i = 0; i < agents.Count; i++)
        {
            Drone agent = agents[i];
            List<Transform> context = GetNearbyObjects(agent);
            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;

            // If we are following the cursor, override the movement
            if (isFollowingCursor)
            {
                HandleCursorFollow(agent, ref move);
            }

            // If we are following the leader, override the movement
            if (isFollowingLeader && leaderDrone != null)
            {
                HandleLeaderFollow(agent, ref move);
            }

            agent.Move(move);
        }

        // Performance tracking
        TrackPerformance();
    }

        void HandleCursorFollow(Drone agent, ref Vector2 move)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;  // Ensure we're using 2D space
            move = (mouseWorldPosition - agent.transform.position).normalized * driveFactor;
        }

        void HandleLeaderFollow(Drone agent, ref Vector2 move)
        {
            // Move the leader in a more controlled direction (optional: replace with desired leader movement logic)
            Vector2 leaderDirection = leaderDrone.transform.up; // Or use another movement method to control the leader
            leaderDrone.Move(leaderDirection * leaderSpeed * 0.03f);  // Adjust movement factor here if necessary

            // Get the position of the leader
            Vector2 leaderPosition = leaderDrone.transform.position;

            // Define an offset distance between drones
            float offsetDistance = 2.0f;

            // Sort drones based on their ID for consistent follower hierarchy
            // Sorting should ideally happen once per frame and not within the movement loop itself
            if (agents[0].DroneID == leaderDrone.DroneID)
            {
                agents.Sort((a, b) => a.DroneID.CompareTo(b.DroneID));  // Sorting should happen here, before moving drones
            }

            // Move the followers
            for (int i = 0; i < agents.Count; i++)
            {
                Drone drone = agents[i];

                if (drone != leaderDrone)
                {
                    Vector2 targetPosition = GetTargetPosition(i, leaderPosition, drone);
                    Vector2 followDirection = (targetPosition - (Vector2)drone.transform.position).normalized;

                    // Move the drone towards the target position
                    drone.Move(followDirection * followerSpeed * 0.1f); // Adjust the multiplier here if needed
                }
            }
        }



        Vector2 GetTargetPosition(int index, Vector2 leaderPosition, Drone drone)
        {
            float offsetDistance = 2.0f;
            if (index == 1) // First follower stays in front of the leader
            {
                return leaderPosition + (Vector2)leaderDrone.transform.up * offsetDistance;
            }
            else // Remaining followers align behind the previous drone
            {
                Drone previousDrone = agents[index - 1];
                return (Vector2)previousDrone.transform.position - (Vector2)previousDrone.transform.up * offsetDistance;
            }
        }

        void TrackPerformance()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            PartitionDrones();
            stopwatch.Stop();
            TimeSpan Timing = stopwatch.Elapsed;
            WriteCSV(Timing, fps, append: !firstWrite);
            firstWrite = false;
        }


    List<Transform> GetNearbyObjects(Drone agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }

    public void PartitionDrones()
    {
        Drone firstDrone = agents[0];
        float BatteryPivot = firstDrone.BatteryLevel;

        HighBattLLComms.Clear();
        LowBattLLComms.Clear();
        HighBattBTComms.Clear();
        LowBattBTComms.Clear();
        HighBattNetwork.Clear();
        LowBattNetwork.Clear();

        List<Drone> highBatteryDrones = new List<Drone>();
        List<Drone> lowBatteryDrones = new List<Drone>();

        foreach (Drone drone in agents)
        {
            SpriteRenderer droneSpriteRenderer = drone.gameObject.GetComponent<SpriteRenderer>();
            if (drone.BatteryLevel >= BatteryPivot)
            {
                highBatteryDrones.Add(drone);
                HighBattLLComms.AddDrone(drone);
                HighBattBTComms.Insert(drone);
                HighBattNetwork.AddDrone(drone);
                droneSpriteRenderer.sprite = highBattery;
            }
            else
            {
                lowBatteryDrones.Add(drone);
                LowBattLLComms.AddDrone(drone);
                LowBattBTComms.Insert(drone);
                LowBattNetwork.AddDrone(drone);
                droneSpriteRenderer.sprite = lowBattery;
            }
        }
    }

    void WriteCSV(TimeSpan Timing, float fps, bool append = true)
    {
        string FilePath = Path.Combine(Application.dataPath, "Results.csv");

        using (StreamWriter Writer = new StreamWriter(FilePath, append))
        {
            if (!append)
            {
                Writer.WriteLine("Time Taken(ms), FPS");
            }
            Writer.WriteLine($"{Timing.TotalMilliseconds}, {fps}");
        }
    }
    public void LLsearch()
    {
        bool foundInHighBatt = false;

        if (DroneU.LLSearchDrone(HighBattLLComms))
        {
            foundInHighBatt = true;
        }

        if (!foundInHighBatt)
        {
            DroneU.LLSearchDrone(LowBattLLComms);
        }
    }

    public void LLSelfDestruct()
    {
        bool Destruct = false;

        if (DroneU.LLSelfDestruct(HighBattLLComms))
        {
            Destruct = true;
        }
        if (!Destruct)
        {
            DroneU.LLSelfDestruct(LowBattLLComms);
        }
    }

    public void BTSearch()
    {
        bool foundinhighbatt = false;
        if (DroneU.SearchDroneBT(HighBattBTComms))
        {
            foundinhighbatt = true;
        }
        if (!foundinhighbatt)
        {
            DroneU.SearchDroneBT(LowBattBTComms);
        }
    }
    public void BTSelfDestruct()
    {
        bool Destruct = false;

        if (DroneU.SelfDestructBT(HighBattBTComms))
        {
            Destruct = true;
        }
        if (!Destruct)
        {
            DroneU.SelfDestructBT(LowBattBTComms);
        }
    }

    public void BTBatteryDroneSearch()
    {
        bool FoundDrone = false;
        if (DroneU.SearchDroneBattery(HighBattBTComms))
        {
            FoundDrone = true;
        }
        if (!FoundDrone)
        {
            DroneU.SearchDroneBattery(LowBattBTComms);
        }
    }

    public void GraphSearch()
    {
        bool FoundDrone = false;
        if (DroneU.SearchNetwork(HighBattNetwork))
        {
            FoundDrone = true;
        }
        if (!FoundDrone)
        {
            DroneU.SearchNetwork(LowBattNetwork);
        }
    }

    public void GraphShortestPath()
    {
        bool FoundDrone = false;
        if (DroneU.FindShortestPathInNetwork(HighBattNetwork))
        {
            UnityEngine.Debug.Log("GraphShortestPath called. Checking HighBattNetwork...");
            FoundDrone = true;
        }
        if (!FoundDrone)
        {
            UnityEngine.Debug.Log("No path in HighBattNetwork. Checking LowBattNetwork...");
            DroneU.FindShortestPathInNetwork(LowBattNetwork);
        }
    }
    public void SpawnVictim()
    {
    isVictimActive = true;
    victim.SetActive(true);
    victim.transform.position = new Vector3(0, 0, 0);
    messageText.text = "";
    victimMovementScript.ToggleMovement(true);  // Start victim movement
}

    public void OnFollowCursorButtonPressed()
    {
        isFollowingCursor = true;
        isFollowingLeader = false;
    }

    public void OnStopButtonPressed()
    {
        if (isVictimActive)
    {
        victimMovementScript.ToggleMovement(false);  // Stop the victim's movement
        victim.SetActive(false);  // Optionally, deactivate the victim
        messageText.text = "Victim Simulation stopped.";
        isFollowingCursor = false;
        isFollowingLeader = false;
    }
    else
    {
        // Stop the cursor or leader following functionality
        isFollowingCursor = false;
        isFollowingLeader = false;
        messageText.text = "Movement stopped.";
    }
    }

    public void OnFollowLeaderButtonPressed()
    {
        isFollowingLeader = true;
        isFollowingCursor = false;
    }

    public void SetLeaderByDroneID(int id)
    {
        foreach (Drone drone in agents)
        {
            if (drone.DroneID == id)
            {
                SetLeader(drone);
                return;
            }
        }
    }

     public void SetLeader(Drone drone)
    {
        leaderDrone = drone;
    }



}
