using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class DraggableRod : MonoBehaviour
{
    public enum RodState { OnTable, Dragging, Suspended }
    public RodState currentState = RodState.OnTable;

    [Header("Asılma & Stand Ayarları")]
    public Transform hingeSnapPoint;
    public Transform hingeSnapPointStep3;
    public Transform hingeSnapPointStep4;
    public float snapThreshold = 0.5f;
    public float dragHeight = 1.5f;

    [Header("Coulomb (Elektrostatik) Ayarları")]
    public string myChargeType = "Glass";
    public static DraggableRod suspendedRodInScene;
    public static DraggableRod suspendedRodInScene2;
    public float coulombForceMultiplier = 5.0f;

    [Header("Efektler")]
    public Color chargedColor = Color.green;
    public Color forcedOriginalColor = Color.clear;
    private Color originalColor;
    private Renderer rend;

    [Header("Yük Durumu")]
    public bool isCharged = false;

    [Header("Yük Göstergesi")]
    public Vector3 chargeIndicatorOffset = new Vector3(0, 0.5f, 0);

    private Rigidbody rb;
    private Vector3 startPos;
    private Quaternion startRot;
    private Camera mainCamera;
    private float dragDepth;
    private GameObject chargeIndicator;
    private TextMeshPro chargeText;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        startPos = transform.position;
        startRot = transform.rotation;
        if (rend != null)
        {
            originalColor = rend.material.color;
            if (originalColor.a < 0.1f)
            {
                if (forcedOriginalColor != Color.clear)
                    originalColor = forcedOriginalColor;
            }
        }
        CreateChargeIndicator();
        Debug.Log(gameObject.name + " ChargeIndicator oluşturuldu: " + (chargeIndicator != null));
    }

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        ResetToTable();
        if (rend != null) rend.material.color = originalColor;
    }

    private void CreateChargeIndicator()
    {
        chargeIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        chargeIndicator.transform.SetParent(transform);
        chargeIndicator.transform.localPosition = chargeIndicatorOffset;
        chargeIndicator.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

        // Collider'ı kaldır
        Collider col = chargeIndicator.GetComponent<Collider>();
        if (col != null) Destroy(col);

        chargeIndicator.SetActive(false);
    }

    void LateUpdate()
    {
        if (chargeIndicator != null && chargeIndicator.activeSelf && mainCamera != null)
        {
            chargeIndicator.transform.rotation = Quaternion.LookRotation(
                chargeIndicator.transform.position - mainCamera.transform.position
            );
        }
    }

    private void ShowChargeIndicator()
    {
        if (chargeIndicator == null) return;
        chargeIndicator.SetActive(true);
        Renderer r = chargeIndicator.GetComponent<Renderer>();
        if (r != null)
        {
            if (myChargeType == "Glass")
                r.material.color = new Color(1f, 0.4f, 0.1f); // turuncu = pozitif
            else
                r.material.color = new Color(0.2f, 0.4f, 1f); // mavi = negatif
        }
    }

    private void HideChargeIndicator()
    {
        if (chargeIndicator != null)
            chargeIndicator.SetActive(false);
    }

    private Transform GetActiveSnapPoint()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentStep == 4)
            return hingeSnapPointStep4 != null ? hingeSnapPointStep4 : hingeSnapPoint;
        if (GameManager.Instance != null && GameManager.Instance.currentStep == 3)
            return hingeSnapPointStep3 != null ? hingeSnapPointStep3 : hingeSnapPoint;
        return hingeSnapPoint;
    }

    void OnMouseDown()
    {
        if (!enabled)
        {
            if (ObjectInfoManager.Instance != null)
                ObjectInfoManager.Instance.ShowRodInfo(this);
            return;
        }

        // Adım bazlı kilit
        if (GameManager.Instance != null)
        {
            int step = GameManager.Instance.currentStep;
            int sub = GameManager.Instance.step3SubStep;
            if (step == 2 && myChargeType == "Glass") return;
            if (step == 3 && sub == 1 && myChargeType == "Plastic") return;
            if (step == 3 && sub == 2 && myChargeType == "Glass") return;
            if (step == 4 && sub == 1 && gameObject.name == "GlassRod2_Drag") return;
            if (step == 4 && sub == 2 && gameObject.name == "GlassRod_Drag") return;
        }

        if (currentState == RodState.Suspended)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if (ObjectInfoManager.Instance != null)
                ObjectInfoManager.Instance.ShowRodInfo(this);
            return;
        }
        currentState = RodState.Dragging;
        rb.isKinematic = true;
        rb.useGravity = false;
        dragDepth = mainCamera.WorldToScreenPoint(transform.position).z;
    }

    void OnMouseDrag()
    {
        if (!enabled) return;
        if (currentState != RodState.Dragging) return;
        Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragDepth);
        Vector3 newWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        newWorldPos.y = dragHeight;
        transform.position = newWorldPos;
    }

    void OnMouseUp()
    {
        if (!enabled) return;
        if (currentState != RodState.Dragging) return;
        if (GameManager.Instance != null)
        {
            int step = GameManager.Instance.currentStep;
            DraggableCloth[] cloths = FindObjectsByType<DraggableCloth>(FindObjectsSortMode.None);
            bool correctCharged = false;

            foreach (var cloth in cloths)
            {
                if (step == 1 && cloth.gameObject.name == "SilkCloth" && cloth.isCharged) correctCharged = true;
                if (step == 2 && cloth.gameObject.name == "WoolCloth" && cloth.isCharged) correctCharged = true;

                if (step == 3)
                {
                    int subStep = GameManager.Instance.step3SubStep;
                    if (subStep == 1 && myChargeType == "Plastic")
                    {
                        GameManager.Instance.taskText.text = "Önce cam çubuğu ipek kumaşla yükleyip asın!";
                        ResetToTable();
                        return;
                    }
                    if (subStep == 2 && myChargeType == "Glass")
                    {
                        GameManager.Instance.taskText.text = "Simdi plastik çubuğu yünlü kumaşla yükleyip asın!";
                        ResetToTable();
                        return;
                    }
                    if (subStep == 1 && myChargeType == "Glass" && cloth.gameObject.name == "SilkCloth" && cloth.isCharged) correctCharged = true;
                    if (subStep == 2 && myChargeType == "Plastic" && cloth.gameObject.name == "WoolCloth" && cloth.isCharged) correctCharged = true;
                }

                if (step == 4)
                {
                    int subStep = GameManager.Instance.step3SubStep;
                    if (subStep == 1 && gameObject.name == "GlassRod2_Drag")
                    {
                        GameManager.Instance.taskText.text = "Önce birinci cam çubuğu ipek kumaşla yükleyip asın!";
                        ResetToTable();
                        return;
                    }
                    if (subStep == 2 && gameObject.name == "GlassRod_Drag")
                    {
                        GameManager.Instance.taskText.text = "Simdi ikinci cam çubuğu ipek kumaşla yükleyip asın!";
                        ResetToTable();
                        return;
                    }
                    if (subStep == 1 && gameObject.name == "GlassRod_Drag" && cloth.gameObject.name == "SilkCloth" && cloth.isCharged) correctCharged = true;
                    if (subStep == 2 && gameObject.name == "GlassRod2_Drag" && cloth.gameObject.name == "SilkCloth" && cloth.isCharged) correctCharged = true;
                }
            }

            if (!correctCharged)
            {
                if (step == 3 && myChargeType == "Glass")
                    GameManager.Instance.taskText.text = "Cam çubuğu ipek kumaşla yükleyin!";
                else if (step == 3 && myChargeType == "Plastic")
                    GameManager.Instance.taskText.text = "Plastik çubuğu yünlü kumaşla yükleyin!";
                else if (step == 4)
                    GameManager.Instance.taskText.text = "Cam çubuğu ipek kumaşla yükleyin!";
                else
                    GameManager.Instance.taskText.text = "Once cubuğu dogru kumasla yukleyin!";
                ResetToTable();
                return;
            }
        }

        Transform snapPoint = GetActiveSnapPoint();
        if (snapPoint != null)
        {
            Vector2 rodFlatPos = new Vector2(transform.position.x, transform.position.z);
            Vector2 hingeFlatPos = new Vector2(snapPoint.position.x, snapPoint.position.z);
            float distance = Vector2.Distance(rodFlatPos, hingeFlatPos);
            if (distance <= snapThreshold)
            {
                SnapToHinge(snapPoint);
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
        isCharged = false;
        HideChargeIndicator();
        if (rend != null) rend.material.color = originalColor;

        if (GameManager.Instance != null)
        {
            int s = GameManager.Instance.currentStep;
            int sub = GameManager.Instance.step3SubStep;
            if (s == 3 || s == 4)
            {
                if (sub == 1 && GameManager.Instance.silkCloth != null)
                    GameManager.Instance.silkCloth.ResetCloth();
                else if (sub == 2 && GameManager.Instance.woolCloth != null && s == 3)
                    GameManager.Instance.woolCloth.ResetCloth();
                else if (sub == 2 && GameManager.Instance.silkCloth != null && s == 4)
                    GameManager.Instance.silkCloth.ResetCloth();
            }
        }

        if (hingeSnapPoint != null)
        {
            HingeJoint joint = hingeSnapPoint.GetComponent<HingeJoint>();
            if (joint != null) Destroy(joint);
        }
        if (hingeSnapPointStep3 != null)
        {
            HingeJoint joint = hingeSnapPointStep3.GetComponent<HingeJoint>();
            if (joint != null) Destroy(joint);
        }
        if (hingeSnapPointStep4 != null)
        {
            HingeJoint joint = hingeSnapPointStep4.GetComponent<HingeJoint>();
            if (joint != null) Destroy(joint);
        }

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        transform.position = startPos;
        transform.rotation = startRot;

        if (suspendedRodInScene == this) suspendedRodInScene = null;
        if (suspendedRodInScene2 == this) suspendedRodInScene2 = null;
    }

    private void SnapToHinge(Transform snapPoint)
    {
        currentState = RodState.Suspended;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = snapPoint.position;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        HingeJoint oldJoint = snapPoint.GetComponent<HingeJoint>();
        if (oldJoint != null) Destroy(oldJoint);
        if (GuideManager.Instance != null)
        {
            int s = GameManager.Instance.currentStep;
            if (s == 1 || s == 2)
                GuideManager.Instance.CompleteTask(s, 2); // standa as
        }
        if (GuideManager.Instance != null)
        {
            int s = GameManager.Instance.currentStep;
            if (s == 1 || s == 2)
                GuideManager.Instance.CompleteTask(s, 2); // standa as tiki
        }

        HingeJoint newJoint = snapPoint.gameObject.AddComponent<HingeJoint>();
        newJoint.axis = new Vector3(0, 0, 1);
        newJoint.anchor = Vector3.zero;
        newJoint.autoConfigureConnectedAnchor = false;
        newJoint.connectedAnchor = new Vector3(0, 1f, 0);
        newJoint.connectedBody = rb;

        if (GameManager.Instance != null && (GameManager.Instance.currentStep == 3 || GameManager.Instance.currentStep == 4))
        {
            rb.linearDamping = 5;
            rb.angularDamping = 20;
            JointLimits limits = new JointLimits();
            limits.min = -45f;
            limits.max = 45f;
            newJoint.limits = limits;
            newJoint.useLimits = true;
        }
        else
        {
            rb.linearDamping = 20;
            rb.angularDamping = 500;
            JointLimits limits = new JointLimits();
            limits.min = -3f;
            limits.max = 3f;
            newJoint.limits = limits;
            newJoint.useLimits = true;
        }

        if (suspendedRodInScene == null || suspendedRodInScene == this)
            suspendedRodInScene = this;
        else
            suspendedRodInScene2 = this;

        if (GameManager.Instance != null && (GameManager.Instance.currentStep == 3 || GameManager.Instance.currentStep == 4))
        {
            int curStep = GameManager.Instance.currentStep;
            if (GameManager.Instance.step3SubStep == 1)
            {
                GameManager.Instance.step3SubStep = 2;
                GameManager.Instance.taskText.text = curStep == 3
                    ? "Simdi plastik cubugu yünlü kumasla yukleyip asin!"
                    : "Simdi ikinci cam cubugu ipek kumasla yukleyip asin!";
                if (GameManager.Instance.silkCloth != null)
                    GameManager.Instance.silkCloth.ResetCloth();
                if (GameManager.Instance.woolCloth != null)
                    GameManager.Instance.woolCloth.ResetCloth();
                GameManager.Instance.UpdateStep3Access();
            }
            else if (GameManager.Instance.step3SubStep == 2)
            {
                GameManager.Instance.step3SubStep = 3;
                GameManager.Instance.taskText.text = "Ikisinin etkilesimini gozlemleyin!";
                GameManager.Instance.UpdateStep3Access();
            }
        }

        if (GameManager.Instance.currentStep == 1 || GameManager.Instance.currentStep == 2)
            GameManager.Instance.taskText.text = "Çubuk standa asıldı! Yükünü incelemek için üzerine tıklayın.";

        if (GameManager.Instance != null && GuideManager.Instance != null)
            GuideManager.Instance.CompleteTask(GameManager.Instance.currentStep, 2);
    }

    void FixedUpdate()
    {
        if (GameManager.Instance == null) return;
        int step = GameManager.Instance.currentStep;
        if (step != 3 && step != 4) return;
        if (currentState != RodState.Suspended) return;
        if (suspendedRodInScene == null || suspendedRodInScene2 == null) return;
        if (suspendedRodInScene.currentState != RodState.Suspended) return;
        if (suspendedRodInScene2.currentState != RodState.Suspended) return;
        ApplyCoulombForce();
    }

    private void ApplyCoulombForce()
    {
        if (!isCharged) return;
        DraggableRod otherRod = (suspendedRodInScene == this) ? suspendedRodInScene2 : suspendedRodInScene;
        if (otherRod == null) return;
        if (!otherRod.isCharged) return;
        Vector3 direction = (otherRod.transform.position - transform.position);
        float distance = direction.magnitude;
        if (distance < 0.6f || distance > 5.0f) return;
        Vector3 normalizedDirection = direction.normalized;
        float forceDirection = (myChargeType == otherRod.myChargeType) ? -1f : 1f;
        float forceMagnitude = coulombForceMultiplier / (distance * distance);
        Vector3 forceToApply = normalizedDirection * forceDirection * forceMagnitude;
        rb.AddForce(forceToApply, ForceMode.Force);
    }

    public void ChangeToChargedColor()
    {
        isCharged = true;
        if (rend != null) rend.material.color = chargedColor;
        ShowChargeIndicator();
    }

    public Color GetOriginalColor()
    {
        return originalColor;
    }
}