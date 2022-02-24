using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    
    //Hashes useful for animations
    [Header("Animations")]
    private Animator m_animator;
    private int m_velocityZHash;
    private int m_velocityXHash;
    private int m_jumpSlowHash;
    private int m_jumpFastHash;

    [Header("Movement")]
    [SerializeField]
    private CharacterController m_controller;

    [Header("Gravity")]
    private float m_gravity = -9.81f;
    private float m_jumpHeight = 1.0f;
    private float m_groundDistance = 0.1f;
    private bool m_isGrounded;

    [SerializeField]
    private Transform m_groundCheck;
    [SerializeField]
    private LayerMask m_groundMask;
    [SerializeField]
    private Vector3 m_velocity;

    [Header("Speed")] 
    private float m_velocityZ = 0f;
    private float m_velocityX = 0f;
    private float m_velocityMax;

    private float m_acceleration = 0.75f;
    private float m_deceleration = 1.5f;

    [SerializeField]
    private bool m_hasStarted = false;

    void Start()
    {
        // Get animator
        m_animator = GetComponent<Animator>();

        // Get Velocity Hashes
        m_velocityZHash = Animator.StringToHash("VelocityZ");
        m_velocityXHash = Animator.StringToHash("VelocityX");

        // Get Jumping Hashes
        m_jumpSlowHash = Animator.StringToHash("IsJumpingSlow");
        m_jumpFastHash = Animator.StringToHash("IsJumpingFast");

        //StartCoroutine(IntroductionAnimation());
        m_hasStarted = true;
    }

    void Update()
    {
        // Get Axis inputs
        float IsPressedZ = Input.GetAxis("Vertical");
        float IsPressedX = Input.GetAxis("Horizontal");

        // Get Running input
        bool RunPressed = Input.GetKey(KeyCode.LeftShift);
        bool JumpPressed = Input.GetKey(KeyCode.Space);

        // Get Velocity Values
        float VelocityZ = m_animator.GetFloat("VelocityZ");
        float VelocityX = m_animator.GetFloat("VelocityX");

        // Get Jumping Values
        bool IsJumpingSlow = m_animator.GetBool("IsJumpingSlow");
        bool IsJumpingFast = m_animator.GetBool("IsJumpingFast");

        // Check if the player is on the ground
        m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundDistance, m_groundMask);

        // If the player hits the ground, reset jump values to false
        if((m_isGrounded && IsJumpingFast) || (m_isGrounded && IsJumpingSlow))
        {
            m_animator.SetBool(m_jumpFastHash, false);
            m_animator.SetBool(m_jumpSlowHash, false);
        }

        if (m_isGrounded && m_hasStarted)
        {
            // Increase max velocity if velocity is positive and running button is pressed
            m_velocityMax = (VelocityZ > -0.05f && RunPressed) ? 2.0f : 1.0f;

            // Jump input is pressed
            if (JumpPressed && VelocityZ > 0.05f)
            {
                // Add vertical force
                m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2.0f * m_gravity);

                // Select jump's animation
                if (VelocityZ > 1)
                    m_animator.SetBool(m_jumpFastHash, true);
                else if (VelocityZ > 0.05f)
                    m_animator.SetBool(m_jumpSlowHash, true);
            }
            else
                m_velocity.y = 0f;

            // If a vertical input is pressed
            if (Mathf.Abs(IsPressedZ) == 1 && Mathf.Abs(VelocityZ) <= m_velocityMax)
            {
                // If opposite input is pressed we make the deceleration twice more efficient than acceleration
                if ((IsPressedZ == 1 && VelocityZ <= -0.05) || (IsPressedZ == -1 && VelocityZ >= 0.05))
                    m_velocityZ = m_velocityZ < 0 ? m_velocityZ + m_deceleration * Time.deltaTime : m_velocityZ - m_deceleration * Time.deltaTime;
                else
                    m_velocityZ += m_acceleration * Time.deltaTime * IsPressedZ;

                // If the velocity is higher than the velocity max, gives to velocity the max value
                if (Mathf.Abs(m_velocityZ) > m_velocityMax)
                    m_velocityZ = (m_velocityZ < 0) ? -m_velocityMax : m_velocityMax;
            }

            // If an horizontal input is pressed
            if (Mathf.Abs(IsPressedX) == 1 && Mathf.Abs(VelocityX) <= m_velocityMax)
            {
                // If opposite input is pressed
                if ((IsPressedX == 1 && VelocityX <= -0.05) || (IsPressedX == -1 && VelocityX >= 0.05))
                    m_velocityX = m_velocityX < 0 ? m_velocityX + m_deceleration * Time.deltaTime : m_velocityX - m_deceleration * Time.deltaTime;
                else
                    m_velocityX += m_acceleration * Time.deltaTime * IsPressedX;

                // If the velocity is higher than the velocity max, gives to velocity the max value
                if (Mathf.Abs(m_velocityX) > m_velocityMax)
                    m_velocityX = (m_velocityX < 0) ? -m_velocityMax : m_velocityMax;
            }

            // If vertical is not pressed or is higher than velocity max
            if ((Mathf.Abs(IsPressedZ) != 1 && Mathf.Abs(m_velocityZ) > 0) || Mathf.Abs(VelocityZ) > m_velocityMax)
            {
                m_velocityZ = m_velocityZ < 0 ? m_velocityZ + m_deceleration * Time.deltaTime : m_velocityZ - m_deceleration * Time.deltaTime;

                if (Mathf.Abs(m_velocityZ) <= 0.05f)
                    m_velocityZ = 0;
            }

            // If horizontal is not pressed or is higher than velocity max
            if ((Mathf.Abs(IsPressedX) != 1 && Mathf.Abs(m_velocityX) > 0) || Mathf.Abs(VelocityX) > m_velocityMax)
            {
                m_velocityX = m_velocityX < 0 ? m_velocityX + m_deceleration * Time.deltaTime : m_velocityX - m_deceleration * Time.deltaTime;

                if (Mathf.Abs(m_velocityX) <= 0.05f)
                    m_velocityX = 0;
            }

        }

        // Apply new velocities
        m_animator.SetFloat(m_velocityZHash, m_velocityZ);
        m_animator.SetFloat(m_velocityXHash, m_velocityX);

        // Move character with different velocities
        Vector3 move = new Vector3();
        if (m_velocityZ < -0.05)
            move += transform.forward * m_velocityZ;
        else if (m_velocityZ <= 1)
            move += transform.forward * m_velocityZ * 1.5f;
        else
            move += transform.forward * m_velocityZ * 2.5f;

        if (Mathf.Abs(m_velocityX) <= 1)
            move += transform.right * m_velocityX * 1.5f;
        else
            move += transform.right * m_velocityX * 3.0f;

        m_controller.Move(move * Time.deltaTime);

        m_velocity.y += m_gravity * Time.deltaTime;
        m_controller.Move(m_velocity * Time.deltaTime);
    }

    /// <summary>
    /// Block player's input to let the animation's introduction play
    /// </summary>
    /// <returns></returns>
    private IEnumerator IntoductionAnimation()
    {
        // Speed at start of 1st animation
        m_velocityZ = 1.2f; 
        yield return new WaitForSeconds(1.333f);

        // Stay at the same place then
        m_velocityZ = 0.0f;
        yield return new WaitForSeconds(2.533f - 1.333f);
        yield return new WaitForSeconds(2.6f);
        m_hasStarted = true;
    }
}
