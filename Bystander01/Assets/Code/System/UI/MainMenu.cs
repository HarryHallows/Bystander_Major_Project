using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    //Start Scene first 
    /*
     * load the start menu scene first then navigate based on the location destination required
     * if play then load warning message which will allow the player to return to menu or start game -- Needs to add this screen in the scene directory
     * if quit then close application
     * if credits then load credit scene
     */

    private float transitionTimer = 3f;

    private bool transitioning = false;

    public Animator transition;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(transitioning == true)
        {
            transitionTimer -= Time.deltaTime;

            transition.SetBool("FadeToBlack", true);

            if (transitionTimer <= 0)
            {
                SceneManager.LoadScene(2);
            }
        }
    }

    public void PlayGame()
    {
        transitioning = true;
    }

    public void CreditsScene()
    {
        //hook up to the credits button
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        //hook up to the quit button
        Application.Quit();
    }


   
}
