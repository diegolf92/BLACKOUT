using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxEffectMultiplier = 0.5f;  // Adjust this to control the intensity of the parallax effect
    private Transform mainCameraTransform;
    private Vector3 lastCameraPosition;

    void Start()
    {
        mainCameraTransform = Camera.main.transform;
        lastCameraPosition = mainCameraTransform.position;
    }

    void Update()
    {
        // Calculate parallax movement based on camera's movement since last frame
        Vector3 deltaMovement = mainCameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, deltaMovement.y * parallaxEffectMultiplier, 0);

        // Update last camera position
        lastCameraPosition = mainCameraTransform.position;
    }
}
