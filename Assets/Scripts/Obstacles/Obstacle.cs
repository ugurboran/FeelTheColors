// Obstacle.cs - COROUTİNE İPTALİ İLE
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Color lineColor;

    private SpriteRenderer spriteRenderer;
    private ObstaclePool pool;
    private bool isReturned = false; // Geri döndü mü? - YENİ! ✨

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = lineColor;
    }

    void OnEnable()
    {
        // Aktif olduğunda flag'i sıfırla - YENİ! ✨
        isReturned = false;
    }

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

            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        // Zaten döndüyse tekrar dönme - YENİ! ✨
        if (isReturned) return;

        isReturned = true; // İşaretle

        if (pool != null)
        {
            pool.ReturnObstacle(gameObject);
        }
        else
        {
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