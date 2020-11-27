using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    //Scripts
    GameManager gm;

    //Animators
    Animator _animator;

    //Vectors
    public Vector3 destination;
    private Vector3 lastPosition;
    Vector3 velocity;

    //Floats
    public float rotationSpeed = 120f;
    public float movementSpeed = 1f;
    public float stopDistance = 2f;

    public float startDestroyTime = 2f;

    [SerializeField]
    private float destroyTime;

    //Booleans
    public bool reachedDestination;
    public bool moveChange = false; //contorls the movement speed parameters of the player

    private void Awake()
    {
        movementSpeed = Random.Range(2f, 3f);
        _animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        destroyTime = startDestroyTime;
    }

    // Update is called once per frame
    void Update()
    {
        WaypointControl();
    }

    private void WaypointControl()
    {
        if (transform.position != destination)
        {

            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;

            float destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= stopDistance)
            {
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            }
            else
            {
                reachedDestination = true;
            }

            velocity = (transform.position - lastPosition) / Time.deltaTime;
            velocity.y = 0;

            var velocityMagnitude = velocity.magnitude;
            velocity = velocity.normalized;
            var forwardDotProduct = Vector3.Dot(transform.forward, velocity);
            var rightDotProduct = Vector3.Dot(transform.right, velocity);


            // _animator.SetFloat("Horizontal", rightDotProduct);
            // _animator.SetFloat("Forward", forwardDotProduct);

        }

        lastPosition = transform.position;
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
      
    }

    private void OnTriggerEnter(Collider collide)
    {
        if (collide.CompareTag("SpeedBox"))
        {
            gm.gameState = "Scene2";
            gm.UITrigger = true;
            moveChange = true;
        }

        if (collide.CompareTag("SpeedBox2"))
        {
            gm.gameState = "Scene3";
            gm.UITrigger = true;
            gm.assaultStart = true;
            moveChange = true;
        }

        if (collide.CompareTag("SoundBox"))
        {
            gm.crowdOff = true;
        }

    }
}
