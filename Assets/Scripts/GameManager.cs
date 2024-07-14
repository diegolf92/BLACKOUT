using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject bossPrefab;
    public GameObject bossPos;
    bool bossOn = false;

    void Start()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player" && !bossOn)
        {
            bossOn = true;
            Instantiate(bossPrefab, bossPos.transform.position, Quaternion.identity);
        }
    }
}
