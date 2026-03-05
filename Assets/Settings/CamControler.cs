using UnityEngine;
using Unity.Cinemachine; // Yeni Cinemachine kütüphanesi
using UnityEngine.InputSystem; // Yeni Giriþ Sistemi kütüphanesi

public class CamControler : MonoBehaviour
{
    private CinemachineInputAxisController axisController;

    void Start()
    {
        // Kameradaki kontrolcü bileþenini buluyoruz
        axisController = GetComponent<CinemachineInputAxisController>();
    }

    void Update()
    {
        // Fare objesinin varlýðýný kontrol et ve sol týk basýlý mý bak
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            axisController.enabled = true; // Basýlýyken çalýþtýr
        }
        else
        {
            axisController.enabled = false; // Býrakýnca durdur
        }
    }
}
