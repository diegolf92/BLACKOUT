using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySc : MonoBehaviour
{
    public GameObject player; // Reference to the player's transform
    public float moveSpeed = 5f; // Speed at which the enemy moves towards the player
    public float minDistance = 2f; // Minimum distance the enemy tries to maintain from the player
    public float maxDistance = 10f; // Maximum distance the enemy can get close to the player
    public bool flashed = false;
    public Animator anim;

    private Rigidbody2D rb;

    public AudioSource source;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    bool isScreaming = false;

    void Update()
    {
        if (player != null && !flashed)
        {
            anim.SetBool("isFlashed", false);
            Vector2 direction = player.transform.position - transform.position;
            float distance = direction.magnitude;

            // Clamp distance to restrict how close the enemy can get to the player
            distance = Mathf.Clamp(distance, minDistance, maxDistance);

            // Check if the enemy is too close to the player
            if (distance > minDistance)
            {
                // Move towards the player at full speed
                direction.Normalize();
                rb.velocity = direction * moveSpeed;
            }
            else
            {
                // Slow down when too close to the player
                rb.velocity = direction.normalized * (moveSpeed * distance / minDistance);
            }
            isScreaming = false;
        } 
        else if(flashed)
        {
            if(!isScreaming)
            {
                isScreaming = true;
                source.PlayOneShot(player.GetComponent<PlayerController>().audioClipArray[5]);
            }
            anim.SetBool("isFlashed", true);
            Vector2 direction = transform.position - player.transform.position;
            transform.position = Vector3.Lerp(transform.position, direction, moveSpeed/4 * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Damage();
        }
    }
}
