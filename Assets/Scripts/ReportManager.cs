using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReportManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject reportPanel;
    public Button toggleButton;
    public TextMeshProUGUI toggleButtonText;

    [Header("Adım 1 Raporu")]
    public TMP_InputField step1Input;
    public Button step1NextButton;

    private bool isPanelOpen = false;

    void Start()
    {
        reportPanel.SetActive(false);
        step1NextButton.interactable = false;
        step1Input.onValueChanged.AddListener(OnStep1InputChanged);
        toggleButton.onClick.AddListener(TogglePanel);
        step1NextButton.onClick.AddListener(GoToStep2);
    }

    void TogglePanel()
    {
        isPanelOpen = !isPanelOpen;
        reportPanel.SetActive(isPanelOpen);
        toggleButtonText.text = isPanelOpen ? "✕  Kapat" : "📋  Deney Raporu";
    }

    void OnStep1InputChanged(string value)
    {
        step1NextButton.interactable = value.Length >= 3;
    }

    void GoToStep2()
    {
        Debug.Log("Adım 2'ye geçiliyor!");
    }
}