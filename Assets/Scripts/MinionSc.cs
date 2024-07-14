using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSc : MonoBehaviour
{
    public float moveSpeed = 3f; // Speed at which the enemy moves to the left
    
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        // Move the enemy to the left
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }
}
