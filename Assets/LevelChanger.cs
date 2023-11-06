using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private AudioSource menuSound;
    public int levelToLoad;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            menuSound.Play();
            FadeToNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ExitedGame");
            Application.Quit();
        }


    }
    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void FadeToLevel(int levelindex)
    {
        levelToLoad = levelindex;
        animator.SetTrigger("FadeOut");
    }
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            //FadeToNextLevel();
            //animator.SetTrigger("FadeOut");
            SceneManager.LoadScene(3);
            //animator.SetTrigger("FadeOut");
        }
    }
}
