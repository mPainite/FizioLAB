using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PendulumRod : MonoBehaviour
{
    [Header("Asýlma Ayarlarý")]
    public Transform hingeSnapPoint;
    public float rodHalfLength = 1f;

    [Header("Yük Ayarlarý")]
    public string myChargeType = "None";
    public float coulombForceMultiplier = 3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        AutoHang();
    }

    public void AutoHang()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = hingeSnapPoint.position + new Vector3(0, -1f, 0);
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearDamping = 2f;
        rb.angularDamping = 8f;

        HingeJoint newJoint = hingeSnapPoint.gameObject.AddComponent<HingeJoint>();
        newJoint.axis = new Vector3(0, 0, 1);
        newJoint.anchor = Vector3.zero;
        newJoint.autoConfigureConnectedAnchor = false;
        newJoint.connectedAnchor = new Vector3(0, 1f, 0);
        newJoint.connectedBody = rb;
    }

    public void ResetPendulum()
    {
        HingeJoint j = hingeSnapPoint.GetComponent<HingeJoint>();
        if (j != null) Destroy(j);
        rb.isKinematic = true;
        rb.useGravity = false;
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (GameManager.Instance == null) return;
        int step = GameManager.Instance.currentStep;
        if (step != 1 && step != 2) return;
        ApplyCoulombForce();
    }

    private void ApplyCoulombForce()
    {
        DraggableRod suspendedRod = DraggableRod.suspendedRodInScene;
        if (suspendedRod == null || !suspendedRod.isCharged) return;

        Vector3 direction = suspendedRod.transform.position - transform.position;
        float distance = direction.magnitude;
        if (distance < 0.1f || distance > 5f) return;

        float forceDirection;
        if (myChargeType == "None")
            forceDirection = 0.3f;
        else if (myChargeType == suspendedRod.myChargeType)
            forceDirection = -1f;
        else
            forceDirection = 1f;

        float forceMagnitude = coulombForceMultiplier / (distance * distance);
        Vector3 force = direction.normalized * forceDirection * forceMagnitude;
        rb.AddForce(force, ForceMode.Force);
    }
}