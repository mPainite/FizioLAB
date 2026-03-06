using UnityEngine;
using TMPro; // TextMeshPro bileþenlerine eriþmek için þart

public class ElectricSwitch : MonoBehaviour
{
    [Header("Durum")]
    public bool isPowered = true;

    [Header("Rotasyon Ayarlarý")]
    [SerializeField] private Quaternion rotationON;
    [SerializeField] private Quaternion rotationOFF;

    [Header("UI Ayarlarý")]
    [SerializeField] private TextMeshProUGUI statusText; // GameObject yerine TextMeshProUGUI kullanýyoruz

    private void Start()
    {
        // Baþlangýçta yazýnýn rengini ve rotasyonu ayarla
        UpdateStatus();
    }

    private void OnMouseDown()
    {
        TogglePower();
    }

    public void TogglePower()
    {
        isPowered = !isPowered;
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        // Anahtarýn rotasyonunu güncelle
        transform.localRotation = isPowered ? rotationON : rotationOFF;

        // UI Yazýsýnýn rengini deðiþtir
        if (statusText != null)
        {
            // Eðer güç açýksa Yeþil, kapalýysa Beyaz yap
            statusText.color = isPowered ? Color.green : Color.white;
            
            // Alternatif: Yazý içeriðini de deðiþtirmek istersen:
            // statusText.text = isPowered ? "Güç: AÇIK" : "Güç: KAPALI";
        }
    }
}