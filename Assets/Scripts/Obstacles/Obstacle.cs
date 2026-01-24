// Obstacle.cs - POOLING DESTEĞİ İLE
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Obstacle : MonoBehaviour
{
    public Color lineColor;

    private SpriteRenderer spriteRenderer;
    private ObstaclePool pool; // Pool referansı

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = lineColor;
    }

    // Pool referansını ayarla - YENİ! ✨
    public void SetPool(ObstaclePool poolReference)
    {
        pool = poolReference;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BallController ball = other.GetComponent<BallController>();

            if (ball.GetCurrentColor() != lineColor)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                GameManager.Instance.AddScore();

                FaceController face = other.GetComponentInChildren<FaceController>();
                if (face != null)
                {
                    face.ShowHappy();
                }

                PlayScoreEffect(other.gameObject);
            }

            // Collision sonrası havuza geri dön - YENİ! ✨
            ReturnToPool();
        }
    }

    // Havuza geri dön - YENİ! ✨
    void ReturnToPool()
    {
        if (pool != null)
        {
            pool.ReturnObstacle(gameObject);
        }
        else
        {
            // Pool yoksa normal destroy (güvenlik)
            Destroy(gameObject);
        }
    }

    void PlayScoreEffect(GameObject player)
    {
        ParticleSystem scoreParticles = player.transform.Find("ScoreParticles")?.GetComponent<ParticleSystem>();

        if (scoreParticles != null)
        {
            scoreParticles.Play();
        }
    }
}