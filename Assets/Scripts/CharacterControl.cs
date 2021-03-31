using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{


    Animator animator;
    CharacterController controller;

    public float MoveSpeed = 10.0f;
    private Vector3 velocity;

    public float rotateSpeedX = 10f;
    public float rotateSpeedY = 8f;


    //this is for animation blending for instant change
    //so the animations arent jarred the current x and y pos are lerped inbetween to smooth
    float prevHorizontal = 0;
    float prevForward = 0;
    public float animationSwapTime = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        rotateSpeedX *= 100f;
        rotateSpeedY *= 100f;
        Cursor.lockState = CursorLockMode.Locked;
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

        Vector3 move = forward * transform.forward + transform.right * horizontal;

        forward = Mathf.Lerp(prevForward, forward, animationSwapTime);
        horizontal = Mathf.Lerp(prevHorizontal, horizontal, animationSwapTime);

        animator.SetFloat("Xpos", horizontal);
        animator.SetFloat("Ypos", forward);

        prevHorizontal = horizontal;
        prevForward = forward;

        
        controller.Move(move * MoveSpeed * Time.deltaTime);
        

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(new Vector3(0, 1, 0), mouseX * rotateSpeedX * Time.deltaTime);
    }
}
