using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    NavMeshAgent m_agent;

    [Header("FOV")]
    [SerializeField]
    public float m_FOVLongRadius;
    [SerializeField]
    public float m_FOVShortRadius;
    [SerializeField]
    public float m_FOVLongAngle;

    public bool m_canLongSeePlayer = false;
    public bool m_canShortSeePlayer= false;

    [Header("Targets mask")]
    [SerializeField]
    public LayerMask m_playerMask;
    [SerializeField]
    public LayerMask m_groundMask;
    [SerializeField]
    public LayerMask m_wallMask;

    [Header("Target Ref")]
    [SerializeField]
    GameObject m_playerRef;
    Transform m_transformPlayer;

    [Header("Patroling")]
    Vector3 walkPoint;
    public float walkPointRange;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_transformPlayer = m_playerRef.transform;
        walkPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        // starts the path searching
        StartCoroutine(SearchingCoroutine());
    }

    /// <summary>
    /// Function which launch every 0.1 sec the "SearchingFOV" function
    /// </summary>
    /// <returns></returns>
    private IEnumerator SearchingCoroutine()
    {
        // We check the FOV each 0.1 sec
        yield return new WaitForSeconds(0.1f);
        SearchingInFOV();

        StartCoroutine(SearchingCoroutine());
    }

    /// <summary>
    /// Search for the player in the FOV : if he is in, it will chase it, if not he will call the searchWalkPoint function
    /// </summary>
    private void SearchingInFOV()
    {
        // Get all the elements with the player mask around the enemy
        Collider[] longRangeChecks = Physics.OverlapSphere(transform.position, m_FOVLongRadius, m_playerMask);

        // If there is some player in the long range
        if (longRangeChecks.Length != 0 && !m_canShortSeePlayer)
        {
            // Get the relative position of the player
            Transform playerTransform = longRangeChecks[0].transform;
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

            // Check if the player is in the FOV angle of the enemy
            if (Vector3.Angle(transform.forward, directionToPlayer) < m_FOVLongAngle / 2)
            {
                //Get the relative distance of the player
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

                // Check if there is a wall between the enemy and the player
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, m_wallMask))
                {
                    //See the player then follow it
                    m_canLongSeePlayer = true;
                    m_agent.SetDestination(m_transformPlayer.position);
                }
                else
                    m_canLongSeePlayer = false;
            }
            else
                m_canLongSeePlayer = false;
        }
        else
            m_canLongSeePlayer = false;

        
        Collider[] shortRangeChecks = Physics.OverlapSphere(transform.position, m_FOVShortRadius, m_playerMask);
        // If there is some player in the short range (because it's short range it doesn't require to check the angle, it's 360 degrees around the enemy)
        if (shortRangeChecks.Length != 0 && !m_canLongSeePlayer)
        {
            Transform playerTransform = shortRangeChecks[0].transform;

            // Get the direction and distance to the player
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Check if there is a wall between the enemy and the player
            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, m_wallMask))
            {
                // If the player is too near, he lose
                if (distanceToPlayer < 0.3f)
                {
                    GameManager.current.SetVictory(false);
                    GameManager.current.SetFinished(1);
                }

                // See the player then follow it
                m_canShortSeePlayer = true;
                m_agent.SetDestination(m_transformPlayer.position);
            }
            else
                m_canShortSeePlayer = false;
        }
        else
            m_canShortSeePlayer = false;

        // if enemy don't see the player, he will patrol 
        if(!m_canLongSeePlayer && !m_canShortSeePlayer)
        {
            SearchWalkPoint();
        }
    }

    /// <summary>
    /// Make the enemy travel randomly in the building's area 
    /// </summary>
    private void SearchWalkPoint()
    {
        // check if the walkpoint is far or not
        Vector3 distance = transform.position - walkPoint;
        if(distance.magnitude < 1f)
        {
            // take random values for walking away
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            
            // If the enemy can reach the walkpoint
            if(transform.position.x + randomX > 0.5 && transform.position.x + randomX < 11.5 && transform.position.z + randomZ > 2 && transform.position.z + randomZ < 37)
            {
                walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
                m_agent.SetDestination(walkPoint);
            }
            else
                SearchWalkPoint();
        }

    }

}
