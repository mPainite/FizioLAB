using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReportManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject reportPanel;
    public Button toggleButton;
    public TextMeshProUGUI toggleButtonText;

    [Header("Rapor Butonları")]
    public TMP_InputField step1Input;
    public TMP_InputField[] allInputFields;
    public Button step1NextButton;
    public Button step2NextButton;
    public Button step3NextButton;

    private bool isPanelOpen = false;

    void Start()
    {
        reportPanel.SetActive(false);
        step1NextButton.interactable = false;
        step1Input.onValueChanged.AddListener(OnStep1InputChanged);
        toggleButton.onClick.AddListener(TogglePanel);
        step1NextButton.onClick.AddListener(GoToStep2);

        if (step2NextButton != null)
        {
            step2NextButton.interactable = false;
            step2NextButton.gameObject.SetActive(false);
            step2NextButton.onClick.AddListener(GoToStep3);
        }

        if (step3NextButton != null)
        {
            step3NextButton.interactable = false;
            step3NextButton.gameObject.SetActive(false);
            step3NextButton.onClick.AddListener(GoToStep4);
        }

        if (allInputFields != null && allInputFields.Length >= 2)
            allInputFields[1].onValueChanged.AddListener(OnStep2InputChanged);

        if (allInputFields != null && allInputFields.Length >= 3)
            allInputFields[2].onValueChanged.AddListener(OnStep3InputChanged);
    }

    void TogglePanel()
    {
        isPanelOpen = !isPanelOpen;
        reportPanel.SetActive(isPanelOpen);
        toggleButtonText.text = isPanelOpen ? "Kapat" : "Deney Raporu";
    }

    void OnStep1InputChanged(string value)
    {
        step1NextButton.interactable = value.Length >= 3;
    }

    void OnStep2InputChanged(string value)
    {
        if (step2NextButton != null)
            step2NextButton.interactable = value.Length >= 3;
    }

    void OnStep3InputChanged(string value)
    {
        if (step3NextButton != null)
            step3NextButton.interactable = value.Length >= 3;
    }

    void GoToStep2()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetStep(2);
            GameManager.Instance.CompleteStep(1);
        }
        if (allInputFields != null && allInputFields.Length >= 2)
        {
            allInputFields[0].interactable = false;
            allInputFields[1].interactable = true;
        }
        if (step1NextButton != null) step1NextButton.gameObject.SetActive(false);
        if (step2NextButton != null) step2NextButton.gameObject.SetActive(true);
        isPanelOpen = false;
        reportPanel.SetActive(false);
        toggleButtonText.text = "Deney Raporu";
        Debug.Log("Adım 2'ye geçildi!");
    }

    void GoToStep3()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetStep(3);
            GameManager.Instance.CompleteStep(2);
        }
        if (allInputFields != null && allInputFields.Length >= 3)
        {
            allInputFields[1].interactable = false;
            allInputFields[2].interactable = true;
        }
        if (step2NextButton != null) step2NextButton.gameObject.SetActive(false);
        if (step3NextButton != null) step3NextButton.gameObject.SetActive(true);
        isPanelOpen = false;
        reportPanel.SetActive(false);
        toggleButtonText.text = "Deney Raporu";
        Debug.Log("Adım 3'e geçildi!");
    }

    void GoToStep4()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetStep(4);
            GameManager.Instance.CompleteStep(3);
        }
        if (allInputFields != null && allInputFields.Length >= 4)
        {
            allInputFields[2].interactable = false;
            allInputFields[3].interactable = true;
        }
        if (step3NextButton != null) step3NextButton.gameObject.SetActive(false);
        isPanelOpen = false;
        reportPanel.SetActive(false);
        toggleButtonText.text = "Deney Raporu";
        Debug.Log("Adım 4'e geçildi!");
    }
}