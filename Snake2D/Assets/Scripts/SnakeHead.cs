
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    Player player;
    void Start()
    {
        player = transform.parent.GetComponent<Player>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Mass Gainer")
        {
            player.Grow();
            player.AddScore();
            Destroy(other.gameObject);
        }
        if (other.tag == "Obstacle")
        {
            player.SnakeDie();
        }
        if (other.tag == "PowerUp")
        {
            Destroy(other.gameObject);
            StartCoroutine(player.SpeedPowerUp());
        }
        if (other.tag == "Shield")
        {
            Destroy(other.gameObject);
            StartCoroutine(player.ShieldPowerUp());
        }
        if (other.tag == "ScoreBoost")
        {
            Destroy(other.gameObject);
            StartCoroutine(player.ScoreBoostPowerUp());
        }
        if (other.tag == "Mass Burner")
        {
            player.Reduce();
            player.SubScore();
            Destroy(other.gameObject);
        }
    }

}
