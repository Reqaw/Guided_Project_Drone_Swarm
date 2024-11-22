using UnityEngine;

public class VictimMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed at which the victim moves
    private bool isMoving = false;  // Flag to check if victim is moving

    void Update()
    {
        if (isMoving)
        {
            // Move the victim based on arrow key input
            float moveHorizontal = Input.GetAxis("Horizontal"); // Left/Right Arrow
            float moveVertical = Input.GetAxis("Vertical");     // Up/Down Arrow

            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
    }

    // Function to start or stop victim's movement
    public void ToggleMovement(bool shouldMove)
    {
        isMoving = shouldMove;
    }

    // Detect when a drone collides with the victim
    private void OnTriggerEnter2D(Collider2D other)
    {
    if (other.CompareTag("Drone"))
    {
        Drone drone = other.GetComponent<Drone>();  // Get the Drone component from the collided object
        
        if (drone != null)
        {
            // Handle collision with Drone
            FindObjectOfType<Flock>().messageText.text = "Victim Found!";
            ToggleMovement(false);  // Stop movement
            FindObjectOfType<Flock>().victim.SetActive(false);  // Optionally, deactivate the victim
        }
    }
}
}
