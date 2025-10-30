using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    float attackDelay = 1f, attackDistance = 2f;
    [SerializeField]
    Vector3 destination;
    float attackTimer = 0f;
    PlayerMovement player;
    NavMeshAgent agent;
    //TODO: Get the player by its type instead, and when I hit it, call its takeDamage function
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        bool hasDestination = agent.SetDestination(player.transform.position);
        print(hasDestination);

        destination = agent.pathEndPosition;
        if (hasDestination && agent.remainingDistance <= 2f && attackTimer >= attackDelay)
        {
            //Check to see if we are close enough to the target and can see it
            Vector3 direction = player.transform.position - transform.position;

            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, direction, out hit,Mathf.Infinity)){
                if (hit.collider.gameObject.tag == "Player")
                {
                    print("We can attack!!!");
                    player.takeDamage(1);
                    print("attack");
                    attackTimer = 0f;
                }
            }
            
        }
    

    }
}
