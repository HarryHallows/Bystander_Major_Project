﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigationController : MonoBehaviour
{

    Animator _animator;

    public  Vector3 destination;
    private Vector3 lastPosition;
            Vector3 velocity;

    public float rotationSpeed = 120f;
    public float movementSpeed = 1f;
    public float stopDistance = 2f;

    public float startDestroyTime = 2f;

    [SerializeField]
    private float destroyTime;

    public bool reachedDestination;


    private void Awake()
    {
        movementSpeed = Random.Range(.8f, 2f);
        _animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        destroyTime = startDestroyTime;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(transform.position != destination)
        {

            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;

            float destinationDistance = destinationDirection.magnitude;

            if(destinationDistance >= stopDistance)
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
        if(collision.gameObject.tag == ("DestroyerTerrain"))
        {
            destroyTime--;

            if(destroyTime <= 0)
            {
                Destroy(this.gameObject);
                destroyTime = startDestroyTime;
            }

        }
    }
}
