using UnityEngine;

public class ElectricFieldZone : MonoBehaviour
{
    private ElectricSwitch powerSwitch;
    private Collider zoneCollider;

    void Start()
    {
        zoneCollider = GetComponent<Collider>();
        powerSwitch = FindObjectOfType<ElectricSwitch>();
        if (powerSwitch == null) Debug.LogError("Hata: Sahnede ElectricSwitch scripti bulunamadý!");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("OilDrop"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null && powerSwitch != null)
            {
                if (powerSwitch.isPowered)
                {
                    // Her damla için benzersiz bir hedef yükseklik hesapla (Alan sýnýrlarý içinde)
                    float minY = zoneCollider.bounds.min.y;
                    float maxY = zoneCollider.bounds.max.y;

                    // Objenin ID'sini kullanarak rastgele ama o damla için SABÝT bir hedef Y noktasý seçiyoruz
                    float seed = (float)other.gameObject.GetInstanceID() / 1000f;
                    float targetY = Mathf.Lerp(minY, maxY, Mathf.Abs(Mathf.Sin(seed)));

                    // Damla henüz hedef yüksekliðine ulaþmadýysa düþmeye devam etsin
                    if (other.transform.position.y > targetY)
                    {
                        // Hafif bir yavaþlatma (Süzülme hissi)
                        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.down * 0.1f, Time.deltaTime * 5f);
                        rb.useGravity = true;
                    }
                    else
                    {
                        // Hedefe ulaþtýðýnda sabitle
                        rb.useGravity = false;
                        rb.linearVelocity = Vector3.zero;

                        // Brownian Motion (Titreme) - Bilimsel gerçekçilik için
                        rb.position += Random.insideUnitSphere * 0.001f;
                    }
                }
                else
                {
                    rb.useGravity = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OilDrop"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) rb.useGravity = true;
        }
    }
}