
using UnityEngine;

public class Food : MonoBehaviour
{
    public FoodSpawner foodSpawner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.GetComponent<Player>() != null)
        {
            Destroy(gameObject);
            foodSpawner.SpawnFood();
        }
    }
}
