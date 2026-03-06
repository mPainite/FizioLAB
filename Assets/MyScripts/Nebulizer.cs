using UnityEngine;

public class Nebulizer : MonoBehaviour
{
    [Header("Referanslar")]
    [Tooltip("Assets klasöründeki OilDrop prefabýný buraya sürükle.")]
    public GameObject oilDropPrefab;

    [Tooltip("Hierarchy'deki OilSpawnPoint objesini buraya sürükle.")]
    public Transform spawnPoint;

    [Header("Püskürtme Ayarlarý")]
    public int dropCountPerClick = 5; // Her týklamada kaç damla oluþsun?
    public float spreadAmount = 0.05f; // Damlalarýn daðýlma alaný

    private void OnMouseDown()
    {
        Spray();
    }

    void Spray()
    {
        // Güvenlik kontrolü: Eðer atamalar yapýlmadýysa hata ver
        if (oilDropPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Hata: OilDrop prefabý veya SpawnPoint atanmamýþ!");
            return;
        }

        for (int i = 0; i < dropCountPerClick; i++)
        {
            // Damlalarýn üst üste binmemesi için küçük rastgele sapmalar
            Vector3 randomOffset = Random.insideUnitSphere * spreadAmount;
            Vector3 spawnPos = spawnPoint.position + randomOffset;

            // OilDrop prefabýný oluþtur
            GameObject newDrop = Instantiate(oilDropPrefab, spawnPos, Quaternion.identity);

            // Performans için damlayý 15 saniye sonra sahneden sil
            Destroy(newDrop, 15f);
        }

        Debug.Log("Yað damlalarý püskürtüldü!");
    }
}