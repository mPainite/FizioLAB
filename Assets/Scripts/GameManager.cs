using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Adım Takibi")]
    public int currentStep = 1;

    [Header("UI")]
    public TextMeshProUGUI taskText;

    [Header("Adım 1 Objeleri")]
    public DraggableCloth silkCloth;
    public DraggableCloth woolCloth;
    public DraggableRod glassRod;
    public PendulumRod pendulumRod;

    [Header("Adım 2 Objeleri")]
    public DraggableRod plasticRod;
    public PendulumRod plasticPendulumRod;

    [Header("Adım Durumları")]
    public bool step1Completed = false;
    public bool step2Completed = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        SetStep(1);
    }

    public void SetStep(int step)
    {
        currentStep = step;
        UpdateTaskText();
        UpdateObjectAccess();

        if (step == 2)
        {
            if (glassRod != null) glassRod.ResetToTablePublic();
            if (pendulumRod != null) pendulumRod.gameObject.SetActive(false);
            if (plasticPendulumRod != null) plasticPendulumRod.gameObject.SetActive(true);
        }
    }

    void UpdateTaskText()
    {
        switch (currentStep)
        {
            case 1:
                taskText.text = "Adım 1: Cam çubuğu elektriklemek için\nipek kumaşı üzerine sürükleyerek sürt.";
                break;
            case 2:
                taskText.text = "Adım 2: Plastik çubuğu elektriklemek için\nyünlü kumaşı üzerine sürükleyerek sürt.";
                break;
            case 3:
                taskText.text = "Adım 3: Yüklü cam ve plastik çubukları\nkarşılaştırın, etkileşimi gözlemleyin.";
                break;
        }
    }

    void UpdateObjectAccess()
    {
        switch (currentStep)
        {
            case 1:
                SetClothActive(silkCloth, true);
                SetClothActive(woolCloth, false);
                SetRodDraggable(glassRod, true);
                SetRodDraggable(plasticRod, false);
                break;
            case 2:
                SetClothActive(silkCloth, false);
                SetClothActive(woolCloth, true);
                SetRodDraggable(glassRod, false);
                SetRodDraggable(plasticRod, true);
                break;
            case 3:
                SetClothActive(silkCloth, true);
                SetClothActive(woolCloth, true);
                SetRodDraggable(glassRod, true);
                SetRodDraggable(plasticRod, true);
                break;
        }
    }

    void SetClothActive(DraggableCloth cloth, bool active)
    {
        if (cloth == null) return;
        cloth.enabled = active;
        Renderer r = cloth.GetComponent<Renderer>();
        if (r != null) r.material.color = active ? Color.white : new Color(0.4f, 0.4f, 0.4f);
    }

    void SetRodDraggable(DraggableRod rod, bool active)
    {
        if (rod == null) return;
        rod.enabled = active;
        Renderer r = rod.GetComponent<Renderer>();
        if (r != null) r.material.color = active ? rod.chargedColor : new Color(0.4f, 0.4f, 0.4f);
    }

    public void CompleteStep(int step)
    {
        if (step == 1) step1Completed = true;
        if (step == 2) step2Completed = true;
        Debug.Log("Adım " + step + " tamamlandı!");
    }
}