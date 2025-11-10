using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    GameObject[] spawners;
    [SerializeField] //serialized so I can bring in the prefab I want
    GameObject enemyPrefab;
    [SerializeField]
    float spawnFrequency = 3f;

    float spawnTimer = 0f;
    void spawnEnemy()
    {
        //get the random number of the spawner
        int target = Random.Range(0, spawners.Length);

        //spawn the enemy in that location
        Instantiate(enemyPrefab, spawners[target].transform.position, Quaternion.identity);

        //TODO: put a limit on the number of enemies
    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        spawnEnemy();

    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnFrequency && EnemyScript.enemyCount <= 5)
        {
            spawnEnemy();
            spawnTimer = 0f;
        }
    }
}
