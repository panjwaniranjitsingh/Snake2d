using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float boundX, boundY;
    [SerializeField] float moveSpeed;
    [SerializeField] Direction dir;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            dir=Direction.Up;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            dir = Direction.Down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            dir = Direction.Left;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            dir = Direction.Right;
        Move(dir);
    }

    private void Move(Direction dir)
    {
        Vector3 position = transform.position;
        if (dir == Direction.Right)
        {
            MoveSnake(90,moveSpeed,0);
        }
        if (dir == Direction.Left)
        {
            MoveSnake(-90, -moveSpeed, 0);
        }
        if (dir == Direction.Up)
        {
            MoveSnake(180, 0, moveSpeed);
        }
        if (dir == Direction.Down)
        {
            MoveSnake(0, 0, -moveSpeed);
        }
    }

    public void MoveSnake(float angle,float xSpeed,float ySpeed)
    {
        Vector3 position = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        position.x += Time.deltaTime * xSpeed;
        position.y += Time.deltaTime * ySpeed;
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
}

public enum Direction { Left,Right,Up,Down }