using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{


    Animator animator;
    CharacterController controller;

    public float MoveSpeed = 10.0f;
    private Vector3 velocity;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Animation changing for aiming 
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("IsAiming", true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("IsAiming", false);
        }

        float forward = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        velocity.x = horizontal * MoveSpeed * Time.deltaTime;
        velocity.z = forward * MoveSpeed * Time.deltaTime;

        animator.SetFloat("Xpos", horizontal);
        animator.SetFloat("Ypos", forward);


        controller.Move(velocity);
    }
}
