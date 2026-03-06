using UnityEngine;
using TMPro; // TextMeshPro bileþenlerine eriþmek için ekledik

public class Nebulizer : MonoBehaviour
{
    [Header("Referanslar")]
    public GameObject oilDropPrefab;
    public Transform spawnPoint;
    public TextMeshProUGUI nebulizerStatusText; // Yeni eklenen UI referansý

    [Header("Püskürtme Ayarlarý")]
    public int dropCountPerClick = 5;
    public float spreadAmount = 0.05f;

    private bool hasSprayedBefore = false; // Rengin sadece bir kez deðiþmesi için

    private void OnMouseDown()
    {
        Spray();
    }

    void Spray()
    {
        if (oilDropPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Hata: OilDrop prefabý veya SpawnPoint atanmamýþ!");
            return;
        }

        // Püskürtme iþlemi
        for (int i = 0; i < dropCountPerClick; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * spreadAmount;
            Vector3 spawnPos = spawnPoint.position + randomOffset;
            GameObject newDrop = Instantiate(oilDropPrefab, spawnPos, Quaternion.identity);
            Destroy(newDrop, 15f);
        }

        // Yazý rengini güncelleme (Sadece ilk kullanýmda çalýþýr)
        if (!hasSprayedBefore && nebulizerStatusText != null)
        {
            nebulizerStatusText.color = Color.green;
            hasSprayedBefore = true;
            Debug.Log("Püskürtücü ilk kez kullanýldý, yazý yeþile döndü.");
        }

        Debug.Log("Yað damlalarý püskürtüldü!");
    }
}