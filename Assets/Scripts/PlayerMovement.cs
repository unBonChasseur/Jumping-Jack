using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController m_controller;

    [SerializeField]
    private Transform m_groundCheck;

    [SerializeField]
    private LayerMask m_groundMask;

    private float m_speed = 5f;
    private float m_gravity = -9.81f;
    private float m_jumpHeight = 3f;

    private float m_groundDistance = 0.4f;
    private bool m_isGrounded;
    [SerializeField]
    private Vector3 m_velocity;

    void Start()
    {
        
    }

    void Update()
    {

        m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundDistance, m_groundMask);

        if(m_isGrounded && m_velocity.y < 0f)
        {
            m_velocity.y = -2f;
        }

        float x = 0f;
        float z = 0f;

        
        // Control forward/backward
        if (Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.S))
        {
            z = 1;
        }
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Z))
        {
            z = -1;
        }

        // Control Left/Right
        if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.D))
        {
            x = -1;
        }
        else if(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.Q))
        {
            x = 1;
        }


        if (Input.GetKey(KeyCode.LeftShift))
        {
            x *= 1.5f;
            z *= 1.5f;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            x /= 1.5f;
            z /= 1.5f;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        m_controller.Move(move * m_speed * Time.deltaTime);

        if (Input.GetKeyDown("space") && m_isGrounded)
        {
            m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
        }

        m_velocity.y += m_gravity * Time.deltaTime;

        m_controller.Move(m_velocity * Time.deltaTime);
    }
}
