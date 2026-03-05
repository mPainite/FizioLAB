using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCameraTransform;

    [Header("Takip Ayarlarý")]
    public Transform hedefObje; // Yazýnýn havada takip edeceði çubuk veya kumaþ
    public float yukseklik = 0.5f; // Objenin ne kadar tepesinde dursun?

    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // 1. YENÝ: Eðer bir hedefi varsa, her karede onun belirlediðimiz kadar üstüne ýþýnlan
        if (hedefObje != null)
        {
            transform.position = hedefObje.position + new Vector3(0, yukseklik, 0);
        }

        // 2. ESKÝ: Yönünü sürekli kameraya doðru çevir (Ayçiçeði taktiði)
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
                         mainCameraTransform.rotation * Vector3.up);
    }
}