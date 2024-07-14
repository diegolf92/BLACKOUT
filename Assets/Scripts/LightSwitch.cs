using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameObject boss;
    public GameObject blackness;
    public GameObject minions;
    public GameObject win;

    void Update()
    {
        if(boss == null && !win.gameObject.activeInHierarchy)
        {
            boss = GameObject.FindGameObjectWithTag("Boss");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            blackness.gameObject.SetActive(false);
            boss.gameObject.SetActive(false);
            minions.gameObject.SetActive(false);
            win.SetActive(true);
        }
    }
}
