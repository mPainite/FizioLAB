using UnityEngine;
using TMPro; // TextMeshPro kullanmak için gerekli

public class ElectricSwitch : MonoBehaviour
{
    [Header("Durum")]
    public bool isPowered = true;

    [Header("Rotasyon Ayarlarý")]
    [SerializeField] private Quaternion rotationON;
    [SerializeField] private Quaternion rotationOFF;

    [Header("UI Ayarlarý")]
    [SerializeField] private GameObject powerMessage; // Ekranda görünecek yazý objesi

    private void Start()
    {
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

        // UI Mesajýný aç veya kapat
        if (powerMessage != null)
        {
            powerMessage.SetActive(isPowered);
        }
    }
}