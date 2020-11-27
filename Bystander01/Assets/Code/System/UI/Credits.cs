using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{

    public float creditTimer = 45f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        creditTimer -= Time.deltaTime;

        if(creditTimer <= 0)
        {
            SceneManager.LoadScene(1);
        }
    }


    public void ExitCredits()
    {
        SceneManager.LoadScene(1);
    }
}
