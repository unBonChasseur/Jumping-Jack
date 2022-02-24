using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [SerializeField]
    private Transform m_playerTransform;
    [SerializeField]
    private float m_mouseSensitivity = 100f;
    private float m_xRotation = 0f;

    void Start()
    {
        // We lock the cursor on the screen.
        //Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Allows player to rotate the camera with his mouse
    /// </summary>
    void Update()
    {
        //If we move mouse on X axis, the character turn on X axis
        float mouseX = Input.GetAxis("Mouse X") * m_mouseSensitivity * Time.deltaTime;
        m_playerTransform.Rotate(Vector3.up * mouseX);

        //If we move mouse on X axis, the character turn on Y axis
        float mouseY = Input.GetAxis("Mouse Y") * m_mouseSensitivity * Time.deltaTime;

        m_xRotation -= mouseY;
        // Reminder : Clamp to stop rotation between two values
        m_xRotation = Mathf.Clamp(m_xRotation, -90f, 90f);
        // Reminder : Quaternion is needed for rotations
        transform.localRotation = Quaternion.Euler(m_xRotation, 0f, 0f);
    }
}
