using UnityEngine;
using UnityEngine.AI;

public class AIEnemyManager : MonoBehaviour
{
    public NavMeshAgent agent;                      // To determine the MavMesh Component of a gameObject
    public Transform player;                        // Get the player position

    public LayerMask whatIsPlayer;    // Layer Mask to filter whatIsPlayer is what AI sees this should be set to "Players".  

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

     //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;



    private void Awake()
    {
        player = GameObject.Find("Tank_Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void Patroling()
    {
        if (!walkPointSet) RandomPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        //Walkpoint reached
        if (!agent.pathPending && agent.remainingDistance < 1f)
            walkPointSet = false;
    }
    private void RandomPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 randomPoint = transform.position + new Vector3(randomX, 0, randomZ);
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
            
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

      
        if (!alreadyAttacked)
        {
            ///Attack code here
            GameObject bulletInstance = Instantiate(projectile, transform.position, Quaternion.identity);

            Rigidbody bulletRigidbody = bulletInstance.AddComponent<Rigidbody>();
            bulletInstance.GetComponent<ShellExplosion>()._TankMask = whatIsPlayer;
        
             // Set the shell's velocity to the launch force in the fire position's forward direction.
            bulletRigidbody.linearVelocity = 32f * transform.forward;
            ///End of attack code
            
           alreadyAttacked = true;
           Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        
    }

     private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
