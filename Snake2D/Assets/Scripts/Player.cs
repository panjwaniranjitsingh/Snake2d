using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int boundX, boundY;
    [SerializeField] int moveSpeed=1;
    [SerializeField] Direction dir;
    [SerializeField] List<Transform> bodyParts;
    [SerializeField] Transform bodyPrefab;
    [SerializeField] bool isAlive;
    [SerializeField] int initialSize;
    // Start is called before the first frame update
    void Start()
    {
        SetupSnake();
    }

    private void SetupSnake()
    {
        initialSize = 3;
        isAlive = true;
        bodyParts = new List<Transform>();
        bodyParts.Add(transform);
        for (int i = 1; i < initialSize; i++)
            Grow();
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && dir!=Direction.Down)
            dir=Direction.Up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) && dir != Direction.Up)
            dir = Direction.Down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && dir != Direction.Right)
            dir = Direction.Left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && dir != Direction.Left)
            dir = Direction.Right;
    }

    void FixedUpdate()
    {
        if(isAlive)
        {
            MoveSnakeBodyParts();

            MoveSnakeHead(dir);
        }
    }

    private void MoveSnakeBodyParts()
    {
        for(int i=bodyParts.Count-1;i>0;i--)
        {
            bodyParts[i].position = bodyParts[i - 1].position;
            bodyParts[i].rotation = bodyParts[i - 1].rotation;
        }
    }

    private void MoveSnakeHead(Direction dir)
    {
        Vector3 position = transform.position;
        if (dir == Direction.Right)
        {
            MoveSnake(-90,moveSpeed,0);
        }
        if (dir == Direction.Left)
        {
            MoveSnake(90, -moveSpeed, 0);
        }
        if (dir == Direction.Up)
        {
            MoveSnake(0, 0, moveSpeed);
        }
        if (dir == Direction.Down)
        {
            MoveSnake(180, 0, -moveSpeed);
        }
    }

    public void MoveSnake(float angle,int xSpeed,int ySpeed)
    {
        Vector3 position = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        position.x += xSpeed;
        position.y += ySpeed;
        transform.position = position;
        if (xSpeed != 0)
        {
            if (transform.position.x >= boundX)
                position.x = -boundX;
            else if (transform.position.x <= -boundX)
                position.x = boundX;
            transform.position = position;
        }
        if (ySpeed != 0)
        {
            if (transform.position.y >= boundY)
                position.y = -boundY;
            else if (transform.position.y <= -boundY)
                position.y = boundY;
            transform.position = position;
        }
    }

    void Grow()
    {
        Transform snakeBody = Instantiate(bodyPrefab);
        snakeBody.parent = transform.parent;
        snakeBody.position = bodyParts[bodyParts.Count - 1].position;
        bodyParts.Add(snakeBody);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Food>()!=null)
        {
            Grow();
        }
        if(other.tag=="Obstacle")
        {
            StartCoroutine(SnakeDie());
        }
    }

    IEnumerator SnakeDie()
    {
        Debug.Log("Snake touched the body");
        isAlive = false;
        yield return new WaitForSeconds(3f);
        for (int i = bodyParts.Count - 1; i > 0; i--)
        {
            Destroy(bodyParts[i].gameObject);
        }
        bodyParts.Clear();
        SetupSnake();
    }
}

public enum Direction { Left,Right,Up,Down }