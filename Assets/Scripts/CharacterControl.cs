using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* MADE BY THOMAS LAMB 9th / 04/ 2021
 * THIS IS THE CHARACTER CONTROLLER
 * anything to do with the character is done in this script including
 * ui material changes
 * Last Edited 9/04/2021
 */

public class CharacterControl : MonoBehaviour
{
    //different materials to apply to player
    public Material m_hologram = null;
    public Material m_forceField = null;
    public Material m_rimLight = null;
    private SkinnedMeshRenderer m_meshRenderer = null;


    public float m_jumpForce = 10f;
    private bool m_canJump = true;
    public float m_gravity = -9.81f;
    private bool m_holdingJump = false;

    public Transform m_feetPosition = null;
    public float m_detectionRange = 0.1f;

    Animator m_animator;
    CharacterController m_controller;

    public float m_moveSpeed = 10.0f;
    private Vector3 m_velocity;

    //this is for animation blending for instant change
    //so the animations arent jarred the current x and y pos are lerped inbetween to smooth
    float m_prevHorizontal = 0;
    float m_prevForward = 0;
    public float m_animationSwapTime = 1f;

    void Start()
    {
        //gets all the components needed
        m_animator = GetComponent<Animator>();
        m_controller = GetComponent<CharacterController>();
        m_meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        //locks the cursor to be inbetween the application window
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        //get inputs
        float forward = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        
        Vector3 move = forward * transform.forward + transform.right * horizontal;
        
        //lerp between the animations so it isnt jarring between them
        forward = Mathf.Lerp(m_prevForward, forward, m_animationSwapTime);
        horizontal = Mathf.Lerp(m_prevHorizontal, horizontal, m_animationSwapTime);
        //set all the animator values
        m_animator.SetFloat("Xpos", horizontal);
        m_animator.SetFloat("Ypos", forward);
        
        //set the previous values to lerp between next frame
        m_prevHorizontal = horizontal;
        m_prevForward = forward;
        
        //move character
        m_controller.Move(move * m_moveSpeed * Time.deltaTime);
        
        //jump check
        if (Input.GetKeyDown(KeyCode.Space) && m_canJump)
        {
            m_canJump = false;
            m_velocity.y = m_jumpForce;
            m_holdingJump = true;
        }
        //apply gravity before to not cause clipping issues
        m_velocity.y += m_gravity * Time.deltaTime;
        m_controller.Move(m_velocity * Time.deltaTime);
        
        //when key is up we arent holding space anymore
        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_holdingJump = false;
        }
        if (m_holdingJump == false)
        {
            //apply more gravity when the holding is gone so simulate better jumping
            m_velocity.y += m_gravity * Time.deltaTime;
        }

        //ground check 
        Collider[] collisionCheck = Physics.OverlapSphere(m_feetPosition.position, m_detectionRange, ~0);
        
        foreach (Collider collider in collisionCheck)
        {
            //if the player is in the air then we cant jump
            bool inAir = true;
            if (collider.gameObject.CompareTag("Platform"))
            {
                //if the player has collided then we are not in the air and can set the velocity to 0
                //so it doesnt build up in negative from gravity
                inAir = false;
                m_canJump = true;
                if (m_velocity.y < 0.1f)
                    m_velocity.y = 0;
                //exit the foreach so it doesnt detect ground as well
                break;
            }
            //if we hit ground reset
            if (collider.gameObject.CompareTag("Ground"))
            {
                m_controller.enabled = false;
                transform.position = new Vector3(0, 4, 72);
                m_controller.enabled = true;
            }
            //if it is true by the end of the check then we cannot jump
            if (inAir)
            {
                m_canJump = false;
            }
        }

        
    }
    //shader setting functions. called by buttons in ui
    public void Hologram() { m_meshRenderer.material = m_hologram; }
    public void ForceField() { m_meshRenderer.material = m_forceField; }
    public void RimLight() { m_meshRenderer.material = m_rimLight; }

}
