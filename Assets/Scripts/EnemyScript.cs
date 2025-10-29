using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    //TODO: Get the player by its type instead, and when I hit it, call its takeDamage function
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);

    }
}
