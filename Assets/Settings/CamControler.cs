using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CamControl : MonoBehaviour
{
    private CinemachineInputAxisController axisController;

    void Start()
    {
        // Kamerayý kontrol eden asýl bileþeni buluyoruz
        axisController = GetComponent<CinemachineInputAxisController>();
    }

    void Update()
    {
        // Eðer sistemde bir fare varsa ve kamera kontrolcüsü yerindeyse:
        if (Mouse.current != null && axisController != null)
        {
            // SADECE farenin SAÐ tuþuna (rightButton) basýlý tutulduðunda kamerayý aktifleþtir
            if (Mouse.current.rightButton.isPressed)
            {
                axisController.enabled = true;
            }
            else // Sað tuþ býrakýldýðý an (veya sol týka basýldýðýnda) kamerayý dondur
            {
                axisController.enabled = false;
            }
        }
    }
}