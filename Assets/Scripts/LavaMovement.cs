using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMovement : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < 9)
        {
            this.transform.position = new Vector3(0, transform.position.y + m_speed * Time.deltaTime, 0);
        }
    }
}
