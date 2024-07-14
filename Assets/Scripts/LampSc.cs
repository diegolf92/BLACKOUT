using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampSc : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Boss")
        {
            other.GetComponent<EnemySc>().flashed = true;
        }

        if(other.transform.tag == "Enemy")
        {
            Destroy(other.gameObject);
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
