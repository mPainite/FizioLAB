using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DraggableRod : MonoBehaviour
{
    // Çubuğun o anki durumunu kesin olarak belirliyoruz (State Machine)
    public enum RodState { OnTable, Dragging, Suspended }
    public RodState currentState = RodState.OnTable;

    [Header("Asılma & Stand Ayarları")]
    [Tooltip("Hinge Joint'in bulunduğu havada asılı olan obje")]
    public Transform hingeSnapPoint;
    public float snapThreshold = 0.5f;
    [Tooltip("Çubuğun masaya sürtünmeden havada sürükleneceği yükseklik")]
    public float dragHeight = 1.5f;

    [Header("Coulomb (Elektrostatik) Ayarları")]
    [Tooltip("Çubuğun yük türü: Glass veya Plastic yazın")]
    public string myChargeType = "Glass";
    public static DraggableRod suspendedRodInScene; // Asılı olan çubuğun hafızası
    public float coulombForceMultiplier = 5.0f; // İtme/Çekme gücü

    [Header("Efektler")]
    public Color chargedColor = Color.green;
    private Color originalColor;
    private Renderer rend;

    // Arka plan değişkenleri
    private Rigidbody rb;
    private Vector3 startPos;
    private Quaternion startRot;
    private Camera mainCamera;
    private float dragDepth;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        if (rend != null) originalColor = rend.material.color;

        // Çubuğun masadaki ilk halini hafızaya alıyoruz (Yamuk dönmesini engeller)
        startPos = transform.position;
        startRot = transform.rotation;

        // Oyun başladığında çubuğu masaya kesin olarak sabitler
        ResetToTable();
    }

    void OnMouseDown()
    {
        // Çubuk asılıysa tıklamayı reddet
        if (currentState == RodState.Suspended) return;

        currentState = RodState.Dragging;

        // Sürüklerken fizik motoru karışmasın diye donduruyoruz
        rb.isKinematic = true;
        rb.useGravity = false;

        // Farenin 3D dünyadaki derinliğini hesaplıyoruz
        dragDepth = mainCamera.WorldToScreenPoint(transform.position).z;
    }

    void OnMouseDrag()
    {
        if (currentState != RodState.Dragging) return;

        // Kusursuz sürükleme matematiği
        Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragDepth);
        Vector3 newWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        // Çubuğu masanın üstünde belirlediğimiz yüksekliğe kilitliyoruz
        newWorldPos.y = dragHeight;

        transform.position = newWorldPos;
    }

    void OnMouseUp()
    {
        if (currentState != RodState.Dragging) return;

        // Yüklü mü kontrol et
        DraggableCloth[] cloths = FindObjectsByType<DraggableCloth>(FindObjectsSortMode.None);
        bool isCharged = false;
        foreach (var cloth in cloths)
        {
            if (cloth.isCharged) isCharged = true;
        }

        if (!isCharged)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.taskText.text = "⚠️ Önce çubuğu kumaşla yükleyin!";
            ResetToTable();
            return;
        }

        if (hingeSnapPoint != null)
        {
            Vector2 rodFlatPos = new Vector2(transform.position.x, transform.position.z);
            Vector2 hingeFlatPos = new Vector2(hingeSnapPoint.position.x, hingeSnapPoint.position.z);
            float distance = Vector2.Distance(rodFlatPos, hingeFlatPos);

            if (distance <= snapThreshold)
            {
                SnapToHinge();
                return;
            }
        }

        ResetToTable();
    }

    public void ResetToTablePublic()
    {
        ResetToTable();
    }

    private void ResetToTable()
    {
        currentState = RodState.OnTable;

        transform.position = startPos;
        transform.rotation = startRot;

        // Önce isKinematic=true yap, SONRA joint'i kopar
        rb.isKinematic = true;
        rb.useGravity = false;
        // isKinematic=true iken velocity set etme, gerek yok zaten

        if (hingeSnapPoint != null)
        {
            HingeJoint joint = hingeSnapPoint.GetComponent<HingeJoint>();
            if (joint != null && joint.connectedBody == rb)
            {
                joint.connectedBody = null;
            }
        }

        if (suspendedRodInScene == this) suspendedRodInScene = null;
    }

    private void SnapToHinge()
    {
        currentState = RodState.Suspended;

        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = hingeSnapPoint.position;

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearDamping = 10f;
        rb.angularDamping = 200f;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        HingeJoint oldJoint = hingeSnapPoint.GetComponent<HingeJoint>();
        if (oldJoint != null) Destroy(oldJoint);

        HingeJoint newJoint = hingeSnapPoint.gameObject.AddComponent<HingeJoint>();
        newJoint.axis = new Vector3(0, 0, 1);
        newJoint.anchor = Vector3.zero;
        newJoint.autoConfigureConnectedAnchor = false;
        newJoint.connectedAnchor = new Vector3(0, 1f, 0);
        newJoint.connectedBody = rb;

        JointLimits limits = new JointLimits();
        limits.min = -3f;
        limits.max = 3f;
        newJoint.limits = limits;
        newJoint.useLimits = true;

        suspendedRodInScene = this;
        Debug.Log(gameObject.name + " standa asıldı!");
    }
    // Fizik hesaplamaları (Coulomb) Update'de değil, FixedUpdate'de yapılmalıdır.
    void FixedUpdate()
    {
      
    }

    private void ApplyCoulombForce()
    {
        Rigidbody suspendedRb = suspendedRodInScene.GetComponent<Rigidbody>();
        if (suspendedRb == null) return;

        // Masadaki çubuktan, asılı çubuğa doğru olan yönü bul
        Vector3 directionTowardsTableRod = (transform.position - suspendedRodInScene.transform.position);
        float distance = directionTowardsTableRod.magnitude;

        if (distance < 0.1f || distance > 5.0f) return; // Çok uzaksa veya çok yakınsa işlem yapma

        Vector3 normalizedDirection = directionTowardsTableRod.normalized;

        // Aynı yükler iter (-1), zıt yükler çeker (1)
        float forceDirection = (myChargeType == suspendedRodInScene.myChargeType) ? -1f : 1f;

        // Gerçekçi fizik formülü: Kuvvet = Çarpan / (Mesafe * Mesafe)
        float forceMagnitude = coulombForceMultiplier / (distance * distance);

        Vector3 forceToApply = normalizedDirection * forceDirection * forceMagnitude;

        // Kuvveti ASILI OLAN çubuğa uygula ki bize doğru gelsin veya bizden kaçsın
        suspendedRb.AddForce(forceToApply, ForceMode.Force);
    }

    public void ChangeToChargedColor()
    {
        if (rend != null) rend.material.color = chargedColor;
    }
}