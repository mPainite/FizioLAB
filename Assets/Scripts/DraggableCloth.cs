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

    public void ResetCloth()
    {
        chargeLevel = 0;
        isCharged = false;
        transform.position = startPos;
        if (progressText != null) progressText.text = "";
        if (rend != null) rend.material.color = originalColor;
    }

    void OnMouseDown()
    {
        if (!enabled) return;
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }


    void OnMouseDrag()
    {
        if (!enabled) return;
        Vector3 newPos = GetMouseAsWorldPoint() + mOffset;
        newPos.y = 1.06f;
        transform.position = newPos;
    }

    void OnMouseUp()
    {
        transform.position = startPos; // startPos doðru mu?
        transform.rotation = Quaternion.identity;
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
        if (!enabled) return;
        Debug.Log("Kumaþ þu objeden ayrýldý: " + other.name);
        if (other.CompareTag("Rod") && !isCharged)
        {
            RodIdentity hitRod = other.GetComponent<RodIdentity>();
            if (hitRod != null)
            {
                if (GameManager.Instance != null && GameManager.Instance.currentStep == 3)
                {
                    DraggableRod rod = other.GetComponent<DraggableRod>();
                    if (rod != null)
                    {
                        bool isWool = gameObject.name == "WoolCloth";
                        bool isSilk = gameObject.name == "SilkCloth";
                        if (isWool && rod.myChargeType == "Plastic") return;
                        if (isSilk && rod.myChargeType == "Glass") return;
                    }
                }

                chargeLevel++;
                int percentage = (chargeLevel * 100) / maxCharge;
                progressText.text = hitRod.rodName + " Yükleniyor... %" + percentage;
                rend.material.color = sparkColor;
                Invoke("ResetColor", 0.15f);

                if (chargeLevel >= maxCharge)
                {
                    isCharged = true;
                    DraggableRod hitDraggable = other.GetComponent<DraggableRod>();
                    if (hitDraggable != null)
                        hitDraggable.ChangeToChargedColor();

                    taskText.text = "Harika! " + hitRod.rodName + " yüklendi. Simdi onu standa asabilirsin.";
                    taskText.color = Color.green;
                    progressText.text = hitRod.rodName + " Tamamen Yuklendi!";
                    progressText.color = Color.green;
                }
            }
        }
    }
}