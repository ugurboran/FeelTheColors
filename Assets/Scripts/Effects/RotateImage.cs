// RotateImage.cs
using UnityEngine;

public class RotateImage : MonoBehaviour
{
    // Dönüş hızı (derece/saniye)
    public float rotationSpeed = 20f;

    // Saat yönünde mi tersi yönde mi
    public bool clockwise = true;

    // Her frame çalışır
    void Update()
    {
        // Dönüş miktarını hesapla
        float rotation = rotationSpeed * Time.deltaTime;

        // Eğer saat yönü tersiyse negatif yap
        if (!clockwise)
        {
            rotation = -rotation;
        }

        // Z ekseninde döndür
        transform.Rotate(0f, 0f, rotation);
    }
}