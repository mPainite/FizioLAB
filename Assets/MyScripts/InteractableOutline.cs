using UnityEngine;

public class InteractableOutline : MonoBehaviour
{
    private Renderer rend;
    private Color originalEmission;
    private bool wasEmissionEnabled;

    [Header("Efekt Ayarlarý")]
    [ColorUsage(true, true)] // HDR renk seçiciyi açar (parlama için)
    public Color outlineColor = new Color(0f, 0.5f, 1f); // Parlak Mavi

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            // Malzemenin baþlangýçtaki emission durumunu kaydet
            originalEmission = rend.material.GetColor("_EmissionColor");
            wasEmissionEnabled = rend.material.IsKeywordEnabled("_EMISSION");
        }
    }

    private void OnMouseEnter()
    {
        if (rend != null)
        {
            // Emission'ý aktif et ve mavi yap (Parlat)
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", outlineColor);
        }
    }

    private void OnMouseExit()
    {
        if (rend != null)
        {
            // Baþlangýçtaki duruma geri dön
            rend.material.SetColor("_EmissionColor", originalEmission);
            if (!wasEmissionEnabled)
            {
                rend.material.DisableKeyword("_EMISSION");
            }
        }
    }
}