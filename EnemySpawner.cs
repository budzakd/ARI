using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("AI References")]
    [SerializeField] private GameObject[] EnemyToSpawn;

    [Header("AI Settings")]
    [SerializeField] private Transform SpawnPosition;
    [SerializeField] private int enemyCounter = 0;
    [SerializeField] private int howManyEnemiesSpawn = 3;
    [SerializeField] private float spawnTime = 7f;
    [SerializeField] private bool canSpawnEnemy;

    [Header("Spawner RayCast")]
    [SerializeField] private Transform EnemyViewCast;
    [SerializeField] private float raycastLenght = 1f;
    [SerializeField] private LayerMask raycastLayerMask;
    private RaycastHit2D RaycastHit;

    private void Update()
    {
        RaycastHit = Physics2D.Raycast(new Vector2(EnemyViewCast.position.x, EnemyViewCast.position.y), Vector2.left, raycastLenght, raycastLayerMask);
        if (RaycastHit.collider != null) 
        { 
            canSpawnEnemy = false;
            Debug.DrawRay(new Vector2(EnemyViewCast.position.x, EnemyViewCast.position.y), Vector2.left * raycastLenght, Color.red);
        }
        else
        {
            canSpawnEnemy = true;
            Debug.DrawRay(new Vector2(EnemyViewCast.position.x, EnemyViewCast.position.y), Vector2.left * raycastLenght, Color.green);
        }
        
        if (canSpawnEnemy && enemyCounter <= howManyEnemiesSpawn)
        {
            Invoke("SpawnEnemy", spawnTime);
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(EnemyToSpawn[Random.Range(0, EnemyToSpawn.Length)], SpawnPosition.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Enemy"))
        {
            enemyCounter++;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Enemy"))
        {
            enemyCounter--;
        }
    }

}