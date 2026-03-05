using UnityEngine;
using TMPro;

public class DraggableCloth : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    [Header("Sürtünme Ayarlarý")]
    public int chargeLevel = 0;
    public int maxCharge = 5;
    public bool isCharged = false;

    [Header("Arayüz Baðlantýsý")]
    public TextMeshProUGUI taskText;

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    void OnMouseDrag()
    {
        Vector3 newPos = GetMouseAsWorldPoint() + mOffset;

        // Kumaþýn masadan havalanmasýný engellemek için yüksekliði (Y) kilitliyoruz.
        // Kumaþlarýný Y: 1.06 yüksekliðine koymuþtuk, burada o deðeri sabitliyoruz.
        newPos.y = 1.06f;

        transform.position = newPos;
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

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
            }
        }
    }
}