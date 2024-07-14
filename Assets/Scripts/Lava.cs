using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    AudioSource audios;
    public AudioClip audioClipArray;

    void Start()
    {
        audios = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.isOnLava = true;
            player.LavaFloor();
            audios.PlayOneShot(audioClipArray);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            other.GetComponent<PlayerController>().isOnLava = false;
        }
    }
}
