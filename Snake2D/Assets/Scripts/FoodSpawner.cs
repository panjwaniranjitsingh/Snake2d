
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] GameObject foodPrefab;
    [SerializeField] GameObject powerUpPrefab;
    BoxCollider2D gridArea;
    [SerializeField] Player player;
    GameObject food;
    // Start is called before the first frame update
    void Start()
    {
        gridArea = GetComponent<BoxCollider2D>();
        food = Spawn(foodPrefab);
        GameObject powerUp = Spawn(powerUpPrefab);
    }

    void Update()
    {
        if(food==null)
            food = Spawn(foodPrefab);
    }

    public GameObject Spawn(GameObject prefab)
    {
        Vector3 position = Vector3.zero;
        do {
            Bounds bounds = gridArea.bounds;
            position.x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
            position.y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));
            position.z = 0;
        } while (player.SnakeAtPos(position));
        GameObject spawnedItem = Instantiate(prefab,position,Quaternion.identity);
        return spawnedItem;
    }
    
}
