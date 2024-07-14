using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampSc : MonoBehaviour
{
    public PlayerController player;
    public GameObject bloodParticle; 
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Boss")
        {
            other.GetComponent<EnemySc>().flashed = true;
        }

        if(other.transform.tag == "Enemy")
        {
            player.DamageEnemy();
            Destroy(other.gameObject);
            Instantiate(bloodParticle, other.transform.position, Quaternion.identity);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.tag == "Boss")
        {
            other.GetComponent<EnemySc>().flashed = false;
        }
    }
}
