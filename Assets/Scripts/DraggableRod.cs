using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DraggableRod : MonoBehaviour
{
    public enum RodState { OnTable, Dragging, Suspended }
    public RodState currentState = RodState.OnTable;

    [Header("Asılma & Stand Ayarları")]
    public Transform hingeSnapPoint;
    public Transform hingeSnapPointStep3;
    public float snapThreshold = 0.5f;
    public float dragHeight = 1.5f;

    [Header("Coulomb (Elektrostatik) Ayarları")]
    public string myChargeType = "Glass";
    public static DraggableRod suspendedRodInScene;
    public static DraggableRod suspendedRodInScene2;
    public float coulombForceMultiplier = 5.0f;

    [Header("Efektler")]
    public Color chargedColor = Color.green;
    private Color originalColor;
    private Renderer rend;

    private Rigidbody rb;
    private Vector3 startPos;
    private Quaternion startRot;
    private Camera mainCamera;
    private float dragDepth;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null) originalColor = rend.material.color;
    }

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        startPos = transform.position;
        startRot = transform.rotation;
        ResetToTable();
        if (rend != null) rend.material.color = originalColor;
    }

    private Transform GetActiveSnapPoint()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentStep == 3)
            return hingeSnapPointStep3 != null ? hingeSnapPointStep3 : hingeSnapPoint;
        return hingeSnapPoint;
    }

    void OnMouseDown()
    {
        if (currentState == RodState.Suspended) return;
        currentState = RodState.Dragging;
        rb.isKinematic = true;
        rb.useGravity = false;
        dragDepth = mainCamera.WorldToScreenPoint(transform.position).z;
    }

    void OnMouseDrag()
    {
        if (currentState != RodState.Dragging) return;
        Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragDepth);
        Vector3 newWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        newWorldPos.y = dragHeight;
        transform.position = newWorldPos;
    }

    void OnMouseUp()
    {
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
                        GameManager.Instance.taskText.text = "Önce cam çubuğu yünlü kumaşla yükleyip asın!";
                        ResetToTable();
                        return;
                    }
                    if (subStep == 2 && myChargeType == "Glass")
                    {
                        GameManager.Instance.taskText.text = "Simdi plastik çubuğu ipek kumaşla yükleyip asın!";
                        ResetToTable();
                        return;
                    }
                    if (subStep == 1 && myChargeType == "Glass" && cloth.gameObject.name == "WoolCloth" && cloth.isCharged) correctCharged = true;
                    if (subStep == 2 && myChargeType == "Plastic" && cloth.gameObject.name == "SilkCloth" && cloth.isCharged) correctCharged = true;
                }
            }

            if (!correctCharged)
            {
                if (step == 3 && myChargeType == "Glass")
                    GameManager.Instance.taskText.text = "Cam çubuğu yünlü kumaşla yükleyin!";
                else if (step == 3 && myChargeType == "Plastic")
                    GameManager.Instance.taskText.text = "Plastik çubuğu ipek kumaşla yükleyin!";
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
        if (rend != null) rend.material.color = originalColor;

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

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

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

        HingeJoint newJoint = snapPoint.gameObject.AddComponent<HingeJoint>();
        newJoint.axis = new Vector3(0, 0, 1);
        newJoint.anchor = Vector3.zero;
        newJoint.autoConfigureConnectedAnchor = false;
        newJoint.connectedAnchor = new Vector3(0, 1f, 0);
        newJoint.connectedBody = rb;

        if (GameManager.Instance != null && GameManager.Instance.currentStep == 3)
        {
            rb.linearDamping = 2;
            rb.angularDamping = 5;
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

        Debug.Log(gameObject.name + " standa asıldı!");

        if (GameManager.Instance != null && GameManager.Instance.currentStep == 3)
        {
            if (myChargeType == "Glass" && GameManager.Instance.step3SubStep == 1)
            {
                GameManager.Instance.step3SubStep = 2;
                GameManager.Instance.taskText.text = "Simdi plastik cubugu ipek kumasla yukleyip asin!";
                GameManager.Instance.UpdateStep3Access();
            }
            else if (myChargeType == "Plastic" && GameManager.Instance.step3SubStep == 2)
            {
                GameManager.Instance.step3SubStep = 3;
                GameManager.Instance.taskText.text = "Ikisinin etkilesimini gozlemleyin!";
                GameManager.Instance.UpdateStep3Access();
            }
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance == null || GameManager.Instance.currentStep != 3) return;
        if (currentState != RodState.Suspended) return;
        if (suspendedRodInScene == null || suspendedRodInScene2 == null) return;
        ApplyCoulombForce();
    }

    private void ApplyCoulombForce()
    {
        DraggableRod otherRod = (suspendedRodInScene == this) ? suspendedRodInScene2 : suspendedRodInScene;
        if (otherRod == null) return;

        Rigidbody myRb = GetComponent<Rigidbody>();
        if (myRb == null) return;

        Vector3 direction = (otherRod.transform.position - transform.position);
        float distance = direction.magnitude;

        if (distance < 0.1f || distance > 5.0f) return;

        Vector3 normalizedDirection = direction.normalized;
        float forceDirection = (myChargeType == otherRod.myChargeType) ? -1f : 1f;
        float forceMagnitude = coulombForceMultiplier / (distance * distance);
        Vector3 forceToApply = normalizedDirection * forceDirection * forceMagnitude;

        myRb.AddForce(forceToApply, ForceMode.Force);
    }

    public void ChangeToChargedColor()
    {
        if (rend != null) rend.material.color = chargedColor;
    }

    public Color GetOriginalColor()
    {
        return originalColor;
    }
}