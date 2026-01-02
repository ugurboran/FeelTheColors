// Obstacle.cs - GÜNCELLENMİŞ
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Bu engelin rengi (Spawner tarafından ayarlanacak)
    public Color lineColor;

    // Engelin rengini değiştirmek için sprite renderer
    private SpriteRenderer spriteRenderer;

    // Engel oluşturulduğunda bir kez çalışır
    void Start()
    {
        // Bu objenin sprite renderer bileşenini al
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Engelin rengini ayarlanmış renge çevir
        spriteRenderer.color = lineColor;
    }

    // Bir şey bu engele trigger olarak temas edince çalışır
    void OnTriggerEnter2D(Collider2D other)
    {
        // Temas eden obje "Player" tag'ine sahip mi kontrol et
        if (other.CompareTag("Player"))
        {
            // Player objesinden BallController script'ini al
            BallController ball = other.GetComponent<BallController>();

            // Topun şu anki rengi bu engelin rengiyle aynı mı?
            if (ball.GetCurrentColor() != lineColor)
            {
                // RENKLER UYUŞMUYOR - Oyun bitti
                GameManager.Instance.GameOver();
            }
            else
            {
                // RENKLER UYUŞUYOR - Puan kazan
                GameManager.Instance.AddScore();

                // Puan efektini çalıştır (Player'daki particle)
                PlayScoreEffect(other.gameObject);
            }
        }
    }

    // Puan kazanma efektini çalıştır
    void PlayScoreEffect(GameObject player)
    {
        // Player'daki ScoreParticles'ı bul ve çalıştır
        ParticleSystem scoreParticles = player.transform.Find("ScoreParticles")?.GetComponent<ParticleSystem>();

        if (scoreParticles != null)
        {
            scoreParticles.Play();
        }
    }
}