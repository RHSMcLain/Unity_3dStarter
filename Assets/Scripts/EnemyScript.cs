using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    float attackDelay = 1f;
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

        agent.SetDestination(player.transform.position);
        if (agent.remainingDistance <= 2f && attackTimer >= attackDelay)
        {
            player.takeDamage(1);
            print("attack");
            attackTimer = 0f;
        }

    }
}
