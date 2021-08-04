
using System;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] Food foodPrefab;
    [SerializeField] float boundX, boundY;
    // Start is called before the first frame update
    void Start()
    {
        SpawnFood();  
    }

    public void SpawnFood()
    {
        Vector3 position = new Vector3();
        position.x = Mathf.Round(UnityEngine.Random.Range(-boundX,boundX-1));
        position.y = Mathf.Round(UnityEngine.Random.Range(-boundY, boundY-1));
        position.z = 0;
        Food food = Instantiate(foodPrefab,position,Quaternion.identity);
        food.foodSpawner = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
