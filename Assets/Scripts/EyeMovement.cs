using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    Vector3 originalPos;
    public bool playerOn;
    public GameObject eye;

    // Reference to the player GameObject (assign this in the Inspector)
    public GameObject player;

    // Define the boundaries of the horizontal movement area
    public float limit = -5f;

    // Speed of object movement
    public float moveSpeed = 5f;

    void Start()
    {
        originalPos = eye.transform.position;
    }

    void Update()
    {
        if(playerOn)
        {
            // Get positions of the player and this GameObject
            Vector3 playerPosition = player.transform.position;

            // Compare positions to determine relative position
            if (playerPosition.x < transform.position.x)
            {
                Vector3 newPosition = new Vector3(eye.transform.position.x - limit,eye.transform.position.y,0);
                // Smoothly move towards the new position
                eye.transform.position = Vector3.Lerp(originalPos, newPosition, moveSpeed * Time.deltaTime);
            } else if (playerPosition.x > originalPos.x)
            {
                Vector3 newPosition = new Vector3(eye.transform.position.x + limit,eye.transform.position.y,0);
                // Smoothly move towards the new position
                eye.transform.position = Vector3.Lerp(originalPos, newPosition, moveSpeed * Time.deltaTime);
            } else
            {
                eye.transform.position = originalPos;
            }
        } 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            playerOn = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            playerOn = false;
            eye.transform.position = Vector3.Lerp(eye.transform.position, originalPos, moveSpeed * Time.deltaTime);
        }
    }
}
