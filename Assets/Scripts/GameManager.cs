using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject bossPrefab;
    public GameObject bossPos;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            Instantiate(bossPrefab, bossPos.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
