using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PendulumRod : MonoBehaviour
{
    [Header("Asýlma Ayarlarý")]
    public Transform hingeSnapPoint;     // 2. standýn Hinge_SnapPoint2'si
    public float rodHalfLength = 1f;     // Çubuðun yarý boyu

    [Header("Yük Ayarlarý")]
    public string myChargeType = "None"; // "None", "Glass", "Plastic"
    public float coulombForceMultiplier = 3f;

    private Rigidbody rb;
    private HingeJoint joint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        AutoHang();
    }

    private void AutoHang()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = hingeSnapPoint.position + new Vector3(0, -1f, 0);

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearDamping = 2f;
        rb.angularDamping = 8f; // Düþük yap, hareket etsin

        HingeJoint newJoint = hingeSnapPoint.gameObject.AddComponent<HingeJoint>();
        newJoint.axis = new Vector3(0, 0, 1); // Z ekseni — ileri geri
        newJoint.anchor = Vector3.zero;
        newJoint.autoConfigureConnectedAnchor = false;
        newJoint.connectedAnchor = new Vector3(0, 1f, 0);
        newJoint.connectedBody = rb;
    }

    void FixedUpdate()
    {
       
        ApplyCoulombForce();
    }

    private void ApplyCoulombForce()
    {
        DraggableRod suspendedRod = DraggableRod.suspendedRodInScene;
        if (suspendedRod == null) return;

        Vector3 direction = suspendedRod.transform.position - transform.position;
        float distance = direction.magnitude;

        if (distance < 0.1f || distance > 5f) return;

        float forceDirection;

        if (myChargeType == "None")
        {
            forceDirection = 0.3f; // yüksüz: hafif çekilir
        }
        else if (myChargeType == suspendedRod.myChargeType)
        {
            forceDirection = -1f; // ayný yük: iter
        }
        else
        {
            forceDirection = 1f; // zýt yük: çeker
        }

        float forceMagnitude = coulombForceMultiplier / (distance * distance);
        Vector3 force = direction.normalized * forceDirection * forceMagnitude;

        rb.AddForce(force, ForceMode.Force);
    }
}