// MenuManager.cs
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Oyunu başlat - Play butonu çağırır
    public void PlayGame()
    {
        // GameScene sahnesini yükle
        SceneManager.LoadScene("GameScene");
    }
    
    // Ayarlar menüsünü aç (şimdilik boş)
    public void OpenSettings()
    {
        Debug.Log("Ayarlar açılacak");
        // TODO: Ayarlar paneli oluşturulacak
    }
    
    // Oyundan çık (Android'de kullanılmaz)
    public void QuitGame()
    {
        Debug.Log("Oyun kapatılıyor");
        Application.Quit();
    }
}