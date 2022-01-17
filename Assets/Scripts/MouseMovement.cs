using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    private float m_mouseSensitivity = 100f;

    [SerializeField]
    private Transform m_playerTransform;

    private float m_xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //On veut que le personnage tourne sur l'axe des x.
        float mouseX = Input.GetAxis("Mouse X") * m_mouseSensitivity * Time.deltaTime;


        //On veut que la caméra tourne sur l'axe des y 
        float mouseY = Input.GetAxis("Mouse Y") * m_mouseSensitivity * Time.deltaTime;
        m_xRotation -= mouseY;
        // Reminder : Clamp to stop rotation between two values
        m_xRotation = Mathf.Clamp(m_xRotation, -90f, 90f);

        // Reminder : Quaternion is needed rotation
        transform.localRotation = Quaternion.Euler(m_xRotation, 0f, 0f);
        m_playerTransform.Rotate(Vector3.up * mouseX);
    }
}
