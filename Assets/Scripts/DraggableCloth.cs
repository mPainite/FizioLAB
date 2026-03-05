using UnityEngine;
using TMPro;

public class DraggableCloth : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    [Header("Sürtünme Ayarlarý")]
    public int chargeLevel = 0;
    public int maxCharge = 5; // Kumaþýn çubuða 5 kere sürtünmesi yeterli olsun
    public bool isCharged = false;

    [Header("Arayüz Baðlantýsý")]
    public TextMeshProUGUI taskText;

    // Fare objeye týkladýðýnda çalýþýr
    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    // Fareyi sürüklediðimiz sürece çalýþýr
    void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + mOffset;
    }

    // Ekran koordinatlarýný 3D dünya koordinatlarýna çevirir
    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    // Kumaþ "Rod" etiketli çubuðun içinden geçtiðinde (sürtündüðünde) çalýþýr
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rod") && !isCharged)
        {
            chargeLevel++;
            Debug.Log(gameObject.name + " sürtündü! Güncel Yük: " + chargeLevel);

            if (chargeLevel >= maxCharge)
            {
                isCharged = true;
                taskText.text = "Harika! Çubuk yüklendi. Þimdi onu standa asabilirsin.";
                taskText.color = Color.green;
                // Ýlerleyen aþamalarda buraya ufak bir elektrik partikül efekti ekleyeceðiz.
            }
        }
    }
}