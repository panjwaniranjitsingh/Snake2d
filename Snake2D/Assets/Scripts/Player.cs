using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] SnakeHead snakeHeadPrefab;
    [SerializeField] Transform bodyPrefab;
    [SerializeField] BoxCollider2D gridArea;
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode down;
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] int initialSize;
    [SerializeField] float powerUpCoolDownPeriod;
    [SerializeField] Text ScoreText;
    [SerializeField] int score,pointsToAdd;
    [SerializeField] Direction dir,moveDir;
    [SerializeField] List<Transform> bodyParts;
    [SerializeField] bool isAlive;
    [SerializeField] bool wallsActive = false;
    public bool shieldOn = false;
    GameObject snakeHead;
    const int MOVESPEED = 1;
    // Start is called before the first frame update
    void Start()
    {
        SetupSnake();
    }

    private void SetupSnake()
    {
        score = 0;
        ScoreText.text =gameObject.name + " Score:" + score;
        pointsToAdd = 1;
        snakeHead= Instantiate(snakeHeadPrefab.gameObject);
        snakeHead.transform.parent = transform;
        snakeHead.transform.position = transform.position;
        isAlive = true;
        bodyParts = new List<Transform>();
        bodyParts.Add(snakeHead.transform);
        for (int i = 1; i < initialSize; i++)
            Grow();
    }

    public bool SnakeAtPos(Vector3 position)
    {
        for(int i=0;i<bodyParts.Count;i++)
        {
            if (bodyParts[i].position == position)
                return true;
        }
        return false;
    }

    public void SubScore()
    {
        score -= pointsToAdd;
        ScoreText.text = gameObject.name + " Score:" + score;
    }

    public void Reduce()
    {
        if (bodyParts.Count > 1)
        {
            Transform snakeBody = bodyParts[bodyParts.Count - 1];
            bodyParts.Remove(snakeBody);
            Destroy(snakeBody.gameObject);
        }
        else
            SnakeDie();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(up))
            dir=Direction.Up;
        else if (Input.GetKeyDown(down))
            dir = Direction.Down;
        else if (Input.GetKeyDown(left))
            dir = Direction.Left;
        else if (Input.GetKeyDown(right))
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
        Vector3 position = snakeHead.transform.position;
        if (dir == Direction.Right && moveDir != Direction.Left)
        {
            moveDir = dir;
            MoveSnakeInDir(-90, MOVESPEED, 0);
        }
        else if (dir == Direction.Left && moveDir != Direction.Right)
        {
            moveDir = dir;
            MoveSnakeInDir(90, -MOVESPEED, 0);
        }
        else if (dir == Direction.Up && moveDir != Direction.Down)
        {
            moveDir = dir;
            MoveSnakeInDir(0, 0, MOVESPEED);
        }
        else if (dir == Direction.Down && moveDir != Direction.Up)
        {
            moveDir = dir;
            MoveSnakeInDir(180, 0, -MOVESPEED);
        }
        else
            MoveSnakeHead(moveDir);
    }

    public void MoveSnakeInDir(float angle,int xSpeed,int ySpeed)
    {
        Vector3 position = snakeHead.transform.position;
        snakeHead.transform.rotation = Quaternion.Euler(0, 0, angle);
        position.x += xSpeed;
        position.y += ySpeed;
        snakeHead.transform.position = position;
        if (xSpeed != 0)
        {
            if (snakeHead.transform.position.x > gridArea.bounds.max.x)
            {
                if (wallsActive && !shieldOn)
                    SnakeDie();
                else
                    position.x = gridArea.bounds.min.x;
            }
            else if (snakeHead.transform.position.x < gridArea.bounds.min.x)
            {
                if (wallsActive && !shieldOn)
                    SnakeDie();
                else
                    position.x = gridArea.bounds.max.x;
            }
        }
        if (ySpeed != 0)
        {
            if (snakeHead.transform.position.y > gridArea.bounds.max.y)
            {
                if (wallsActive && !shieldOn)
                    SnakeDie();
                else
                    position.y = gridArea.bounds.min.y;
            }
            else if (snakeHead.transform.position.y < gridArea.bounds.min.y)
            {
                if (wallsActive && !shieldOn)
                    SnakeDie();
                else
                    position.y = gridArea.bounds.max.y;
            }
        }
        snakeHead.transform.position = position;
    }

    public void Grow()
    {
        Transform snakeBody = Instantiate(bodyPrefab);
        snakeBody.parent = transform;
        snakeBody.position = bodyParts[bodyParts.Count - 1].position;
        bodyParts.Add(snakeBody);
    }

    public void AddScore()
    {
        score+=pointsToAdd;
        ScoreText.text = gameObject.name + " Score:" + score;
    }

    public IEnumerator ShieldPowerUp()
    {
        shieldOn = true;
        yield return new WaitForSeconds(powerUpCoolDownPeriod);
        shieldOn = false;
    }

    public IEnumerator SpeedPowerUp()
    {
        Time.timeScale = 2;
        yield return new WaitForSeconds(powerUpCoolDownPeriod*2);
        Time.timeScale = 1;
    }

    public void SnakeDie()
    {
        if (shieldOn)
        {
            return;
        }
        else
            StartCoroutine(SnakeDied());
    }

    public IEnumerator SnakeDied()
    {
        Debug.Log("Snake Died");
        isAlive = false;
        yield return new WaitForSeconds(3f);
        for (int i = bodyParts.Count - 1; i >= 0; i--)
        {
            Destroy(bodyParts[i].gameObject);
        }
        bodyParts.Clear();
        SetupSnake();
    }

    public IEnumerator ScoreBoostPowerUp()
    {
        pointsToAdd *= 2;
        yield return new WaitForSeconds(powerUpCoolDownPeriod);
        pointsToAdd /= 2;
    }
}

public enum Direction { Left,Right,Up,Down }