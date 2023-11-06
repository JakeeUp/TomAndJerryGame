using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    
    LevelChanger level;
    private void Start()
    {
        level = GetComponent<LevelChanger>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            level.FadeToNextLevel();
        }
    }
}
