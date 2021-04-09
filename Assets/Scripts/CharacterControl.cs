using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    //different materials to apply to player
    public Material m_hologram = null;
    public Material m_forceField = null;
    public Material m_rimLight = null;
    private SkinnedMeshRenderer m_meshRenderer = null;

    private bool m_cursorLocked = true;

    public float m_jumpForce = 10f;
    private bool m_canJump = true;
    public float m_gravity = -9.81f;
    private bool m_holdingJump = false;

    public Transform m_feetPosition = null;
    public float m_detectionRange = 0.1f;

    Animator m_animator;
    CharacterController m_controller;

    public float m_moveSpeed = 10.0f;

    public float m_rotateSpeedX = 10f;
    public float m_rotateSpeedY = 8f;
    private Vector3 m_velocity;

    //this is for animation blending for instant change
    //so the animations arent jarred the current x and y pos are lerped inbetween to smooth
    float prevHorizontal = 0;
    float prevForward = 0;
    public float animationSwapTime = 1f;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_controller = GetComponent<CharacterController>();
        m_meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        m_rotateSpeedX *= 100f;
        m_rotateSpeedY *= 100f;
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float forward = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        
        Vector3 move = forward * transform.forward + transform.right * horizontal;
        
        forward = Mathf.Lerp(prevForward, forward, animationSwapTime);
        horizontal = Mathf.Lerp(prevHorizontal, horizontal, animationSwapTime);
        
        m_animator.SetFloat("Xpos", horizontal);
        m_animator.SetFloat("Ypos", forward);
        
        prevHorizontal = horizontal;
        prevForward = forward;
        
        m_controller.Move(move * m_moveSpeed * Time.deltaTime);
        
        if (Input.GetKeyDown(KeyCode.Space) && m_canJump)
        {
            m_canJump = false;
            m_velocity.y = m_jumpForce;
            m_holdingJump = true;
        }
        m_velocity.y += m_gravity * Time.deltaTime;
        m_controller.Move(m_velocity * Time.deltaTime);
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_holdingJump = false;
        }
        if (m_holdingJump == false)
        {
            m_velocity.y += m_gravity * Time.deltaTime;
        }

        Collider[] collisionCheck = Physics.OverlapSphere(m_feetPosition.position, m_detectionRange, ~0);
        
        foreach (Collider collider in collisionCheck)
        {
            bool inAir = true;
            if (collider.gameObject.CompareTag("Platform"))
            {
                inAir = false;
                m_canJump = true;
                if (m_velocity.y < 0.1f)
                    m_velocity.y = 0;
                //exit the foreach so it doesnt detect ground as well
                break;
            }
            if (collider.gameObject.CompareTag("Ground"))
            {
                m_controller.enabled = false;
                transform.position = new Vector3(0, 4, 72);
                m_controller.enabled = true;
            }
            if (inAir)
            {
                m_canJump = false;
            }
        }

        //This is just for ease of use, ui elements in world did not like the locked camera
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_cursorLocked)
            {
                m_cursorLocked = false;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else if (m_cursorLocked == false)
            {
                m_cursorLocked = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    
    public void Hologram() { m_meshRenderer.material = m_hologram; }
    public void ForceField() { m_meshRenderer.material = m_forceField; }
    public void RimLight() { m_meshRenderer.material = m_rimLight; }

}
