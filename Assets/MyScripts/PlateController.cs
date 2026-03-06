using UnityEngine;

public class PlateController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Çarpan objenin Tag'i "OilDrop" ise
        if (collision.gameObject.CompareTag("OilDrop"))
        {
            // Damlacýðý yok et
            Destroy(collision.gameObject);
            Debug.Log("Damlacýk levhaya çarptý ve temizlendi.");
        }
    }
}
