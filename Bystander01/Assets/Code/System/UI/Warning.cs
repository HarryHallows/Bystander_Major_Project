using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warning : MonoBehaviour
{
    public float messageTimer = 5f;

    private bool proceeding = false;

    public Animator warningFades;
    public GameObject headphoneObj;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(proceeding == true)
        {
            messageTimer -= Time.deltaTime;

            if(messageTimer <= 0)
            {
                SceneManager.LoadScene(1);
            }
        }
       
    }


    public void ExitGame()
    {
        Application.Quit();
    }


    public void Proceed()
    {
        warningFades.SetBool("Proceed", true);
        headphoneObj.SetActive(true);
        proceeding = true;
    }
}
