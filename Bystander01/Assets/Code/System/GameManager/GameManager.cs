using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [Header("CANVAS ELEMENTS")]

    //Canvas

    public RectTransform scrollRect;

    //Images
    [Space]
    public Image scrim;

    //Buttons
    public GameObject[] SceneButtons;
    public GameObject phoneTrig;

    //Phone
    public GameObject phone;

    [Header("DATA TYPES")]

    //External Scripts Calls
    [Space]
    [Header("Scripts")]
    public CharacterController playerControl;
    public CameraController camControl;
    public AudioManager audioManager;

    //AudioSource
    public AudioSource footsteps;
    public AudioSource[] soundsEffects;

    //Animators
    [Header("Animators")]
    public Animator[] AIAnims;
    public Animator transAnim;

    //GameObject
    public GameObject victim;

    //Transforms

    //Vectors

    //Floats
    [SerializeField] private float animTimer = 10f;
    [SerializeField] private float walkTimer = 0.75f;
    [SerializeField] private float harassTimer = 1.5f;
    [SerializeField] private float lastRunTimer = 30f;
    [SerializeField] private float transitionTimer = 2f;
    [SerializeField] private float endTimer = 40f;
    [SerializeField] private float finalNarTimer = 20f;

    [SerializeField] private float narrativeTimer;
    [SerializeField] private float phoneMovement = 1000f;
    //Integers

    //Booleans

    [SerializeField] public bool startWalk;  //anim bool
    [SerializeField] public bool transition; // anim bool

    [SerializeField] public bool UITrigger;
    [SerializeField] public bool crowdOff = false;
    [SerializeField] public bool assaultStart = false;

    [SerializeField] private bool finalNar = false;
    [SerializeField] private bool narrativeDone;
    [SerializeField] private bool lockCursor;
    [SerializeField] private bool UIStart;
    [SerializeField] private bool phoneUp; //Canvas element button
    [SerializeField] private bool lastRun = false; // scene 3 run button option
    [SerializeField] private bool lastSelection = false;

    //Strings
    public string gameState;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GameObject.Find("Player").GetComponent<CharacterController>();
        camControl = GameObject.Find("Main Camera").GetComponent<CameraController>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        scrollRect = phone.GetComponent<RectTransform>();
      
        UIStart = true;
        lockCursor = false;
        UITrigger = false;
        narrativeDone = false;

        transAnim.SetBool("FadeToBlack", false);

        gameState = "Scene1";
    }

    // Update is called once per frame
    void Update()
    {
        CursorControl();
        SceneManagament();
        Footsteps();
    }

    //Controls the cursor management switching on and off
    private void CursorControl()
    {
        if (lockCursor == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if(lockCursor == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    //Controlling the different Scene States 
    private void SceneManagament()
    {
        //====== Game Scene =====
        /*
         Scene 1:
         * Scene1 has the initial canvas setup for the mobile phone UI which prompts the player to access careerApp
         * this then allows the player to move through the to the different information on the platform feed
         * once player encounters the first Scene they will be given a prompt of three options to choose from
         * once player has chosen the mobile will be closed down and the game scene will start up.
        */

        if(lastSelection == true)
        {
            endTimer -= Time.deltaTime;

            if(endTimer <= 0)
            {
                transAnim.SetBool("FadeToBlack", true);

                transitionTimer -= Time.deltaTime;

                if(transitionTimer <= 0)
                {
                    SceneManager.LoadScene(3);
                }
            
            }
        }

        if (finalNar == true)
        {
            finalNarTimer -= Time.deltaTime;

            if (finalNarTimer <= 0)
            {
                transAnim.SetBool("FadeToBlack", true);
                transitionTimer -= Time.deltaTime;

                if (transitionTimer <= 0)
                {
                    SceneManager.LoadScene(3);
                }

            }
        }

        if (phoneUp == true)
        {
            //Change the RectTransform's anchored positions depending on the Slider values
            scrollRect.anchoredPosition += new Vector2(0, phoneMovement) * Time.deltaTime;

            if (scrollRect.anchoredPosition.y >= 0)
            {
                scrollRect.anchoredPosition = new Vector2(0, 0);
            }
        }
        else
        {
            scrollRect.anchoredPosition -= new Vector2(0, phoneMovement) * Time.deltaTime;

            if (scrollRect.anchoredPosition.y <= -600)
            {
                scrollRect.anchoredPosition = new Vector2(0, -600);
            }
        }

        if (gameState == "Scene1")
        {
            Scene1();
        }

        /*
         Scene 2:
         * Player will be moved through the player waypoints
         * Once player has come into contact with desired waypoint position they will slow down their movement speed
         * Girl will change animation states from looking at river to walking past bench
         * Audio of the three guys harassing the girl will begin 
         * While this is happening/once the scene has finished the player will be prompted with a selection of options
         * The option to stand up for the girl will have a cooldown based on how long the girl is being verbally harassed once she gets past them this will end the option
         */

        if (gameState == "Scene2")
        {
            Scene2();          
        }


        /*
         Scene 3:
         * Player will then be prompted by the phone UI in the corner of the screen with a text message "Hey! Sorry im late, I just got out of waterloo station, I'll wait for you there"
         * Player will be prompted with a narrative line by the character stating cutting across the green is quicker to get to the station ( separating themselves from the crowd )
         * Player will then start to hear two people in a fight with one another only slightly and will be given the option to investigate or mind their own business and continue.
         * if player continues then game will end and the game will go to the closing screen credits..
        */

        if (gameState == "Scene3")
        {
            if (narrativeDone == true)
            {
                narrativeTimer -= Time.deltaTime;
            }

            if (narrativeTimer <= 0)
            {
                Scene3();
            }


            /*
             Scene 3.2:
             * Player will have more of an idea whats happening around the corner from themselves
             * Struggle Audio sounds will be played throughout the scene from the alleyway
             * Player will be prompted with options again to choose from

             Credits Scene:
             * Move to the credit scene after the end game condition has been met.
            */
        }
    }

    private void Footsteps()
    {
        if(playerControl.movementSpeed > 0.5f)
        {
            footsteps.enabled = true;
        }
        else if(playerControl.movementSpeed <= 0.5f)
        {
            footsteps.enabled = false;
        }     

        if(lastRun == true)
        {
            lastRunTimer -= Time.deltaTime;

            soundsEffects[2].volume -= 0.02f * Time.deltaTime;

            playerControl.movementSpeed += 0.5f * Time.deltaTime;
            

            if (lastRunTimer <= 0)
            {
                playerControl.movementSpeed = 0;
            }
        }
    }


    private void Scene1()
    {
        soundsEffects[0].volume += 0.001f * Time.deltaTime;

        if(soundsEffects[0].volume >= 0.01f)
        {
            soundsEffects[0].volume = 0.01f;
        }
      
        if (narrativeDone == true)
        {
            narrativeTimer -= Time.deltaTime;
        }
       
        if (UIStart == true)
        {
            
            //once button choice has been made play that option's voice lines
            lockCursor = false;
            playerControl.movementSpeed = 0;
            camControl.mouseSensitivity = 0f;
        }
        else
        {
            if (narrativeTimer <= 0)
            {
                lockCursor = true;
                playerControl.movementSpeed = Random.Range(2f, 3f);
                camControl.mouseSensitivity = 3f;
                narrativeDone = false;
            }          
        }
    }

    private void Scene2()
    {
        if(crowdOff == true)
        {
            soundsEffects[0].volume -= 0.00001f;
        }

        if (playerControl.moveChange == true)
        {
            playerControl.movementSpeed -= Time.deltaTime;

            if (playerControl.movementSpeed <= 0)
            {
                playerControl.movementSpeed = 0;
            }
        }
        else
        {
            playerControl.movementSpeed = Random.Range(2f, 3f);
        }


        if (narrativeDone == true)
        {
            narrativeTimer -= Time.deltaTime;
        }


        if (UITrigger == true)
        {
            //turn on button choices for scene2
            //once button choice has been made turn off UITrigger
            //once button choice has been made play that option's voice lines
            //once button choice has been made then turn moveChange to false
            lockCursor = false;
            camControl.mouseSensitivity = 1f;

            if (camControl.cameraYaw == 80f)
            {
                camControl.mouseSensitivity = 0f;
                animTimer -= Time.deltaTime;

                AIAnims[0].SetBool("transition", true); 
                //Start animations from idle
                
                walkTimer -= Time.deltaTime;
                if(walkTimer <= 0)
                {
                    harassTimer -= Time.deltaTime;

                    AIAnims[0].SetBool("startWalk", true);    
                    
                    if(harassTimer <= 0)
                    {
                        AIAnims[1].SetBool("harass", true);

                        //play audio by harasser

                        Debug.Log("I should be hearing the harassment!");
                        soundsEffects[1].enabled = true;
                    }
                }
               
            }

            if (animTimer <= 0)
            {
                SceneButtons[1].gameObject.SetActive(true);
                scrim.gameObject.SetActive(true);
            }
        }
        else
        {
            //Start moving and proceeding after narrative audio
            if (narrativeTimer <= 0)
            {
                lockCursor = true;
                camControl.mouseSensitivity = 3f;
                playerControl.moveChange = false;
                animTimer = 5f;
            }  
        }

    }

    private void Scene3()
    {
        //crowd sound effect off
        soundsEffects[0].enabled = false;

        if (playerControl.moveChange == true)
        {
            playerControl.movementSpeed -= Time.deltaTime;

            if (playerControl.movementSpeed <= 0)
            {
                playerControl.movementSpeed = 0;
            }
        }


        

        if (UITrigger == true)
        {
            //turn on button choices for scene2
            //once button choice has been made turn off UITrigger
            //once button choice has been made play that option's voice lines
            //once button choice has been made then turn moveChange to false
            lockCursor = false;
            animTimer -= Time.deltaTime;
            playerControl.moveChange = true;

            if(assaultStart == true)
            {
                soundsEffects[2].enabled = true;
                soundsEffects[2].volume += 0.001f;
            }

            if (animTimer >= 0)
            {
                camControl.mouseSensitivity = 1f;
            }

            if (animTimer <= 0)
            {
                camControl.mouseSensitivity = 0f;
                SceneButtons[2].gameObject.SetActive(true);
                scrim.gameObject.SetActive(true);
            }


           
               
            
        }

       
    }



#region Canvas Buttons Functions
    public void PhoneTrigger()
    {
        phoneUp = true;
        phoneTrig.SetActive(false);
    }


    //Canvas Scene 1 Options 
    public void Scene1Trigger()
    {
        //activates the scene 1 buttons and scrim
        SceneButtons[0].gameObject.SetActive(true);
        scrim.gameObject.SetActive(true);      
    }


    public void VoiceButtonPassive1()
    {
        //Play voice line attached
        //deactivate all of canvas buttons
        //activate all movement code and camera code
        //lock cursor and set cursor visability to false

        SceneButtons[0].gameObject.SetActive(false);
        scrim.gameObject.SetActive(false);
        phoneUp = false;
        UIStart = false;

        narrativeTimer = 4.5f;

        audioManager.Play("Passive_Scene1");

        narrativeDone = true;
        
        // phone.gameObject.SetActive(false);
    }

    public void VoiceButtonPositive1()
    {
        //Play voice line attached
        //deactivate all of canvas buttons
        //activate all movement code and camera code
        //lock cursor and set cursor visability to false
        SceneButtons[0].gameObject.SetActive(false);
        scrim.gameObject.SetActive(false);
        phoneUp = false;
        UIStart = false;

        // phone.gameObject.SetActive(false);

        narrativeTimer = 6.5f;

        audioManager.Play("Positive_Scene1");
        narrativeDone = true;
    }


    public void VoiceButtonNegative1()
    {
        //Play voice line attached
        //deactivate all of canvas buttons
        //activate all movement code and camera code
        //lock cursor and set cursor visability to false
        SceneButtons[0].gameObject.SetActive(false);
        scrim.gameObject.SetActive(false);
        phoneUp = false;
        UIStart = false;

        // phone.gameObject.SetActive(false);

        narrativeTimer = 7.5f;

        audioManager.Play("Negative_Scene1");
        narrativeDone = true;
    }

    //Canvas Scene 2 Options
    public void VoiceButtonPassive2()
    {
        //Play voice line attached
        //deactivate all of canvas buttons
        //activate all movement code and camera code
        //lock cursor and set cursor visability to false
        SceneButtons[1].gameObject.SetActive(false);
        scrim.gameObject.SetActive(false);
        UITrigger = false;

        narrativeTimer = 4.5f;

        audioManager.Play("Passive_Scene2");
        narrativeDone = true;
    }

    public void VoiceButtonPositive2()
    {
        //Play voice line attached
        //deactivate all of canvas buttons
        //activate all movement code and camera code
        //lock cursor and set cursor visability to false
        SceneButtons[1].gameObject.SetActive(false);
        scrim.gameObject.SetActive(false);
        UITrigger = false;

        narrativeTimer = 3.5f;

        audioManager.Play("Positive_Scene2");
        narrativeDone = true;
    }

    public void VoiceButtonNegative2()
    {
        //Play voice line attached
        //deactivate all of canvas buttons
        //activate all movement code and camera code
        //lock cursor and set cursor visability to false
        SceneButtons[1].gameObject.SetActive(false);
        scrim.gameObject.SetActive(false);
        UITrigger = false;

        narrativeTimer = 6.5f;

        audioManager.Play("Negative_Scene2");
        narrativeDone = true;
    }

    //Canvas Scene 3 Options
    public void VoiceButtonPassive3()
    {
        //Play voice line attached
        //deactivate all of canvas buttons
        //activate camera movement back
        //lock cursor and set cursor visability to false
        //Set end of game transition
        //change scene to credits

        narrativeTimer = 30.5f;
        finalNarTimer = 32f;
        finalNar = true;
        audioManager.Play("Passive_Scene3");

        SceneButtons[2].gameObject.SetActive(false);
        scrim.gameObject.SetActive(false);

        narrativeDone = true;

        lastSelection = true;
    }

    public void VoiceButtonPositive3()
    {
        //Play voice line attached
        //deactivate all of canvas buttons
        //activate camera movement back
        //lock cursor and set cursor visability to false
        //Set end of game transition
        //change scene to credits

        narrativeTimer = 22.5f;
        finalNarTimer = 25f;
        finalNar = true;
        audioManager.Play("Positive_Scene3");

        SceneButtons[2].gameObject.SetActive(false);
        scrim.gameObject.SetActive(false);

        narrativeDone = true;

        lastSelection = true;
    }

    public void VoiceButtonNegative3()
    {
        //Play voice line attached #
        //deactivate all of canvas buttons #
        //activate camera movement back #
        //lock cursor and set cursor visability to false #


        //Set end of game transition
        //change scene to credits

        narrativeTimer = 27.5f;
        finalNarTimer = 29f;
        finalNar = true;
        audioManager.Play("Negative_Scene3");
        narrativeDone = true;

        SceneButtons[2].gameObject.SetActive(false);
        scrim.gameObject.SetActive(false);

        playerControl.moveChange = false;
        lastRun = true;

        lockCursor = true;
        Cursor.visible = false;
        camControl.mouseSensitivity = 3f;

        lastSelection = true;
    }
    #endregion
}
