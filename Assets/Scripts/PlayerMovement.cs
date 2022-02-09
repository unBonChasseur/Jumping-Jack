using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movements")]
    private bool m_hasStarted = false;
    private float m_speed = 5f;
    [SerializeField]
    private CharacterController m_controller;


    [Header("Gravity")]
    private float m_gravity = -9.81f;
    private float m_jumpHeight = 1.5f;
    private float m_groundDistance = 0.1f;
    private bool m_isGrounded;

    [SerializeField]
    private Transform m_groundCheck;
    [SerializeField]
    private LayerMask m_groundMask;
    [SerializeField]
    private Vector3 m_velocity;

    [Header("Lava")]
    [SerializeField]
    private LayerMask m_lavaMask;

    void Start()
    {
        StartCoroutine(StartAnimation());
    }

    void Update()
    {
        if (m_hasStarted)
        {
            // Control forward/backward
            float z = 0f;
            z = Input.GetAxis("Vertical");

            // Control Left/Right
            float x = 0f;
            x = Input.GetAxis("Horizontal");

            // Disable the acceleration when you go in 2 directions at the same time.
            if (x != 0 && z != 0)
            {
                x /= 1.5f;
                z /= 1.5f;
            }

            // Control Run fonctionnality
            if (Input.GetKey(KeyCode.LeftShift))
            {
                x *= 2f;
                z *= 2f;
            }

            // Used to move the gameObject by using direction (not X and Z axis)
            Vector3 move = transform.right * x + transform.forward * z;
            m_controller.Move(move * m_speed * Time.deltaTime);



            // Check if the player is on the ground
            m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundDistance, m_groundMask);

            if (m_isGrounded)
            {
                // Lock the velocity to a value  if it's under 0 value
                if (m_velocity.y <= 0)
                {
                    m_velocity.y = 0f;
                }

                // Add a vertical positive acceleration on the velocity
                if (Input.GetKeyDown("space"))
                {
                    m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
                }

            }

            // Calculates the new velocity at each iteration
            m_velocity.y += m_gravity * Time.deltaTime;
            m_controller.Move(m_velocity * Time.deltaTime);

            // Check if the player touches the lava
            if (Physics.CheckSphere(m_groundCheck.position, m_groundDistance, m_lavaMask))
            {
                //transform.position = new Vector3(2, 3, 19);
            }
        }
    }

    private IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(2.533f);
        yield return new WaitForSeconds(2.6f);
        m_hasStarted = true;
    }
}
