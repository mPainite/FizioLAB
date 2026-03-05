using UnityEngine;
using TMPro;

public class DraggableCloth : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    private Vector3 startPos;

    [Header("Sürtünme Ayarlarý")]
    public int chargeLevel = 0;
    public int maxCharge = 5;
    public bool isCharged = false;

    [Header("Efekt Ayarlarý")]
    private Renderer rend;
    private Color originalColor;
    public Color sparkColor = Color.yellow;

    [Header("Arayüz Baðlantýsý")]
    public TextMeshProUGUI taskText;
    public TextMeshProUGUI progressText; // Ekranda yüzdeyi gösterecek yeni yazýmýz

    void Start()
    {
        startPos = transform.position;
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    void OnMouseDrag()
    {
        Vector3 newPos = GetMouseAsWorldPoint() + mOffset;
        newPos.y = 1.06f;
        transform.position = newPos;
    }

    void OnMouseUp()
    {
        transform.position = startPos;
        // Kumaþý býraktýðýmýzda ekrandaki yüzde yazýsý gizlensin
        if (progressText != null && !isCharged)
        {
            progressText.text = "";
        }
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    // Enter yerine tekrar Exit yaptýk. Ýleri-geri sürtme hareketini kusursuz algýlar.
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Kumaþ þu objeden ayrýldý: " + other.name);
        if (other.CompareTag("Rod") && !isCharged)
        {
            // Çarptýðýmýz çubuðun kimliðini (RodIdentity) alýyoruz
            RodIdentity hitRod = other.GetComponent<RodIdentity>();

            // Eðer kimliði varsa iþlemleri baþlat
            if (hitRod != null)
            {
                chargeLevel++;

                // Yüzde hesaplama (Örn: 1/5 = %20)
                int percentage = (chargeLevel * 100) / maxCharge;

                // Ekrana çubuðun ismini ve doluluk yüzdesini yazdýr
                progressText.text = hitRod.rodName + " Yükleniyor... %" + percentage;

                rend.material.color = sparkColor;
                Invoke("ResetColor", 0.15f);

                if (chargeLevel >= maxCharge)
                {
                    isCharged = true;
                    taskText.text = "Harika! " + hitRod.rodName + " yüklendi. Þimdi onu standa asabilirsin.";
                    taskText.color = Color.green;
                    progressText.text = hitRod.rodName + " Tamamen Yüklendi!";
                    progressText.color = Color.green;
                }
            }
        }
    }

    void ResetColor()
    {
        if (rend != null)
        {
            rend.material.color = originalColor;
        }
    }
}